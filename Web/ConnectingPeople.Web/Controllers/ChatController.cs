namespace ConnectingPeople.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ConnectingPeople.Data;
    using ConnectingPeople.Data.Models;
    using ConnectingPeople.Services.Data;
    using ConnectingPeople.Web.ViewModels.Chat;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;

    public class ChatController : BaseController
    {
        private readonly IChatService chatService;

        public ChatController(
            IChatService chatService)
        {
            this.chatService = chatService;
        }
        [Authorize]
        public IActionResult All()
        {
            var viewModel = this.chatService.MapUserChats<UserSummaryChatViewModel>(this.User.Identity.Name);
            foreach (var model in viewModel)
            {
                var chatPartner = this.chatService.GetUserChatByUserId(model.Id)
                    .User.FirstOrDefault(u => u.UserName != this.User.Identity.Name);
                model.ChatPartner = chatPartner.UserName;
                model.ChatPartnerImageName = chatPartner.ImageName;
            }
            viewModel = viewModel.Reverse().ToList();
            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult ChatById(int id)
        {
            var viewModel = this.chatService.MapChatById<ChatByIdViewModel>(id);
            return this.View(viewModel);
        }
    }
}