using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using congress;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class SenatorsHandler : IRequestHandler<SenatorsRequest, IEnumerable<object>>
{
    private readonly CongressDataContext context;

    public SenatorsHandler(CongressDataContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<object>> Handle(SenatorsRequest request, CancellationToken cancellationToken)
    {
        if (request.CongressNumber <= 0 || request.SessionNumber <= 0)
        {
            return await context.Sessions.ToListAsync();
        }

        /*
        return
            from session in context.Sessions
            join legislativeItem in context.LegislativeItems on session.Id equals legislativeItem.SessionId
            join vote in context.Votes on legislativeItem.Id equals vote.LegislativeItemId
            join senator in context.Senators on vote.SenatorId equals senator.Id
            where session.CongressNumber == congressNumber && session.SessionNumber == sessionNumber
            group senator by new { senator.Id, senator.LisMemberId, senator.State, senator.FullName, senator.LastName, senator.FirstName } into g
            orderby g.Key.State
            select g.Key;
        */

        return await context.Sessions
            .Where(session => session.SessionNumber == request.SessionNumber && session.CongressNumber == request.CongressNumber)
            .Join(context.LegislativeItems, s => s.Id, l => l.SessionId, (s, l) => l)
            .Join(context.Votes, l => l.Id, v => v.LegislativeItemId, (l, v) => v)
            .Join(context.Senators, v => v.SenatorId, s => s.Id, (v, s) => s)
            .GroupBy(senator => new { senator.Id, senator.LisMemberId, senator.FullName, senator.State })
            .OrderBy(g => g.Key.State)
            .Select(g => g.Key)
            .ToListAsync();
    }
}