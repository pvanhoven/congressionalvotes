using MediatR;

public class SenatorSessionVoteCommand : IRequest<SenatorSessionVote>
{
    public SenatorSessionVoteCommand(string lisMemberId, int sessionId)
    {
        LisMemberId = lisMemberId;
        SessionId = sessionId;
    }

    public string LisMemberId { get; private set; }

    public int SessionId { get; private set; }
}