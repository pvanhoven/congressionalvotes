using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace congress.Controllers
{
    [ApiController]
    [Route("senate")]
    public class SenateController : ControllerBase
    {
        private readonly ILogger<SenateController> logger;
        private readonly IMediator mediator;

        public SenateController(ILogger<SenateController> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        [HttpGet("senators")]
        public async Task<IEnumerable<object>> GetSenators(int congressNumber, int sessionNumber)
        {
            var request = new SenatorsRequest(congressNumber, sessionNumber);
            var result = await mediator.Send(request);
            return result;
        }

        [HttpGet("currentsessionvotes")]
        public async Task<object> GetCurrentSessionVotesForSenator(string lisMemberId)
        {
            if (string.IsNullOrEmpty(lisMemberId))
            {
                // Can't seem to get HttpResponseException here
                throw new Exception("Invalid LisMemberId");
            }

            var request = new CurrentSessionVotesCommand(lisMemberId);
            var result = await mediator.Send(request);
            return result;
        }
    }
}