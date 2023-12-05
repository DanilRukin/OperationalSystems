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
            var user = await _dataContext
                .Users
                .FirstOrDefaultAsync(u => u.Login == username && u.Password == password);
            return user;
        }
    }
}
