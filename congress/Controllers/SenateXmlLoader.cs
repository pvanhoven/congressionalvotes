using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public class SenateXmlLoader
{
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

                XmlSerializer serializer = new XmlSerializer(typeof(SenateSession));
                using(StreamReader reader = new StreamReader(summaryFileName))
                {
                    SenateSession session = (SenateSession) serializer.Deserialize(reader);
                    sessions.Add(session);
                }
            }
        }

        return sessions;
    }
}