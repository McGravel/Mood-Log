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
            // TODO: Split these files into monthly files perhaps? i.e. "02_2021.json" or something.
            var fileName = Environment.CurrentDirectory + @"\mood.json";
            
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"Tried to open {fileName} - Not found.");
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
                var lastRating = -1;

                Console.WriteLine("{0}",new string('=', 40));
                Console.WriteLine("Here's your log from the past day or so:");
                Console.WriteLine("{0}",new string('=', 40));

                while (!readFile.EndOfStream)
                {
                    var mood = JsonSerializer.Deserialize<Mood>(readFile.ReadLine());

                    var parsedDate = DateTime.Parse(mood.CurrentDate);

                    if (parsedDate <= DateTime.Today.AddDays(-2))
                    {
                        continue;
                    }
                    
                    // Perhaps this logic can be reworked now I am aware of DateTime.Parse()
                    if (lastDate != mood.CurrentDate)
                    {
                        Console.Write($"\n{mood.CurrentDate}\n");
                        lastDate = mood.CurrentDate;
                    }

                    var castRating = int.Parse(mood.CurrentRating);
                    
                    Console.WriteLine($"  {mood.CurrentTime}: {mood.CurrentRating} {RatingDifference(lastRating, castRating)} {mood.OptionalComment}");
                    
                    lastRating = castRating;
                }
            }

            Console.Write("Press a key to exit... ");
            Console.ReadKey();
        }

        // TODO: Perhaps better formatting than this.
        private static string RatingDifference(int lastRating, int newRating)
        {
            if (lastRating == newRating || lastRating < 0)
            {
                return "    ";
            }
            
            var difference = Math.Abs(lastRating - newRating);

            if (lastRating > newRating)
            {
                return "(-" + difference + ")";
            }
            else
            {
                return "(+" + difference + ")";
            }
        }
        
        private static string GetComment()
        {
            Console.WriteLine("Any comment regarding your current mood? (Press Enter to skip)\n");
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
                // Sorta hacky code to keep the cursor/caret in place whilst receiving input.
                if (Console.GetCursorPosition().Left - 1 > 0)
                {
                    // Manually move the cursor/caret back left each time an invalid char is entered.
                    Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.GetCursorPosition().Top);
                }
                else
                {
                    // The 58 is just basically the length of the text printed to the screen.
                    // So the cursor just gets nudged back to where it was before
                    // if a non-number is entered with the enter key.
                    // A bit of a hack, but it does what I want it to.
                    Console.SetCursorPosition(Console.GetCursorPosition().Left + 58, Console.GetCursorPosition().Top);
                }
            }
            
            Console.WriteLine();
            return key;
        }
    }
}