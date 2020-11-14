using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using congress;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class SenatorSessionVoteHandler : IRequestHandler<SenatorSessionVoteCommand, SenatorSessionVote>
{
    private readonly CongressDataContext context;

    public SenatorSessionVoteHandler(CongressDataContext context)
    {
        this.context = context;
    }

    public async Task<SenatorSessionVote> Handle(SenatorSessionVoteCommand request, CancellationToken cancellationToken)
    {
        var items = await context.Sessions
            .Where(s => s.Id == request.SessionId)
            .Join(context.LegislativeItems, s => s.Id, l => l.SessionId, (s, l) => l)
            .Join(context.Votes, l => l.Id, v => v.LegislativeItemId, (l, v) => new { LegislativeItem = l, Vote = v })
            .Join(context.Senators, v => v.Vote.SenatorId, s => s.Id, (r, s) => new { r.Vote, r.LegislativeItem, Senator = s })
            .Where(r => r.Senator.LisMemberId == request.LisMemberId)
            .OrderByDescending(r => r.LegislativeItem.VoteDate)
            .ToListAsync();

        return new SenatorSessionVote(
            items.First().Senator,
            items.Select(i => new SenatorVote(i.LegislativeItem, i.Vote)));
    }
}