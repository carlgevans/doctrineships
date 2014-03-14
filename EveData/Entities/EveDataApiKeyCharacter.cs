namespace EveData.Entities
{
    using System;

    public sealed class EveDataApiKeyCharacter : IEveDataApiKeyCharacter
    {
        public int CharacterId { get; set; }
        public int CorporationId { get; set; }
        public string CharacterName { get; set; }
        public string CorporationName { get; set; }
    }
}
