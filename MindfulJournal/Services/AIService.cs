using System.Text;
using System.Text.Json;

namespace MindfulJournal.Services
{
    public class AIService
    {
        private readonly HttpClient _httpClient;
        private const string API_URL =
            "https://api.groq.com/openai/v1/chat/completions";
        private const string API_KEY =
            "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";

        public AIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async Task<string> CallGroqAsync(string prompt)
        {
            try
            {
                var requestBody = new
                {
                    model = "llama-3.1-8b-instant",
                    max_tokens = 200,
                    messages = new[]
                    {
                new { role = "user", content = prompt }
            }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var request = new HttpRequestMessage(HttpMethod.Post, API_URL);
                request.Headers.Add("Authorization", $"Bearer {API_KEY}");
                request.Content = new StringContent(
                    json, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                var responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return $"API Error {response.StatusCode}: {responseJson}";
                }

                using var doc = JsonDocument.Parse(responseJson);
                return doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? "Keep journaling every day!";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> GetAISuggestionAsync(
            List<string> recentEntries,
            Dictionary<string, int> moodCounts)
        {
            var moodSummary = string.Join(", ",
                moodCounts.Select(m => $"{m.Key}: {m.Value} times"));

            var entriesSummary = string.Join("\n",
                recentEntries.Take(5)
                .Select((e, i) => $"Entry {i + 1}: {e}"));

            var prompt = $@"You are a compassionate mental wellness advisor. 
Based on this user's journal data, provide ONE short 
personalized wellness suggestion (max 2 sentences).

Recent mood pattern this week: {moodSummary}
Recent journal entries:
{entriesSummary}

Give a warm, supportive, actionable suggestion. 
Be specific to their mood pattern. No bullet points.";

            return await CallGroqAsync(prompt);
        }

        public async Task<string> GetMoodPredictionAsync(
            List<string> moodHistory)
        {
            var history = string.Join(", ", moodHistory.Take(14));

            var prompt = $@"Based on this mood history (most recent last): 
{history}

Predict tomorrow's likely mood in ONE sentence. 
Be specific and encouraging. Max 20 words.";

            return await CallGroqAsync(prompt);
        }

        public async Task<string> ChatWithJournalAsync(
            string question,
            List<string> entries,
            List<string> moods)
        {
            var entriesContext = string.Join("\n",
                entries.Take(10)
                .Select((e, i) => $"Entry {i + 1}: {e}"));

            var moodContext = string.Join(", ", moods.Take(20));

            var prompt = $@"You are a personal journal assistant. 
The user has these journal entries:
{entriesContext}

Recent moods: {moodContext}

User question: {question}

Answer based ONLY on their journal data. 
Be warm, specific, and concise (max 3 sentences).";

            return await CallGroqAsync(prompt);
        }
    }
}