using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.Jobs.Queries.GetJob
{
    public class GetJobQuieryHandler : IRequestHandler<GetJobQuiery, Job>
    {
        private readonly IJobRepository _jobRepository;

        public GetJobQuieryHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        async Task<Job> IRequestHandler<GetJobQuiery, Job>.Handle(GetJobQuiery request, CancellationToken cancellationToken)
        {
            var job =await _jobRepository.Get().FirstOrDefaultAsync(j => j.Id == request.id);
            return job;
        }
    }
}
