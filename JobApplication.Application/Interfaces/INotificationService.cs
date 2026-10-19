using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface INotificationService
    {
       Task NotifyReqruiter(int applicationId);
    }
}
