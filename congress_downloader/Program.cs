using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace congress_downloader
{
    class Program
    {
        const string SENATE_URL = "https://www.senate.gov/legislative/LIS/roll_call_lists/vote_menu_{0}_{1}.xml";
        const string roll_call = "http://www.senate.gov/legislative/LIS/roll_call_votes/vote{0}{1}/vote_{0}_{1}_{2}.xml";

        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            //var congresses = new [] { 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116 };
            var congresses = new [] { 116 };
            var sessions = new [] { 1, 2 };
            string currentDirectory = Directory.GetCurrentDirectory();
            string dataDir = $"{currentDirectory}/data";
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            HttpClient client = new HttpClient();
            foreach (var congress in congresses)
            {
                foreach (var session in sessions)
                {
                    string summaryUrl = string.Format(SENATE_URL, congress, session.ToString());
                    Console.WriteLine($"Summary Url: {summaryUrl}");

                    var response = client.GetAsync(summaryUrl).Result;
                    Console.WriteLine($"Response: {response.StatusCode}");
                    string directoryName = $"{dataDir}/{congress}/{session}";
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    string summaryFileName = $"{directoryName}/summary.xml";

                    using(var fileStream = File.Create(summaryFileName))
                    {
                        Stream responseContentStream = response.Content.ReadAsStreamAsync().Result;
                        responseContentStream.Seek(0, SeekOrigin.Begin);
                        responseContentStream.CopyTo(fileStream);
                    }

                    using(StreamReader reader = new StreamReader(summaryFileName))
                    {
                        string fileContent = reader.ReadToEnd();
                        XDocument doc = XDocument.Parse(fileContent);
                        string json = JsonConvert.SerializeXNode(doc);
                        dynamic data = JsonConvert.DeserializeObject<ExpandoObject>(json);

                        Console.WriteLine($"congression: {congress}, session: {session} data: {data}");
                        foreach (var vote in data.vote_summary.votes)
                        {
                            Console.WriteLine($"Vote: {vote}");
                            foreach (var el in vote.Value)
                            {
                                string voteUrl = string.Format(roll_call, congress, session, el.vote_number);
                                Console.WriteLine($"Url: {voteUrl}");
                                var voteResponse = client.GetAsync(voteUrl).Result;
                                string voteFileName = $"{directoryName}/{el.vote_number}";

                                using(var fileStream = File.Create(voteFileName))
                                {
                                    Stream responseContentStream = response.Content.ReadAsStreamAsync().Result;
                                    responseContentStream.Seek(0, SeekOrigin.Begin);
                                    responseContentStream.CopyTo(fileStream);
                                }

                            }
                        }
                    }

                }
            }
        }
    }
}