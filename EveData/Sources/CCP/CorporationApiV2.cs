namespace EveData.Sources.CCP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using EveData.Entities;
    using Tools;

    /// <summary>
    /// Version 2 Api methods from the CCP /char/ URI location.
    /// </summary>
    internal sealed class CorporationApiV2
    {
        private readonly ISystemLogger logger;

        internal CorporationApiV2(ISystemLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Populate a list of contracts for the corporation api key passed.
        /// </summary>
        /// <param name="apiId">A valid eve corporation api id (keyID).</param>
        /// <param name="apiKey">A valid eve corporation api key (vCode).</param>
        /// <param name="apiUrl">An optional apiUrl parameter that overrides the method's internal value. Primarily used in unit tests to force errors.</param>
        /// <returns>Returns a populated list of IEveDataContract objects relating to the corporation.</returns>
        internal IEnumerable<IEveDataContract> CorporationContracts(int apiId, string apiKey, string apiUrl = "")
        {
            XDocument xmlDoc;
            IEnumerable<IEveDataContract> xmlValues = null;

            // Assign the Api Url to a string if it has not already been passed as a parameter.
            if (apiUrl == string.Empty)
            {
                apiUrl = "https://api.eveonline.com/corp/Contracts.xml.aspx?keyID=" + apiId + "&vCode=" + apiKey;
            }

            try
            {
                xmlDoc = XDocument.Load(apiUrl);

                xmlValues = xmlDoc.Descendants("row")
                            .Select(x => new EveDataContract()
                            {
                                ContractId = Conversion.StringToLong(x.Attribute("contractID").Value),
                                IssuerId = Conversion.StringToLong(x.Attribute("issuerID").Value),
                                IssuerCorpId = Conversion.StringToLong(x.Attribute("issuerCorpID").Value),
                                AssigneeId = Conversion.StringToLong(x.Attribute("assigneeID").Value),
                                AcceptorId = Conversion.StringToLong(x.Attribute("acceptorID").Value),
                                StartStationId = Conversion.StringToInt32(x.Attribute("startStationID").Value),
                                EndStationId = Conversion.StringToInt32(x.Attribute("endStationID").Value),
                                Type = Conversion.StringToEnum<EveDataContractType>(x.Attribute("type").Value, EveDataContractType.ItemExchange),
                                Status = Conversion.StringToEnum<EveDataContractStatus>(x.Attribute("status").Value, EveDataContractStatus.Deleted),
                                Title = x.Attribute("title").Value,
                                ForCorp = Conversion.StringToBool(x.Attribute("forCorp").Value, true),
                                Availability = Conversion.StringToEnum<EveDataContractAvailability>(x.Attribute("availability").Value, EveDataContractAvailability.Private),
                                DateIssued = Conversion.StringToDateTime(x.Attribute("dateIssued").Value),
                                DateExpired = Conversion.StringToDateTime(x.Attribute("dateExpired").Value),
                                DateAccepted = Conversion.StringToDateTime(x.Attribute("dateAccepted").Value),
                                NumDays = Conversion.StringToInt32(x.Attribute("numDays").Value),
                                DateCompleted = Conversion.StringToDateTime(x.Attribute("dateCompleted").Value),
                                Price = Conversion.StringToDouble(x.Attribute("price").Value),
                                Reward = Conversion.StringToDouble(x.Attribute("reward").Value),
                                Collateral = Conversion.StringToDouble(x.Attribute("collateral").Value),
                                Buyout = Conversion.StringToDouble(x.Attribute("buyout").Value),
                                Volume = Conversion.StringToDouble(x.Attribute("volume").Value)
                            })
                            .ToList();
            }
            catch (System.FormatException e)
            {
                logger.LogMessage("An error occured while parsing the CCP CorporationContracts Xml for ApiID: " + apiId + ". Is the data source present and correct?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Net.WebException e)
            {
                logger.LogMessage("An error occured while parsing the CCP CorporationContracts Xml for ApiID: " + apiId + ". Is the data source present and correct?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Xml.XmlException e)
            {
                logger.LogMessage("An error occured while loading the CCP CorporationContracts Xml for ApiID: " + apiId + ". Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception)
            {
                throw;
            }

            return xmlValues;
        }

        /// <summary>
        /// Populate a list of contracts items for the corporation api key passed.
        /// </summary>
        /// <param name="apiId">A valid eve api id (keyID).</param>
        /// <param name="apiKey">A valid eve api key (vCode).</param>
        /// <param name="contractId">An existing contract id.</param>
        /// <param name="apiUrl">An optional apiUrl parameter that overrides the method's internal value. Primarily used in unit tests to force errors.</param>
        /// <returns>Returns a populated list of IEveDataContractItem objects relating to the corporation api key and contract id.</returns>
        internal IEnumerable<IEveDataContractItem> CorporationContractItems(int apiId, string apiKey, long contractId, string apiUrl = "")
        {
            XDocument xmlDoc;
            IEnumerable<IEveDataContractItem> xmlValues = null;

            // Assign the Api Url to a string if it has not already been passed as a parameter.
            if (apiUrl == string.Empty)
            {
                apiUrl = "https://api.eveonline.com/corp/ContractItems.xml.aspx?keyID=" + apiId + "&vCode=" + apiKey + "&contractID=" + contractId;
            }

            try
            {
                xmlDoc = XDocument.Load(apiUrl);

                xmlValues = xmlDoc.Descendants("row")
                            .Select(x => new EveDataContractItem()
                            {
                                RecordId = Conversion.StringToLong((string)x.Attribute("recordID").Value ?? String.Empty),
                                TypeId = Conversion.StringToInt32((string)x.Attribute("typeID").Value ?? String.Empty),
                                Quantity = Conversion.StringToLong((string)x.Attribute("quantity").Value ?? String.Empty),
                                RawQuantity = Conversion.StringToInt32((string)x.Attribute("rawQuantity") ?? String.Empty),
                                Singleton = Conversion.StringToInt32((string)x.Attribute("singleton").Value ?? String.Empty),
                                Included = Conversion.StringToInt32((string)x.Attribute("included").Value ?? String.Empty)
                            })
                            .ToList();
            }
            catch (System.FormatException e)
            {
                logger.LogMessage("An error occured while parsing the CCP CorporationContractItems Xml for ApiID: " + apiId + ". ContractId: " + contractId + ". Is the data source present and correct?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Net.WebException e)
            {
                logger.LogMessage("An error occured while parsing the CCP CorporationContractItems Xml for ApiID: " + apiId + ". ContractId: " + contractId + ". Is the data source present and correct?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Xml.XmlException e)
            {
                logger.LogMessage("An error occured while loading the CCP CorporationContractItems Xml for ApiID: " + apiId + ". ContractId: " + contractId + ". Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception)
            {
                throw;
            }

            return xmlValues;
        }

        /// <summary>
        /// Populate a list of wallet journal entries for the corporation api key passed.
        /// </summary>
        /// <param name="apiId">A valid eve corporation api id (keyID).</param>
        /// <param name="apiKey">A valid eve corporation api key (vCode).</param>
        /// <param name="accountKey">The wallet account key (1000-1006). Defaults to 1000.</param>
        /// <param name="rowCount">The number of rows to return from the Api. Defaults to the maximum value of 2560.</param>
        /// <param name="apiUrl">An optional apiUrl parameter that overrides the method's internal value. Primarily used in unit tests to force errors.</param>
        /// <returns>Returns a populated list of IEveDataWalletJournalEntry objects relating to the corporation.</returns>
        internal IEnumerable<IEveDataWalletEntry> WalletJournal(int apiId, string apiKey, int accountKey = 1000, int rowCount = 2560, string apiUrl = "")
        {
            XDocument xmlDoc;
            IEnumerable<IEveDataWalletEntry> xmlValues = null;

            // Assign the Api Url to a string if it has not already been passed as a parameter.
            if (apiUrl == string.Empty)
            {
                apiUrl = "https://api.eveonline.com/corp/WalletJournal.xml.aspx?keyID=" + apiId + "&vCode=" + apiKey + "&accountKey=" + accountKey + "&rowCount=" + rowCount;
            }

            try
            {
                xmlDoc = XDocument.Load(apiUrl);

                xmlValues = xmlDoc.Descendants("row")
                            .Select(x => new EveDataWalletEntry()
                            {
                                Amount = Conversion.StringToDouble(x.Attribute("amount").Value),
                                ArgId1 = Conversion.StringToInt32(x.Attribute("argID1").Value),
                                ArgName1 = Conversion.StringToSafeString((string)x.Attribute("argName1") ?? String.Empty),
                                Balance = Conversion.StringToDouble(x.Attribute("balance").Value),
                                Date = Conversion.StringToDateTime(x.Attribute("date").Value),
                                OwnerId1 = Conversion.StringToInt32(x.Attribute("ownerID1").Value),
                                OwnerId2 = Conversion.StringToInt32(x.Attribute("ownerID2").Value),
                                OwnerName1 = Conversion.StringToSafeString((string)x.Attribute("ownerName1") ?? String.Empty),
                                OwnerName2 = Conversion.StringToSafeString((string)x.Attribute("ownerName2") ?? String.Empty),
                                Reason = Conversion.StringToSafeString((string)x.Attribute("reason") ?? String.Empty),
                                RefId = Conversion.StringToLong(x.Attribute("refID").Value),
                                RefTypeId = Conversion.StringToInt32(x.Attribute("refTypeID").Value),
                                TaxAmount = Conversion.StringToSafeString((string)x.Attribute("taxAmount") ?? String.Empty),
                                TaxReceiverId = Conversion.StringToSafeString((string)x.Attribute("taxReceiverID") ?? String.Empty)
                            })
                            .ToList();
            }
            catch (System.FormatException e)
            {
                logger.LogMessage("An error occured while parsing the CCP WalletJournal Xml for ApiID: " + apiId + ". Is the data source present and correct?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Net.WebException e)
            {
                logger.LogMessage("An error occured while parsing the CCP WalletJournal Xml for ApiID: " + apiId + ". Is the data source present and correct?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Xml.XmlException e)
            {
                logger.LogMessage("An error occured while loading the CCP WalletJournal Xml for ApiID: " + apiId + ". Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception)
            {
                throw;
            }

            return xmlValues;
        }
    }
}
