using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using congress;
using congress.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class SenatorsHomeCommand : IRequest<SenatorsHomeResult> { }

public class SenatorsHomeHandler : IRequestHandler<SenatorsHomeCommand, SenatorsHomeResult>
{
    private readonly CongressDataContext context;

    public SenatorsHomeHandler(CongressDataContext context)
    {
        this.context = context;
    }

    public async Task<SenatorsHomeResult> Handle(SenatorsHomeCommand request, CancellationToken cancellationToken)
    {
        // TODO: Would like to move this to db to reduce network hops to 1
        IEnumerable<Session> sessions = await context.Sessions
            .OrderByDescending(s => s.CongressNumber)
            .ThenByDescending(s => s.SessionNumber)
            .ToListAsync();

        if (sessions == null || !sessions.Any())
        {
            throw new Exception("Could not find any sessions");
        }

        var mostRecentSession = sessions.First();
        var currentSenators = await context.Sessions
            .Where(session => session.SessionNumber == mostRecentSession.SessionNumber && session.CongressNumber == mostRecentSession.CongressNumber)
            .Join(context.LegislativeItems, s => s.Id, l => l.SessionId, (s, l) => l)
            .Join(context.Votes, l => l.Id, v => v.LegislativeItemId, (l, v) => v)
            .Join(context.Senators, v => v.SenatorId, s => s.Id, (v, s) => s)
            .GroupBy(senator => new { senator.Id, senator.LisMemberId, senator.FullName, senator.State, senator.FirstName, senator.LastName, senator.Party })
            .OrderBy(g => g.Key.State)
            .Select(s => s.Key)
            .Select(s => new Senator
            {
                Id = s.Id,
                    LisMemberId = s.LisMemberId,
                    FullName = s.FullName,
                    State = s.State,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Party = s.Party
            })
            .ToListAsync();

        return new SenatorsHomeResult(currentSenators, mostRecentSession.Id, sessions);
    }
}