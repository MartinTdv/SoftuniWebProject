using ConnectingPeople.Services.Data;
using ConnectingPeople.Web.Models;
using ConnectingPeople.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ConnectingPeople.Web.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService chatService;
        public ChatHub(IChatService chatService)
        {
            this.chatService = chatService;
        }
        public async Task Send(string messageText)
        {
            if (string.IsNullOrEmpty(messageText))
            {
                return;
            }
            var chatId = int.Parse(this.Context.GetHttpContext().Request.RouteValues["id"].ToString());
            var chat = this.chatService.GetChatById(chatId);
            var message = new Data.Message
            {
                ChatId = chatId,
                SenderUsername = this.Context.User.Identity.Name,
                Text = messageText,
            };
            await this.chatService.SaveMessageToChat(message);

            await this.Clients.Client(this.Context.ConnectionId).SendAsync(
                "MyMessage",
                new Models.Message
                {
                    User = message.SenderUsername,
                    Text = message.Text,
                });

            await this.Clients.OthersInGroup(chat.ChatGroupName).SendAsync(
                "NewMessage",
                new Models.Message
                {
                    User = message.SenderUsername,
                    Text = message.Text,
                });
        }

        public override async Task OnConnectedAsync()
        {
            var chatId = int.Parse(this.Context.GetHttpContext().Request.RouteValues["id"].ToString());
            var chat = await this.chatService.BindConnectionIdToUserAsync(chatId, this.Context.User.Identity.Name, this.Context.ConnectionId);

                if(this.Context.User.Identity.Name == chat.OthersideUsername)
                {
                    await this.Groups.AddToGroupAsync(chat.OthersideConnectionId, chat.ChatGroupName);
                }
                else
                {
                    await this.Groups.AddToGroupAsync(chat.TaskCreatorConnectionId, chat.ChatGroupName);
                }
            await base.OnConnectedAsync();
        }
    }
}
