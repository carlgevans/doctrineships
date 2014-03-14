using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EveData.Sources.ThirdParty;
using EveData.Test.Fakes;

namespace EveData.Test
{
    [TestClass]
    public class MarketDataLocationPricesApiV1Tests
    {
        private readonly MarketDataLocationPricesApiV1 marketDataLocationPricesApiV1 = new MarketDataLocationPricesApiV1(new FakeLogger());

        [TestMethod]
        public void PriceTypeStationBuy5Pc_PassValidTypeId_MatchKnownValue()
        {
            int typeId = 16229;
            int stationId = 60003760;
            double expectedPrice = 42100002.51;
            double actualPrice = 0;
            string apiUrl = "Fakes/MarketDataLocationPricesApiV1Tests_PriceTypeStationBuy5Pc_PassValidTypeId_MatchKnownValue.xml";

            actualPrice = this.marketDataLocationPricesApiV1.PriceTypeStationBuy5Pc(typeId, stationId, apiUrl);

            Assert.AreEqual(expectedPrice, actualPrice);
        }

        [TestMethod]
        public void PriceTypeStationBuy5Pc_PassRubbish_Returns0()
        {
            int typeId = 2828282;
            int stationId = 98989898;
            double expectedPrice = 0;
            double actualPrice = 1;
            string apiUrl = "Fakes/MarketDataLocationPricesApiV1Tests_PriceTypeStationBuy5Pc_PassValidTypeId_MatchKnownValue.xml";

            actualPrice = this.marketDataLocationPricesApiV1.PriceTypeStationBuy5Pc(typeId, stationId, apiUrl);

            Assert.AreEqual(expectedPrice, actualPrice);
        }

        [TestMethod]
        public void PriceTypeStationBuy5Pc_PassAnInvalidUrl_Returns0()
        {
            int typeId = 16229;
            int stationId = 60003760;
            double expectedPrice = 0;
            double actualPrice = 1;
            string apiUrl = "http://fsdfisudfo324234234isud.com/lol.xml";

            actualPrice = this.marketDataLocationPricesApiV1.PriceTypeStationBuy5Pc(typeId, stationId, apiUrl);

            Assert.AreEqual(expectedPrice, actualPrice);
        }

        [TestMethod]
        public void PriceTypeStationBuy5Pc_PassAnInvalidXmlUrl_Returns0()
        {
            int typeId = 16229;
            int stationId = 60003760;
            double expectedPrice = 0;
            double actualPrice = 1;
            string apiUrl = "https://api.eveonline.com/eve/CharacterInfo.xml.aspx";

            actualPrice = this.marketDataLocationPricesApiV1.PriceTypeStationBuy5Pc(typeId, stationId, apiUrl);

            Assert.AreEqual(expectedPrice, actualPrice);
        }

        [TestMethod]
        public void PriceTypeStationSell5Pc_PassValidTypeId_MatchKnownValue()
        {
            int typeId = 16229;
            int stationId = 60003760;
            double expectedPrice = 44684459.65;
            double actualPrice = 0;
            string apiUrl = "Fakes/MarketDataLocationPricesApiV1Tests_PriceTypeStationSell5Pc_PassValidTypeId_MatchKnownValue.xml";

            actualPrice = this.marketDataLocationPricesApiV1.PriceTypeStationSell5Pc(typeId, stationId, apiUrl);

            Assert.AreEqual(expectedPrice, actualPrice);
        }

        [TestMethod]
        public void PriceTypeStationSell5Pc_PassRubbish_Returns0()
        {
            int typeId = 2828282;
            int stationId = 98989898;
            double expectedPrice = 0;
            double actualPrice = 1;
            string apiUrl = "Fakes/MarketDataLocationPricesApiV1Tests_PriceTypeStationSell5Pc_PassValidTypeId_MatchKnownValue.xml";

            actualPrice = this.marketDataLocationPricesApiV1.PriceTypeStationSell5Pc(typeId, stationId, apiUrl);

            Assert.AreEqual(expectedPrice, actualPrice);
        }

        [TestMethod]
        public void PriceTypeStationSell5Pc_PassAnInvalidUrl_Returns0()
        {
            int typeId = 16229;
            int stationId = 60003760;
            double expectedPrice = 0;
            double actualPrice = 1;
            string apiUrl = "http://fsdfisudfo324234234isud.com/lol.xml";

            actualPrice = this.marketDataLocationPricesApiV1.PriceTypeStationSell5Pc(typeId, stationId, apiUrl);

            Assert.AreEqual(expectedPrice, actualPrice);
        }

        [TestMethod]
        public void PriceTypeStationSell5Pc_PassAnInvalidXmlUrl_Returns0()
        {
            int typeId = 16229;
            int stationId = 60003760;
            double expectedPrice = 0;
            double actualPrice = 1;
            string apiUrl = "https://api.eveonline.com/eve/CharacterInfo.xml.aspx";

            actualPrice = this.marketDataLocationPricesApiV1.PriceTypeStationSell5Pc(typeId, stationId, apiUrl);

            Assert.AreEqual(expectedPrice, actualPrice);
        }

        [TestMethod]
        public void PriceTypeStationBuy5Pc_PassValidTypeIdList_MatchKnownValue()
        {
            IDictionary<int, double> priceList;
            int stationId = 60003760;
            double expectedPrice = 12901.26;
            double actualPrice = 0;
            string apiUrl = "Fakes/MarketDataLocationPricesApiV1Tests_PriceTypeStationBuy5Pc_PassValidTypeIdList_MatchKnownValue.xml";

            // Instantiate and populate a large list of type ids.
            List<int> typeIdList = new List<int>();
            typeIdList.Add(16229);
            typeIdList.Add(16229);
            typeIdList.Add(16229); // Duplicate Ids.
            typeIdList.AddRange(Enumerable.Range(16230, 100));

            // Populate the dictionary with the XML values.
            priceList = this.marketDataLocationPricesApiV1.PriceTypeStationBuy5Pc(typeIdList, stationId, apiUrl);

            // Output the full list.
            foreach (KeyValuePair<int, double> pair in priceList)
            {
                System.Diagnostics.Debug.WriteLine("{0}, {1}", pair.Key, pair.Value);
            }

            priceList.TryGetValue(16297, out actualPrice);

            Assert.AreEqual(expectedPrice, actualPrice);
        }

        [TestMethod]
        public void PriceTypeStationSell5Pc_PassValidTypeIdList_MatchKnownValue()
        {
            IDictionary<int, double> priceList;
            int stationId = 60003760;
            double expectedPrice = 22901.00;
            double actualPrice = 0;
            string apiUrl = "Fakes/MarketDataLocationPricesApiV1Tests_PriceTypeStationSell5Pc_PassValidTypeIdList_MatchKnownValue.xml";

            // Instantiate and populate a large list of type ids.
            List<int> typeIdList = new List<int>();
            typeIdList.Add(16229);
            typeIdList.Add(16229);
            typeIdList.Add(16229); // Duplicate Ids.
            typeIdList.AddRange(Enumerable.Range(16230, 100));

            // Populate the dictionary with the XML values.
            priceList = this.marketDataLocationPricesApiV1.PriceTypeStationSell5Pc(typeIdList, stationId, apiUrl);

            // Output the full list.
            foreach (KeyValuePair<int, double> pair in priceList)
            {
                System.Diagnostics.Debug.WriteLine("{0}, {1}", pair.Key, pair.Value);
            }

            priceList.TryGetValue(16297, out actualPrice);

            Assert.AreEqual(expectedPrice, actualPrice);
        }
    }
}
