using Grpc.Core;


namespace Messanger.Server.Services
{
    public class MessangerService : Messanger.MessangerBase
    {
        private ILogger<MessangerService> _logger;

        public MessangerService(ILogger<MessangerService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override Task<Sended> SendMessage(MessageRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Message recieved from {request.SourceAddress} " +
                $"and will be sended to {request.DestinationAddress}");
            return base.SendMessage(request, context);
        }
    }
}
