using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface IBackgroundJobScheduler
    {
        void Enqueu<T>(Expression<Action<T>> methodCall);
    }
}
