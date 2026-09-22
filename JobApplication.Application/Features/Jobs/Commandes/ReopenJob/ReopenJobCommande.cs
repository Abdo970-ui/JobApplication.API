using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.Jobs.Commandes.ReopenJob
{
    public class ReopenJobCommande : IRequest<bool>
    {
        
        public int jobId { get; set; }
        public int currentRecruiterId { get; set; }
    }
}
