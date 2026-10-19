using JobApplication.Application.Interfaces;
using Microsoft.Extensions.Logging;
namespace JobApplication.Infrastructure.Services
{
    public class EmailNotificationService : INotificationService
    {
       private readonly IApplicationRepository _applicationRepo;
        private readonly ILogger<EmailNotificationService> _logger;
        public EmailNotificationService(IApplicationRepository application_applicationRepo, ILogger<EmailNotificationService> logger)
        {
            _applicationRepo = application_applicationRepo;
            _logger = logger;
        }

        public async Task NotifyReqruiter(int applicationId)
        {
            var app =await _applicationRepo.GetApplicationByIdAsync(applicationId);
            if (app is null)
            {
                _logger.LogWarning("app is {applicationId} not found ", applicationId);
                return;
            }
            _logger.LogInformation("Send Emai : candidte {CandidateId} has applied to {JobId} and applicationId is {applicationId}",
                app.CandidateId, app.JobId , applicationId);
        }
    }
}
