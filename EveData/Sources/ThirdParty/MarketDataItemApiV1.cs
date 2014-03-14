namespace EveData.Sources.ThirdParty
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Tools;

    /// <summary>
    /// Version 1 Api methods from the eve-marketdata.com item URI location.
    /// </summary>
    internal sealed class MarketDataItemApiV1
    {
        private readonly ISystemLogger logger;

        internal MarketDataItemApiV1(ISystemLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Resolve a type name to its typeid.
        /// </summary>
        /// <param name="typeName">The type name. E.g. 'Veldspar'.</param>
        /// <param name="apiUrl">An optional apiUrl parameter that overrides the method's internal value. Primarily used in unit tests to force errors.</param>
        /// <returns>Returns an integer containing a type id or 0 if not found.</returns>
        internal int TypeId(string typeName, string apiUrl = "")
        {
            XDocument xmlDoc;
            int typeId = 0;

            // Assign the Api Url to a string if it has not already been passed as a parameter.
            if (apiUrl == string.Empty)
            {
                apiUrl = "http://api.eve-marketdata.com/api/type_id.xml?char_name=DS&v=" + typeName;
            }

            // Create a new XMLDoc object loaded from the Api Url.
            try
            {
                xmlDoc = XDocument.Load(apiUrl);

                // Populate a list of the type ids within the xml results where the id attribute matches typeName.
                var xmlValue = xmlDoc.Descendants("val")
                    .Where(x => (string)x.Attribute("id") == typeName)
                    .Select(x => x.Value);

                // Convert the first value found to an integer.
                typeId = Conversion.StringToInt32(xmlValue.FirstOrDefault());
            }
            catch (System.Net.WebException e)
            {
                logger.LogMessage("An error occured while parsing the eve-marketdata.com type_id Xml.", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Xml.XmlException e)
            {
                logger.LogMessage("An error occured while loading the eve-marketdata.com type_id Xml. Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception)
            {
                throw;
            }

            return typeId;
        }

        /// <summary>
        /// Resolve a type id to its name.
        /// </summary>
        /// <param name="typeId">The type id. E.g. '16229' for a Brutix.</param>
        /// <param name="apiUrl">An optional apiUrl parameter that overrides the method's internal value. Primarily used in unit tests to force errors.</param>
        /// <returns>Returns a string containing a type name or 'Unknown' if not found.</returns>
        internal string TypeName(int typeId, string apiUrl = "")
        {
            XDocument xmlDoc;
            string typeName = "Unknown";

            // Assign the Api Url to a string if it has not already been passed as a parameter.
            if (apiUrl == string.Empty)
            {
                apiUrl = "http://api.eve-marketdata.com/api/type_name.xml?char_name=DS&v=" + typeId;
            }

            // Create a new XMLDoc object loaded from the Api Url.
            try
            {
                xmlDoc = XDocument.Load(apiUrl);

                // Populate a list of the type ids within the xml results where the id attribute matches typeId.
                var xmlValue = xmlDoc.Descendants("val")
                    .Where(x => (int)x.Attribute("id") == typeId)
                    .Select(x => x.Value);

                // If the first value found is not null or empty then assign it.
                if (xmlValue.FirstOrDefault() != null && xmlValue.FirstOrDefault() != string.Empty)
                {
                    typeName = xmlValue.FirstOrDefault();
                }
            }
            catch (System.Net.WebException e)
            {
                logger.LogMessage("An error occured while parsing the eve-marketdata.com type_name Xml.", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Xml.XmlException e)
            {
                logger.LogMessage("An error occured while loading the eve-marketdata.com type_name Xml. Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception)
            {
                throw;
            }

            return typeName;
        }

        /// <summary>
        /// Resolve a type id to its packaged volume.
        /// </summary>
        /// <param name="typeId">The type id. E.g. '16229' for a Brutix.</param>
        /// <param name="apiUrl">An optional apiUrl parameter that overrides the method's internal value. Primarily used in unit tests to force errors.</param>
        /// <returns>Returns a double containing the volume of the passed type or 0 if not found.</returns>
        internal double TypeVolume(int typeId, string apiUrl = "")
        {
            XDocument xmlDoc;
            double typeVolume = 0;

            // Assign the Api Url to a string if it has not already been passed as a parameter.
            if (apiUrl == string.Empty)
            {
                apiUrl = "http://api.eve-marketdata.com/api/type_volume.xml?char_name=DS&v=" + typeId;
            }

            // Create a new XMLDoc object loaded from the Api Url.
            try
            {
                xmlDoc = XDocument.Load(apiUrl);

                // Populate a list of the type ids within the xml results where the id attribute matches typeId.
                var xmlValue = xmlDoc.Descendants("val")
                    .Where(x => (double)x.Attribute("id") == typeId)
                    .Select(x => x.Value);

                // Convert the first value found to a double.
                typeVolume = Conversion.StringToDouble(xmlValue.FirstOrDefault());
            }
            catch (System.Net.WebException e)
            {
                logger.LogMessage("An error occured while parsing the eve-marketdata.com type_volume Xml.", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (System.Xml.XmlException e)
            {
                logger.LogMessage("An error occured while loading the eve-marketdata.com type_volume Xml. Is the API available?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception)
            {
                throw;
            }

            return typeVolume;
        }
    }
}
