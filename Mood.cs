using System;

namespace MoodLog
{
    public class Mood
    {
        public string CurrentDate   { get; }
        public string CurrentTime   { get; }
        public string CurrentRating { get; }

        public Mood()
        {
            ConsoleKeyInfo key = default;
            while (!IsNumber(key.Key))
            {
                key = Console.ReadKey();
            }

            CurrentDate = DateTime.Now.ToShortDateString();
            CurrentTime = DateTime.Now.ToShortTimeString();
            CurrentRating = key.KeyChar.ToString();
        }
        
        private static bool IsNumber(ConsoleKey inputKey)
        {
            return ((inputKey >= ConsoleKey.D0 && inputKey <= ConsoleKey.D9) ||
                    (inputKey >= ConsoleKey.NumPad0 && inputKey <= ConsoleKey.NumPad9));
        }
    }
}