using JobApplication.Application.DTOs.Requste;
using JobApplication.Application.Services;
using JobApplication.Domain.Entities;
using JobApplication.Infrastructure.Prestance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Candidate")]
    public class ApplicatonsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationService _applicationService;
        private readonly JobService _jobService;

        public ApplicatonsController(ApplicationService applicationService, JobService jobService, ApplicationDbContext context)
        {
            _applicationService = applicationService;
            _jobService = jobService;
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Candidate")]
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

            return Ok(new { AppId = appId });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelApplication(int id)
        {
            var error = await _applicationService.CancelApplicationAsync(id);

            if (error == "Application not found.")
                return NotFound(error);

            if (error is not null)
                return BadRequest(error);

            return NoContent();
        }
    }
}
