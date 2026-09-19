using JobApplication.Domain.Enms;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.DTOs.Requste
{
    public class RegisterRequestDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
