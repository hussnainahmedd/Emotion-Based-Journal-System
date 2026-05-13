using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MindfulJournal.Models;

namespace MindfulJournal.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<Mood> Moods { get; set; }
        public DbSet<Suggestion> Suggestions { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Mood>().HasData(
                new Mood
                {
                    MoodId = 1,
                    MoodName = "Happy",
                    Emoji = "😄",
                    Color = "#4CAF50"
                },
                new Mood
                {
                    MoodId = 2,
                    MoodName = "Sad",
                    Emoji = "😢",
                    Color = "#2196F3"
                },
                new Mood
                {
                    MoodId = 3,
                    MoodName = "Stressed",
                    Emoji = "😫",
                    Color = "#FF9800"
                },
                new Mood
                {
                    MoodId = 4,
                    MoodName = "Angry",
                    Emoji = "😡",
                    Color = "#F44336"
                },
                new Mood
                {
                    MoodId = 5,
                    MoodName = "Neutral",
                    Emoji = "😐",
                    Color = "#9E9E9E"
                },
                new Mood
                {
                    MoodId = 6,
                    MoodName = "Calm",
                    Emoji = "😌",
                    Color = "#00BCD4"
                },
                new Mood
                {
                    MoodId = 7,
                    MoodName = "Anxious",
                    Emoji = "😰",
                    Color = "#9C27B0"
                }
            );

            builder.Entity<Suggestion>().HasData(
                new Suggestion
                {
                    SuggestionId = 1,
                    MoodType = "Sad",
                    Threshold = 3,
                    Message = "You seem sad lately. Consider talking to someone."
                },
                new Suggestion
                {
                    SuggestionId = 2,
                    MoodType = "Stressed",
                    Threshold = 4,
                    Message = "Stress detected. Try meditation."
                },
                new Suggestion
                {
                    SuggestionId = 3,
                    MoodType = "Angry",
                    Threshold = 3,
                    Message = "Try deep breathing exercises."
                },
                new Suggestion
                {
                    SuggestionId = 4,
                    MoodType = "Happy",
                    Threshold = 5,
                    Message = "Great! Keep up this positive momentum!"
                },
                new Suggestion
                {
                    SuggestionId = 5,
                    MoodType = "Anxious",
                    Threshold = 3,
                    Message = "Consider a calming activity."
                }
            );
        }
    }
}