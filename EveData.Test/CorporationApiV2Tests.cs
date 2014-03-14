using System.Collections.Generic;
using EveData.Entities;
using EveData.Sources.CCP;
using EveData.Test.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EveData.Test
{
    [TestClass]
    public class CorporationApiV2Tests
    {
        private readonly CorporationApiV2 corporationApiV2 = new CorporationApiV2(new FakeLogger());

        [TestMethod]
        public void WalletJournal_PassValidXmlUrl_MatchKnownValues()
        {
            int corpApiId = 999999;
            string corpApiKey = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            int accountKey = 1003;
            IEnumerable<IEveDataWalletEntry> walletEntries;

            walletEntries = this.corporationApiV2.WalletJournal(corpApiId, corpApiKey, accountKey);

            // The returned contract list should not be empty.
            Assert.IsNotNull(walletEntries);

            foreach (var walletEntry in walletEntries)
            {
                System.Diagnostics.Debug.WriteLine("---------------------------------------------------");
                System.Diagnostics.Debug.WriteLine("Amount:\t" + walletEntry.Amount);
                System.Diagnostics.Debug.WriteLine("ArgId1:\t" + walletEntry.ArgId1);
                System.Diagnostics.Debug.WriteLine("ArgName1:\t" + walletEntry.ArgName1);
                System.Diagnostics.Debug.WriteLine("Balance:\t" + walletEntry.Balance);
                System.Diagnostics.Debug.WriteLine("Date:\t" + walletEntry.Date);
                System.Diagnostics.Debug.WriteLine("OwnerId1:\t" + walletEntry.OwnerId1);
                System.Diagnostics.Debug.WriteLine("OwnerId2:\t" + walletEntry.OwnerId2);
                System.Diagnostics.Debug.WriteLine("OwnerName1:\t" + walletEntry.OwnerName1);
                System.Diagnostics.Debug.WriteLine("OwnerName2:\t" + walletEntry.OwnerName2);
                System.Diagnostics.Debug.WriteLine("Reason:\t" + walletEntry.Reason);
                System.Diagnostics.Debug.WriteLine("RefId:\t" + walletEntry.RefId);
                System.Diagnostics.Debug.WriteLine("RefTypeId:\t" + walletEntry.RefTypeId);
                System.Diagnostics.Debug.WriteLine("TaxAmount:\t" + walletEntry.TaxAmount);
                System.Diagnostics.Debug.WriteLine("TaxReceiverId:\t" + walletEntry.TaxReceiverId);
            }
        }
    }
}
