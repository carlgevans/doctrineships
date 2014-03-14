using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EveData.Sources.ThirdParty;
using EveData.Test.Fakes;

namespace EveData.Test
{
    [TestClass]
    public class MarketDataSolarSystemApiV1Tests
    {
        private readonly MarketDataSolarSystemApiV1 marketDataSolarSystemApiV1 = new MarketDataSolarSystemApiV1(new FakeLogger());

        [TestMethod]
        public void SolarSystemName_PassValidStationId_MatchKnownValue()
        {
            int solarSystemId = 30000142;
            string expectedSolarSystemName = "Jita";
            string actualSolarSystemName = "";

            actualSolarSystemName = this.marketDataSolarSystemApiV1.SolarSystemName(solarSystemId);

            Assert.AreEqual(expectedSolarSystemName, actualSolarSystemName);
        }

        [TestMethod]
        public void SolarSystemName_PassRubbish_ReturnsUnknown()
        {
            int solarSystemId = 98989898;
            string expectedSolarSystemName = "Unknown";
            string actualSolarSystemName = "";

            actualSolarSystemName = this.marketDataSolarSystemApiV1.SolarSystemName(solarSystemId);

            Assert.AreEqual(expectedSolarSystemName, actualSolarSystemName);
        }

        [TestMethod]
        public void SolarSystemName_PassAnInvalidXmlUrl_ReturnsUnknown()
        {
            int solarSystemId = 30000142;
            string expectedSolarSystemName = "Unknown";
            string actualSolarSystemName = "";
            string apiUrl = "https://api.eveonline.com/eve/CharacterInfo.xml.aspx";

            actualSolarSystemName = this.marketDataSolarSystemApiV1.SolarSystemName(solarSystemId, apiUrl);

            Assert.AreEqual(expectedSolarSystemName, actualSolarSystemName);
        }

        [TestMethod]
        public void SolarSystemName_PassAnInvalidUrl_ReturnsUnknown()
        {
            int solarSystemId = 30000142;
            string expectedSolarSystemName = "Unknown";
            string actualSolarSystemName = "";
            string apiUrl = "http://fsdfisudfo324234234isud.com/lol.xml";

            actualSolarSystemName = this.marketDataSolarSystemApiV1.SolarSystemName(solarSystemId, apiUrl);

            Assert.AreEqual(expectedSolarSystemName, actualSolarSystemName);
        }
    }
}
