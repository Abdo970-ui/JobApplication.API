using JobApplication.Application.DTOs.Requste;
using JobApplication.Application.Features.Applications.Commande.CancelApplication;
using JobApplication.Application.Features.Applications.Commande.CreateApplication;
using JobApplication.Application.Services;
using JobApplication.Domain.Entities;
using JobApplication.Infrastructure.Prestance;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    /// <summary>
    /// Handles job application operations for candidates (create and cancel).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Candidate")]
    public class ApplicatonsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationService _applicationService;
        private readonly JobService _jobService;
        public readonly IMediator _mediator;

        public ApplicatonsController(ApplicationService applicationService, JobService jobService, ApplicationDbContext context, IMediator mediator)
        {
            _applicationService = applicationService;
            _jobService = jobService;
            _context = context;
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new job application for the currently authenticated candidate.
        /// </summary>
        /// <param name="requset">The job application details, including the JobId.</param>
        /// <returns>The Id of the newly created application.</returns>
        /// <response code="200">Application created successfully.</response>
        /// <response code="401">The candidate is not authenticated or the token is invalid.</response>
        /// <response code="404">The specified job or candidate was not found.</response>
        /// <response code="409">The job is closed and no longer accepting applications.</response>
        [HttpPost]
        [Authorize(Roles = "Candidate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateApplication(CreateApplicationRequest requset)
        {
            // هات الـ CandidateId من التوكن، مش من الـ Body
            var candidateIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (candidateIdClaim == null || !int.TryParse(candidateIdClaim, out var candidateId))
                return Unauthorized();

            var job = _context.Jobs.FirstOrDefault(e => e.Id == requset.JobId);
            var candidate = _context.Candidates.FirstOrDefault(e => e.Id == candidateId);   // ✅ من التوكن

            if (job == null || candidate == null)
                return NotFound("candidate or job not found");

            if (job.ClosedAt != null || !job.IsActive)
                return Conflict("This job is closed and no longer accepting applications");

            var appId = await _applicationService.CreateApplicationAsync(requset, candidateId);
            //var appId = await _mediator.Send(new CreateApplicationCommande());

            return Ok(new { AppId = appId });
        }

        /// <summary>
        /// Cancels an existing job application.
        /// </summary>
        /// <param name="id">The Id of the application to cancel.</param>
        /// <response code="204">Application cancelled successfully.</response>
        /// <response code="400">The application could not be cancelled (e.g. invalid state).</response>
        /// <response code="404">Application not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelApplication(int id)
        {
            //var error = await _applicationService.CancelApplicationAsync(id);
            var error = await _mediator.Send(new CancelApplicationCommande()
            {
                applicationId = id
            });

            if (error == "Application not found.")
                return NotFound(error);

            if (error is not null)
                return BadRequest(error);

            return NoContent();
        }
    }
}