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
        public IEnumerable<object> GetSenators()
        {
            return context.Senators.ToList();
        }

        [HttpGet("votes")]
        public IEnumerable<object> GetVotes(int congress, int session, int legislativeItemId)
        {
            return context.Votes.Where(v => v.LegislativeItemId == legislativeItemId);
        }
    }
}