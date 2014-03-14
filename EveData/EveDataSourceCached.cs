namespace EveData
{
    using System.Collections.Generic;
    using Tools;

    public sealed class EveDataSourceCached : EveDataSource
    {
        // External Dependencies.
        private readonly ISystemLogger logger;
        
        // Internal Dependencies (Instantiated On-Demand By Accessors).
        private static IDictionary<int, string> cachedValues_GetSolarSystemName = new Dictionary<int, string>();
        private static IDictionary<int, int> cachedValues_GetStationSolarSystemId = new Dictionary<int, int>();
        private static IDictionary<int, string> cachedValues_GetStationName = new Dictionary<int, string>();
        private static IDictionary<string, int> cachedValues_GetTypeId = new Dictionary<string, int>();
        private static IDictionary<int, string> cachedValues_GetTypeName = new Dictionary<int, string>();
        private static IDictionary<int, double> cachedValues_GetTypeVolume = new Dictionary<int, double>();

        /// <summary>
        /// Initialises a new instance of a cached EveDataSource.
        /// </summary>
        public EveDataSourceCached(ISystemLogger logger) : base(logger)
        {
            this.logger = logger;
        }

        public override string GetSolarSystemName(int solarSystemId)
        {
            string cachedValue;

            // If the value is cached return cached value, otherwise call EveData in the base class.
            if (!cachedValues_GetSolarSystemName.TryGetValue(solarSystemId, out cachedValue))
            {
                cachedValue = base.GetSolarSystemName(solarSystemId);
                cachedValues_GetSolarSystemName.Add(solarSystemId, cachedValue);
            }

            return cachedValue;
        }

        public override int GetStationSolarSystemId(int stationId)
        {
            int cachedValue;

            // If the value is cached return cached value, otherwise call EveData in the base class.
            if (!cachedValues_GetStationSolarSystemId.TryGetValue(stationId, out cachedValue))
            {
                cachedValue = base.GetStationSolarSystemId(stationId);
                cachedValues_GetStationSolarSystemId.Add(stationId, cachedValue);
            }

            return cachedValue;
        }

        public override string GetStationName(int stationId)
        {
            string cachedValue;

            // If the value is cached return cached value, otherwise call EveData in the base class.
            if (!cachedValues_GetStationName.TryGetValue(stationId, out cachedValue))
            {
                cachedValue = base.GetStationName(stationId);
                cachedValues_GetStationName.Add(stationId, cachedValue);
            }

            return cachedValue;
        }

        public override int GetTypeId(string typeName)
        {
            int cachedValue;

            // If the value is cached return cached value, otherwise call EveData in the base class.
            if (!cachedValues_GetTypeId.TryGetValue(typeName, out cachedValue))
            {
                cachedValue = base.GetTypeId(typeName);
                cachedValues_GetTypeId.Add(typeName, cachedValue);
            }

            return cachedValue;
        }

        public override string GetTypeName(int typeId)
        {
            string cachedValue;

            // If the value is cached return cached value, otherwise call EveData in the base class.
            if (!cachedValues_GetTypeName.TryGetValue(typeId, out cachedValue))
            {
                cachedValue = base.GetTypeName(typeId);
                cachedValues_GetTypeName.Add(typeId, cachedValue);
            }

            return cachedValue;
        }

        public override double GetTypeVolume(int typeId)
        {
            double cachedValue;

            // If the value is cached return cached value, otherwise call EveData in the base class.
            if (!cachedValues_GetTypeVolume.TryGetValue(typeId, out cachedValue))
            {
                cachedValue = base.GetTypeVolume(typeId);
                cachedValues_GetTypeVolume.Add(typeId, cachedValue);
            }

            return cachedValue;
        }
    }
}