using System;

namespace MoodLog
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"Current Date for entries: {DateTime.Now.Date.ToShortDateString()}");
            Console.WriteLine("What's your mood, in a rating from 0(Lowest) to 9(Highest)?");
            var mood = new Mood();
            Console.WriteLine($"\n{mood.CurrentDate} @ {mood.CurrentTime}: {mood.CurrentRating}");
        }
    }
}