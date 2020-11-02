using System;
using congress.Controllers;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class SenateControllerTests
{
    [Fact]
    public void GetCurrentSessionVotesForSenatorThrowsExceptionForMissingLisMemberId()
    {
        var loggerMock = new Mock<ILogger<SenateController>>();
        var mediatorMock = new Mock<IMediator>();
        SenateController controller = new SenateController(loggerMock.Object, null);

        bool exceptionThrown = false;
        try
        {
            var ignoreVariable = controller.GetCurrentSessionVotesForSenator(null).Result;
        }
        catch (Exception)
        {
            exceptionThrown = true;
        }

        Assert.True(exceptionThrown);
    }
}