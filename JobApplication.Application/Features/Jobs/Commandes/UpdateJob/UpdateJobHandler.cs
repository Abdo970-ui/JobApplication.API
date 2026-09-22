using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JobApplication.Application.DTOs.Requste;
using JobApplication.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobApplication.Application.Features.Jobs.Commandes.UpdateJob
{
    public class UpdateJobHandler : IRequestHandler<UpdateJobCommande>
    {
        private readonly IJobRepository _jobRepository;

        public UpdateJobHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task Handle(UpdateJobCommande request, CancellationToken cancellationToken)
        {
            var job =await _jobRepository.Get().FirstOrDefaultAsync(j => j.Id == request.Id);
            if (job != null)
            {
                job.Title = request.Title;
                job.Description = request.Description;
                job.IsActive = request.IsActive;
                await _jobRepository.UpdateJobAsync(request.Id, new UpdateJobDto
                {
                    Title = request.Title,
                    Description = request.Description,
                    IsActive = request.IsActive
                });
                await _jobRepository.SaveChangesAsync();
            }
        }
    }
}
