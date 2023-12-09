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
    /// <summary>
    /// Выбор собеседника
    /// </summary>
    public class SelectInterlocutorView : IView
    {
        private IServiceProvider _services;
        private Presenter _presenter;

        public SelectInterlocutorView(IServiceProvider services, Presenter presenter)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        public bool Show()
        {
            IConfiguration config = _services.GetRequiredService<IConfiguration>();
            ConsoleColor textColor = Console.ForegroundColor;
            ConsoleColor backgroundColor = Console.BackgroundColor;
            Console.BackgroundColor = SelectInterlocutorViewSettings.BackgroundColor;
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = SelectInterlocutorViewSettings.TextColor;
                Console.WriteLine("Введите номер друга, с которым хотите пообщаться," +
                    "либо введите ESC для выхода");

                using (var channel = GrpcChannel.ForAddress(config["ServerAddress"]))
                {
                    var client = new Messanger.MessangerClient(channel);
                    var getFriendsRequest = new UserId() { Id = Cash.UserId.ToString() };
                    var friends = client.GetFriendsList(getFriendsRequest);
                    if (friends != null && friends.Users != null)
                    {
                        int friendNumber = 1;
                        foreach (var friend in friends.Users)
                        {
                            Console.WriteLine($"{friendNumber}. {friend.FirstName} {friend.LastName} {friend.Patronymic}");
                            friendNumber++;
                        }
                    }
                    Console.ForegroundColor = SelectInterlocutorViewSettings.UserInputColor;
                    string? command = Console.ReadLine();
                    if (command?.ToLower() == "esc")
                    {
                        _presenter.SetView(_services.GetRequiredService<MainView>());
                        Console.BackgroundColor = backgroundColor;
                        Console.ForegroundColor = textColor;
                        return true;
                    }    
                    if (int.TryParse(command, out int number))
                    {
                        if (number > 0 && number <= friends.Users.Count)
                        {
                            _presenter.SetView(_services.GetRequiredService<ChatView>());
                            Cash.CurrentInterlocutor = Guid.Parse(friends.Users[number - 1].Id);
                            Console.BackgroundColor = backgroundColor;
                            Console.ForegroundColor = textColor;
                            return true;
                        }
                    }
                }
            }
        }
    }
}
