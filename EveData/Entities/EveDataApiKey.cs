namespace EveData.Entities
{
    using System;
    using System.Collections.Generic;

    public sealed class EveDataApiKey : IEveDataApiKey
    {
        public int AccessMask { get; set; }
        public EveDataApiKeyType Type { get; set; }
        public DateTime Expires { get; set; }
        public IEnumerable<IEveDataApiKeyCharacter> Characters { get; set; }
    }
}
