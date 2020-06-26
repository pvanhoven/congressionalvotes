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
        private readonly ILogger<WeatherForecastController> _logger;

        public SenateController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("sessions")]
        public IEnumerable<Session> Get()
        {
            using var context = new CongressDataContext();
            return context.Sessions.ToList();
        }

        [HttpGet("senators")]
        public IEnumerable<object> GetSenators()
        {
            using var context = new CongressDataContext();
            return context.Senators.ToList();
        }

        [HttpGet("votes")]
        public IEnumerable<object> GetVotes(int congress, int session, int legislativeItemId)
        {
            using var context = new CongressDataContext();
            return context.Votes.Where(v => v.LegislativeItemId == legislativeItemId);
        }
    }
}