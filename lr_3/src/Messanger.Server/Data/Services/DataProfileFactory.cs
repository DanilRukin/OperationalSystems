using Messanger.Server.Data.DataProfiles;
using Messanger.Server.Data.DataProfiles.Base;

namespace Messanger.Server.Data.Services
{
    public class DataProfileFactory : IDataProfileFactory
    {
        private IConfiguration _configuration;
        public DataProfileFactory(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public DataProfile CreateProfile()
        {
            string profileName = _configuration["UseProfile"];
            var section = _configuration.GetSection("Profiles");
            var profile = section.GetSection(profileName);
            string connectionString = profile[nameof(DataProfile.ConnectionString)];
            bool useSeedData = FromString(profile[nameof(DataProfile.UseSeedData)]);
            bool migrateDatabase = FromString(profile[nameof(DataProfile.MigrateDatabase)]);
            bool createDatabase = FromString(profile[nameof(DataProfile.CreateDatabase)]);
            switch (profileName)
            {
                case string _ when profileName == "MySqlProfile_Messanger_test":
                    string migrationAssembly = profile[nameof(MySqlTestDataProfile.MigrationAssembly)];
                    return new MySqlTestDataProfile(profileName, connectionString, useSeedData,
                        migrateDatabase, createDatabase, migrationAssembly);
                case string _ when profileName == "PostgresProfile_Messanger_test":
                    string postgresMigrationAssembly = profile[nameof(PostgresTestDataProfile.MigrationAssembly)];
                        return new PostgresTestDataProfile(profileName, connectionString, useSeedData,
                        migrateDatabase, createDatabase, postgresMigrationAssembly);
                default:
                    throw new InvalidOperationException($"Unable to create '{profileName}'" +
                    $" database profile");
            }
        }

        private bool FromString(string value) => value == "true" ? true : false;
    }
}
