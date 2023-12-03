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
            do
            {
                Console.Clear();
                Console.WriteLine($"Попытка залогиниться #{numberOfAttempts}");
                Console.Write("\tИмя пользователя: ");
                login = Console.ReadLine() ?? string.Empty;
                Console.Write("\tПароль: ");
                password = Console.ReadLine() ?? string.Empty;
                using (var channel = GrpcChannel.ForAddress(config["ServerAddress"]))
                {
                    var client = new Messanger.MessangerClient(channel);
                    var loginRequest = new LoginMessage() { Login = login, Password = password };
                    var token = client.Login(loginRequest);
                    if (Guid.TryParse(token.UserId, out Guid userId))
                    {
                        _presenter.SetView(_services.GetRequiredService<MainView>());
                        Cash.UserId = userId;
                        return true;
                    }
                }
                numberOfAttempts--;
            } while (numberOfAttempts > 0);
            Console.WriteLine("Даун не смог залогиниться. Пошел нахер, черт..." +
                "(нажми что-нибудь, скотина, чтобы я тебя больше не видел)");
            Console.ReadKey(true);
            return false;
        }
    }
}
