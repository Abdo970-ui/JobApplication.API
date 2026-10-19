using Hangfire;
using JobApplication.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace JobApplication.Infrastructure.Services
{
    public class HangfireBackgroundJobScheduler : IBackgroundJobScheduler
    {
       private readonly IBackgroundJobClient _backgroundJobClient;

        public HangfireBackgroundJobScheduler(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

        public void Enqueu<T>(Expression<Action<T>> methodCall)
        {
            _backgroundJobClient.Enqueue<T>(methodCall);
        }
    }
}
