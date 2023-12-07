using Messanger.Server.Model;

namespace Messanger.Server.Extensions
{
    public static class UserExtensions
    {
        public static UserData ToUserData(this User user) =>
            new UserData()
            { 
                Id = user.Id.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Patronymic = user.Patronymic
            };

    }
}
