using BCrypt.Net;
using JobApplication.Application.DTOs.Requste;
using JobApplication.Application.DTOs.Response;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Enms;
using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRecruiterRepository _recruiterRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ICandidateRepository _candidateRepository;

        public AuthService(IRecruiterRepository recruiterRepository, IJwtTokenGenerator jwtTokenGenerator, ICandidateRepository candidateRepository)
        {
            _recruiterRepository = recruiterRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _candidateRepository = candidateRepository;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            // تأكد إن الإيميل مش مستخدم في الجدولين مع بعض
            var emailExists = await _recruiterRepository.GetByEmailAsync(request.Email) != null
                || await _candidateRepository.GetByEmailAsync(request.Email) != null;

            if (emailExists)
                throw new InvalidOperationException("Email already registered");

            if (request.Role == UserRole.Recruiter)
            {
                var recruiter = new Recruiter
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    CreatedAt = DateTime.UtcNow
                };

                await _recruiterRepository.AddAsync(recruiter);
                await _recruiterRepository.SaveChangesAsync();

                var token = _jwtTokenGenerator.GenerateToken(recruiter.Id, recruiter.Email, "Recruiter");

                return new AuthResponseDto
                {
                    Token = token,
                    Name = recruiter.Name,
                    Email = recruiter.Email,
                    Role = "Recruiter"
                };
            }
            else // Candidate
            {
                var candidate = new Candidate
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    CreatedAt = DateTime.UtcNow
                };

                await _candidateRepository.AddAsync(candidate);
                await _candidateRepository.SaveChangesAsync();

                var token = _jwtTokenGenerator.GenerateToken(candidate.Id, candidate.Email, "Candidate");

                return new AuthResponseDto
                {
                    Token = token,
                    Name = candidate.Name,
                    Email = candidate.Email,
                    Role = "Candidate"
                };
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var recruiter = await _recruiterRepository.GetByEmailAsync(request.Email);
            if (recruiter != null && BCrypt.Net.BCrypt.Verify(request.Password, recruiter.PasswordHash))
            {
                var token = _jwtTokenGenerator.GenerateToken(recruiter.Id, recruiter.Email, "Recruiter");
                return new AuthResponseDto { Token = token, Name = recruiter.Name, Email = recruiter.Email, Role = "Recruiter" };
            }

            var candidate = await _candidateRepository.GetByEmailAsync(request.Email);
            if (candidate != null && BCrypt.Net.BCrypt.Verify(request.Password, candidate.PasswordHash))
            {
                var token = _jwtTokenGenerator.GenerateToken(candidate.Id, candidate.Email, "Candidate");
                return new AuthResponseDto { Token = token, Name = candidate.Name, Email = candidate.Email, Role = "Candidate" };
            }

            throw new UnauthorizedAccessException("Invalid email or password");
        }
    }
}