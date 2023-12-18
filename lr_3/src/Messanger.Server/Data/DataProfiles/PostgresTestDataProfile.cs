using Messanger.Server.Data.DataProfiles.Base;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Server.Data.DataProfiles
{
    public class PostgresTestDataProfile : DataProfile
    {
        public string MigrationAssembly { get; set; }
        public PostgresTestDataProfile(string name,
            string connectionString,
            bool useSeedData,
            bool migrateDatabase,
            bool createDatabase,
            string migrationAssembly) 
        : base(name, connectionString, useSeedData, migrateDatabase, createDatabase)
        {
            MigrationAssembly = migrationAssembly;
        }

        public override void ConfigureDbContextOptionsBuilder(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(ConnectionString, sql => sql.MigrationsAssembly(MigrationAssembly));
        }
    }
}
