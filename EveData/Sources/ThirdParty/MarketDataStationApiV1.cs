namespace EveData.Sources.ThirdParty
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Tools;

    /// <summary>
    /// Version 1 Api methods from the eve-marketdata.com station URI location.
    /// </summary>
    internal sealed class MarketDataStationApiV1
    {
        private readonly ISystemLogger logger;

        internal MarketDataStationApiV1(ISystemLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Resolve a station id to the id of its solar system location.
        /// </summary>
        /// <param name="stationId">The station id.</param>
        /// <param name="apiUrl">An optional apiUrl parameter that overrides the method's internal value. Primarily used in unit tests to force errors.</param>
        /// <returns>Returns an integer containing a solar system id or -1 if not found.</returns>
        internal int StationSolarSystemId(int stationId, string apiUrl = "")
        {
            XDocument xmlDoc;
            int solarSystemId = 0;

            // Assign the Api Url to a string if it has not already been passed as a parameter.
            if (apiUrl == string.Empty)
            {
                apiUrl = "http://api.eve-marketdata.com/api/station_solarsystem_id.xml?char_name=DS&v=" + stationId;
            }

            // Create a new XMLDoc object loaded from the Api Url.
            try
            {
                xmlDoc = XDocument.Load(apiUrl);

                // Populate a list of the station ids within the xml results where the name attribute matches stationId.
                var xmlValue = xmlDoc.Descendants("val")
                    .Where(x => (int)x.Attribute("id") == stationId)
                    .Select(x => x.Value);

                // Convert the first value found to an integer.
                solarSystemId = Conversion.StringToInt32(xmlValue.FirstOrDefault()); 
            }
            catch (System.Net.WebException e)
            {
                logger.LogMessage("An error occured while parsing the eve-marketdata.com station_solarsystem_id Xml.", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Xml.XmlException e)
            {
                logger.LogMessage("An error occured while loading the eve-marketdata.com station_solarsystem_id Xml. Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception)
            {
                throw;
            }

            return solarSystemId;
        }

        /// <summary>
        /// Resolve a station id to the name of the station.
        /// </summary>
        /// <param name="stationId">The station id.</param>
        /// <param name="apiUrl">An optional apiUrl parameter that overrides the method's internal value. Primarily used in unit tests to force errors.</param>
        /// <returns>Returns a string containing the station name or 'Unknown' if not found.</returns>
        internal string StationName(int stationId, string apiUrl = "")
        {
            XDocument xmlDoc;
            string stationName = "Unknown";

            // Assign the Api Url to a string if it has not already been passed as a parameter.
            if (apiUrl == string.Empty)
            {
                apiUrl = "http://api.eve-marketdata.com/api/station_name.xml?char_name=DS&v=" + stationId;
            }

            // Create a new XMLDoc object loaded from the Api Url.
            try
            {
                xmlDoc = XDocument.Load(apiUrl);

                // Populate a list of the station ids within the xml results where the name attribute matches stationId.
                var xmlValue = xmlDoc.Descendants("val")
                    .Where(x => (int)x.Attribute("id") == stationId)
                    .Select(x => x.Value);

                // If the first value found is not null or empty then assign it.
                if (xmlValue.FirstOrDefault() != null && xmlValue.FirstOrDefault() != string.Empty)
                {
                    stationName = xmlValue.FirstOrDefault();
                }
            }
            catch (System.Net.WebException e)
            {
                logger.LogMessage("An error occured while parsing the eve-marketdata.com station_name Xml.", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Xml.XmlException e)
            {
                logger.LogMessage("An error occured while loading the eve-marketdata.com station_name Xml. Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception)
            {
                throw;
            }

            return stationName;
        }
    }
}
