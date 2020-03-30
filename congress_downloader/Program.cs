using System;
using System.IO;
using System.Net.Http;

namespace congress_downloader
{
    class Program
    {
        const string SENATE_URL = "https://www.senate.gov/legislative/LIS/roll_call_lists/vote_menu_{0}_{1}.xml";
        const string roll_call = "http://www.senate.gov/legislative/LIS/roll_call_votes/vote{0}{1}/vote_{0}_{1}_{2}.xml";

        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            var congresses = new [] {100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116};
            var sessions = new[] { 1, 2 };
            string currentDirectory = Directory.GetCurrentDirectory();
            string dataDir = $"{currentDirectory}/data";
            if (!Directory.Exists(dataDir)){
                Directory.CreateDirectory(dataDir);
            }

            HttpClient client = new HttpClient();
            foreach (var congress in congresses)
            {
                foreach (var session in sessions)
                {
                    string url = string.Format(SENATE_URL, congress, session.ToString());
                    Console.WriteLine($"Url: {url}");

                    var response = client.GetAsync(url).Result;
                    Console.WriteLine($"Response: {response.StatusCode}");
                    string fileName = $"{dataDir}/{congress}_{session}";

                    using(var fileStream = File.Create(fileName)){
                        Stream responseContentStream = response.Content.ReadAsStreamAsync().Result;
                        responseContentStream.Seek(0, SeekOrigin.Begin);
                        responseContentStream.CopyTo(fileStream);
                    }
                }
            }
        }
    }
}
