using System;
using System.Dynamic;
using System.IO;
using System.Net.Http;
using System.Xml.Linq;
using Newtonsoft.Json;
using Oakton;

[Description("Download voting record xml files")]
public class DownloadCommand : OaktonCommand<Options>
{

    private const string SENATE_URL = "https://www.senate.gov/legislative/LIS/roll_call_lists/vote_menu_{0}_{1}.xml";
    private const string roll_call = "http://www.senate.gov/legislative/LIS/roll_call_votes/vote{0}{1}/vote_{0}_{1}_{2}.xml";
    private static readonly int[] congresses = new [] { 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116 };

    public DownloadCommand()
    {
        Usage("Downloads voting records to structured files");
    }

    public override bool Execute(Options input)
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string dataDir = $"{currentDirectory}/data";
        if (!Directory.Exists(dataDir))
        {
            Directory.CreateDirectory(dataDir);
        }

        HttpClient client = new HttpClient();
        foreach (var congress in input.CongressesFlag)
        {
            foreach (var session in input.SessionsFlag)
            {
                ProcessSession(client, congress, session, dataDir);
            }
        }

        return true;
    }

    public static void ProcessSession(HttpClient client, int congress, int session, string dataDir)
    {
        var response = GetSummaryDataResponse(client, congress, session);
        Console.WriteLine($"Response: {response.StatusCode}");
        string directoryName = $"{dataDir}/{congress}/{session}";
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        string summaryFileName = $"{directoryName}/summary.xml";
        CreateSummaryFile(summaryFileName, response);

        var data = GetSummaryData(summaryFileName);
        Console.WriteLine($"congression: {congress}, session: {session} data: {data}");
        foreach (var vote in data.vote_summary.votes)
        {
            ProcessVotes(vote, directoryName, congress, session, client);
        }
    }

    public static HttpResponseMessage GetSummaryDataResponse(HttpClient client, int congress, int session)
    {
        string summaryUrl = string.Format(SENATE_URL, congress, session.ToString());
        Console.WriteLine($"Summary Url: {summaryUrl}");

        var response = client.GetAsync(summaryUrl).Result;
        return response;
    }

    public static void CreateSummaryFile(string summaryFileName, HttpResponseMessage response)
    {
        using(var fileStream = File.Create(summaryFileName))
        {
            Stream responseContentStream = response.Content.ReadAsStreamAsync().Result;
            responseContentStream.Seek(0, SeekOrigin.Begin);
            responseContentStream.CopyTo(fileStream);
        }
    }

    public static void ProcessVotes(dynamic vote, string directoryName, int congress, int session, HttpClient client)
    {
        Console.WriteLine($"Vote: {vote}");
        foreach (var el in vote.Value)
        {
            var voteResponse = GetVoteResponse(congress, session, el, client);
            WriteFile(directoryName, voteResponse, el);
        }
    }

    public static dynamic GetSummaryData(string summaryFileName)
    {
        using(StreamReader reader = new StreamReader(summaryFileName))
        {
            string fileContent = reader.ReadToEnd();
            XDocument doc = XDocument.Parse(fileContent);
            string json = JsonConvert.SerializeXNode(doc);
            dynamic data = JsonConvert.DeserializeObject<ExpandoObject>(json);
            return data;
        }
    }

    public static HttpResponseMessage GetVoteResponse(int congress, int session, dynamic el, HttpClient client)
    {
        string voteUrl = string.Format(roll_call, congress, session, el.vote_number);
        Console.WriteLine($"Url: {voteUrl}");
        var voteResponse = client.GetAsync(voteUrl).Result;
        return voteResponse;
    }

    public static void WriteFile(string directoryName, HttpResponseMessage voteResponse, dynamic el)
    {
        string voteFileName = $"{directoryName}/{el.vote_number}";
        using(var fileStream = File.Create(voteFileName))
        {
            Stream responseContentStream = voteResponse.Content.ReadAsStreamAsync().Result;
            responseContentStream.Seek(0, SeekOrigin.Begin);
            responseContentStream.CopyTo(fileStream);
        }
    }
}