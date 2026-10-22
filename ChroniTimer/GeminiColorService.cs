using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChroniTimer
{
    /// <summary>
    /// Calls the Gemini API to generate a curated list of beautiful
    /// background gradient colors for the ChroniTimer app.
    /// </summary>
    public class GeminiColorService
    {
        private static readonly HttpClient _httpClient = new();
        private const string ApiKey = "AIzaSyATXRE3__gat87ZM1I4MvA8d-98aaJMkYA";
        private const string Endpoint =
            "https://generativelanguage.googleapis.com/v1beta/models/gemini-3.5-flash-lite:generateContent?key=" + ApiKey;

        /// <summary>
        /// Asks Gemini for 10 beautiful gradient color pairs suitable for
        /// a dark-themed timer app background.
        /// Returns a list of (StartColor, EndColor, Name) tuples.
        /// </summary>
        public static async Task<List<(Color Start, Color End, string Name)>> GetSuggestedColorsAsync()
        {
            var prompt = @"Generate exactly 10 beautiful gradient color pairs for a light-themed desktop timer application background. 
Each gradient should be visually stunning, modern.
I like the combinations of bright colors.

Return ONLY a valid JSON array with no markdown formatting, no code fences, no extra text.
Each element must have: name, startR, startG, startB, endR, endG, endB.
Example: [{""name"":""Ocean Dusk"",""startR"":20,""startG"":30,""startB"":60,""endR"":50,""endG"":80,""endB"":140}]";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 1.0,
                    maxOutputTokens = 1024
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(Endpoint, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Gemini API error ({response.StatusCode}): {responseBody}");
            }

            return ParseGeminiResponse(responseBody);
        }

        private static List<(Color Start, Color End, string Name)> ParseGeminiResponse(string responseBody)
        {
            var result = new List<(Color Start, Color End, string Name)>();

            var root = JObject.Parse(responseBody);
            var textContent = root["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

            if (string.IsNullOrEmpty(textContent))
                return GetFallbackColors();

            // Strip markdown code fences if present
            textContent = textContent.Trim();
            if (textContent.StartsWith("```"))
            {
                int firstNewline = textContent.IndexOf('\n');
                if (firstNewline >= 0)
                    textContent = textContent.Substring(firstNewline + 1);
                if (textContent.EndsWith("```"))
                    textContent = textContent.Substring(0, textContent.Length - 3);
                textContent = textContent.Trim();
            }

            try
            {
                var colors = JsonConvert.DeserializeObject<List<GeminiColorEntry>>(textContent);
                if (colors == null || colors.Count == 0)
                    return GetFallbackColors();

                foreach (var c in colors)
                {
                    var start = Color.FromArgb(Clamp(c.startR), Clamp(c.startG), Clamp(c.startB));
                    var end = Color.FromArgb(Clamp(c.endR), Clamp(c.endG), Clamp(c.endB));
                    result.Add((start, end, c.name ?? "Unnamed"));
                }
            }
            catch
            {
                return GetFallbackColors();
            }

            return result;
        }

        private static int Clamp(int value) => Math.Max(0, Math.Min(255, value));

        private static List<(Color Start, Color End, string Name)> GetFallbackColors()
        {
            return new List<(Color Start, Color End, string Name)>
            {
                (Color.FromArgb(15, 25, 55), Color.FromArgb(60, 100, 170), "Sapphire Night"),
                (Color.FromArgb(30, 15, 50), Color.FromArgb(120, 50, 130), "Amethyst Glow"),
                (Color.FromArgb(10, 35, 35), Color.FromArgb(30, 120, 100), "Emerald Depths"),
                (Color.FromArgb(40, 25, 15), Color.FromArgb(150, 80, 30),  "Copper Sunset"),
                (Color.FromArgb(20, 20, 40), Color.FromArgb(80, 60, 140),  "Lavender Haze"),
                (Color.FromArgb(10, 30, 50), Color.FromArgb(40, 90, 160),  "Atlantic Blue"),
                (Color.FromArgb(35, 10, 30), Color.FromArgb(140, 40, 80),  "Rose Noir"),
                (Color.FromArgb(15, 30, 20), Color.FromArgb(40, 130, 70),  "Forest Mist"),
                (Color.FromArgb(30, 30, 30), Color.FromArgb(90, 90, 90),   "Silver Shadow"),
                (Color.FromArgb(25, 15, 45), Color.FromArgb(100, 50, 160), "Plum Twilight"),
            };
        }

        private class GeminiColorEntry
        {
            public string? name { get; set; }
            public int startR { get; set; }
            public int startG { get; set; }
            public int startB { get; set; }
            public int endR { get; set; }
            public int endG { get; set; }
            public int endB { get; set; }
        }
    }
}
