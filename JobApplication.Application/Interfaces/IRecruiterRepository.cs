using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface IRecruiterRepository
    {
        Task<Recruiter?> GetByEmailAsync(string email);
        Task<Recruiter?> GetByIdAsync(int id);
        Task AddAsync(Recruiter recruiter);
        Task SaveChangesAsync();
    }
}
