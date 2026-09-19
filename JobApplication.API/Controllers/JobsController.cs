using JobApplication.Application.DTOs.Requste;
using JobApplication.Application.Interfaces;
using JobApplication.Application.Services;
using JobApplication.Infrastructure.Repositores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly JobService _jobService;
        private readonly IJobRepository _jobRepo;

        public JobsController(JobService jobService, IJobRepository jobRepo)
        {
            _jobService = jobService;
            _jobRepo = jobRepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateJobDto createJobDto)
        {
            var jobId = await _jobService.CreateJobAsync(createJobDto);
            return Ok(new { JobId = jobId });
        }
        [HttpGet("{id}")]
        public IActionResult GetJob(int id)
        {
            var job = _jobService.GetJob(id);
            if (job == null)
            {
                return NotFound("Job not found for the given ID");
            }
            return Ok(job);
        }
        [HttpGet("GetAllJobs")]
        public IActionResult GetAllJobs()
        {
            var jobs = _jobService.GetAllJobs();
            return Ok(jobs);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(int id, UpdateJobDto updateJobDto)
        {
            var job = _jobService.GetJob(id);
            if (job == null)
            {
                return NotFound("Job not found for the given ID");
            }
            await _jobService.UpdateJobAsync(id, updateJobDto);
            return Ok("Job updated successfully.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = _jobService.GetJob(id);
            if (job == null)
            {
                return NotFound("Job not found for the given ID");
            }
            await _jobService.DeleteJob(id);
            return Ok("Job deleted successfully.");
        }

        [HttpPut("{id}/close")]
        [Authorize]
        public async Task<IActionResult> Close(int id)
        {
            var recruiterIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (recruiterIdClaim == null || !int.TryParse(recruiterIdClaim, out var recruiterId))
                return Unauthorized();

            try
            {
                await _jobService.CloseAsync(id, recruiterId);
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
        [HttpPut("{id}/reopen")]
        [Authorize]
        public async Task<IActionResult> Reopen(int id)
        {
            var recruiterIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (recruiterIdClaim == null || !int.TryParse(recruiterIdClaim, out var recruiterId))
                return Unauthorized();

            try
            {
                await _jobService.ReopenAsync(id, recruiterId);
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
