using Google.Protobuf.Collections;
using Grpc.Core;
using Messanger.Server.Extensions;


namespace Messanger.Server.Services
{
    public class MessangerService : Messanger.MessangerBase
    {
        private readonly ILogger<MessangerService> _logger;
        private readonly LoginService _loginService;
        private readonly FriendService _friendService;
        private readonly SendMessageService _sendMessageService;
        private readonly CommonOperationsService _commonOperationsService;
        private readonly FetchService _fetchService;

        public MessangerService(ILogger<MessangerService> logger,
            SendMessageService sendMessageService,
            CommonOperationsService commonOperationsService,
            LoginService loginService,
            FriendService friendService,
            FetchService fetchService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sendMessageService = sendMessageService ?? throw new ArgumentNullException(nameof(sendMessageService));
            _commonOperationsService = commonOperationsService ?? throw new ArgumentNullException(nameof(commonOperationsService));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _friendService = friendService ?? throw new ArgumentNullException(nameof(friendService));
            _fetchService = fetchService ?? throw new ArgumentNullException(nameof(fetchService));
        }

        public override async Task<Sended> SendMessage(MessageRequest request, ServerCallContext context)
        {
            try
            {
                return new Sended() { Success = await _sendMessageService.SendMessage(request) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Sended() { Success = false };
            }
        }

        public override async Task<ListUsers> GetAllUsers(Empty request, ServerCallContext context)
        {
            try
            {
                ListUsers users = new ListUsers();
                users.Users.AddRange((await _commonOperationsService.GetAllUsers())
                    ?.Select(u => new UserData() 
                        { 
                            Id = u.Id.ToString(),
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Patronymic = u.Patronymic 
                        }));
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ListUsers();
            }
        }

        public override async Task<ListUsers> GetFriendsList(UserId request, ServerCallContext context)
        {
            try
            {
                ListUsers friends = new ListUsers();
                friends.Users.AddRange((await _commonOperationsService
                    .GetFriendsOfUser(new Model.User() { Id = Guid.Parse(request.Id) }))
                    ?.Select(u => new UserData()
                    {
                        Id = u.Id.ToString(),
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Patronymic = u.Patronymic
                    }));
                return friends;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ListUsers();
            }
        }

        public override async Task<UserData> GetUser(UserId request, ServerCallContext context)
        {
            try
            {
                var user = await _commonOperationsService.GetUserById(Guid.Parse(request.Id));
                if (user != null)
                    return new UserData()
                    {
                        Id = user.Id.ToString(),
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Patronymic = user.Patronymic
                    };
                _logger.LogInformation($"Пользователь с id = '{request.Id}' не найден в базе");
                return new UserData(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new UserData();
            }
        }

        public override async Task<LoginResponse> Login(LoginMessage request, ServerCallContext context)
        {
            try
            {
                var result = await _loginService.Login(request.Login, request.Password);
                if (result != null)
                    return new LoginResponse()
                    {
                        Success = true,
                        UserId = result.Id.ToString()
                    };
                _logger.LogInformation($"Не удалось аутентифицировать пользователя '{request.Login}'");
                return new LoginResponse() { Success = false, UserId = Guid.Empty.ToString() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new LoginResponse() { Success = false, UserId = Guid.Empty.ToString() };
            }
        }

        public override async Task<Empty> Subscribe(SubscribeMessage request, ServerCallContext context)
        {
            try
            {
                await _friendService.Subscribe(Guid.Parse(request.Subscriber.Id),
                    Guid.Parse(request.Subcribed.Id));
                return new Empty();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Empty();
            }
        }

        public override async Task<FetchMessageResponse> FetchMessages(FetchMessageRequest request, ServerCallContext context)
        {
            try
            {
                var result = await _fetchService.FetchMessages(new Model.User() { Id = Guid.Parse(request.SenderId) },
                    new Model.User() { Id = Guid.Parse(request.RecieverId) });
                if (result != null)
                {
                    var response = new FetchMessageResponse();
                    response.Messages.AddRange(result);
                    return response;
                }
                _logger.LogInformation($"Сообщений для пользователя '{request.RecieverId}'" +
                    $" от пользователя '{request.SenderId}' не найдено");
                return new FetchMessageResponse();
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new FetchMessageResponse();
            }
        }
    }
}
