namespace EveData
{
    using System.Collections.Generic;
    using EveData.Entities;
    using EveData.Sources.CCP;
    using EveData.Sources.ThirdParty;
    using Tools;

    public class EveDataSource : IEveDataSource
    {
        #region Fields

        // External Dependencies.
        private readonly ISystemLogger logger;

        // Internal Dependencies (Instantiated On-Demand By Accessors).
        private AccountApiV2 accountApi;
        private EveApiV2 eveApi;
        private CharacterApiV2 characterApi;
        private CorporationApiV2 corporationApi;
        private MiscApiV2 miscApi;
        private MarketDataStationApiV1 stationApi;
        private MarketDataSolarSystemApiV1 solarSystemApi;
        private MarketDataItemApiV1 itemApi;
        private MarketDataLocationPricesApiV1 locationPricesApi;

        #endregion

        public static string ConnectionString = string.Empty;
        /// <summary>
        /// Initialises a new instance of an EveDataSource.
        /// </summary>
        /// <param name="logger">An ISystemLogger logger instance.</param>
        public EveDataSource(ISystemLogger logger)
        {
            this.logger = logger;
        }

        #region Accessors

        private AccountApiV2 AccountApi
        {
            get
            {
                if (this.accountApi == null)
                {
                    this.accountApi = new AccountApiV2(logger);
                }

                return this.accountApi;
            }
        }

        private EveApiV2 EveApi
        {
            get
            {
                if (this.eveApi == null)
                {
                    this.eveApi = new EveApiV2(logger);
                }

                return this.eveApi;
            }
        }

        private CharacterApiV2 CharacterApi
        {
            get
            {
                if (this.characterApi == null)
                {
                    this.characterApi = new CharacterApiV2(logger);
                }

                return this.characterApi;
            }
        }

        private CorporationApiV2 CorporationApi
        {
            get
            {
                if (this.corporationApi == null)
                {
                    this.corporationApi = new CorporationApiV2(logger);
                }

                return this.corporationApi;
            }
        }

        private MiscApiV2 MiscApi
        {
            get
            {
                if (this.miscApi == null)
                {
                    this.miscApi = new MiscApiV2(logger);
                }

                return this.miscApi;
            }
        }

        private MarketDataStationApiV1 StationApi
        {
            get
            {
                if (this.stationApi == null)
                {
                    this.stationApi = new MarketDataStationApiV1(logger);
                }

                return this.stationApi;
            }
        }

        private MarketDataSolarSystemApiV1 SolarSystemApi
        {
            get
            {
                if (this.solarSystemApi == null)
                {
                    this.solarSystemApi = new MarketDataSolarSystemApiV1(logger);
                }

                return this.solarSystemApi;
            }
        }

        private MarketDataItemApiV1 ItemApi
        {
            get
            {
                if (this.itemApi == null)
                {
                    this.itemApi = new MarketDataItemApiV1(logger);
                }

                return this.itemApi;
            }
        }

        private MarketDataLocationPricesApiV1 LocationPricesApi
        {
            get
            {
                if (this.locationPricesApi == null)
                {
                    this.locationPricesApi = new MarketDataLocationPricesApiV1(logger);
                }

                return this.locationPricesApi;
            }
        }

        #endregion

        public virtual IEveDataApiKey GetApiKeyInfo(int apiId, string apiKey)
        {
            return this.AccountApi.ApiKeyInfo(apiId, apiKey);
        }

        public virtual int GetCharacterId(string characterName)
        {
            return this.EveApi.EveCharacterId(characterName);
        }

        public virtual string GetCharacterPortraitUrl(int characterId)
        {
            return this.MiscApi.MiscImage(characterId, EveDataImageType.Character, 128);
        }

        public virtual string GetCorporationLogoUrl(int corporationId)
        {
            return this.MiscApi.MiscImage(corporationId, EveDataImageType.Corporation, 128);
        }

        public virtual string GetAllianceLogoUrl(int allianceId)
        {
            return this.MiscApi.MiscImage(allianceId, EveDataImageType.Alliance, 128);
        }

        public virtual int GetStationSolarSystemId(int stationId)
        {
            return this.StationApi.StationSolarSystemId(stationId);
        }

        public virtual string GetStationName(int stationId)
        {
            return this.StationApi.StationName(stationId);
        }

        public virtual string GetSolarSystemName(int solarSystemId)
        {
            return this.SolarSystemApi.SolarSystemName(solarSystemId);
        }

        public virtual int GetTypeId(string typeName)
        {
            return this.ItemApi.TypeId(typeName);
        }

        public virtual string GetTypeName(int typeId)
        {
            return this.ItemApi.TypeName(typeId);
        }

        public virtual double GetTypeVolume(int typeId)
        {
            return this.ItemApi.TypeVolume(typeId);
        }

        public virtual string GetTypeImageUrl(int typeId, int size = 32)
        {
            return this.MiscApi.MiscImage(typeId, EveDataImageType.InventoryType, size);
        }

        public string GetTypeRenderImageUrl(int typeId, int size = 256)
        {
            return this.MiscApi.MiscImage(typeId, EveDataImageType.Render, size);
        }

        public virtual double GetStationBuyPrice(int typeId, int stationId)
        {
            return this.LocationPricesApi.PriceTypeStationBuy5Pc(typeId, stationId);
        }

        public virtual IDictionary<int, double> GetStationBuyPrice(IEnumerable<int> typeIds, int stationId)
        {
            return this.LocationPricesApi.PriceTypeStationBuy5Pc(typeIds, stationId);
        }

        public virtual double GetStationSellPrice(int typeId, int stationId)
        {
            return this.LocationPricesApi.PriceTypeStationSell5Pc(typeId, stationId);
        }

        public virtual IDictionary<int, double> GetStationSellPrice(IEnumerable<int> typeIds, int stationId)
        {
            return this.LocationPricesApi.PriceTypeStationSell5Pc(typeIds, stationId);
        }

        public virtual IEnumerable<IEveDataContract> GetContracts(int apiId, string apiKey, int characterId = 0, bool isCorp = false)
        {
            if (isCorp == false)
            {
                return this.CharacterApi.CharacterContracts(characterId, apiId, apiKey);
            }
            else
            {
                return this.CorporationApi.CorporationContracts(apiId, apiKey);
            }
        }

        public virtual IEnumerable<IEveDataContractItem> GetContractItems(int apiId, string apiKey, long contractId, int characterId = 0, bool isCorp = false)
        {
            if (isCorp == false)
            {
                return this.CharacterApi.CharacterContractItems(characterId, apiId, apiKey, contractId);
            }
            else
            {
                return this.CorporationApi.CorporationContractItems(apiId, apiKey, contractId);
            }
        }

        public virtual IEnumerable<IEveDataWalletEntry> GetCorporationWalletEntries(int apiId, string apiKey, int accountKey = 1000, int rowCount = 2560)
        {
            return this.CorporationApi.WalletJournal(apiId, apiKey, accountKey, rowCount);
        }
    }
}
