using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MoodLog
{
    [Serializable]
    public class Mood
    {
        public Mood(bool manual = false)
        {
            CurrentDate = DateTime.Now.ToShortDateString();
            CurrentTime = DateTime.Now.ToShortTimeString();
            if (manual)
            {
                CurrentRating = GetNumberOnly();
            }
        }

        [JsonConstructor]
        public Mood()
        {
            
        }

        public string CurrentDate { get; set; }
        public string CurrentTime { get; set; }
        public string CurrentRating { get; set; }

        private static bool IsNumber(ConsoleKey inputKey)
        {
            return ((inputKey >= ConsoleKey.D0 && inputKey <= ConsoleKey.D9) ||
                    (inputKey >= ConsoleKey.NumPad0 && inputKey <= ConsoleKey.NumPad9));
        }

        private static string GetNumberOnly()
        {
            ConsoleKeyInfo key = default;
            while (!IsNumber(key.Key))
            {
                key = Console.ReadKey();
                Console.WriteLine("Gotten Key.");
            }
            return key.KeyChar.ToString();
        }
    }
}