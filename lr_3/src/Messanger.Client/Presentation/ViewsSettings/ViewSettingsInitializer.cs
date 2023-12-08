using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Client.Presentation.ViewsSettings
{
    public class ViewSettingsInitializer
    {
        public void InitFromConfiguration(IConfiguration config)
        {
            InitLoginView(config);
            InitAddFriendView(config);
            InitMainView(config);
            InitSelectInterlocutorView(config);
            InitChatView(config);
        }

        private void InitLoginView(IConfiguration config)
        {
            LoginViewSettings.UserInputColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["LoginViewSettings:UserInputColor"]);
            LoginViewSettings.TextColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["LoginViewSettings:TextColor"]);
            LoginViewSettings.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["LoginViewSettings:BackgroundColor"]);
            LoginViewSettings.ErrorsMessagesColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["LoginViewSettings:ErrorsMessagesColor"]);
            LoginViewSettings.InformationMessagesColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["LoginViewSettings:InformationMessagesColor"]);
        }

        private void InitAddFriendView(IConfiguration config)
        {
            AddFriendViewSettings.UserInputColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["AddFriendViewSettings:UserInputColor"]);
            AddFriendViewSettings.TextColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["AddFriendViewSettings:TextColor"]);
            AddFriendViewSettings.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["AddFriendViewSettings:BackgroundColor"]);
            AddFriendViewSettings.ErrorsMessagesColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["AddFriendViewSettings:ErrorsMessagesColor"]);
            AddFriendViewSettings.InformationMessagesColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["AddFriendViewSettings:InformationMessagesColor"]);
        }

        private void InitMainView(IConfiguration config)
        {
            SelectInterlocutorViewSettings.UserInputColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["MainViewSettings:UserInputColor"]);
            MainViewSettings.TextColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["MainViewSettings:TextColor"]);
            MainViewSettings.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["MainViewSettings:BackgroundColor"]);
            MainViewSettings.ErrorsMessagesColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["MainViewSettings:ErrorsMessagesColor"]);
            MainViewSettings.InformationMessagesColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["MainViewSettings:InformationMessagesColor"]);
        }

        private void InitSelectInterlocutorView(IConfiguration config)
        {
            SelectInterlocutorViewSettings.UserInputColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["SelectInterlocutorViewSettings:UserInputColor"]);
            SelectInterlocutorViewSettings.TextColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["SelectInterlocutorViewSettings:TextColor"]);
            SelectInterlocutorViewSettings.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["SelectInterlocutorViewSettings:BackgroundColor"]);
            SelectInterlocutorViewSettings.ErrorsMessagesColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["SelectInterlocutorViewSettings:ErrorsMessagesColor"]);
            SelectInterlocutorViewSettings.InformationMessagesColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["SelectInterlocutorViewSettings:InformationMessagesColor"]);
        }

        private void InitChatView(IConfiguration config)
        {
            ChatViewSettings.UserInputColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["ChatViewSettings:UserInputColor"]);
            ChatViewSettings.TextColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["ChatViewSettings:TextColor"]);
            ChatViewSettings.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["ChatViewSettings:BackgroundColor"]);
            ChatViewSettings.ErrorsMessagesColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["ChatViewSettings:ErrorsMessagesColor"]);
            ChatViewSettings.InformationMessagesColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["ChatViewSettings:InformationMessagesColor"]);
            ChatViewSettings.MessagesFromFriendColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                config["ChatViewSettings:MessagesFromFriendColor"]);
        }
    }
}
