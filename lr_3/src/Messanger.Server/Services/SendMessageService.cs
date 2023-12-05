using Messanger.Server.Data;
using Messanger.Server.Extensions;

namespace Messanger.Server.Services
{
    public class SendMessageService
    {
        private readonly ILogger<SendMessageService> _logger;
        private readonly MessangerDataContext _dataContext;

        public SendMessageService(ILogger<SendMessageService> logger, MessangerDataContext dataContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<bool> SendMessage(MessageRequest messageRequest)
        {
            try
            {
                await _dataContext.Messages.AddAsync(messageRequest.ToMessage());
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
