using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messanger.Client;
using Microsoft.Extensions.DependencyInjection;
using Messanger.Client.Data;
using Google.Protobuf.WellKnownTypes;
using Messanger.Client.Presentation.ViewsSettings;

namespace Messanger.Client.Presentation.Views
{
    public class LoginView : IView
    {
        private IServiceProvider _services;
        private Presenter _presenter;

        public LoginView(IServiceProvider services, Presenter presenter)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _presenter = presenter;
        }

        public bool Show()
        {
            string password;
            string login;
            IConfiguration config = _services.GetRequiredService<IConfiguration>();
            int numberOfAttempts = Convert.ToInt32(config["NumberOfAttempts"]);
            ConsoleColor textColor = Console.ForegroundColor;
            ConsoleColor backgroundColor = Console.BackgroundColor;
            Console.BackgroundColor = LoginViewSettings.BackgroundColor;
            int loginAttempt = 1;
            do
            {
                Console.Clear();
                Console.ForegroundColor = LoginViewSettings.TextColor;
                Console.WriteLine($"Попытка залогиниться #{loginAttempt}");
                Console.Write("\tИмя пользователя: ");
                Console.ForegroundColor = LoginViewSettings.UserInputColor;
                login = Console.ReadLine() ?? string.Empty;
                Console.ForegroundColor = LoginViewSettings.TextColor;
                Console.Write("\tПароль: ");
                Console.ForegroundColor = LoginViewSettings.UserInputColor;
                password = Console.ReadLine() ?? string.Empty;
                using (var channel = GrpcChannel.ForAddress(config["ServerAddress"]))
                {
                    var client = new Messanger.MessangerClient(channel);
                    var loginRequest = new LoginMessage() { Login = login, Password = password };
                    var token = client.Login(loginRequest);
                    if (Guid.TryParse(token.UserId, out Guid userId))
                    {
                        if (userId != Guid.Empty)
                        {
                            _presenter.SetView(_services.GetRequiredService<MainView>());
                            Cash.UserId = userId;
                            Console.ForegroundColor = textColor;
                            Console.BackgroundColor = backgroundColor;
                            return true;
                        } 
                    }
                    Console.ForegroundColor = LoginViewSettings.InformationMessagesColor;
                    Console.WriteLine("Неверный логин или пароль. Попробуйте еще раз. Нажмите любую клавишу...");
                    Console.ReadKey(true);
                    Console.ForegroundColor = LoginViewSettings.TextColor;
                }
                loginAttempt++;
                numberOfAttempts--;
            } while (numberOfAttempts > 0);
            Console.ForegroundColor = LoginViewSettings.ErrorsMessagesColor;
            Console.WriteLine("Даун не смог залогиниться. Пошел нахер, черт..." +
                "(нажми что-нибудь, скотина, чтобы я тебя больше не видел)");
            Console.ReadKey(true);
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor;
            return false;
        }
    }
}
