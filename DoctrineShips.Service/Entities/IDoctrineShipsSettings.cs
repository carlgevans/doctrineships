namespace DoctrineShips.Service.Entities
{
    using LinqToTwitter;

    public interface IDoctrineShipsSettings
    {
        string TaskKey { get; }
        string SecondKey { get; }
        string WebsiteDomain { get; }
        string TwitterConsumerKey { get; }
        string TwitterConsumerSecret { get; }
        string TwitterAccessToken { get; }
        string TwitterAccessTokenSecret { get; }
        string Brand { get; }
        TwitterContext TwitterContext { get; }
    }
}
