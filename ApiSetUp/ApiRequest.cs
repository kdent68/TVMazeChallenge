using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TVMazeChallenge.Models;

namespace TVMazeChallenge
{
    public class ApiRequest
    {
        static HttpClient client = new HttpClient();

        internal static async Task<List<ShowData>> GetShowObjectRequest(string url)
        {

            HttpResponseMessage response = await client.GetAsync(url);
            var results = new List<ShowData>();
            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadAsStreamAsync();

                var streamReader = new StreamReader(contentStream);
                var jsonReader = new JsonTextReader(streamReader);

                JsonSerializer serializer = new JsonSerializer();

                try
                {
                    results = serializer.Deserialize<List<ShowData>>(jsonReader);
                    return results;
                }
                catch (JsonReaderException ex)
                {
                    Console.WriteLine("Invalid JSON.");
                }
            }
            return results;
        }
        internal static async Task<List<Episode>> GetEpisodeObjectRequest(string url)
        {

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadAsStreamAsync();

                var streamReader = new StreamReader(contentStream);
                var jsonReader = new JsonTextReader(streamReader);

                JsonSerializer serializer = new JsonSerializer();

                try
                {
                    return serializer.Deserialize<List<Episode>>(jsonReader);
                }
                catch (JsonReaderException)
                {
                    Console.WriteLine("Invalid JSON.");
                }
            }
            return null;
        }


    }
}
