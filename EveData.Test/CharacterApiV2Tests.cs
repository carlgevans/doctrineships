using EveData.Entities;
using EveData.Sources.CCP;
using EveData.Test.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace EveData.Test
{
    [TestClass]
    public class CharacterApiV2Tests
    {
        private readonly CharacterApiV2 characterApiV2 = new CharacterApiV2(new FakeLogger());

        [TestMethod]
        public void CharacterContracts_PassValidXmlUrl_MatchKnownValues()
        {
            int charId = 99999999;
            int apiId = 9999999;
            string apiKey = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            string apiUrl = "Fakes/CharacterApiV2Tests_CharacterContracts_PassValidXmlFile_MatchKnownValues.xml";
            IEnumerable<IEveDataContract> contractList;

            contractList = this.characterApiV2.CharacterContracts(charId, apiId, apiKey, apiUrl);

            // The returned contract list should not be empty.
            Assert.IsNotNull(contractList);

            foreach(var contract in contractList)
            {
                System.Diagnostics.Debug.WriteLine("---------------------------------------------------");
                System.Diagnostics.Debug.WriteLine("AcceptorId:\t" + contract.AcceptorId);
                System.Diagnostics.Debug.WriteLine("AssigneeId:\t" + contract.AssigneeId);
                System.Diagnostics.Debug.WriteLine("Availability:\t" + contract.Availability);
                System.Diagnostics.Debug.WriteLine("Buyout:\t\t" + contract.Buyout);
                System.Diagnostics.Debug.WriteLine("Collateral:\t" + contract.Collateral);
                System.Diagnostics.Debug.WriteLine("ContractId:\t" + contract.ContractId);
                System.Diagnostics.Debug.WriteLine("DateAccepted:\t" + contract.DateAccepted);
                System.Diagnostics.Debug.WriteLine("DateCompleted:\t" + contract.DateCompleted);
                System.Diagnostics.Debug.WriteLine("DateExpired:\t" + contract.DateExpired);
                System.Diagnostics.Debug.WriteLine("DateIssued:\t" + contract.DateIssued);
                System.Diagnostics.Debug.WriteLine("EndStationId:\t" + contract.EndStationId);
                System.Diagnostics.Debug.WriteLine("ForCorp:\t\t" + contract.ForCorp);
                System.Diagnostics.Debug.WriteLine("IssuerCorpId:\t" + contract.IssuerCorpId);
                System.Diagnostics.Debug.WriteLine("IssuerId:\t\t" + contract.IssuerId);
                System.Diagnostics.Debug.WriteLine("NumDays:\t" + contract.NumDays);
                System.Diagnostics.Debug.WriteLine("Price:\t\t" + contract.Price);
                System.Diagnostics.Debug.WriteLine("Reward:\t\t" + contract.Reward);
                System.Diagnostics.Debug.WriteLine("StartStationId:\t" + contract.StartStationId);
                System.Diagnostics.Debug.WriteLine("Status:\t\t" + contract.Status);
                System.Diagnostics.Debug.WriteLine("Title:\t\t" + contract.Title);
                System.Diagnostics.Debug.WriteLine("Type:\t\t" + contract.Type);
                System.Diagnostics.Debug.WriteLine("Volume:\t\t" + contract.Volume);

                // All IssuerIds should match the passed character Id.
                Assert.AreEqual(contract.IssuerId, charId);
            }
        }

        [TestMethod]
        public void CharacterContracts_PassInvalidXmlUrl_EmptyList()
        {
            int charId = 99999999;
            int apiId = 9999999;
            string apiKey = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            string apiUrl = "Fakes/CharacterApiV2Tests_CharacterContracts_PassInvalidXmlFile_EmptyList.xml";
            IEnumerable<IEveDataContract> contractList;

            contractList = this.characterApiV2.CharacterContracts(charId, apiId, apiKey, apiUrl);

            // The returned contract list should be empty.
            Assert.IsFalse(contractList.Any());
        }

        [TestMethod]
        public void CharacterContractItems_PassValidXmlUrl_MatchKnownValues()
        {
            int charId = 99999999;
            int contractId = 77621068;
            int apiId = 9999999;
            string apiKey = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            string apiUrl = "Fakes/CharacterApiV2Tests_CharacterContractItems_PassValidXmlFile_MatchKnownValues.xml";
            IEnumerable<IEveDataContractItem> contractItemsList;

            contractItemsList = this.characterApiV2.CharacterContractItems(charId, apiId, apiKey, contractId, apiUrl);

            // The returned contract item list should not be empty.
            Assert.IsNotNull(contractItemsList);

            foreach (var contractItem in contractItemsList)
            {
                System.Diagnostics.Debug.WriteLine("---------------------------------------------------");
                System.Diagnostics.Debug.WriteLine("RecordId:\t" + contractItem.RecordId);
                System.Diagnostics.Debug.WriteLine("TypeId:\t\t" + contractItem.TypeId);
                System.Diagnostics.Debug.WriteLine("Quantity:\t" + contractItem.Quantity);
                System.Diagnostics.Debug.WriteLine("RawQuantity:\t" + contractItem.RawQuantity);
                System.Diagnostics.Debug.WriteLine("Singleton:\t" + contractItem.Singleton);
                System.Diagnostics.Debug.WriteLine("Included:\t" + contractItem.Included);
            }
        }

        [TestMethod]
        public void CharacterContractItems_PassInvalidXmlUrl_EmptyList()
        {
            int contractId = 77621068;
            int charId = 99999999;
            int apiId = 9999999;
            string apiKey = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            string apiUrl = "Fakes/CharacterApiV2Tests_CharacterContractItems_PassInvalidXmlFile_EmptyList.xml";
            IEnumerable<IEveDataContractItem> contractItemsList;

            contractItemsList = this.characterApiV2.CharacterContractItems(charId, apiId, apiKey, contractId, apiUrl);

            // The returned contract list should be empty.
            Assert.IsFalse(contractItemsList.Any());
        }
    }
}
