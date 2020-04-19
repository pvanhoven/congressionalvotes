using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

public class SenateXmlLoader
{
    public IEnumerable<SenateSession> LoadSessions()
    {
        List<SenateSession> sessions = new List<SenateSession>();
        var files = Directory.EnumerateFiles("./data");
        foreach (string file in files)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SenateSession));
            using (StreamReader reader = new StreamReader(file))
            {
                SenateSession session = (SenateSession)serializer.Deserialize(reader);
                sessions.Add(session);
            }
        }

        return sessions;
    }
}
