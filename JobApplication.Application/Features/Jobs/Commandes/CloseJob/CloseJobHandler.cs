using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JobApplication.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobApplication.Application.Features.Jobs.Commandes.CloseJob
{
    public class CloseJobHandler : IRequestHandler<CloseJobCommande, bool>
    {
        private readonly IJobRepository _jobRepository;

        public CloseJobHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<bool> Handle(CloseJobCommande request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.Get()
                .FirstOrDefaultAsync(j => j.Id == request.JobId, cancellationToken);

            if (job == null)
                throw new KeyNotFoundException("Job not found");

            if (job.RecruiterId == null || job.RecruiterId != request.CurrentRecruiterId)
                throw new UnauthorizedAccessException("You are not the owner of this job");

            if (job.ClosedAt != null)
                throw new InvalidOperationException("Job is already closed");

            job.ClosedAt = DateTime.UtcNow;
            job.ClosedBy = request.CurrentRecruiterId;
            job.IsActive = false;

            await _jobRepository.SaveChangesAsync();
            return true;
        }
    }
}
