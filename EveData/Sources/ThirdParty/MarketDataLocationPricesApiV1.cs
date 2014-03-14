namespace EveData.Sources.ThirdParty
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Tools;

    /// <summary>
    /// Version 1 Api methods from the eve-marketdata.com location prices URI location.
    /// </summary>
    internal sealed class MarketDataLocationPricesApiV1
    {
        private readonly ISystemLogger logger;

        internal MarketDataLocationPricesApiV1(ISystemLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Resolve a type id and station id to a station buy price.
        /// </summary>
        /// <param name="typeId">The type id. E.g. '16229' for a Brutix.</param>
        /// <param name="stationId">The station id. E.g. '60003760' for 'Jita IV - Moon 4 - Caldari Navy Assembly Plant'.</param>
        /// <returns>Returns a double containing the station buy price of the passed type or 0 if not found.</returns>
        internal double PriceTypeStationBuy5Pc(int typeId, int stationId, string apiUrl = "")
        {
            XDocument xmlDoc;
            double price = 0;

            // Assign the Api Url to a string if it has not already been passed as a parameter.
            if (apiUrl == string.Empty)
            {
                apiUrl = "http://api.eve-marketdata.com/api/price_type_station_buy_5pct.xml?char_name=DS&type_id=" + typeId + "&v=" + stationId;
            }

            // Create a new XMLDoc object loaded from the Api Url.
            try
            {
                xmlDoc = XDocument.Load(apiUrl);

                // Populate a list of the type ids within the xml results where the id attribute matches typeId.
                var xmlValue = xmlDoc.Descendants("val")
                    .Where(x => (double)x.Attribute("type_id") == typeId)
                    .Select(x => x.Value);

                // Convert the first value found to a double.
                price = Conversion.StringToDouble(xmlValue.FirstOrDefault());
            }
            catch (System.Net.WebException e)
            {
                logger.LogMessage("An error occured while parsing the eve-marketdata.com price_type_station_buy_5Pct Xml.", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Xml.XmlException e)
            {
                logger.LogMessage("An error occured while loading the eve-marketdata.com price_type_station_buy_5Pct Xml. Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception)
            {
                throw;
            }

            return price;
        }

        /// <summary>
        /// Resolve a list of type ids and a station id to a dictionary of typeids and their buy prices at that station.
        /// </summary>
        /// <param name="typeIds">A list of type ids. E.g. '16229' for a Brutix.</param>
        /// <param name="stationId">The station id. E.g. '60003760' for 'Jita IV - Moon 4 - Caldari Navy Assembly Plant'.</param>
        /// <returns>Returns a dictionary containing the typeid and the station buy price of each type.</returns>
        internal IDictionary<int, double> PriceTypeStationBuy5Pc(IEnumerable<int> typeIds, int stationId, string apiUrl = "")
        {
            XDocument xmlDoc;
            Dictionary<int, double> prices = null;

            // Assign the Api Url to a string if it has not already been passed as a parameter.
            if (apiUrl == string.Empty)
            {
                apiUrl = "http://api.eve-marketdata.com/api/price_type_station_buy_5pct.xml?char_name=DS&type_id=" + String.Join<int>(",", typeIds) +"&v=" + stationId;
            }

            // Create a new XMLDoc object loaded from the Api Url.
            try
            {
                xmlDoc = XDocument.Load(apiUrl);

                // Populate a dictionary of the type ids and prices within the xml results.
                prices = xmlDoc.Descendants("val")
                                .Select(x => new 
                                {
                                    TypeId = Conversion.StringToInt32(x.Attribute("type_id").Value),
                                    Price = Conversion.StringToDouble(x.Value)
                                })
                                .ToDictionary(x => x.TypeId, x => x.Price);
            }
            catch (System.Net.WebException e)
            {
                logger.LogMessage("An error occured while parsing the eve-marketdata.com price_type_station_buy_5Pct Xml.", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Xml.XmlException e)
            {
                logger.LogMessage("An error occured while loading the eve-marketdata.com price_type_station_buy_5Pct Xml. Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception)
            {
                throw;
            }

            return prices;
        }

        /// <summary>
        /// Resolve a type id and station id to a station sell price.
        /// </summary>
        /// <param name="typeId">The type id. E.g. '16229' for a Brutix.</param>
        /// <param name="stationId">The station id. E.g. '60003760' for 'Jita IV - Moon 4 - Caldari Navy Assembly Plant'.</param>
        /// <returns>Returns a double containing the station sell price of the passed type or 0 if not found.</returns>
        internal double PriceTypeStationSell5Pc(int typeId, int stationId, string apiUrl = "")
        {
            XDocument xmlDoc;
            double price = 0;

            // Assign the Api Url to a string if it has not already been passed as a parameter.
            if (apiUrl == string.Empty)
            {
                apiUrl = "http://api.eve-marketdata.com/api/price_type_station_sell_5pct.xml?char_name=DS&type_id=" + typeId + "&v=" + stationId;
            }

            // Create a new XMLDoc object loaded from the Api Url.
            try
            {
                xmlDoc = XDocument.Load(apiUrl);

                // Populate a list of the type ids within the xml results where the id attribute matches typeId.
                var xmlValue = xmlDoc.Descendants("val")
                    .Where(x => (double)x.Attribute("type_id") == typeId)
                    .Select(x => x.Value);

                // Convert the first value found to a double.
                price = Conversion.StringToDouble(xmlValue.FirstOrDefault());
            }
            catch (System.Net.WebException e)
            {
                logger.LogMessage("An error occured while parsing the eve-marketdata.com price_type_station_sell_5Pct Xml.", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Xml.XmlException e)
            {
                logger.LogMessage("An error occured while loading the eve-marketdata.com price_type_station_sell_5Pct Xml. Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception)
            {
                throw;
            }

            return price;
        }

        /// <summary>
        /// Resolve a list of type ids and a station id to a dictionary of typeids and their sell prices at that station.
        /// </summary>
        /// <param name="typeIds">A list of type ids. E.g. '16229' for a Brutix.</param>
        /// <param name="stationId">The station id. E.g. '60003760' for 'Jita IV - Moon 4 - Caldari Navy Assembly Plant'.</param>
        /// <returns>Returns a dictionary containing the typeid and the station sell price of each type.</returns>
        internal IDictionary<int, double> PriceTypeStationSell5Pc(IEnumerable<int> typeIds, int stationId, string apiUrl = "")
        {
            XDocument xmlDoc;
            Dictionary<int, double> prices = null;

            // Assign the Api Url to a string if it has not already been passed as a parameter.
            if (apiUrl == string.Empty)
            {
                apiUrl = "http://api.eve-marketdata.com/api/price_type_station_sell_5pct.xml?char_name=DS&type_id=" + String.Join<int>(",", typeIds) + "&v=" + stationId;
            }

            // Create a new XMLDoc object loaded from the Api Url.
            try
            {
                xmlDoc = XDocument.Load(apiUrl);

                // Populate a dictionary of the type ids and prices within the xml results.
                prices = xmlDoc.Descendants("val")
                                .Select(x => new
                                {
                                    TypeId = Conversion.StringToInt32(x.Attribute("type_id").Value),
                                    Price = Conversion.StringToDouble(x.Value)
                                })
                                .ToDictionary(x => x.TypeId, x => x.Price);
            }
            catch (System.Net.WebException e)
            {
                logger.LogMessage("An error occured while parsing the eve-marketdata.com price_type_station_sell_5Pct Xml.", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Xml.XmlException e)
            {
                logger.LogMessage("An error occured while loading the eve-marketdata.com price_type_station_sell_5Pct Xml. Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception)
            {
                throw;
            }

            return prices;
        }
    }
}
