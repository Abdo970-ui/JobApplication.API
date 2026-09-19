using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface ICandidateRepository
    {
        Task<Candidate?> GetByEmailAsync(string email);
        Task<Candidate?> GetByIdAsync(int id);
        Task AddAsync(Candidate candidate);
        Task SaveChangesAsync();
    }
}
