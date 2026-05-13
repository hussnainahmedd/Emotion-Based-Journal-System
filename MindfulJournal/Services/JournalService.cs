using Microsoft.EntityFrameworkCore;
using MindfulJournal.Data;
using MindfulJournal.Models;

namespace MindfulJournal.Services
{
    public class JournalService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;

        public JournalService(IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<List<JournalEntry>> GetEntriesAsync(string userId)
        {
            using var db = await _factory.CreateDbContextAsync();
            return await db.JournalEntries
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.EntryDate)
                .ToListAsync();
        }

        public async Task<List<JournalEntry>> GetRecentEntriesAsync(
            string userId, int count = 3)
        {
            using var db = await _factory.CreateDbContextAsync();
            return await db.JournalEntries
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.EntryDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task AddEntryAsync(JournalEntry entry)
        {
            using var db = await _factory.CreateDbContextAsync();
            db.JournalEntries.Add(entry);
            await db.SaveChangesAsync();
        }

        public async Task UpdateEntryAsync(JournalEntry updatedEntry)
        {
            using var db = await _factory.CreateDbContextAsync();
            var entry = await db.JournalEntries
                .FirstOrDefaultAsync(e => e.EntryId == updatedEntry.EntryId);
            if (entry != null)
            {
                entry.Title = updatedEntry.Title;
                entry.Content = updatedEntry.Content;
                entry.Mood = updatedEntry.Mood;
                entry.EntryDate = updatedEntry.EntryDate;
                entry.UpdatedAt = DateTime.Now;
                await db.SaveChangesAsync();
            }
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

        public async Task<Dictionary<string, int>> GetWeeklyMoodCountsAsync(
            string userId)
        {
            using var db = await _factory.CreateDbContextAsync();
            var weekAgo = DateTime.Today.AddDays(-7);
            var entries = await db.JournalEntries
                .Where(e => e.UserId == userId && e.EntryDate >= weekAgo)
                .ToListAsync();
            return entries
                .GroupBy(e => e.Mood)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<string, int>> GetMonthlyMoodCountsAsync(
            string userId)
        {
            using var db = await _factory.CreateDbContextAsync();
            var monthStart = new DateTime(
                DateTime.Today.Year, DateTime.Today.Month, 1);
            var entries = await db.JournalEntries
                .Where(e => e.UserId == userId && e.EntryDate >= monthStart)
                .ToListAsync();
            return entries
                .GroupBy(e => e.Mood)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<List<string>> GetSuggestionsAsync(string userId)
        {
            var suggestions = new List<string>();
            var moodCounts = await GetWeeklyMoodCountsAsync(userId);

            moodCounts.TryGetValue("Sad", out int sadCount);
            moodCounts.TryGetValue("Stressed", out int stressedCount);
            moodCounts.TryGetValue("Angry", out int angryCount);
            moodCounts.TryGetValue("Happy", out int happyCount);
            moodCounts.TryGetValue("Anxious", out int anxiousCount);

            if (happyCount >= 5)
                suggestions.Add("Positive Streak! You're doing great! 🌟");
            if (sadCount >= 3)
                suggestions.Add("You seem sad lately. Consider talking to someone. 💙");
            if (stressedCount >= 3)
                suggestions.Add("Manage Stress: Remember to take breaks. 🧘");
            if (angryCount >= 3)
                suggestions.Add("Try stress relief activities like exercise. 🏃");
            if (anxiousCount >= 3)
                suggestions.Add("Consider a calming activity if feeling anxious. 🌿");

            if (suggestions.Count == 0)
                suggestions.Add("Keep reflecting on your emotions daily. 📖");

            return suggestions;
        }

        public async Task<int> GetTotalEntriesAsync(string userId)
        {
            using var db = await _factory.CreateDbContextAsync();
            return await db.JournalEntries
                .CountAsync(e => e.UserId == userId);
        }

        

        public async Task<JournalEntry?> GetEntryByIdAsync(int entryId)
        {
            using var db = await _factory.CreateDbContextAsync();
            return await db.JournalEntries.FindAsync(entryId);
        }

        public async Task<int> GetStreakAsync(string userId)
        {
            using var db = await _factory.CreateDbContextAsync();
            var entries = await db.JournalEntries
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.EntryDate)
                .Select(e => e.EntryDate)
                .ToListAsync();

            if (entries.Count == 0) return 0;

            int streak = 0;
            var checkDate = DateTime.Today;

            foreach (var date in entries.Distinct())
            {
                if (date.Date == checkDate)
                {
                    streak++;
                    checkDate = checkDate.AddDays(-1);
                }
                else break;
            }

            return streak;
        }

        public async Task<string> GetMostCommonMoodAsync(string userId)
        {
            using var db = await _factory.CreateDbContextAsync();
            var entries = await db.JournalEntries
                .Where(e => e.UserId == userId)
                .ToListAsync();

            if (entries.Count == 0) return "None";

            return entries
                .GroupBy(e => e.Mood)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? "None";
        }

    }


}