﻿using Google.Protobuf.Collections;
using Grpc.Net.Client;
using Messanger.Client.Data;
using Messanger.Client.Presentation.ViewsSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Client.Presentation.Views
{
    public class AddFriendView : IView
    {
        private IServiceProvider _services;
        private Presenter _presenter;

        public AddFriendView(IServiceProvider services, Presenter presenter)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        public bool Show()
        {
            IConfiguration config = _services.GetRequiredService<IConfiguration>();
            ConsoleColor textColor = Console.ForegroundColor;
            ConsoleColor backgroundColor = Console.BackgroundColor;
            Console.BackgroundColor = AddFriendViewSettings.BackgroundColor;
            using (var channel = GrpcChannel.ForAddress(config["ServerAddress"]))
            {
                var client = new Messanger.MessangerClient(channel);
                var empty = new Empty();
                while (true)
                {
                    Console.ForegroundColor = AddFriendViewSettings.TextColor;
                    Console.Clear();
                    Console.WriteLine("Введите номер человека, с которым хотите подружиться," +
                        "либо введите ESC для выхода");

                    var users = client.GetAllUsers(empty);
                    var friends = client.GetFriendsList(new UserId() { Id = Cash.UserId.ToString() });
                    var uniqueUsers = ExcludeFriends(users, friends, Cash.UserId);
                    if (uniqueUsers != null)
                    {
                        int friendNumber = 1;
                        foreach (var friend in uniqueUsers)
                        {
                            Console.WriteLine($"{friendNumber}. {friend.FirstName} {friend.LastName} {friend.Patronymic}");
                            friendNumber++;
                        }
                    }
                    Console.ForegroundColor = AddFriendViewSettings.UserInputColor;
                    string? command = Console.ReadLine();
                    if (command?.ToLower() == "esc")
                    {
                        _presenter.SetView(_services.GetRequiredService<MainView>());
                        Console.ForegroundColor = textColor;
                        Console.BackgroundColor = backgroundColor;
                        return true;
                    }
                    if (int.TryParse(command, out int number))
                    {
                        if (number > 0 && number <= uniqueUsers?.Count)
                        {
                            var selectedUser = uniqueUsers[number - 1];
                            Console.ForegroundColor = AddFriendViewSettings.TextColor;
                            client.Subscribe(new SubscribeMessage()
                            {
                                Subscriber = new UserId() { Id = Cash.UserId.ToString() },
                                Subcribed = new UserId() { Id = selectedUser.Id }
                            });
                            Console.WriteLine($"Вы отправили заявку человеку: '{selectedUser.FirstName} {selectedUser.LastName} {selectedUser.Patronymic}'." +
                                $" Для продолжения нажмите что-нибудь...");
                            Console.ReadKey(true);
                            _presenter.SetView(_services.GetRequiredService<MainView>());
                            Console.ForegroundColor = textColor;
                            Console.BackgroundColor = backgroundColor;
                            return true;
                        }
                    }
                } 
            }
        }

        private RepeatedField<UserData> ExcludeFriends(ListUsers users, ListUsers friends, Guid self)
        {
            var uniqueUsers = users.Users.Except(friends.Users, new Comparer()).ToList();
            uniqueUsers.Remove(uniqueUsers.First(u => u.Id == self.ToString()));
            var result = new RepeatedField<UserData>();
            foreach (var user in uniqueUsers)
            {
                result.Add(user);
            }
            return result;
        }

        private class Comparer : IEqualityComparer<UserData>
        {
            public bool Equals(UserData? x, UserData? y) => x?.Id == y?.Id;

            public int GetHashCode([DisallowNull] UserData obj) => obj.GetHashCode();
        }
    }
}
