namespace MindfulJournal.Models
{
    public class Mood
    {
        public int MoodId { get; set; }
        public string MoodName { get; set; } = string.Empty;
        public string Emoji { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}