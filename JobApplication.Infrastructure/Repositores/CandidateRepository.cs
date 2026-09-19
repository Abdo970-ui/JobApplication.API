using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Infrastructure.Prestance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Infrastructure.Repositores
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDbContext _context;

        public CandidateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Candidate?> GetByEmailAsync(string email)
        {
            return await _context.Candidates
                .Where(c => c.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<Candidate?> GetByIdAsync(int id)
        {
            return await _context.Candidates
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Candidate candidate)
        {
            await _context.Candidates.AddAsync(candidate);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
