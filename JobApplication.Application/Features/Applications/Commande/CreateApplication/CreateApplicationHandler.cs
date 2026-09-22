using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.Applications.Commande.CreateApplication
{
    public class CreateApplicationHandler : IRequestHandler<CreateApplicationCommande, int>
    {
        private readonly IApplicationRepository _applicationRepo;

        public CreateApplicationHandler(IApplicationRepository applicationRepo)
        {
            _applicationRepo = applicationRepo;
        }

        public async Task<int> Handle(CreateApplicationCommande request, CancellationToken cancellationToken)
        {
            var application = new JobCandidateApplication
            {
                JobId = request.JobId,
                CandidateId = request.candidateId,

            };
            await _applicationRepo.CreateApplicationAsync(application);
            await _applicationRepo.SaveChangesAsync();
            return application.Id;
        }
    }
}
