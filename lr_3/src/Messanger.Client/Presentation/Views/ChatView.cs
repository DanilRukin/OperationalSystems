using Grpc.Net.Client;
using Messanger.Client.Data;
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
            using (var channel = GrpcChannel.ForAddress(config["ServerAddress"]))
            {
                var client = new Messanger.MessangerClient(channel);
                userData = client.GetUser(new UserId() { Id = Cash.CurrentInterlocutor.ToString() });
                if (userData == null)
                {
                    Console.WriteLine("Не удалось получить собеседника...Нажмите любую клавишу, чтобы вернуться к списку чатов");
                    Console.ReadKey(true);
                    _presenter.SetView(_services.GetRequiredService<SelectInterlocutorView>());
                    return true;
                }
            }
            string message;
            Console.WriteLine($"Чат с пользователем {userData.FirstName} {userData.LastName} {userData.Patronymic}");
            using (var channel = GrpcChannel.ForAddress(config["ServerAddress"]))
            {
                var client = new Messanger.MessangerClient(channel);
                while (true)
                {
                    Console.Write("Ваше сообщение (esc для выхода): ");
                    message = Console.ReadLine() ?? "";
                    if (message.ToLower() == "esc")
                    {
                        _presenter.SetView(_services.GetRequiredService<SelectInterlocutorView>());
                        Cash.CurrentInterlocutor = Guid.Empty;
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
                        ConsoleColor color = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Не удалось отправить сообщение...");
                        Console.ForegroundColor = color;
                    }
                }
            }
        }
    }
}
