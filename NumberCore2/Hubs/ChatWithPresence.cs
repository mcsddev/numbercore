﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace NumberCore2.Hubs
{
    public class ChatWithPresence : HubWithPresence
    {
        public ChatWithPresence(IUserTracker<ChatWithPresence> userTracker)
            : base(userTracker)
        {
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).InvokeAsync("SetUsersOnline", await GetUsersOnline());

            await base.OnConnectedAsync();
        }

        public override Task OnUsersJoined(UserDetails[] users)
        {
            return Clients.Client(Context.ConnectionId).InvokeAsync("UsersJoined", new[] { users });
        }

        public override Task OnUsersLeft(UserDetails[] users)
        {
            return Clients.Client(Context.ConnectionId).InvokeAsync("UsersLeft", new[] { users });
        }

        public async Task Send(string message)
        {
            await Clients.All.InvokeAsync("Send", Context.User.Identity.Name, message);
        }
    }


}