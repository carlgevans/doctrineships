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
    /// Version 2 Api methods from the CCP /account/ URI location.
    /// </summary>
    internal sealed class AccountApiV2
    {
        private readonly ISystemLogger logger;

        internal AccountApiV2(ISystemLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// This method returns information about an Api Key and the characters exposed by it.
        /// </summary>
        /// <param name="apiId">A valid eve api id (keyID).</param>
        /// <param name="apiKey">A valid eve api key (vCode).</param>
        /// <param name="apiUrl">An optional apiUrl parameter that overrides the method's internal value. Primarily used in unit tests to force errors.</param>
        /// <returns>Returns a populated IEveDataApiKey object relating to the passed Api Key, or a null object if a problem occured.</returns>
        internal IEveDataApiKey ApiKeyInfo(int apiId, string apiKey, string apiUrl = "")
        {
            XDocument xmlDoc;
            IEveDataApiKey apiKeyInfo = null;

            // Assign the Api Url to a string if it has not already been passed as a parameter.
            if (apiUrl == string.Empty)
            {
                apiUrl = "https://api.eveonline.com/account/APIKeyInfo.xml.aspx?keyID=" + apiId + "&vCode=" + apiKey;
            }

            try
            {
                xmlDoc = XDocument.Load(apiUrl);
                
                // Populate key values.
                apiKeyInfo = xmlDoc.Descendants("key")
				                .Select(x => new EveDataApiKey()
				                {
                                    AccessMask = Conversion.StringToInt32(x.Attribute("accessMask").Value),
                                    Expires = Conversion.StringToDateTime(x.Attribute("expires").Value),
                                    Type = Conversion.StringToEnum<EveDataApiKeyType>(x.Attribute("type").Value, EveDataApiKeyType.Character)
				                })
                                .FirstOrDefault();

                // Populate key characters.
                apiKeyInfo.Characters = xmlDoc.Descendants("row")
                            .Select(x => new EveDataApiKeyCharacter()
                            {
                                CharacterId = Conversion.StringToInt32(x.Attribute("characterID").Value),
                                CharacterName = x.Attribute("characterName").Value,
                                CorporationId = Conversion.StringToInt32(x.Attribute("corporationID").Value),
                                CorporationName = x.Attribute("corporationName").Value
                            })
                            .ToList();
            }
            catch (System.FormatException e)
            {
                logger.LogMessage("An error occured while parsing the CCP ApiKeyInfo Xml for ApiID: " + apiId + ". Is the data source present and correct?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Net.WebException e)
            {
                logger.LogMessage("An error occured while parsing the CCP ApiKeyInfo Xml for ApiID: " + apiId + ". Is the data source present and correct?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Xml.XmlException e)
            {
                logger.LogMessage("An error occured while loading the CCP ApiKeyInfo Xml for ApiID: " + apiId + ". Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.NullReferenceException e)
            {
                logger.LogMessage("An error occured while loading the CCP ApiKeyInfo Xml for ApiID: " + apiId + ". Is the API key correct?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception)
            {
                throw;
            }

            return apiKeyInfo;
        }
    }
}
