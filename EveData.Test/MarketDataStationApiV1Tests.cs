using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EveData.Sources.ThirdParty;
using EveData.Test.Fakes;

namespace EveData.Test
{
    [TestClass]
    public class MarketDataStationApiV1Tests
    {
        private readonly MarketDataStationApiV1 marketDataStationApiV1 = new MarketDataStationApiV1(new FakeLogger());

        [TestMethod]
        public void StationSolarSystemId_PassValidStationId_MatchKnownValue()
        {
            int stationId = 60003760;
            int expectedSolarSystemId = 30000142;
            int actualSolarSystemId = 0;

            actualSolarSystemId = this.marketDataStationApiV1.StationSolarSystemId(stationId);

            Assert.AreEqual(expectedSolarSystemId, actualSolarSystemId);
        }

        [TestMethod]
        public void StationSolarSystemId_PassRubbish_Returns0()
        {
            int stationId = 98989898;
            int expectedSolarSystemId = 0;
            int actualSolarSystemId = 1;

            actualSolarSystemId = this.marketDataStationApiV1.StationSolarSystemId(stationId);

            Assert.AreEqual(expectedSolarSystemId, actualSolarSystemId);
        }

        [TestMethod]
        public void StationSolarSystemId_PassAnInvalidUrl_Returns0()
        {
            int stationId = 60003760;
            int expectedSolarSystemId = 0;
            int actualSolarSystemId = 1;
            string apiUrl = "http://fsdfisudfo324234234isud.com/lol.xml";

            actualSolarSystemId = this.marketDataStationApiV1.StationSolarSystemId(stationId, apiUrl);

            Assert.AreEqual(expectedSolarSystemId, actualSolarSystemId);
        }

        [TestMethod]
        public void StationSolarSystemId_PassAnInvalidXmlUrl_Returns0()
        {
            int stationId = 60003760;
            int expectedSolarSystemId = 0;
            int actualSolarSystemId = 1;
            string apiUrl = "https://api.eveonline.com/eve/CharacterInfo.xml.aspx";

            actualSolarSystemId = this.marketDataStationApiV1.StationSolarSystemId(stationId, apiUrl);

            Assert.AreEqual(expectedSolarSystemId, actualSolarSystemId);
        }

        [TestMethod]
        public void StationName_PassValidStationId_MatchKnownValue()
        {
            int stationId = 60003760;
            string expectedStationName = "Jita IV - Moon 4 - Caldari Navy Assembly Plant";
            string actualStationName = "";

            actualStationName = this.marketDataStationApiV1.StationName(stationId);

            Assert.AreEqual(expectedStationName, actualStationName);
        }

        [TestMethod]
        public void StationName_PassRubbish_ReturnsUnknown()
        {
            int stationId = 98989898;
            string expectedStationName = "Unknown";
            string actualStationName = "";

            actualStationName = this.marketDataStationApiV1.StationName(stationId);

            Assert.AreEqual(expectedStationName, actualStationName);
        }

        [TestMethod]
        public void StationName_PassAnInvalidXmlUrl_ReturnsUnknown()
        {
            int stationId = 60003760;
            string expectedStationName = "Unknown";
            string actualStationName = "";
            string apiUrl = "https://api.eveonline.com/eve/CharacterInfo.xml.aspx";

            actualStationName = this.marketDataStationApiV1.StationName(stationId, apiUrl);

            Assert.AreEqual(expectedStationName, actualStationName);
        }

        [TestMethod]
        public void StationName_PassAnInvalidUrl_ReturnsUnknown()
        {
            int stationId = 60003760;
            string expectedStationName = "Unknown";
            string actualStationName = "";
            string apiUrl = "http://fsdfisudfo324234234isud.com/lol.xml";

            actualStationName = this.marketDataStationApiV1.StationName(stationId, apiUrl);

            Assert.AreEqual(expectedStationName, actualStationName);
        }
    }
}
