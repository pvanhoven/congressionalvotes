using System.Xml;
using System.Xml.Serialization;

[XmlRoot("vote_summary")]
public class SenateSession
{
    [XmlElement("congress")]
    public string Congress { get; set; }

    [XmlElement("session")]
    public string Session { get; set; }

    [XmlElement("congress_year")]
    public string Year { get; set; }

    [XmlElement("votes")]
    public Votes Votes { get; set; }
}

[XmlRoot("votes")]
public class Votes
{
    [XmlElement("vote")]
    public Vote[] VoteElements { get; set; }
}
public class Vote
{
    [XmlElement("vote_number")]
    public string VoteNumber { get; set; }

    [XmlElement("vote_date")]
    public string VoteDate { get; set; }

    // <issue><A HREF="http://thomas.loc.gov/cgi-bin/bdquery/z?d105:HR02264:">H.R. 2264</A></issue>
    [XmlElement("issue")]
    public object[] Issue { get; set; }

    // Xml may look like the below. Object[] is easiest way to make this work until it can be parsed better
    // The nested element after text content makes it weird
    // <question>On the Motion to Table <measure>S.Amdt. 1188</measure></question>
    [XmlElement("question")]
    public object[] Question { get; set; }

    [XmlElement("result")]
    public string Result { get; set; }

    [XmlElement("vote_tally")]
    public VoteTally VoteTally { get; set; }

    [XmlElement("title")]
    public string Title { get; set; }
}

public class VoteTally
{
    [XmlElement("yeas")]
    public string YeasString { get; set; }

    [XmlIgnore]
    public int? Yeas => int.TryParse(YeasString, out int parsedYeas) ? parsedYeas : (int?) null;

    [XmlElement("nays")]
    public string NaysString { get; set; }

    [XmlIgnore]
    public int? Nays => int.TryParse(NaysString, out int parsedNays) ? parsedNays : (int?) null;
}