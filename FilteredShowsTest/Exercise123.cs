using NUnit.Framework;
using log4net;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using TVMazeChallenge.Models;
using System.Reflection;
using System.IO;

namespace TVMazeChallenge
{
    [TestFixture]
    public class ApiTests
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ApiTests));
        private static readonly ILoggerRepository repository = log4net.LogManager.GetRepository(Assembly.GetCallingAssembly());

        [OneTimeSetUp]
        public void Setup()
        {
            var fileInfo = new FileInfo(@"Log4net.config");
            log4net.Config.XmlConfigurator.Configure(repository, fileInfo);

            GetFilteredResults();
            GetEpisodes();

        }

        private List<ShowData> FilteredShows = new List<ShowData>();
        private List<ShowData> GetFilteredResults()
        {
            log.Info("Entering GetFilteredResults to call API");

            var shows = ApiRequest.GetShowObjectRequest("https://api.tvmaze.com/shows").Result;
            log.Debug("All Returned Shows Count: " + shows.Count);
            FilteredShows = shows.Where(c => c.Network?.Id == 8 && c.Genres.Contains("Drama") && c.Premiered.Year > 2012 && c.Premiered.Year < 2016).ToList();
            log.Debug("Returned Filtered Shows: " + FilteredShows);
            return FilteredShows;

            log.Info("Exiting GetFilteredResults");
        }

        private void GetEpisodes()
        {
            log.Info("Entering GetEpisodes to call API");

            foreach (ShowData show in FilteredShows)
            {

                var episodes = ApiRequest.GetEpisodeObjectRequest("https://api.tvmaze.com/shows/" + show.Id +"/episodes").Result;
                log.Debug("Episode API returned Episodes Count For Show ID " + show.Id + ": " + episodes.Count);

                foreach (Episode ep in episodes)
                {
                    show.Episodes.Add(ep);
                }

                log.Debug("Episode Count Now Added To Show Model For " + show.Id + ": " + show.Episodes.Count);
            }

            log.Info("Exiting GetEpisodes");

        }

        public int SeasonRuntime(int showId, int season)
        {
            log.Info("Entering Get Season Runtime");

            var selectedShow = FilteredShows.Where(e => e.Id == showId).FirstOrDefault();
            var selectedSeason = selectedShow.Episodes.Where(e => e.Season == season).ToList();

            int runTimes = 0;

            foreach (Episode ep in selectedSeason)
            {
                runTimes += ep.Runtime;
            }

            log.Debug("Season Runtime Calculated For " + showId + " season " + season + ": " + runTimes);
            return runTimes;

            log.Info("Exiting Get Season Runtime");
        }

        public Dictionary<int, float> GetAllShowsRuntime()
        {
            log.Info("Entering GetAllShowsRuntime");

            Dictionary<int, float> showsAvgRuntime = new Dictionary<int, float>();

            foreach (ShowData show in FilteredShows)
            {
                float totalRuntime = 0;
                float totalEpisode = 0;
                float avgRuntime = 0;
                
                totalEpisode = show.Episodes.Count;

                foreach (Episode ep in show.Episodes)
                {
                    totalRuntime += ep.Runtime;

                }
                log.Debug("Total Episodes: " + totalEpisode + "Total Runtime: " + totalRuntime + "For Show " + show.Id);

                avgRuntime = totalRuntime / totalEpisode;
                showsAvgRuntime.Add(show.Id, avgRuntime);

            }

            return showsAvgRuntime;
            log.Info("Entering GetAllShowsRuntime");
        }


        [Test]
        [TestCase (5, 138, 140, TestName = "Subset Shows Title Match")]
        public void AssertFilteredShows(int id1, int id2, int id3)
        {
            log.Info("Entering Assert Filtered Shows");

            Assert.That(FilteredShows.Count, Is.EqualTo(3));

            List<ShowData> data = new List<ShowData>();
            data.AddRange(FilteredShows);
            
            for(int i = data.Count - 1; i >=0; i--)
            {
                if (data[i].Id ==id1 || data[i].Id == id2 || data[i].Id == id3)
                {
                    data.RemoveAt(i);
                }
            }

            Assert.AreEqual(0, data.Count);
            log.Info("Exiting Assert Filtered Shows");
        }


        [Test]
        [TestCase(138, 1, 10, TestName = "Leftovers Season 1 Episode Count")]
        [TestCase(138, 2, 10, TestName = "Leftovers Season 2 Episode Count")]
        [TestCase(138, 3, 8, TestName = "Leftovers Season 3 Episode Count")]
        [TestCase(140, 1, 8, TestName = "Looking Season 1 Episode Count")]
        [TestCase(140, 2, 10, TestName = "Looking Season 2 Episode Count")]
        [TestCase(5, 1, 8, TestName = "True Detective Season 1 Episode Count")]
        [TestCase(5, 2, 8, TestName = "True Detective Season 2 Episode Count")]
        [TestCase(5, 3, 8, TestName = "True Detective Season 3 Episode Count")]
        public void SeasonEpisodeCount(int showId, int season, int episodeCount)
        {
            log.Info("Entering Season Episode Count");

            List<Episode> seasonEpisodes = new List<Episode>();
            foreach (ShowData show in FilteredShows)
            {
                if (show.Id == showId)
                {
                    seasonEpisodes = show.Episodes.Where(s => s.Season == season).ToList();
                }

                log.Debug("Show: " + showId + "Season " + season + "Episode Count: " + episodeCount);
            }

            Assert.AreEqual(episodeCount, seasonEpisodes.Count);
            log.Info("Exiting Season Episode Count");
        }

        [Test]
        [TestCase(138, 1, 600, TestName = "Leftovers Season 1 Runtime")]
        [TestCase(138, 2, 600, TestName = "Leftovers Season 2 Runtime")]
        [TestCase(138, 3, 495, TestName = "Leftovers Season 3 Runtime")]
        [TestCase(140, 1, 240, TestName = "Looking Season 1 Runtime")]
        [TestCase(140, 2, 300, TestName = "Looking Season 2 Runtime")]
        [TestCase(5, 1, 480, TestName = "True Detective Season 1 Runtime")]
        [TestCase(5, 2, 510, TestName = "True Detective Season 2 Runtime")]
        [TestCase(5, 3, 503, TestName = "True Detective Season 3 Runtime")]
        public void AssertSeasonRunTime(int showId, int season, int expectedRuntime)
        {
            log.Info("Entering Assert Season Runtime");

            var calculatedSeasonRuntime = SeasonRuntime(showId, season);
            log.Debug("Calculated Season" + season + "Runtime For " + showId + ": " + calculatedSeasonRuntime);

            Assert.AreEqual(expectedRuntime, calculatedSeasonRuntime);

            log.Info("Exiting Assert Season Runtime");
        }

        [Test]
        public void HighestAvgEpisodeRunTime()
        {
            log.Info("Entering Highest Avg Episode Runtime");

            Dictionary<int, float> allTimes = GetAllShowsRuntime();
            log.Debug("All Shows Avg Runtime: " + allTimes);

            var maxValue = allTimes.Aggregate((x, y) => x.Value > y.Value ? x : y).Value;
            log.Debug("Max Runtime Value: " + maxValue);

            var keyOfMaxValue = allTimes.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

            Assert.AreEqual(keyOfMaxValue, 5);
            Assert.AreEqual(maxValue, (float)62.2083321);

            log.Info("Exiting Highest Avg Episode Runtime");
        }



    }       
}