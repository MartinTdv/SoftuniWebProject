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
    using ConnectingPeople.Services.Data.Models;
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
            if (!ModelState.IsValid)
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
        public async Task<IActionResult> StartHelpTask(int helpTaskId, string partnerId)
        {
            await this.helpTaskService.StartHelpTaskAsync(helpTaskId, partnerId);
            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult Finish(int id)
        {
            var helpTask = this.helpTaskService.GetHelpTaskById(id);
            var viewModel = new FinishFormInputModel
            {
                HelpTaskId = id,
                CreatorUsername = helpTask.CreatorUserName,
                About = helpTask.Title,
                OthersideUsername = helpTask.PartnerUserName,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Finish(FinishFormInputModel input)
        {
            var isCreator = input.CreatorComment != null;
            var comment = isCreator ? input.CreatorComment : input.OthersideComment;
            var rating = isCreator ? input.CreatorRating : input.OthersideRating;
            var username = isCreator ? input.CreatorUsername : input.OthersideUsername;

            await this.helpTaskService.FinishHelpTaskAsync(rating, comment, username, input.HelpTaskId);
            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult Rate(int id)
        {
            var rateFormPageDTO = this.helpTaskService.GetRateFormPageDTOByRatingId(id);
            var currentUserName = this.User.Identity.Name;
            if (rateFormPageDTO.CreatorUsername != currentUserName && rateFormPageDTO.OthersideUsername != currentUserName)
            {
                return this.RedirectToAction("Index", "Home");
            }
            else if (rateFormPageDTO.CreatorUsername == currentUserName && rateFormPageDTO.CreatorComment != null)
            {
                return this.RedirectToAction("Index", "Home");
            }
            else if (rateFormPageDTO.OthersideUsername == currentUserName && rateFormPageDTO.OthersideComment != null)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var viewModel = new RateFormInputModel
            {
                RatingId = id,
                About = rateFormPageDTO.Title,
                CurrentUserName = currentUserName,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Rate(RateFormInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Rate", new { id = input.RatingId });
            }
            await this.helpTaskService.AddCommentAndRating(input.RatingId, input.CurrentUserName, input.Comment, input.Rating);
            return this.RedirectToAction("StartedTasks", "Help");
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