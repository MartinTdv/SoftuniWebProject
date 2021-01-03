namespace ConnectingPeople.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ConnectingPeople.Data.Models;
    using ConnectingPeople.Common.Enums;
    using ConnectingPeople.Services.Data;
    using Microsoft.AspNetCore.Mvc;
    using ConnectingPeople.Web.ViewModels.HelpTasks;

    public class HelpController : BaseController
    {
        private readonly IHelpTaskService helpTaskService;
        private readonly IProfileService profileService;

        public HelpController(
            IHelpTaskService helpTaskService,
            IProfileService profileService)
        {
            this.helpTaskService = helpTaskService;
            this.profileService = profileService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Offer()
        {
            var viewModel = this.helpTaskService.GetAllOfType<HelpTaskViewModel>(TaskType.Offer)
                .Where(x => x.CreatorUserName != this.User.Identity.Name)
                .Reverse()
                .ToList();
            return this.View(viewModel);
        }

        public IActionResult Need()
        {
            var viewModel = this.helpTaskService.GetAllOfType<HelpTaskViewModel>(TaskType.Need)
                .Where(x => x.CreatorUserName != this.User.Identity.Name)
                .Reverse()
                .ToList();
            return this.View(viewModel);
        }

        public IActionResult StartedTasks()
        {
            var viewModel = this.helpTaskService.GetUserAllStartedTasks(this.User.Identity.Name).ToList();
            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.helpTaskService.Delete(id);
            return this.RedirectToAction("Index", "Profile");
        }
    }
}