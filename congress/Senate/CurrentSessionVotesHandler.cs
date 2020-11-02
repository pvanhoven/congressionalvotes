using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using congress;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class CurrentSessionVotesHandler : IRequestHandler<CurrentSessionVotesCommand, object>
{
    private readonly CongressDataContext context;

    public CurrentSessionVotesHandler(CongressDataContext context)
    {
        this.context = context;
    }

    public async Task<object> Handle(CurrentSessionVotesCommand request, CancellationToken cancellationToken)
    {
        var items = await context.Sessions
            .Where(s => s.CongressNumber == 116 && s.SessionNumber == 2)
            .Join(context.LegislativeItems, s => s.Id, l => l.SessionId, (s, l) => l)
            .Join(context.Votes, l => l.Id, v => v.LegislativeItemId, (l, v) => new { LegislativeItem = l, Vote = v })
            .Join(context.Senators, v => v.Vote.SenatorId, s => s.Id, (r, s) => new { r.Vote, r.LegislativeItem, Senator = s })
            .Where(r => r.Senator.LisMemberId == request.LisMemberID)
            .OrderByDescending(r => r.LegislativeItem.VoteDate)
            .ToListAsync();

        return new
        {
            Senator = items.First().Senator,
                Votes = items.Select(i => new { Vote = i.Vote, LegislativeItem = i.LegislativeItem })
        };
    }
}