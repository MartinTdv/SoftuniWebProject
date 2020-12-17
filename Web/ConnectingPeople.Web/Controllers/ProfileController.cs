namespace ConnectingPeople.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using ConnectingPeople.Common.Enums;
    using ConnectingPeople.Data.Common.Repositories;
    using ConnectingPeople.Data.Models;
    using ConnectingPeople.Services.Data;
    using ConnectingPeople.Web.ViewModels.Profile;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    
    public class ProfileController : BaseController
    {
        private readonly IProfileService profileService;

        public ProfileController(
            IProfileService profileService)
        {
            this.profileService = profileService;
        }

        [Authorize]
        public IActionResult Index()
        {
            return this.RedirectToAction("ByName", new { username = this.User.Identity.Name });
        }

        [Authorize]
        public IActionResult ByName(string username, string search, TaskType? type, bool? isActive)
        {
            ProfileViewModel viewModel;
            if (type != null)
            {
                search = search == null ? string.Empty : search;
                viewModel = this.profileService.GetProfileInfoByNameWithSearch(username, search, (TaskType) type, isActive);
            }
            else
            {
                viewModel = this.profileService.GetProfileInfoByName(username);
            }

            viewModel.HelpTasks = viewModel.HelpTasks.Reverse();
            return this.View(viewModel);
        }
    }
}
