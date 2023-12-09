using Grpc.Net.Client;
using Messanger.Client.Data;
using Messanger.Client.Presentation.ViewsSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Client.Presentation.Views
{
    public class ChatView : IView
    {
        private IServiceProvider _services;
        private Presenter _presenter;

        public ChatView(IServiceProvider services, Presenter presenter)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        public bool Show()
        {
            Console.Clear();
            IConfiguration config = _services.GetRequiredService<IConfiguration>();
            UserData userData;
            ConsoleColor textColor = Console.ForegroundColor;
            ConsoleColor backgroundColor = Console.BackgroundColor;
            Console.BackgroundColor = ChatViewSettings.BackgroundColor;
            using (var channel = GrpcChannel.ForAddress(config["ServerAddress"]))
            {
                var client = new Messanger.MessangerClient(channel);
                userData = client.GetUser(new UserId() { Id = Cash.CurrentInterlocutor.ToString() });
                if (userData == null)
                {
                    Console.ForegroundColor = ChatViewSettings.ErrorsMessagesColor;
                    Console.WriteLine("Не удалось получить собеседника...Нажмите любую клавишу, чтобы вернуться к списку чатов");
                    Console.ReadKey(true);
                    Console.ForegroundColor = textColor;
                    Console.BackgroundColor = backgroundColor;
                    _presenter.SetView(_services.GetRequiredService<SelectInterlocutorView>());
                    return true;
                }
            }
            string message;
            Console.ForegroundColor = ChatViewSettings.TextColor;
            Console.WriteLine($"Чат с пользователем {userData.FirstName} {userData.LastName} {userData.Patronymic}");
            using (var channel = GrpcChannel.ForAddress(config["ServerAddress"]))
            {
                var client = new Messanger.MessangerClient(channel);
                while (true)
                {
                    Console.Write("Ваше сообщение (esc для выхода): ");
                    Console.ForegroundColor = ChatViewSettings.UserInputColor;
                    message = Console.ReadLine() ?? "";
                    Console.ForegroundColor = ChatViewSettings.TextColor;
                    if (message.ToLower() == "esc")
                    {
                        _presenter.SetView(_services.GetRequiredService<SelectInterlocutorView>());
                        Cash.CurrentInterlocutor = Guid.Empty;
                        Console.ForegroundColor = textColor;
                        Console.BackgroundColor = backgroundColor;
                        return true;
                    }
                    bool result = client.SendMessage(new MessageRequest() 
                    { 
                        Message = message,
                        SourceAddress = Cash.UserId.ToString(),
                        DestinationAddress = userData.Id
                    }).Success;
                    if (!result)
                    {
                        Console.ForegroundColor = ChatViewSettings.ErrorsMessagesColor;
                        Console.WriteLine("Не удалось отправить сообщение...");
                        Console.ForegroundColor = ChatViewSettings.TextColor;
                    }
                    var fetchResponse = client.FetchMessages(new FetchMessageRequest()
                    {
                        RecieverId = Cash.CurrentInterlocutor.ToString(), // отправителем в данном случае должен быть текущий собеседник
                        SenderId = Cash.UserId.ToString() // получателем - текущий пользователь
                    });
                    Console.ForegroundColor = ChatViewSettings.MessagesFromFriendColor;
                    foreach (var fetchedMessage in fetchResponse.Messages)
                    {
                        Console.WriteLine("\t\t" + fetchedMessage + $" (от {userData.FirstName} {userData.LastName} {userData.Patronymic})");
                    }
                    Console.ForegroundColor = ChatViewSettings.TextColor;
                }
            }
        }
    }
}
