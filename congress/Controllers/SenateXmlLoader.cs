using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

public class SenateXmlLoader
{

    private readonly static XmlSerializer senateSessionSerializer = new XmlSerializer(typeof(SenateSession));
    private readonly static XmlSerializer rollCallVoteSerializer = new XmlSerializer(typeof(RollCallVote));

    public IEnumerable<SenateSession> LoadSessions()
    {
        List<SenateSession> sessions = new List<SenateSession>();
        var congressDirectories = Directory.EnumerateDirectories("./data");
        foreach (var congressDirectory in congressDirectories)
        {
            var sessionDirectories = Directory.EnumerateDirectories(congressDirectory);
            foreach (var sessionDirectory in sessionDirectories)
            {
                string summaryFileName = $"{sessionDirectory}/summary.xml";
                if (!File.Exists(summaryFileName))
                {
                    continue;
                }

                using(StreamReader reader = new StreamReader(summaryFileName))
                {
                    SenateSession session = (SenateSession) senateSessionSerializer.Deserialize(reader);
                    sessions.Add(session);
                }
            }
        }

        return sessions;
    }

    public IEnumerable<RollCallVote> GetRollCallVotes(string congress, string session)
    {
        List<RollCallVote> rollCallVotes = new List<RollCallVote>();
        DirectoryInfo dataDirectory = new DirectoryInfo("./data");
        var congressDirectories = dataDirectory.EnumerateDirectories();
        var matchingCongressDirectory = congressDirectories.FirstOrDefault(d => d.Name.Equals(congress, StringComparison.InvariantCultureIgnoreCase));
        if (matchingCongressDirectory == null)
        {
            return default(IEnumerable<RollCallVote>);
        }

        var sessionDirectories = matchingCongressDirectory.GetDirectories();
        var matchingSessionDirectory = sessionDirectories.FirstOrDefault(d => d.Name.Equals(session, StringComparison.InvariantCultureIgnoreCase));
        if (matchingSessionDirectory == null)
        {
            return default(IEnumerable<RollCallVote>);
        }

        var rollCallVoteFiles = Directory.EnumerateFiles(matchingSessionDirectory.FullName)
            .Where(f => !f.EndsWith("summary.xml"));
        foreach (var rollCallVoteFile in rollCallVoteFiles)
        {
            using(StreamReader reader = new StreamReader(rollCallVoteFile))
            {
                RollCallVote vote = (RollCallVote) rollCallVoteSerializer.Deserialize(reader);
                rollCallVotes.Add(vote);
            }
        }

        return rollCallVotes;
    }
}