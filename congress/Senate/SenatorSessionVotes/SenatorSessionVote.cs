using System.Collections.Generic;
using congress.Model;

public record SenatorSessionVote(Senator Senator, IEnumerable<SenatorVote> Votes);

public record SenatorVote(LegislativeItem LegislativeItem, congress.Model.Vote Vote);