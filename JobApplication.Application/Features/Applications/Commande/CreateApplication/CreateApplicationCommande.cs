using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.Applications.Commande.CreateApplication
{
    public class CreateApplicationCommande : IRequest<int>
    {
        public int JobId { get; set; }
        public int candidateId { get; set; }
    }
}
