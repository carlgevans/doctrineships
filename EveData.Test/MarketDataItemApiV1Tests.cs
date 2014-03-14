using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EveData.Sources.ThirdParty;
using EveData.Test.Fakes;

namespace EveData.Test
{
    [TestClass]
    public class MarketDataItemApiV1Tests
    {
        private readonly MarketDataItemApiV1 marketDataItemApiV1 = new MarketDataItemApiV1(new FakeLogger());

        [TestMethod]
        public void TypeId_PassValidTypeName_MatchKnownValue()
        {
            string typeName = "Veldspar";
            int expectedTypeId = 1230;
            int actualTypeId = 0;

            actualTypeId = this.marketDataItemApiV1.TypeId(typeName);

            Assert.AreEqual(expectedTypeId, actualTypeId);
        }

        [TestMethod]
        public void TypeId_PassRubbish_Returns0()
        {
            string typeName = "RubbishRubbishTypeName";
            int expectedTypeId = 0;
            int actualTypeId = 1;

            actualTypeId = this.marketDataItemApiV1.TypeId(typeName);

            Assert.AreEqual(expectedTypeId, actualTypeId);
        }

        [TestMethod]
        public void TypeId_PassAnInvalidUrl_Returns0()
        {
            string typeName = "Veldspar";
            int expectedTypeId = 0;
            int actualTypeId = 1;
            string apiUrl = "http://fsdfisudfo324234234isud.com/lol.xml";

            actualTypeId = this.marketDataItemApiV1.TypeId(typeName, apiUrl);

            Assert.AreEqual(expectedTypeId, actualTypeId);
        }

        [TestMethod]
        public void TypeId_PassAnInvalidXmlUrl_Returns0()
        {
            string typeName = "Veldspar";
            int expectedTypeId = 0;
            int actualTypeId = 1;
            string apiUrl = "https://api.eveonline.com/eve/CharacterInfo.xml.aspx";

            actualTypeId = this.marketDataItemApiV1.TypeId(typeName, apiUrl);

            Assert.AreEqual(expectedTypeId, actualTypeId);
        }

        [TestMethod]
        public void TypeName_PassValidTypeId_MatchKnownValue()
        {
            int typeId = 1230;
            string expectedTypeName = "Veldspar";
            string actualTypeName = "";

            actualTypeName = this.marketDataItemApiV1.TypeName(typeId);

            Assert.AreEqual(expectedTypeName, actualTypeName);
        }

        [TestMethod]
        public void TypeName_PassRubbish_ReturnsUnknown()
        {
            int typeId = 9898989;
            string expectedTypeName = "Unknown";
            string actualTypeName = "";

            actualTypeName = this.marketDataItemApiV1.TypeName(typeId);

            Assert.AreEqual(expectedTypeName, actualTypeName);
        }

        [TestMethod]
        public void TypeName_PassAnInvalidUrl_ReturnsUnknown()
        {
            int typeId = 1230;
            string expectedTypeName = "Unknown";
            string actualTypeName = "";
            string apiUrl = "http://fsdfisudfo324234234isud.com/lol.xml";

            actualTypeName = this.marketDataItemApiV1.TypeName(typeId, apiUrl);

            Assert.AreEqual(expectedTypeName, actualTypeName);
        }

        [TestMethod]
        public void TypeName_PassAnInvalidXmlUrl_ReturnsUnknown()
        {
            int typeId = 1230;
            string expectedTypeName = "Unknown";
            string actualTypeName = "";
            string apiUrl = "https://api.eveonline.com/eve/CharacterInfo.xml.aspx";

            actualTypeName = this.marketDataItemApiV1.TypeName(typeId, apiUrl);

            Assert.AreEqual(expectedTypeName, actualTypeName);
        }

        [TestMethod]
        public void TypeVolume_PassValidTypeId_MatchKnownValue()
        {
            int typeId = 16229;
            double expectedTypeVolume = 15000;
            double actualTypeVolume = 0;

            actualTypeVolume = this.marketDataItemApiV1.TypeVolume(typeId);

            Assert.AreEqual(expectedTypeVolume, actualTypeVolume);
        }

        [TestMethod]
        public void TypeVolume_PassRubbish_Returns0()
        {
            int typeId = 9898989;
            double expectedTypeVolume = 0;
            double actualTypeVolume = 1;

            actualTypeVolume = this.marketDataItemApiV1.TypeVolume(typeId);

            Assert.AreEqual(expectedTypeVolume, actualTypeVolume);
        }

        [TestMethod]
        public void TypeVolume_PassAnInvalidUrl_Returns0()
        {
            int typeId = 16229;
            double expectedTypeVolume = 0;
            double actualTypeVolume = 1;
            string apiUrl = "http://fsdfisudfo324234234isud.com/lol.xml";
         
            actualTypeVolume = this.marketDataItemApiV1.TypeVolume(typeId, apiUrl);

            Assert.AreEqual(expectedTypeVolume, actualTypeVolume);
        }

        [TestMethod]
        public void TypeVolume_PassAnInvalidXmlUrl_Returns0()
        {
            int typeId = 16229;
            double expectedTypeVolume = 0;
            double actualTypeVolume = 1;
            string apiUrl = "https://api.eveonline.com/eve/CharacterInfo.xml.aspx";

            actualTypeVolume = this.marketDataItemApiV1.TypeVolume(typeId, apiUrl);

            Assert.AreEqual(expectedTypeVolume, actualTypeVolume);
        }
    }
}
