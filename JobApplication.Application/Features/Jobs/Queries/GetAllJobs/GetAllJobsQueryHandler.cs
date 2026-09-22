using JobApplication.Application.Features.Jobs.Queries.GetJob;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.Jobs.Queries.GetAllJobs
{
    public class GetAllJobsQueryHandler : IRequestHandler<GetAllJobsQuery, IEnumerable<Job>>
    {
        private readonly IJobRepository _jobRepository;

        public GetAllJobsQueryHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public Task<IEnumerable<Job>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken)
        {
            var jobs = _jobRepository.GetAllJobs().ToList();
            return Task.FromResult<IEnumerable<Job>>(jobs);
        }
    }
}
