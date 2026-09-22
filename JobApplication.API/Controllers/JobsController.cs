using JobApplication.Application.DTOs.Requste;
using JobApplication.Application.Features.Jobs.Commandes.CloseJob;
using JobApplication.Application.Features.Jobs.Commandes.CreateJob;
using JobApplication.Application.Features.Jobs.Commandes.DeleteJob;
using JobApplication.Application.Features.Jobs.Commandes.ReopenJob;
using JobApplication.Application.Features.Jobs.Commandes.UpdateJob;
using JobApplication.Application.Features.Jobs.Queries.GetAllJobs;
using JobApplication.Application.Features.Jobs.Queries.GetJob;
using JobApplication.Application.Interfaces;
using JobApplication.Application.Services;
using JobApplication.Infrastructure.Repositores;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    /// <summary>
    /// Handles job postings management: create, read, update, delete, close and reopen.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        //private readonly JobService _jobService;
        //private readonly IJobRepository _jobRepo;
        private readonly IMediator _mediator;

        public JobsController(JobService jobService, IJobRepository jobRepo, IMediator mediator)
        {
            //_jobService = jobService;
            //_jobRepo = jobRepo;
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new job posting.
        /// </summary>
        /// <param name="createJobDto">The job details (title and description).</param>
        /// <returns>The Id of the newly created job.</returns>
        /// <response code="200">Job created successfully.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAsync(CreateJobDto createJobDto)
        {
            //var jobId = await _jobService.CreateJobAsync(createJobDto);
            var jobId = await _mediator.Send(new CreateJobCommande
            {
                Title = createJobDto.Title,
                Description = createJobDto.Description
            });
            return Ok(new { JobId = jobId });
        }

        /// <summary>
        /// Gets a single job by its Id.
        /// </summary>
        /// <param name="id">The job Id.</param>
        /// <returns>The job details.</returns>
        /// <response code="200">Job found and returned.</response>
        /// <response code="404">No job exists with the given Id.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetJob(int id)
        {
            //var job = _jobService.GetJob(id);
            var job = await _mediator.Send(new GetJobQuiery { id = id });

            if (job == null)
            {
                return NotFound("Job not found for the given ID");
            }
            return Ok(job);
        }

        /// <summary>
        /// Gets all job postings.
        /// </summary>
        /// <returns>A list of all jobs.</returns>
        /// <response code="200">List of jobs returned successfully.</response>
        [HttpGet("GetAllJobs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllJobs()
        {
            //var jobs = _jobService.GetAllJobs();
            var jobs = await _mediator.Send(new GetAllJobsQuery());
            return Ok(jobs);
        }

        /// <summary>
        /// Updates an existing job posting.
        /// </summary>
        /// <param name="id">The Id of the job to update.</param>
        /// <param name="updateJobDto">The updated job details.</param>
        /// <response code="200">Job updated successfully.</response>
        /// <response code="404">No job exists with the given Id.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateJob(int id, UpdateJobDto updateJobDto)
        {
            var job = await _mediator.Send(new GetJobQuiery { id = id });

            if (job == null)
            {
                return NotFound("Job not found for the given ID");
            }

            await _mediator.Send(new UpdateJobCommande
            {
                Id = id,
                Title = updateJobDto.Title,
                Description = updateJobDto.Description,
                IsActive = updateJobDto.IsActive
            });

            return Ok("Job updated successfully.");
        }

        /// <summary>
        /// Deletes a job posting.
        /// </summary>
        /// <param name="id">The Id of the job to delete.</param>
        /// <response code="200">Job deleted successfully.</response>
        /// <response code="404">No job exists with the given Id.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteJob(int id)
        {
            //var job = _jobService.GetJob(id);
            var job = await _mediator.Send(new GetJobQuiery { id = id });

            if (job == null)
            {
                return NotFound("Job not found for the given ID");
            }

            await _mediator.Send(new DeleteJobCommande
            {
                id = id
            });

            return Ok("Job deleted successfully.");
        }

        /// <summary>
        /// Closes a job posting so it no longer accepts applications.
        /// </summary>
        /// <param name="id">The Id of the job to close.</param>
        /// <response code="204">Job closed successfully.</response>
        /// <response code="401">The recruiter is not authenticated or the token is invalid.</response>
        /// <response code="403">The recruiter is not allowed to close this job.</response>
        /// <response code="404">No job exists with the given Id.</response>
        /// <response code="409">The job cannot be closed in its current state.</response>
        [HttpPut("{id}/close")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Close(int id)
        {
            var recruiterIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (recruiterIdClaim == null || !int.TryParse(recruiterIdClaim, out var recruiterId))
                return Unauthorized();

            try
            {
                await _mediator.Send(new CloseJobCommande
                {
                    JobId = id,
                    CurrentRecruiterId = recruiterId
                });
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Reopens a previously closed job posting.
        /// </summary>
        /// <param name="id">The Id of the job to reopen.</param>
        /// <response code="204">Job reopened successfully.</response>
        /// <response code="401">The recruiter is not authenticated or the token is invalid.</response>
        /// <response code="403">The recruiter is not allowed to reopen this job.</response>
        /// <response code="404">No job exists with the given Id.</response>
        /// <response code="409">The job cannot be reopened in its current state.</response>
        [HttpPut("{id}/reopen")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Reopen(int id)
        {
            var recruiterIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (recruiterIdClaim == null || !int.TryParse(recruiterIdClaim, out var recruiterId))
                return Unauthorized();

            try
            {
                //await _jobService.ReopenAsync(id, recruiterId);
                await _mediator.Send(new ReopenJobCommande
                {
                    jobId = id,
                    currentRecruiterId = recruiterId
                });
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

    }
}