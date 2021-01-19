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
            
            Console.WriteLine("What's your mood, in a rating from 0(Lowest) to 9(Highest)?");
            var currentMood = new Mood()
            {
                CurrentDate = DateTime.Now.ToShortDateString(),
                CurrentTime = DateTime.Now.ToShortTimeString(),
                CurrentRating = HelperMethods.GetNumberOnly()
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
                    var deserializeTest = JsonSerializer.Deserialize<Mood>(readFile.ReadLine() ?? throw new InvalidOperationException());

                    if (lastDate != deserializeTest.CurrentDate)
                    {
                        Console.Write($"{deserializeTest.CurrentDate}\n");
                        lastDate = deserializeTest.CurrentDate;
                    }
                    Console.WriteLine($"\t{deserializeTest.CurrentTime} | {deserializeTest.CurrentRating}");
                }
            }
        }
    }
}