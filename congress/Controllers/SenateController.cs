using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using congress.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace congress.Controllers
{
    [ApiController]
    [Route("senate")]
    public class SenateController : ControllerBase
    {
        private readonly ILogger<SenateController> logger;
        private readonly CongressDataContext context;

        public SenateController(ILogger<SenateController> logger, CongressDataContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        [HttpGet("sessions")]
        public IEnumerable<Session> Get()
        {
            return context.Sessions.ToList();
        }

        [HttpGet("senators")]
        public async Task<IEnumerable<object>> GetSenators(int congressNumber, int sessionNumber)
        {
            if (congressNumber <= 0 || sessionNumber <= 0)
            {
                return context.Senators.ToList();
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

            // Maybe should mark senator as "current" in the db on import
            return await context.Sessions
                .Where(session => session.SessionNumber == sessionNumber && session.CongressNumber == congressNumber)
                .Join(context.LegislativeItems, s => s.Id, l => l.SessionId, (s, l) => l)
                .Join(context.Votes, l => l.Id, v => v.LegislativeItemId, (l, v) => v)
                .Join(context.Senators, v => v.SenatorId, s => s.Id, (v, s) => s)
                .GroupBy(senator => new { senator.Id, senator.LisMemberId, senator.FullName, senator.State })
                .OrderBy(g => g.Key.State)
                .Select(g => g.Key)
                .ToListAsync();
        }

        [HttpGet("currentsessionvotes")]
        public async Task<object> GetCurrentSessionVotesForSenator(string lisMemberId)
        {
            if (string.IsNullOrEmpty(lisMemberId))
            {
                // Can't seem to get HttpResponseException here
                throw new Exception("Invalid LisMemberId");
            }

            var items = await context.Sessions
                .Where(s => s.CongressNumber == 116 && s.SessionNumber == 2)
                .Join(context.LegislativeItems, s => s.Id, l => l.SessionId, (s, l) => l)
                .Join(context.Votes, l => l.Id, v => v.LegislativeItemId, (l, v) => new { LegislativeItem = l, Vote = v })
                .Join(context.Senators, v => v.Vote.SenatorId, s => s.Id, (r, s) => new { r.Vote, r.LegislativeItem, Senator = s })
                .Where(r => r.Senator.LisMemberId == lisMemberId)
                .OrderByDescending(r => r.LegislativeItem.VoteDate)
                .ToListAsync();

            return new
            {
                Senator = items.First().Senator,
                    Votes = items.Select(i => new { Vote = i.Vote, LegislativeItem = i.LegislativeItem })
            };
        }

        [HttpGet("votes")]
        public IEnumerable<object> GetVotes(int congress, int session, int legislativeItemId)
        {
            return context.Votes.Where(v => v.LegislativeItemId == legislativeItemId);
        }
    }
}