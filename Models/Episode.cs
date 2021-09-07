using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVMazeChallenge.Models
{
    class Episode
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public int Season { get; set; }
        public int Number { get; set; }
        public string Type { get; set; }
        public DateTime? AirDate { get; set; }
        public DateTime? Airtime { get; set; }
        public DateTime? AirStamp { get; set; }
        public int Runtime { get; set; }
        public Images Image { get; set; }
        public string Summary { get; set; }
        public Links _Links { get; set; }
    }
}
