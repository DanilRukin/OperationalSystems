using Messanger.Server.Data;
using Messanger.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Server.Services
{
    public class LoginService
    {
        private readonly MessangerDataContext _dataContext;
        private readonly ILogger<LoginService> _logger;
        public LoginService(MessangerDataContext dbContext, ILogger<LoginService> logger) 
        {
            _dataContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<User?> Login(string username, string password)
        {
            try
            {
                var user = await _dataContext
                .Users
                .FirstOrDefaultAsync(u => u.Login == username && u.Password == password);
                if (user == null)
                    _logger.LogInformation($"Не удалось аутентифицировать пользователя {username}");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }  
        }
    }
}
