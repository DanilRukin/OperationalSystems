using Messanger.Server.Data;
using Messanger.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Server.Services
{
    public class FetchService
    {
        private readonly ILogger<FetchService> _logger;
        private readonly MessangerDataContext _dataContext;

        public FetchService(ILogger<FetchService> logger, MessangerDataContext dataContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<IEnumerable<string>> FetchMessages(User fromUser, User toUser)
        {
            try
            {
                _logger.LogInformation($"Вытягивание сообщений для пользователя " +
                    $"'{toUser.Id}' от пользователя '{fromUser.Id}'");
                List<string> messages = await _dataContext
                    ?.Messages
                    ?.Where(m =>
                        m.RecieverId == toUser.Id
                        && m.SenderId == fromUser.Id
                        && m.WasSended == false)
                    ?.Select(m => m.Text)
                    ?.ToListAsync();
                if (messages == null)
                {
                    _logger.LogInformation("Сообщения не найдены");
                    return new List<string>();
                }
                _logger.LogInformation($"Найдено сообщений: {messages.Count}");
                return messages;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return new List<string>();
            }
        }
    }
}
