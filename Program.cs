using System;
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
            
            if (File.Exists(fileName))
            {
                Console.WriteLine("File Exists!");
            }
            else
            {
                Console.WriteLine("File non-existent");
                File.Create(fileName);
            }
            
            Console.Write("Rate your mood from 0 being the lowest and 9 the highest: ");
            var currentMood = new Mood()
            {
                CurrentDate = DateTime.Now.ToShortDateString(),
                CurrentTime = DateTime.Now.ToShortTimeString(),
                CurrentRating = HelperMethods.GetNumberOnly(),
                OptionalComment = HelperMethods.GetComment()
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
                    var mood = JsonSerializer.Deserialize<Mood>(readFile.ReadLine() ?? throw new InvalidOperationException());

                    if (lastDate != mood.CurrentDate)
                    {
                        Console.Write($"{mood.CurrentDate}\n");
                        lastDate = mood.CurrentDate;
                    }
                    
                    Console.WriteLine($"\t{mood.CurrentTime} | {mood.CurrentRating} | {mood.OptionalComment}");
                }
            }
        }
    }
}