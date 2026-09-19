using JobApplication.Domain.Enms;
using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JobApplication.Application.DTOs.Requste
{
    public class CreateApplicationRequest
    {
        public int JobId { get; set; }

        //public int CandidateId { get; set; }

    }
}
