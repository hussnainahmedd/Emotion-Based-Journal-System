namespace MindfulJournal.Models
{
    public class Suggestion
    {
        public int SuggestionId { get; set; }
        public string MoodType { get; set; } = string.Empty;
        public int Threshold { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}