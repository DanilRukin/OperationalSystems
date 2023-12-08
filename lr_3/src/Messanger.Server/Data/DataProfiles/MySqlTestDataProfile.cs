using Messanger.Server.Data.DataProfiles.Base;
using Microsoft.EntityFrameworkCore;


namespace Messanger.Server.Data.DataProfiles
{
    public class MySqlTestDataProfile : DataProfile
    {
        public string MigrationAssembly { get; set; }
        public MySqlTestDataProfile(
            string name,
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
            builder.UseMySQL(ConnectionString, sql => sql.MigrationsAssembly(MigrationAssembly));
        }
    }
}
