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

        public HelpController(IHelpTaskService helpTaskService)
        {
            this.helpTaskService = helpTaskService;
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

        public IActionResult Delete(int id)
        {
            this.helpTaskService.Delete(id);
            return this.RedirectToAction("Index", "Home");
        }
    }
}