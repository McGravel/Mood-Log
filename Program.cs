using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace MoodLog
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"Location: {Environment.CurrentDirectory}");
            var fileName = Environment.CurrentDirectory + @"\mood.json";
            
            if (!File.Exists(fileName))
            {
                Console.WriteLine("File non-existent, creating new one.");
                File.Create(fileName);
            }

            Console.Write("Rate your mood from 0 being the lowest and 9 the highest: ");
            var currentMood = new Mood()
            {
                CurrentDate = DateTime.Now.ToShortDateString(),
                CurrentTime = DateTime.Now.ToShortTimeString(),
                CurrentRating = GetNumberOnly(),
                OptionalComment = GetComment()
            };
            
            var jsonTest = JsonSerializer.Serialize(currentMood);
            
            using (var file = new StreamWriter(fileName, true))
            {
                file.WriteLine(jsonTest);
            }

            using (var readFile = new StreamReader(fileName))
            {
                string lastDate = default;
                
                while (!readFile.EndOfStream)
                {
                    var mood = JsonSerializer.Deserialize<Mood>(readFile.ReadLine());

                    if (lastDate != mood.CurrentDate)
                    {
                        Console.Write($"{mood.CurrentDate}\n");
                        lastDate = mood.CurrentDate;
                    }
                    
                    Console.WriteLine($"\t{mood.CurrentTime} | {mood.CurrentRating} | {mood.OptionalComment}");
                }
            }

            Console.Write("Press a key to exit... ");
            Console.ReadKey();
        }
        
        private static string GetComment()
        {
            Console.WriteLine("Any comment regarding your current mood? (Press Enter to skip)");
            var comment = Console.ReadLine();
            Debug.Assert(comment != null, nameof(comment) + " != null");
            return comment;
        }
        
        private static bool IsNumber(string inputKey)
        {
            return int.TryParse(inputKey, out _);
        }

        private static string GetNumberOnly()
        {
            string key = default;
            while (!IsNumber(key))
            {
                key = Console.ReadKey().KeyChar.ToString();
                Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
            }
            
            Console.WriteLine();
            
            return key;
        }
    }
}