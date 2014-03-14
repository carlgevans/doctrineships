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
    internal sealed class CharacterApiV2
    {
        private readonly ISystemLogger logger;

        internal CharacterApiV2(ISystemLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Populate a list of contracts for the character passed.
        /// </summary>
        /// <param name="characterID">The character id.</param>
        /// <param name="apiId">A valid eve api id (keyID).</param>
        /// <param name="apiKey">A valid eve api key (vCode).</param>
        /// <param name="apiUrl">An optional apiUrl parameter that overrides the method's internal value. Primarily used in unit tests to force errors.</param>
        /// <returns>Returns a populated list of IEveDataContract objects relating to the character.</returns>
        internal IEnumerable<IEveDataContract> CharacterContracts(int characterID, int apiId, string apiKey, string apiUrl = "")
        {
            XDocument xmlDoc;
            IEnumerable<IEveDataContract> xmlValues = null;

            // Assign the Api Url to a string if it has not already been passed as a parameter.
            if (apiUrl == string.Empty)
            {
                apiUrl = "https://api.eveonline.com/char/Contracts.xml.aspx?keyID=" + apiId + "&vCode=" + apiKey + "&characterID=" + characterID;
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
                logger.LogMessage("An error occured while parsing the CCP CharacterContracts Xml for ApiID: " + apiId + ". Is the data source present and correct?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Net.WebException e)
            {
                logger.LogMessage("An error occured while parsing the CCP CharacterContracts Xml for ApiID: " + apiId + ". Is the data source present and correct?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Xml.XmlException e)
            {
                logger.LogMessage("An error occured while loading the CCP CharacterContracts Xml for ApiID: " + apiId + ". Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception)
            {
                throw;
            }

            return xmlValues;
        }

        /// <summary>
        /// Populate a list of contracts items for the character and contract id passed.
        /// </summary>
        /// <param name="characterID">The character id.</param>
        /// <param name="apiId">A valid eve api id (keyID).</param>
        /// <param name="apiKey">A valid eve api key (vCode).</param>
        /// <param name="contractId">An existing contract id.</param>
        /// <param name="apiUrl">An optional apiUrl parameter that overrides the method's internal value. Primarily used in unit tests to force errors.</param>
        /// <returns>Returns a populated list of IEveDataContractItem objects relating to the character and contract id.</returns>
        internal IEnumerable<IEveDataContractItem> CharacterContractItems(int characterID, int apiId, string apiKey, long contractId, string apiUrl = "")
        {
            XDocument xmlDoc;
            IEnumerable<IEveDataContractItem> xmlValues = null;

            // Assign the Api Url to a string if it has not already been passed as a parameter.
            if (apiUrl == string.Empty)
            {
                apiUrl = "https://api.eveonline.com/char/ContractItems.xml.aspx?keyID=" + apiId + "&vCode=" + apiKey + "&characterID=" + characterID + "&contractID=" + contractId;
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
                logger.LogMessage("An error occured while parsing the CCP CharacterContractItems Xml for ApiID: " + apiId + ". ContractId: " + contractId + ". Is the data source present and correct?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Net.WebException e)
            {
                logger.LogMessage("An error occured while parsing the CCP CharacterContractItems Xml for ApiID: " + apiId + ". ContractId: " + contractId + ". Is the data source present and correct?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Xml.XmlException e)
            {
                logger.LogMessage("An error occured while loading the CCP CharacterContractItems Xml for ApiID: " + apiId + ". ContractId: " + contractId + ". Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
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
