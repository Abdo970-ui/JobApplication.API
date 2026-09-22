using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.Jobs.Commandes.DeleteJob
{
    public class DeleteJobCommande : IRequest
    {
        public int id { get; set; }
    }
}
