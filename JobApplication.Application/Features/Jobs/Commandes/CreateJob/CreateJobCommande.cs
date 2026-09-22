using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.Jobs.Commandes.CreateJob
{
    public class CreateJobCommande : IRequest<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
