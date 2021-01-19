using System;

namespace MoodLog
{
    [Serializable]
    public class Mood
    {
        public string CurrentDate { get; set; }
        public string CurrentTime { get; set; }
        public string CurrentRating { get; set; }
        
        public Mood()
        {
            
        }
    }
}