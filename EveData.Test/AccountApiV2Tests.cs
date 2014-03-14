using EveData.Entities;
using EveData.Sources.CCP;
using EveData.Test.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EveData.Test
{
    [TestClass]
    public class AccountApiV2Tests
    {
        private readonly AccountApiV2 accountApiV2 = new AccountApiV2(new FakeLogger());

        [TestMethod]
        public void ApiKeyInfo_PassValidApiKey_MatchKnownValues()
        {
            int apiId = 0;
            string apiKey = "";
            string apiUrl = "Fakes/AccountApiV2Tests_ApiKeyInfo_PassValidApiKey_MatchKnownValues.xml";
            IEveDataApiKey apiKeyInfo;

            int expectedAccessMask = 67108864;

            apiKeyInfo = this.accountApiV2.ApiKeyInfo(apiId, apiKey, apiUrl);

            System.Diagnostics.Debug.WriteLine("Access Mask: " + apiKeyInfo.AccessMask);
            System.Diagnostics.Debug.WriteLine("Expires: " + apiKeyInfo.Expires);
            System.Diagnostics.Debug.WriteLine("Type: " + apiKeyInfo.Type);

            foreach (var character in apiKeyInfo.Characters)
            {
                System.Diagnostics.Debug.WriteLine("---------------------------------------------------");
                System.Diagnostics.Debug.WriteLine("CharacterId:\t\t" + character.CharacterId);
                System.Diagnostics.Debug.WriteLine("CharacterName:\t\t" + character.CharacterName);
                System.Diagnostics.Debug.WriteLine("CorporationId:\t\t" + character.CorporationId);
                System.Diagnostics.Debug.WriteLine("CorporationName:\t" + character.CorporationName);
                System.Diagnostics.Debug.WriteLine("---------------------------------------------------");
            }

            Assert.AreEqual(expectedAccessMask, apiKeyInfo.AccessMask);
        }

        [TestMethod]
        public void ApiKeyInfo_PassInvalidApiKey_ReturnsNull()
        {
            int apiId = 0;
            string apiKey = "";
            string apiUrl = "Fakes/AccountApiV2Tests_ApiKeyInfo_PassInvalidApiKey_ReturnsNull.xml";
            IEveDataApiKey apiKeyInfo;

            apiKeyInfo = this.accountApiV2.ApiKeyInfo(apiId, apiKey, apiUrl);

            Assert.IsNull(apiKeyInfo);
        }
    }
}
