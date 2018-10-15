using System;

namespace WumpWeb.Models
{
    public class Score
    {
        public int ID { get; set; }
        public string Judge { get; set; }
        public string Team { get; set; }
        public int TotalScore { get; set; }
    }
}