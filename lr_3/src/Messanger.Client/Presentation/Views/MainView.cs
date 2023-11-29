﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Client.Presentation.Views
{
    public class MainView : IView
    {
        private IServiceProvider _services;
        private Presenter _presenter;

        public MainView(Presenter presenter, IServiceProvider services)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public bool Show()
        {
            bool continueShow = false;
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("F1 - Найти друга");
                Console.WriteLine("F2 - К списку чатов");
                Console.WriteLine("ESC - Выйти");
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey _ when key == ConsoleKey.F1:
                        _presenter.SetView(_services.GetRequiredService<AddFriendView>());
                        continueShow = true;
                        break;
                    case ConsoleKey _ when key == ConsoleKey.F2:
                        _presenter.SetView(_services.GetRequiredService<SelectInterlocutorView>());
                        continueShow = true;
                        break;
                    case ConsoleKey _ when key == ConsoleKey.Escape:
                        continueShow = false;
                        break;
                    default:
                        continueShow = true;
                        break;
                }
            } while (key != ConsoleKey.Escape);
            return continueShow;
        }
    }
}
