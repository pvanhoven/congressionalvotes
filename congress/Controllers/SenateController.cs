using System;
using System.Collections.Generic;
using System.Linq;
using congress.Model;
using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<object> GetSenators(int congressNumber, int sessionNumber)
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
            return context.Sessions
                .Where(session => session.SessionNumber == sessionNumber && session.CongressNumber == congressNumber)
                .Join(context.LegislativeItems, s => s.Id, l => l.SessionId, (s, l) => l)
                .Join(context.Votes, l => l.Id, v => v.LegislativeItemId, (l, v) => v)
                .Join(context.Senators, v => v.SenatorId, s => s.Id, (v, s) => s)
                .GroupBy(senator => new { senator.Id, senator.LisMemberId, senator.FullName, senator.State })
                .OrderBy(g => g.Key.State)
                .Select(g => g.Key)
                .ToList();
        }

        [HttpGet("votes")]
        public IEnumerable<object> GetVotes(int congress, int session, int legislativeItemId)
        {
            return context.Votes.Where(v => v.LegislativeItemId == legislativeItemId);
        }
    }
}