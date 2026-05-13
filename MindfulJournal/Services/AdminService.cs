using Microsoft.EntityFrameworkCore;
using MindfulJournal.Data;
using MindfulJournal.Models;

namespace MindfulJournal.Services
{
    public class AdminService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;

        public AdminService(
            IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            var db = await _factory.CreateDbContextAsync();
            var users = await db.Users
                .OrderByDescending(u => u.CreatedDate)
                .ToListAsync();
            await db.DisposeAsync();
            return users;
        }

        public async Task<int> GetTotalUsersAsync()
        {
            var db = await _factory.CreateDbContextAsync();
            var count = await db.Users.CountAsync();
            await db.DisposeAsync();
            return count;
        }

        public async Task<int> GetTotalEntriesAsync()
        {
            using var db = await _factory.CreateDbContextAsync();
            return await db.JournalEntries.CountAsync();
        }

        public async Task<int> GetTotalFeedbacksAsync()
        {
            using var db = await _factory.CreateDbContextAsync();
            return await db.Feedbacks.CountAsync();
        }

        public async Task<List<JournalEntry>> GetAllEntriesAsync()
        {
            using var db = await _factory.CreateDbContextAsync();
            return await db.JournalEntries
                .OrderByDescending(e => e.EntryDate)
                .ToListAsync();
        }

        public async Task<List<Feedback>> GetAllFeedbacksAsync()
        {
            using var db = await _factory.CreateDbContextAsync();
            return await db.Feedbacks
                .OrderByDescending(f => f.SubmittedAt)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetMoodStatsAsync()
        {
            using var db = await _factory.CreateDbContextAsync();
            var entries = await db.JournalEntries.ToListAsync();
            return entries
                .GroupBy(e => e.Mood)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<string, int>> GetUserEntryCountsAsync()
        {
            using var db = await _factory.CreateDbContextAsync();
            var entries = await db.JournalEntries.ToListAsync();
            return entries
                .GroupBy(e => e.UserName)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task DeleteUserAsync(string userId)
        {
            var db = await _factory.CreateDbContextAsync();

            var entries = db.JournalEntries
                .Where(e => e.UserId == userId);
            db.JournalEntries.RemoveRange(entries);

            var feedbacks = db.Feedbacks
                .Where(f => f.UserId == userId);
            db.Feedbacks.RemoveRange(feedbacks);

            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
                db.Users.Remove(user);

            await db.SaveChangesAsync();
            await db.DisposeAsync();
        }

        public async Task DeleteEntryAsync(int entryId)
        {
            using var db = await _factory.CreateDbContextAsync();
            var entry = await db.JournalEntries
                .FirstOrDefaultAsync(e => e.EntryId == entryId);
            if (entry != null)
            {
                db.JournalEntries.Remove(entry);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteFeedbackAsync(int feedbackId)
        {
            using var db = await _factory.CreateDbContextAsync();
            var fb = await db.Feedbacks
                .FirstOrDefaultAsync(f => f.FeedbackId == feedbackId);
            if (fb != null)
            {
                db.Feedbacks.Remove(fb);
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<JournalEntry>> GetUserEntriesAsync(
            string userId)
        {
            using var db = await _factory.CreateDbContextAsync();
            return await db.JournalEntries
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.EntryDate)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync()
        {
            using var db = await _factory.CreateDbContextAsync();
            var feedbacks = await db.Feedbacks.ToListAsync();
            if (!feedbacks.Any()) return 0;
            return Math.Round(feedbacks.Average(f => f.Rating), 1);
        }

        public async Task<int> GetTodayEntriesAsync()
        {
            using var db = await _factory.CreateDbContextAsync();
            return await db.JournalEntries
                .CountAsync(e => e.EntryDate == DateTime.Today);
        }
    }
}