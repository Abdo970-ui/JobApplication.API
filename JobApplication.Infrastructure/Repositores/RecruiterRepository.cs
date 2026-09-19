using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Infrastructure.Prestance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Infrastructure.Repositores
{
    public class RecruiterRepository : IRecruiterRepository
    {
        private readonly ApplicationDbContext _context;

        public RecruiterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Recruiter?> GetByEmailAsync(string email)
        {
            return await _context.Recruiters
                .FirstOrDefaultAsync(r => r.Email == email);
        }

        public async Task<Recruiter?> GetByIdAsync(int id)
        {
            return await _context.Recruiters
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(Recruiter recruiter)
        {
            await _context.Recruiters.AddAsync(recruiter);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
