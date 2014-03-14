namespace EveData.Sources.CCP
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Tools;

    /// <summary>
    /// Version 2 Api methods from the CCP /eve/ URI location.
    /// </summary>
    internal sealed class EveApiV2
    {
        private readonly ISystemLogger logger;

        internal EveApiV2(ISystemLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Resolve a character name to a character id.
        /// </summary>
        /// <param name="characterName">The character name.</param>
        /// <param name="apiUrl">An optional apiUrl parameter that overrides the method's internal value. Primarily used in unit tests to force errors.</param>
        /// <returns>Returns an integer containing the character id  or 0 if not found.</returns>
        internal int EveCharacterId(string characterName, string apiUrl = "")
        {
            XDocument xmlDoc;
            int characterId = 0;

            // Assign the Api Url to a string if it has not already been passed as a parameter.
            if (apiUrl == string.Empty)
            {
                apiUrl = "https://api.eveonline.com/eve/CharacterID.xml.aspx?names=" + characterName;
            }

            // Create a new XMLDoc object loaded from the Api Url.
            try
            {
                xmlDoc = XDocument.Load(apiUrl);

                // Populate a list of the character Ids within the xml results where the name attribute matches characterName.
                var xmlValue = xmlDoc.Descendants("row")
                    .Where(x => (string)x.Attribute("name") == characterName)
                    .Select(x => (string)x.Attribute("characterID"));

                // Convert the first value found to an integer.
                characterId = Conversion.StringToInt32(xmlValue.FirstOrDefault());
            }
            catch (System.Net.WebException e)
            {
                logger.LogMessage("An error occured while parsing the CCP CharacterID Xml.", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Xml.XmlException e)
            {
                logger.LogMessage("An error occured while loading the CCP CharacterID Xml. Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception)
            {
                throw;
            }

            return characterId;
        }
    }
}
