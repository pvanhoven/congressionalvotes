using System.Collections.Generic;
using MediatR;

public class SenatorsRequest : IRequest<IEnumerable<object>>
{
    public int SessionNumber { get; private set; }

    public int CongressNumber { get; private set; }

    public SenatorsRequest(int congressNumber, int sessionNumber)
    {
        CongressNumber = congressNumber;
        SessionNumber = sessionNumber;
    }
}