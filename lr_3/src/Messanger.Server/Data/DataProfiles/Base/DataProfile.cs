using Microsoft.EntityFrameworkCore;

namespace Messanger.Server.Data.DataProfiles.Base
{
    public abstract class DataProfile
    {
        public string Name { get; protected set; }
        public string ConnectionString { get; protected set; }
        public bool UseSeedData { get; protected set; }
        public bool MigrateDatabase { get; protected set; }
        public bool CreateDatabase { get; protected set; }

        protected DataProfile(string name, string connectionString, bool useSeedData,
            bool migrateDatabase, bool createDatabase)
        {
            Name = name;
            ConnectionString = connectionString;
            UseSeedData = useSeedData;
            MigrateDatabase = migrateDatabase;
            CreateDatabase = createDatabase;
        }

        public abstract void ConfigureDbContextOptionsBuilder(DbContextOptionsBuilder builder);

        public virtual void UseDbContext(MessangerDataContext context)
        {
            if (UseSeedData)
            {
                if (MigrateDatabase)
                {
                    SeedData.ApplyMigrationAndFillDatabase(context);
                }
                else if (CreateDatabase)
                {
                    SeedData.InitializeDatabase(context);
                }
            }
        }
    }
}
