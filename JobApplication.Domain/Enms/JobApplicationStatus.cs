using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Domain.Enms
{
    public enum JobApplicationStatus
    {
        Applied,
        UnderReview,
        Interview,
        Accepted,
        Rejected,
        Cancelled
    }
}
