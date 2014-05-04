namespace DoctrineShips.Service.Entities
{
    using LinqToTwitter;

    public class DoctrineShipsSettings : IDoctrineShipsSettings
    {
        private readonly string taskKey;
        private readonly string secondKey;
        private readonly string websiteDomain;
        private readonly string twitterConsumerKey;
        private readonly string twitterConsumerSecret;
        private readonly string twitterAccessToken;
        private readonly string twitterAccessTokenSecret;
        private readonly string brand;
        private TwitterContext twitterContext;

        public string TaskKey { get { return this.taskKey; } }
        public string SecondKey { get { return this.secondKey; } }
        public string WebsiteDomain { get { return this.websiteDomain; } }
        public string TwitterConsumerKey { get { return this.twitterConsumerKey; } }
        public string TwitterConsumerSecret { get { return this.twitterConsumerSecret; } }
        public string TwitterAccessToken { get { return this.twitterAccessToken; } }
        public string TwitterAccessTokenSecret { get { return this.twitterAccessTokenSecret; } }
        public string Brand { get { return this.brand; } }

        public TwitterContext TwitterContext 
        { 
            get 
            {
                if (this.twitterContext == null)
                {
                    SingleUserAuthorizer twitterAuth = new SingleUserAuthorizer
                    {
                        CredentialStore = new SingleUserInMemoryCredentialStore
                        {
                            ConsumerKey = this.TwitterConsumerKey,
                            ConsumerSecret = this.TwitterConsumerSecret,
                            AccessToken = this.TwitterAccessToken,
                            AccessTokenSecret = this.TwitterAccessTokenSecret
                        }
                    };

                    this.twitterContext = new TwitterContext(twitterAuth);
                }

                return this.twitterContext;
            }
        }

        public DoctrineShipsSettings(string taskKey, 
                                     string secondKey,
                                     string websiteDomain,
                                     string twitterConsumerKey,
                                     string twitterConsumerSecret,
                                     string twitterAccessToken,
                                     string twitterAccessTokenSecret,
                                     string brand)
        {
            this.taskKey = taskKey ?? string.Empty;
            this.secondKey = secondKey ?? string.Empty;
            this.websiteDomain = websiteDomain ?? string.Empty;
            this.twitterConsumerKey = twitterConsumerKey ?? string.Empty;
            this.twitterConsumerSecret = twitterConsumerSecret ?? string.Empty;
            this.twitterAccessToken = twitterAccessToken ?? string.Empty;
            this.twitterAccessTokenSecret = twitterAccessTokenSecret ?? string.Empty;
            this.brand = brand ?? string.Empty;
        }
    }
}
