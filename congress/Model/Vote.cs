namespace congress.Model
{
    public class Vote
    {
        public int Id { get; set; }

        public int SenatorId { get; set; }

        public int LegislativeItemId { get; set; }

        public string VoteCast { get; set; }
    }
}