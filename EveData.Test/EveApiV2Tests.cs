using EveData.Sources.CCP;
using EveData.Test.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EveData.Test
{
    [TestClass]
    public class EveApiV2Tests
    {
        private readonly EveApiV2 eveApiV2 = new EveApiV2(new FakeLogger());

        [TestMethod]
        public void CharacterId_PassValidName_MatchKnownValue()
        {
            string characterName = "Jasp3r";
            int expectedCharacterId = 1597747654;
            int actualCharacterId = 0;

            actualCharacterId = this.eveApiV2.EveCharacterId(characterName);

            Assert.AreEqual(expectedCharacterId, actualCharacterId);
        }

        [TestMethod]
        public void CharacterId_PassValidNameWithSpaces_MatchKnownValue()
        {
            string characterName = "Sarah Lopaz";
            int expectedCharacterId = 93177965;
            int actualCharacterId = 0;

            actualCharacterId = this.eveApiV2.EveCharacterId(characterName);

            Assert.AreEqual(expectedCharacterId, actualCharacterId);
        }

        [TestMethod]
        public void CharacterId_PassRubbish_Returns0()
        {
            string characterName = "9as8d609862w098h309f823hf092!££$£%38hf02938fh02938hf23";
            int expectedCharacterId = 0;
            int actualCharacterId = 1;

            actualCharacterId = this.eveApiV2.EveCharacterId(characterName);

            Assert.AreEqual(expectedCharacterId, actualCharacterId);
        }

        [TestMethod]
        public void CharacterId_PassNull_Returns0()
        {
            string characterName = null;
            int expectedCharacterId = 0;
            int actualCharacterId = 1;

            actualCharacterId = this.eveApiV2.EveCharacterId(characterName);

            Assert.AreEqual(expectedCharacterId, actualCharacterId);
        }

        [TestMethod]
        public void CharacterId_PassLongNumber_Returns0()
        {
            string characterName = "99999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999";
            int expectedCharacterId = 0;
            int actualCharacterId = 1;

            actualCharacterId = this.eveApiV2.EveCharacterId(characterName);

            Assert.AreEqual(expectedCharacterId, actualCharacterId);
        }

        [TestMethod]
        public void CharacterId_PassAnInvalidUrl_Returns0()
        {
            string characterName = "Jasp3r";
            string apiUrl = "http://fsdfisudfo324234234isud.com/lol.xml";
            int expectedCharacterId = 0;
            int actualCharacterId = 1;

            actualCharacterId = this.eveApiV2.EveCharacterId(characterName, apiUrl);

            Assert.AreEqual(expectedCharacterId, actualCharacterId);
        }

        [TestMethod]
        public void CharacterId_PassAnInvalidXmlUrl_Returns0()
        {
            string characterName = "Jasp3r";
            string apiUrl = "https://api.eveonline.com/eve/CharacterInfo.xml.aspx";
            int expectedCharacterId = 0;
            int actualCharacterId = 1;

            actualCharacterId = this.eveApiV2.EveCharacterId(characterName, apiUrl);

            Assert.AreEqual(expectedCharacterId, actualCharacterId);
        }
    }
}
