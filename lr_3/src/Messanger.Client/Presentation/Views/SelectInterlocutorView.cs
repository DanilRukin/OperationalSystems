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
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("Введите номер друга, с которым хотите пообщаться," +
                    "либо нажмите ESC для выхода");
                key = Console.ReadKey(true).Key;
                if (key != ConsoleKey.Escape)
                {

                }
            } while (key != ConsoleKey.Escape);
            return true;
        }
    }
}
