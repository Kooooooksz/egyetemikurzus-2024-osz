using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IX0WHB.Models
{
    internal class TeamStats
    {
        public int Position { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public int Played { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int Scored { get; set; }
        public int Conceded { get; set; }
        public int Points { get; set; }
    }

}

