using Messanger.Server.Data;
using Messanger.Server.Model;

namespace Messanger.Server.Services
{
    public class FriendService
    {
        private readonly ILogger<FriendService> _logger;
        private readonly MessangerDataContext _dataContext;

        public FriendService(ILogger<FriendService> logger, MessangerDataContext dataContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task Subscribe(Guid subscriberId, Guid subscribedId)
        {
            try
            {
                if (_dataContext.FriendRequests.Any(fr => fr.RequestSenderId == subscriberId && fr.RequestRecieverId == subscribedId))
                {
                    _logger.LogInformation($"Пользователи '{subscriberId}' и '{subscribedId}' уже подписаны друг на друга");
                    return;
                }
                await _dataContext
                    .FriendRequests
                    .AddAsync(new FriendRequest { RequestSenderId = subscriberId, RequestRecieverId = subscribedId, Accepted = true });
                await _dataContext.SaveChangesAsync();
                _logger.LogInformation($"Пользователь '{subscriberId}' подписался на пользователя '{subscribedId}'");
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
