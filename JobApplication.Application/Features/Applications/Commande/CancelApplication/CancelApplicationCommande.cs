using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Features.Applications.Commande.CancelApplication
{
    public class CancelApplicationCommande:IRequest<string?>
    {
       public int applicationId { get; set; }
    }
}
