using Grpc.Core;


namespace Messanger.Server.Services
{
    public class MessangerService : Messanger.MessangerBase
    {
        private readonly ILogger<MessangerService> _logger;
        private readonly LoginService _loginService;
        private readonly FriendService _friendService;
        private readonly SendMessageService _sendMessageService;
        private readonly CommonOperationsService _commonOperationsService;

        public MessangerService(ILogger<MessangerService> logger,
            SendMessageService sendMessageService,
            CommonOperationsService commonOperationsService,
            LoginService loginService,
            FriendService friendService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sendMessageService = sendMessageService ?? throw new ArgumentNullException(nameof(sendMessageService));
            _commonOperationsService = commonOperationsService ?? throw new ArgumentNullException(nameof(commonOperationsService));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _friendService = friendService ?? throw new ArgumentNullException(nameof(friendService));
        }

        public override Task<Sended> SendMessage(MessageRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Message recieved from {request.SourceAddress} " +
                $"and will be sended to {request.DestinationAddress}");
            return base.SendMessage(request, context);
        }

        public override Task<ListUsers> GetAllUsers(Empty request, ServerCallContext context)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public override Task<ListUsers> GetFriendsList(UserId request, ServerCallContext context)
        {
            return base.GetFriendsList(request, context);
        }

        public override Task<UserData> GetUser(UserId request, ServerCallContext context)
        {
            return base.GetUser(request, context);
        }

        public override Task<LoginResponse> Login(LoginMessage request, ServerCallContext context)
        {
            return base.Login(request, context);
        }

        public override Task<Empty> Subscribe(SubscribeMessage request, ServerCallContext context)
        {
            return base.Subscribe(request, context);
        }
    }
}
