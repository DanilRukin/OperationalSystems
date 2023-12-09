using Messanger.Client.Presentation;
using Messanger.Client.Presentation.Views;
using Messanger.Client.Presentation.ViewsSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata.Ecma335;

namespace Messanger.Client
{
    internal class Program
    {
        private static IHost _host;
        private static IHost Host => _host ??= Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(config => config.AddJsonFile("appsettings.json", false))
            .ConfigureServices(ConfigureServices)
            .Build();

        private static IConfiguration Configuration => Host.Services
            .GetRequiredService<IConfiguration>();

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services) => services
            .AddSingleton<LoginView>()
            .AddSingleton<MainView>()
            .AddSingleton<SelectInterlocutorView>()
            .AddSingleton<AddFriendView>()
            .AddSingleton<ChatView>()
            .AddSingleton<Presenter>();

        static async Task Main(string[] args)
        {
            ViewSettingsInitializer initializer = new ViewSettingsInitializer();
            initializer.InitFromConfiguration(Configuration);

            await Host.StartAsync();
            Presenter presenter = Host.Services.GetRequiredService<Presenter>();
            presenter.SetView(Host.Services.GetRequiredService<LoginView>());
            presenter.Show();
            await Host.StopAsync();
        }
    }
}
