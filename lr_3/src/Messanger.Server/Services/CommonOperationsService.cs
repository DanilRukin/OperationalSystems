using Messanger.Server.Data;
using Messanger.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Server.Services
{
    public class CommonOperationsService
    {
        private readonly ILogger<CommonOperationsService> _logger;
        private readonly MessangerDataContext _dataContext;

        public CommonOperationsService(ILogger<CommonOperationsService> logger, MessangerDataContext dataContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Получение списка всех пользователей");
                return await _dataContext.Users.ToArrayAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new List<User>();
            }
        }

        public async Task<IEnumerable<User>> GetFriendsOfUser(User user)
        {
            try
            {
                var friendsIds = await _dataContext
                    .FriendRequests
                    .Where(fr => fr.RequestSenderId == user.Id)
                    .Select(fr => fr.RequestRecieverId)
                    .ToArrayAsync();
                return await _dataContext
                    .Users
                    .Where(u => friendsIds.Any(id => id == u.Id))
                    .ToArrayAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new List<User>();
            }
        }
    }
}
