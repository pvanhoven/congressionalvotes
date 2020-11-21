using System.Xml;
using System.Xml.Serialization;

[XmlRoot("roll_call_vote")]
public class RollCallVote
{
    [XmlElement("congress")]
    public string? Congress { get; set; }

    [XmlElement("session")]
    public string? Session { get; set; }

    [XmlElement("congress_year")]
    public string? CongressYear { get; set; }

    [XmlElement("vote_number")]
    public string? VoteNumber { get; set; }

    [XmlElement("vote_date")]
    public string? VoteDate { get; set; }

    [XmlElement("modify_date")]
    public string? ModifyDate { get; set; }

    [XmlElement("vote_question_text")]
    public string? VoteQuestionText { get; set; }

    [XmlElement("vote_document_text")]
    public string? VoteDocumentText { get; set; }

    [XmlElement("vote_result_text")]
    public string? VoteResultText { get; set; }

    [XmlElement("question")]
    public string? Question { get; set; }

    [XmlElement("vote_title")]
    public string? VoteTitle { get; set; }

    [XmlElement("majority_requirement")]
    public string? MajorityRequirement { get; set; }

    [XmlElement("vote_result")]
    public string? VoteResult { get; set; }

    [XmlElement("document")]
    public Document? Document { get; set; }

    [XmlElement("amendment")]
    public Amendment? Amendment { get; set; }

    [XmlElement("count")]
    public Count? Count { get; set; }

    [XmlElement("tie_breaker")]
    public TieBreaker? TieBreaker { get; set; }

    [XmlElement("members")]
    public Members? Members { get; set; }
}

public class Document
{
    [XmlElement("document_congress")]
    public string? Congress { get; set; }

    [XmlElement("document_type")]
    public string? Type { get; set; }

    [XmlElement("document_number")]
    public string? Number { get; set; }

    [XmlElement("document_name")]
    public string? Name { get; set; }

    [XmlElement("document_title")]
    public string? Title { get; set; }

    [XmlElement("document_short_title")]
    public string? ShortTitle { get; set; }
}

public class Amendment
{
    [XmlElement("amendment_number")]
    public string? Number { get; set; }

    [XmlElement("amendment_to_amendment_number")]
    public string? ToAmendmentNumber { get; set; }

    [XmlElement("amendment_to_amendment_to_amendment_number")]
    public string? ToAmendmentToAmendmentNumber { get; set; }

    [XmlElement("amendment_to_document_number")]
    public string? ToDocumentNumber { get; set; }

    [XmlElement("amendment_to_document_short_title")]
    public string? ToDocumentShortTitle { get; set; }

    [XmlElement("amendment_purpose")]
    public string? Purpose { get; set; }
}

public class Count
{
    [XmlElement("yeas")]
    public string? Yeas { get; set; }

    [XmlElement("nays")]
    public string? Nays { get; set; }

    [XmlElement("present")]
    public string? Present { get; set; }

    [XmlElement("absent")]
    public string? Absent { get; set; }
}

public class TieBreaker
{
    [XmlElement("by_whom")]
    public string? ByWhom { get; set; }

    [XmlElement("tie_breaker_vote")]
    public string? TieBreakerVote { get; set; }
}

[XmlRoot("members")]
public class Members
{
    [XmlElement("member")]
    public Member[]? MemberElements { get; set; }
}

public class Member
{
    [XmlElement("member_full")]
    public string? FullName { get; set; }

    [XmlElement("last_name")]
    public string? LastName { get; set; }

    [XmlElement("first_name")]
    public string? FirstName { get; set; }

    [XmlElement("party")]
    public string? Party { get; set; }

    [XmlElement("state")]
    public string? State { get; set; }

    [XmlElement("vote_cast")]
    public string? VoteCast { get; set; }

    [XmlElement("lis_member_id")]
    public string? LisMemberId { get; set; }
}