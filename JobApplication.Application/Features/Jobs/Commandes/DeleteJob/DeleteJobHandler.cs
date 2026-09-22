using JobApplication.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.Jobs.Commandes.DeleteJob
{

    public class DeleteJobHandler : IRequestHandler<DeleteJobCommande>
    {
    private readonly IJobRepository _jobRepository;

        public DeleteJobHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task Handle(DeleteJobCommande request, CancellationToken cancellationToken)
        {
            var job = _jobRepository.Get().FirstOrDefault(j => j.Id == request.id);
            if (job != null)
            {
                _jobRepository.DeleteJob(job);
                await _jobRepository.SaveChangesAsync();
            }
        }
    }
}
