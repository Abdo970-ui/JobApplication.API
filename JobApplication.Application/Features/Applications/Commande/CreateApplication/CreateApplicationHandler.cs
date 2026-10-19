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
        private readonly INotificationService _notificationService;
        private readonly IBackgroundJobScheduler _backgroundJobScheduler;

        public CreateApplicationHandler(IApplicationRepository applicationRepo, INotificationService notificationService, IBackgroundJobScheduler backgroundJobScheduler)
        {
            _applicationRepo = applicationRepo;
            _notificationService = notificationService;
            _backgroundJobScheduler = backgroundJobScheduler;
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

           //await _notificationService.NotifyReqruiter(application.Id);

            _backgroundJobScheduler.Enqueu<INotificationService>(b=>b.NotifyReqruiter(application.Id));
            return application.Id;
        }
    }
}
