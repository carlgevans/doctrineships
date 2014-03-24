namespace DoctrineShips.Data
{
    using System.Data.Entity;
    using DoctrineShips.Data.Mapping;
    using GenericRepository.Providers.EntityFramework;
    
    public class DoctrineShipsContext : DbContextBase
    {
        public DoctrineShipsContext() :
            base("name=DoctrineShipsBCDb")
        {
            //Database.SetInitializer<DoctrineShipsContext>(new DropCreateDatabaseAlways<DoctrineShipsContext>());
            Database.SetInitializer<DoctrineShipsContext>(null);
            Configuration.ProxyCreationEnabled = false;
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new AccessCodeMap());
            modelBuilder.Configurations.Add(new AccountMap());
            modelBuilder.Configurations.Add(new ArticleMap());
            modelBuilder.Configurations.Add(new ComponentMap());
            modelBuilder.Configurations.Add(new ContractMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new LogMessageMap());
            modelBuilder.Configurations.Add(new NotificationRecipientMap());
            modelBuilder.Configurations.Add(new SalesAgentMap());
            modelBuilder.Configurations.Add(new SettingProfileMap());
            modelBuilder.Configurations.Add(new ShipFitComponentMap());
            modelBuilder.Configurations.Add(new ShipFitMap());
            modelBuilder.Configurations.Add(new ShortUrlMap());
        }
    }
}