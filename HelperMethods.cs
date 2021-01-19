using System;
using System.Diagnostics;

namespace MoodLog
{
    public static class HelperMethods
    {
        public static string GetComment()
        {
            Console.WriteLine("Any comments regarding your current mood?");
            var comment = Console.ReadLine();
            Debug.Assert(comment != null, nameof(comment) + " != null");
            comment = comment.Replace('\"', '\'');
            return comment;
        }
        public static string GetNumberOnly()
        {
            ConsoleKeyInfo key = default;
            while (!IsNumber(key.Key))
            {
                key = Console.ReadKey();
                Console.WriteLine();
            }
            return key.KeyChar.ToString();
        }
        
        private static bool IsNumber(ConsoleKey inputKey)
        {
            return ((inputKey >= ConsoleKey.D0 && inputKey <= ConsoleKey.D9) ||
                    (inputKey >= ConsoleKey.NumPad0 && inputKey <= ConsoleKey.NumPad9));
        }
    }
}