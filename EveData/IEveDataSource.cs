namespace EveData
{
    using System.Collections.Generic;
    using EveData.Entities;
    
    /// <summary>
    /// Represents an instance of an Eve data source, a list of methods to retrieve Eve data from various locations.
    /// </summary>
    public interface IEveDataSource
    {
        /// <summary>
        /// This method returns information about an Api Key and the characters exposed by it.
        /// </summary>
        /// <param name="apiId">A valid eve api id (keyID).</param>
        /// <param name="apiKey">A valid eve api key (vCode).</param>
        /// <returns>Returns a populated IEveDataApiKey object relating to the passed Api Key, or a null object if a problem occured.</returns>
        IEveDataApiKey GetApiKeyInfo(int apiId, string apiKey);

        /// <summary>
        /// This method resolves a character name to a character id.
        /// </summary>
        /// <param name="characterName">The character name.</param>
        /// <returns>Returns an integer containing the character id or 0 if not found.</returns>
        int GetCharacterId(string characterName);

        /// <summary>
        /// This method resolves a character id to a character portrait url.
        /// </summary>
        /// <param name="characterId">The character id.</param>
        /// <returns>Returns a string containing the url of the character's portrait.</returns>
        string GetCharacterPortraitUrl(int characterId);

        /// <summary>
        /// This method resolves a corporation id to a corporation logo url.
        /// </summary>
        /// <param name="corporationId">The corporation id.</param>
        /// <returns>Returns a string containing the url of the corporation's logo.</returns>
        string GetCorporationLogoUrl(int corporationId);

        /// <summary>
        /// This method resolves an alliance id to an alliance logo url.
        /// </summary>
        /// <param name="allianceId">The alliance id.</param>
        /// <returns>Returns a string containing the url of the alliance's logo.</returns>
        string GetAllianceLogoUrl(int allianceId);

        /// <summary>
        /// This method resolves a station id to the id of its solar system location.
        /// </summary>
        /// <param name="stationId">The station id.</param>
        /// <returns>Returns an integer containing a solar system id or -1 if not found.</returns>
        int GetStationSolarSystemId(int stationId);

        /// <summary>
        /// This method resolves a station id to the name of the station.
        /// </summary>
        /// <param name="stationId">The station id.</param>
        /// <returns>Returns a string containing the station name or 'Unknown' if not found.</returns>
        string GetStationName(int stationId);

        /// <summary>
        /// This method resolves a solar system id to its name.
        /// </summary>
        /// <param name="stationId">The station id.</param>
        /// <returns>Returns a string containing the solar system name or 'Unknown' if not found.</returns>
        string GetSolarSystemName(int solarSystemId);

        /// <summary>
        /// This method resolves a type name to its id.
        /// </summary>
        /// <param name="typeName">The type name. E.g. 'Veldspar'.</param>
        /// <returns>Returns an integer containing a type id or 0 if not found.</returns>
        int GetTypeId(string typeName);

        /// <summary>
        /// This method resolves a type id to its name.
        /// </summary>
        /// <param name="typeId">The type id. E.g. '16229' for a Brutix.</param>
        /// <returns>Returns a string containing a type name or 'Unknown' if not found.</returns>
        string GetTypeName(int typeId);

        /// <summary>
        /// Resolve a type id to its packaged volume.
        /// </summary>
        /// <param name="typeId">The type id. E.g. '16229' for a Brutix.</param>
        /// <returns>Returns a double containing the volume of the passed type or 0 if not found.</returns>
        double GetTypeVolume(int typeId);

        /// <summary>
        /// This method resolves a type id to an image url.
        /// </summary>
        /// <param name="typeId">The type id.</param>
        /// <param name="size">An optional size parameter (32 or 64). Defaults to 32.</param>
        /// <returns>Returns a string containing the url of the type's image url.</returns>
        string GetTypeImageUrl(int typeId, int size = 32);

        /// <summary>
        /// This method resolves a type id to a render image url.
        /// </summary>
        /// <param name="typeId">The type id.</param>
        /// <param name="size">An optional size parameter (32, 64, 128, 256 or 512). Defaults to 256.</param>
        /// <returns>Returns a string containing the url of the type's render image url.</returns>
        string GetTypeRenderImageUrl(int typeId, int size = 256);

        /// <summary>
        /// Resolve a type id and station id to a station buy price.
        /// </summary>
        /// <param name="typeId">The type id. E.g. '16229' for a Brutix.</param>
        /// <param name="stationId">The station id. E.g. '60003760' for 'Jita IV - Moon 4 - Caldari Navy Assembly Plant'.</param>
        /// <returns>Returns a double containing the station buy price of the passed type or 0 if not found.</returns>
        double GetStationBuyPrice(int typeId, int stationId);

        /// <summary>
        /// Resolve a list of type ids and a station id to a dictionary of typeids and their buy prices at that station.
        /// </summary>
        /// <param name="typeIds">A list of type ids. E.g. '16229' for a Brutix.</param>
        /// <param name="stationId">The station id. E.g. '60003760' for 'Jita IV - Moon 4 - Caldari Navy Assembly Plant'.</param>
        /// <returns>Returns a dictionary containing the typeid and the station buy price of each type.</returns>
        IDictionary<int, double> GetStationBuyPrice(IEnumerable<int> typeIds, int stationId);

        /// <summary>
        /// Resolve a type id and station id to a station sell price.
        /// </summary>
        /// <param name="typeId">The type id. E.g. '16229' for a Brutix.</param>
        /// <param name="stationId">The station id. E.g. '60003760' for 'Jita IV - Moon 4 - Caldari Navy Assembly Plant'.</param>
        /// <returns>Returns a double containing the station sell price of the passed type or 0 if not found.</returns>
        double GetStationSellPrice(int typeId, int stationId);

        /// <summary>
        /// Resolve a list of type ids and a station id to a dictionary of typeids and their sell prices at that station.
        /// </summary>
        /// <param name="typeIds">A list of type ids. E.g. '16229' for a Brutix.</param>
        /// <param name="stationId">The station id. E.g. '60003760' for 'Jita IV - Moon 4 - Caldari Navy Assembly Plant'.</param>
        /// <returns>Returns a dictionary containing the typeid and the station sell price of each type.</returns>
        IDictionary<int, double> GetStationSellPrice(IEnumerable<int> typeIds, int stationId);

        /// <summary>
        /// This method populates a list of contracts for the character or corporation api passed.
        /// </summary>
        /// <param name="apiId">A valid eve api id (keyID).</param>
        /// <param name="apiKey">A valid eve api key (vCode).</param>
        /// <param name="characterID">An optional character id.</param>
        /// <param name="isCorp">An optional flag. Are we requesting the contracts for a corporation?</param>
        /// <returns>Returns a populated list of IEveDataContract objects.</returns>
        IEnumerable<IEveDataContract> GetContracts(int apiId, string apiKey, int characterId = 0, bool isCorp = false);

        /// <summary>
        /// Populate a list of contracts items for the contract id passed.
        /// </summary>
        /// <param name="apiId">A valid eve api id (keyID).</param>
        /// <param name="apiKey">A valid eve api key (vCode).</param>
        /// <param name="contractId">An existing contract id.</param>
        /// <param name="characterID">An optional character id.</param>
        /// <param name="isCorp">An optional flag. Are we requesting the contracts for a corporation?</param>
        /// <returns>Returns a populated list of IEveDataContractItem objects relating to the character and contract id.</returns>
        IEnumerable<IEveDataContractItem> GetContractItems(int apiId, string apiKey, long contractId, int characterId = 0, bool isCorp = false);

        /// <summary>
        /// Populate a list of wallet entry items for the corporation api key passed.
        /// </summary>
        /// <param name="apiId">A valid eve api id (keyID).</param>
        /// <param name="apiKey">A valid eve api key (vCode).</param>
        /// <param name="accountKey">The wallet account key (1000-1006). Defaults to 1000.</param>
        /// <param name="rowCount">The number of rows to return from the Api. Defaults to the maximum value of 2560.</param>
        /// <returns>Returns a populated list of IEveDataWalletEntry objects relating to the corporation api key passed.</returns>
        IEnumerable<IEveDataWalletEntry> GetCorporationWalletEntries(int apiId, string apiKey, int accountKey = 1000, int rowCount = 2560);
    }
}
