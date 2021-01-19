using System;
using System.Text.Json;

namespace MoodLog
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("What's your mood, in a rating from 0(Lowest) to 9(Highest)?");
            var currentMood = new Mood(true);
            Console.WriteLine($"\n{currentMood.CurrentDate} @ {currentMood.CurrentTime}: {currentMood.CurrentRating}");
            
            var jsonTest = JsonSerializer.Serialize(currentMood);
            
            var deserializeTest = JsonSerializer.Deserialize<Mood>(jsonTest);
        }
    }
}