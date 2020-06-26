using System;

namespace congress.Model
{
    public class LegislativeItem
    {
        public int Id { get; set; }

        public string SessionId { get; set; }

        public string Title { get; set; }

        public string VoteNumber { get; set; }

        public DateTime? VoteDate { get; set; }

        public string VoteQuestionText { get; set; }

        public string VoteDocumentText { get; set; }

        public string MajorityRequirement { get; set; }

        public string DocumentCongress { get; set; }

        public string DocumentType { get; set; }

        public string DocumentNumber { get; set; }

        public string DocumentName { get; set; }

        public string DocumentTitle { get; set; }

        public string DocumentShortTitle { get; set; }

        public string AmendmentNumber { get; set; }

        public string AmendmentToDocumentNumber { get; set; }

        public string AmendmentPurpose { get; set; }

        public string Issue { get; set; }

        public string IssueLink { get; set; }

        public string Question { get; set; }

        public string Result { get; set; }

        public int YeaCount { get; set; }

        public int NayCount { get; set; }

        public int PresentCount { get; set; }

        public int AbsentCount { get; set; }

        public string TieBreakerByWhom { get; set; }

        public string TieBreakerVote { get; set; }
    }
}