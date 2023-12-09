using Messanger.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Server.Data
{
    public static class SeedData
    {
        private static bool _empty = false;
        private static bool _created = false;
        public static User FirstUser { get; private set; }
        public static User SecondUser { get; private set; }

        public static void ApplyMigrationAndFillDatabase(MessangerDataContext context)
        {
            context.Database.Migrate();
            ClearDatabase(context);
            FillDatabase(context);
            _created = true;
        }

        public static void InitializeDatabase(MessangerDataContext context)
        {
            context.Database.EnsureCreated();
            ClearDatabase(context);
            FillDatabase(context);
            _created = true;
        }

        private static void FillDatabase(MessangerDataContext context)
        {
            if (_empty)
            {
                FirstUser = new User()
                {
                    FirstName = "Rukin",
                    LastName = "Danil",
                    Patronymic = "Vitalievich",
                    Id = Guid.NewGuid(),
                    Login = "dvrukin",
                    Password = "qwerty123@"
                };
                SecondUser = new User()
                {
                    FirstName = "Farnieva",
                    LastName = "Elizaveta",
                    Patronymic = "Olegovna",
                    Id = Guid.NewGuid(),
                    Login = "farnieva_eo",
                    Password = "123456789"
                };
                context.Users.Add(FirstUser);
                context.Users.Add(SecondUser);
                context.SaveChanges();

                _empty = false;
            }
        }

        private static void ClearDatabase(MessangerDataContext context)
        {
            if (context.Users.Any())
                context.Users.RemoveRange(context.Users);
            if (context.Messages.Any())
                context.Messages.RemoveRange(context.Messages);
            if (context.FriendRequests.Any())
                context.FriendRequests.RemoveRange(context.FriendRequests);
            context.SaveChanges();
            FirstUser = null;
            SecondUser = null;
            _empty = true;
        }

        public static void ResetDatabase(MessangerDataContext context)
        {
            if (_created)
            {
                ClearDatabase(context);
                FillDatabase(context);
            }
        }
    }
}
