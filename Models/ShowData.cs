using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVMazeChallenge.Models
{
    class ShowData
    {
        public ShowData()
        {
            Episodes = new List<Episode>();
        }
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }
        public List<string> Genres { get; set; }
        public string Status { get; set; }
        public int? Runtime { get; set; }
        public int? AvgRuntime { get; set; }
        public DateTime Premiered { get; set; }
        public string OfficialSite { get; set; }
        public Schedule Schedule { get; set; }
        public Rating Rating { get; set; }
        public string Weight { get; set; }
        public Network Network { get; set; }
        public WebChannel WebChannel { get; set; }
        public Country DvdCountry { get; set; }
        public Externals Externals { get; set; }
        public Images Images { get; set; }
        public string Summary { get; set; }
        public string Updated { get; set; }
        public Links _Links { get; set; }
        public PreviousEpisode PerviousEpisode { get; set; }
        public List<Episode> Episodes { get; set; }
    }
}
