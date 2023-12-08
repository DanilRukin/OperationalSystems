using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Client.Presentation.ViewsSettings
{
    public class AddFriendViewSettings
    {
        public static ConsoleColor TextColor { get; set; }
        public static ConsoleColor BackgroundColor { get; set; }
        public static ConsoleColor UserInputColor { get; set; }
        public static ConsoleColor ErrorsMessagesColor { get; internal set; }
        public static ConsoleColor InformationMessagesColor { get; internal set; }
    }
}
