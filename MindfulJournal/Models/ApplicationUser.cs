using Microsoft.AspNetCore.Identity;

namespace MindfulJournal.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? ProfilePicture { get; set; }
        public ICollection<JournalEntry> JournalEntries { get; set; }
            = new List<JournalEntry>();
    }
}