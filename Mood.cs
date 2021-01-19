using System;

namespace MoodLog
{
    public class Mood
    {
        public string CurrentDate { get; }
        public string CurrentTime { get; }
        public string CurrentRating { get; }

        public Mood()
        {
            CurrentDate = DateTime.Now.ToShortDateString();
            CurrentTime = DateTime.Now.ToShortTimeString();
            CurrentRating = GetNumberOnly();
        }

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