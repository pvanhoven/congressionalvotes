using System;
using Xunit;

public class HealthCheckControllerTests
{
    [Fact]
    public void GetCurrentUtcDateReturnsCurrentTime(){
        HealthCheckController controller = new HealthCheckController();

        DateTime heartbeatDateTime = controller.GetCurrentUtcDate();

        // Ensure current time is close enough
        DateTime currentTime = DateTime.UtcNow;
        Assert.True(heartbeatDateTime < currentTime.AddMinutes(30));
        Assert.True(heartbeatDateTime > currentTime.AddMinutes(-30));
    }
}