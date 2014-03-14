namespace EveData.Entities
{
    using System;

    public interface IEveDataApiKeyCharacter
    {
        int CharacterId { get; set; }
        int CorporationId { get; set; }
        string CharacterName { get; set; }
        string CorporationName { get; set; }
    }
}
