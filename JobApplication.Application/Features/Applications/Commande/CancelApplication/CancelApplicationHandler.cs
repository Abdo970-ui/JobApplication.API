using JobApplication.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace JobApplication.Application.Features.Applications.Commande.CancelApplication
{
    public class CancelApplicationHandler : IRequestHandler<CancelApplicationCommande, string?>
    {
        private readonly IApplicationRepository _applicationRepo;

        public CancelApplicationHandler(IApplicationRepository applicationRepo)
        {
            _applicationRepo = applicationRepo;
        }

        public async Task<string?> Handle(CancelApplicationCommande request, CancellationToken cancellationToken)
        {
            var application = await _applicationRepo.GetApplicationByIdAsync(request.applicationId);

            if (application is null)
                return "Application not found.";

            if (application.JobApplicationStatus != JobApplication.Domain.Enms.JobApplicationStatus.Applied)
                return $"Cannot cancel an application that is already '{application.JobApplicationStatus}'.";

            application.JobApplicationStatus = JobApplication.Domain.Enms.JobApplicationStatus.Cancelled;
            application.StatusUpdatedAt = DateTime.UtcNow;

            await _applicationRepo.SaveChangesAsync();
            return null; // null means success
        }
    }
}
