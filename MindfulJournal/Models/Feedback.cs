using System.ComponentModel.DataAnnotations;

namespace MindfulJournal.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }
}