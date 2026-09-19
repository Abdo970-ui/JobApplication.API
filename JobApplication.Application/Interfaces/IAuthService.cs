using JobApplication.Application.DTOs.Requste;
using JobApplication.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    }
}
