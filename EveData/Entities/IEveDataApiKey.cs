namespace EveData.Entities
{
    using System;
    using System.Collections.Generic;

    public interface IEveDataApiKey
    {
        int AccessMask { get; set; }
        EveDataApiKeyType Type { get; set; }
        DateTime Expires { get; set; }
        IEnumerable<IEveDataApiKeyCharacter> Characters { get; set; }
    }
}
