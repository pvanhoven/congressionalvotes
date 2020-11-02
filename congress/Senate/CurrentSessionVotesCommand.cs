using MediatR;

public class CurrentSessionVotesCommand : IRequest<object>
{
    public string LisMemberID { get; private set; }

    public CurrentSessionVotesCommand(string lisMemberID)
    {
        LisMemberID = lisMemberID;
    }
}