using JobApplication.Domain.Enms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JobApplication.Domain.Entities
{
    public class JobCandidateApplication
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Job))]
        public int JobId { get; set; }
        public Job Job { get; set; }
        [ForeignKey(nameof(Candidate))]
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        public JobApplicationStatus JobApplicationStatus { get; set; } = JobApplicationStatus.Applied;
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
        public DateTime StatusUpdatedAt { get; set; } =  DateTime.UtcNow.AddDays(2);
    }
}
