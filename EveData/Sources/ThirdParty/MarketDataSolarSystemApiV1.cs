namespace EveData.Sources.ThirdParty
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Tools;

    /// <summary>
    /// Version 1 Api methods from the eve-marketdata.com solarsystem URI location.
    /// </summary>
    internal sealed class MarketDataSolarSystemApiV1
    {
        private readonly ISystemLogger logger;

        internal MarketDataSolarSystemApiV1(ISystemLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Resolve a solar system id to its name.
        /// </summary>
        /// <param name="solarSystemId">The solar system id.</param>
        /// <param name="apiUrl">An optional apiUrl parameter that overrides the method's internal value. Primarily used in unit tests to force errors.</param>
        /// <returns>Returns a string containing the solar system name or 'Unknown' if not found.</returns>
        internal string SolarSystemName(int solarSystemId, string apiUrl = "")
        {
            XDocument xmlDoc;
            string solarSystemName = "Unknown";

            // Assign the Api Url to a string if it has not already been passed as a parameter.
            if (apiUrl == string.Empty)
            {
                apiUrl = "http://api.eve-marketdata.com/api/solarsystem_name.xml?char_name=DS&v=" + solarSystemId;
            }

            // Create a new XMLDoc object loaded from the Api Url.
            try
            {
                xmlDoc = XDocument.Load(apiUrl);

                // Populate a list of the station ids within the xml results where the name attribute matches stationId.
                var xmlValue = xmlDoc.Descendants("val")
                    .Where(x => (int)x.Attribute("id") == solarSystemId)
                    .Select(x => x.Value);

                // If the first value found is not null or empty then assign it.
                if (xmlValue.FirstOrDefault() != null && xmlValue.FirstOrDefault() != string.Empty)
                {
                    solarSystemName = xmlValue.FirstOrDefault();
                }
            }
            catch (System.Net.WebException e)
            {
                logger.LogMessage("An error occured while parsing the eve-marketdata.com solarsystem_name Xml.", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Xml.XmlException e)
            {
                logger.LogMessage("An error occured while loading the eve-marketdata.com solarsystem_name Xml. Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception)
            {
                throw;
            }

            return solarSystemName;
        }
    }
}
