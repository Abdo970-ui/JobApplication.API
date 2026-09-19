using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.DTOs.Response
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }   // ✅ جديد
    }
}
