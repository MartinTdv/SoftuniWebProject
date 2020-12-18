namespace ConnectingPeople.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using ConnectingPeople.Common.Enums;
    using ConnectingPeople.Data;
    using ConnectingPeople.Data.Models;
    using ConnectingPeople.Services.Data;
    using ConnectingPeople.Web.ViewModels.Forms;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using SixLabors.ImageSharp;

    public class FormsController : BaseController
    {
        private readonly IWebHostEnvironment webHostEnviroment;
        private readonly IItemService itemService;
        private readonly IHelpTaskService helpTaskService;
        private readonly IProfileService profileService;
        private readonly IChatService chatService;
        private readonly UserManager<ApplicationUser> userManager;

        public FormsController(
            IItemService itemService,
            IHelpTaskService helpTaskService,
            IChatService chatService,
            IProfileService profileService,
            IWebHostEnvironment webHostEnviroment,
            UserManager<ApplicationUser> userManager)
        {
            this.itemService = itemService;
            this.helpTaskService = helpTaskService;
            this.chatService = chatService;
            this.profileService = profileService;
            this.webHostEnviroment = webHostEnviroment;
            this.userManager = userManager;
        }


        [Authorize]
        public IActionResult Offer()
        {
            var items = this.itemService.MapAll<SelectedItemsInputModel>();
            var viewModel = new OfferHelpFormInputModel
            {
                Items = items,
            };
            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Offer(OfferHelpFormInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            // get selected items Id's to list.
            var selectedAvailable = input.Items.Where(i => i.SelectedAsAvailable).Select(i => i.Id).ToList();

            // save image in root folder and get it's name.
            if (input.Image != null)
            {
                this.SaveImage(input.Image);
            }

            input.Creator = await this.userManager.GetUserAsync(this.User);
            input.TaskType = TaskType.Offer;
            input.AvailableItemsId = selectedAvailable;

            await this.helpTaskService.CreateOfferAsync(input);
            return this.RedirectToAction("Index", "Profile");
        }

        [Authorize]
        public IActionResult Need()
        {
            var items = this.itemService.MapAll<SelectedItemsInputModel>();
            var viewModel = new NeedHelpFormInputModel
            {
                Items = items,
            };
            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Need(NeedHelpFormInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            // get selected items Id's to list.
            var selectedMandatory = input.Items.Where(i => i.SelectedAsMandatory).Select(i => i.Id).ToList();
            var selectedHelpful = input.Items.Where(i => i.SelectedAsHelpful).Select(i => i.Id).ToList();

            // remove items selected as helpfull if selected as mandatory too.
            foreach (var itemId in selectedMandatory)
            {
                if (selectedHelpful.Contains(itemId))
                {
                    selectedHelpful.Remove(itemId);
                }
            }

            input.MandatoryItemsId = selectedMandatory;
            input.HelpfulItemsId = selectedHelpful;

            // save image in root folder and get it's name.
            if (input.Image != null)
            {
                input.ImageName = this.SaveImage(input.Image);
            }

            input.Creator = await this.userManager.GetUserAsync(this.User);
            input.TaskType = TaskType.Need;

            await this.helpTaskService.CreateNeedAsync(input);
            return this.RedirectToAction("Index", "Profile");
        }

        [Authorize]
        public IActionResult Apply(int id)
        {
            var titleAndCreatorUsernameDTO = this.helpTaskService.GetTitleAndCreatorUsernameById(id);
            var viewModel = new ApplyFormInputModel
            {
                HelpTaskId = id,
                About = titleAndCreatorUsernameDTO.Title,
                TaskCreatorUsername = titleAndCreatorUsernameDTO.Username,
                OthersideUsername = this.User.Identity.Name,
            };
            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Apply(ApplyFormInputModel input)
        {
            if(string.IsNullOrWhiteSpace(input.MessageText))
            {
                return this.Redirect(this.Request.GetDisplayUrl());
            }
            var chatId = await this.chatService.CreateChatAsync(input);
            return this.RedirectToAction("ChatById", "Chat", new { id = chatId });
        }

        [Authorize]
        public IActionResult StartHelpTask(string partnerUsername, int helpTask)
        {
            var helpTaskTitleAndCreator = this.helpTaskService.GetTitleAndCreatorUsernameById(helpTask);
            var partnerId = this.profileService.GetUserIdByUsername(partnerUsername);
            var viewModel = new StartHelpTaskFormInputModel
            {
                CreatorUsername = helpTaskTitleAndCreator.Username,
                Title = helpTaskTitleAndCreator.Title,
                HelpTaskId = helpTask,
                PartnerId = partnerId,
                PartnerUsername = partnerUsername,
            };
            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult StartHelpTask(int helpTaskId, string partnerId)
        {
            this.helpTaskService.StartHelpTask(helpTaskId, partnerId);
            return this.RedirectToAction("Index", "Home");
        }

        private string SaveImage(IFormFile image)
        {
            Image img;
            try
            {
                img = Image.Load(image.OpenReadStream());
            }
            catch
            {
                return null;
            }
            var imageId = Guid.NewGuid().ToString();
            img.Save($"{this.webHostEnviroment.WebRootPath}/helpTaskPics/{imageId}.png");
            return imageId;
        }
    }
}