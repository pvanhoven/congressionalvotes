using System;
using System.Linq;
using Xunit;

namespace congress.tests
{
    public class XmlLoadTests
    {
        [Fact]
        public void LoadSomeXml()
        {
            SenateXmlLoader loader = new SenateXmlLoader();
            var result = loader.LoadSessions();

            Assert.NotNull(result);
            Assert.True(result.Any());
        }

        [Fact]
        public void LoadSomeOtherXml()
        {
            SenateXmlLoader loader = new SenateXmlLoader();
            var result = loader.GetRollCallVotes("116", "2");

            Assert.NotNull(result);
            Assert.True(result.Any());
        }
    }
}
