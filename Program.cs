﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace MoodLog
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Rate your mood from 0 being the lowest and 9 the highest: ");
            var currentMood = new Mood()
            {
                CurrentDate = DateTime.Now.ToShortDateString(),
                CurrentTime = DateTime.Now.ToShortTimeString(),
                CurrentRating = GetNumberOnly(),
                OptionalComment = GetComment()
            };
            
            var serialize = JsonSerializer.Serialize(currentMood);
            
            var fileName = AppDomain.CurrentDomain.BaseDirectory + DateTime.Today.ToString("MMM") + "_" + DateTime.Today.Year + ".json";
            
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"Tried to open {fileName} - Not found. It will now be created.");
            }
            
            using (var file = new StreamWriter(fileName, true))
            {
                file.WriteLine(serialize);
            }
            
            Console.WriteLine("{0}",new string('=', 40));
            Console.WriteLine("Here's your log from the past day or so:");
            Console.WriteLine("{0}",new string('=', 40));
            
            using (var readFile = new StreamReader(fileName))
            {
                ParseFile(readFile);
            }

            Console.Write("Press a key to exit... ");
            Console.ReadKey();
        }

        private static void ParseFile(StreamReader readFile)
        {
            string lastDate = default;
            var lastRating = -1;

            int worstRating;
            int bestRating;
            
            // Might as well use this function here too instead of repeating code?
            ResetBestAndWorst();

            // Couple of local functions to workaround how this using block works at the moment...
            void ResetBestAndWorst()
            {
                worstRating = 10;
                bestRating = -1;
            }
            
            static void PrintBestAndWorst(int worst, int best)
            {
                var ratingDifference = Math.Abs(worst - best);
                Console.WriteLine($"\nYour rating for this day was {worst} at its worst and {best} at its best.");
                Console.WriteLine($"That's a range of {ratingDifference} over the course of the day.");
                
                if (ratingDifference > 4)
                {
                    Console.WriteLine("Your mood varied a lot.");
                }
                else if (ratingDifference > 2)
                {
                    Console.WriteLine("Your mood moved around a fair bit.");
                }
                else
                {
                    Console.WriteLine("Your mood was consistent.");
                }
            }
            
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
                    if (lastDate != default)
                    {
                        PrintBestAndWorst(worstRating, bestRating);
                    }

                    Console.Write($"\n{mood.CurrentDate}\n");
                    lastDate = mood.CurrentDate;

                    // Maybe there's a better way of resetting these instead. Is a local function overkill?
                    ResetBestAndWorst();
                }

                var castRating = int.Parse(mood.CurrentRating);

                if (castRating > bestRating)
                {
                    bestRating = castRating;
                }

                if (castRating < worstRating)
                {
                    worstRating = castRating;
                }

                Console.WriteLine(
                    $"  {mood.CurrentTime}: {mood.CurrentRating} {RatingDifference(lastRating, castRating)} {mood.OptionalComment}");

                lastRating = castRating;
            }

            // Made a local function to make this workaround slightly less ugly,
            // but the logic shouldn't require this re-printing of best and worst I don't think.
            // Either way, it DOES work as intended...
            PrintBestAndWorst(worstRating, bestRating);
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