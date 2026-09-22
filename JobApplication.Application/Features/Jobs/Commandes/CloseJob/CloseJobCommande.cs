using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.Jobs.Commandes.CloseJob
{
    public class CloseJobCommande : IRequest<bool>
    {
        public int JobId { get; set; }
        public int CurrentRecruiterId { get; set; }
    }
}
