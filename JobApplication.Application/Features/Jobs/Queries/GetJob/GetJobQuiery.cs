using JobApplication.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.Jobs.Queries.GetJob
{
    public class GetJobQuiery : IRequest<Job>
    {
        public int id { get; set; }
    }
}
