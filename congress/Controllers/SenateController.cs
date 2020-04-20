using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace congress.Controllers
{
    [ApiController]
    [Route("senate")]
    public class SenateController : ControllerBase
    {
        public static IEnumerable<SenateSession> Sessions { get; set; }

        static SenateController()
        {
            SenateXmlLoader loader = new SenateXmlLoader();
            var sessions = loader.LoadSessions();
            Sessions = sessions;
        }

        private readonly ILogger<WeatherForecastController> _logger;

        public SenateController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("sessions")]
        public IEnumerable<object> Get()
        {
            return Sessions.Select(s => new { s.Congress, s.Session, s.Year });
        }

        [HttpGet("members")]
        public object GetSenators()
        {
            return Sessions
                .OrderBy(s => s.Year)
                .ThenBy(s => s.Session)
                .Last();
        }

        [HttpGet("votes")]
        public IEnumerable<object> GetVotes(int congress, int session)
        {
            return Sessions
                .Where(s => s.Congress.ToLower() == congress.ToString() && s.Session.ToLower() == session.ToString())
                .SelectMany(s => s.Votes.VoteElements)
                .Select(v => new { v.VoteNumber, v.VoteDate, v.Result, v.VoteTally.Yeas, v.VoteTally.Nays, v.Title })
                .OrderBy(v => v.VoteNumber);
        }
    }
}