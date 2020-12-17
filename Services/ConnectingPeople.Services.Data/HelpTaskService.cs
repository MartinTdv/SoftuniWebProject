using ConnectingPeople.Common.Enums;
using ConnectingPeople.Data.Common.Enums;
using ConnectingPeople.Data.Common.Repositories;
using ConnectingPeople.Data.Models;
using ConnectingPeople.Services.Data.Models;
using ConnectingPeople.Services.Mapping;
using ConnectingPeople.Web.ViewModels.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConnectingPeople.Services.Data
{
    public class HelpTaskService : IHelpTaskService
    {
        private readonly IDeletableEntityRepository<HelpTask> helpTaskRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IItemService itemService;

        public HelpTaskService(
            IDeletableEntityRepository<HelpTask> helpTaskRepository,
            UserManager<ApplicationUser> userManager,
            IItemService itemService)
        {
            this.helpTaskRepo = helpTaskRepository;
            this.userManager = userManager;
            this.itemService = itemService;
        }

        public async Task CreateNeedAsync(NeedHelpFormInputModel input, [CallerMemberName] string callerMethodName = "")
        {

            var helpTask = new HelpTask
            {
                Title = input.Title,
                Description = input.Description,
                Location = input.Location,
                ImageName = input.ImageName,
                Type = input.TaskType,
                Creator = input.Creator,
            };

            // get selected items by their id's.
            var mandatoryItems = this.itemService.GetAll().Where(i => input.MandatoryItemsId.Contains(i.Id)).ToList();
            var helpfulItems = this.itemService.GetAll().Where(i => input.HelpfulItemsId.Contains(i.Id)).ToList();

            // add mandatory items to database .
            foreach (var item in mandatoryItems)
            {
                helpTask.Items.Add(new HelpTaskItems()
                {
                    ItemId = item.Id,
                    ItemUseType = ItemUseType.Mandatory,
                });
            }

            // add helpfull items to database.
            foreach (var item in helpfulItems)
            {
                helpTask.Items.Add(new HelpTaskItems()
                {
                    ItemId = item.Id,
                    ItemUseType = ItemUseType.Helpful,
                });
            }

            await this.helpTaskRepo.AddAsync(helpTask);
            await this.helpTaskRepo.SaveChangesAsync();

        }

        public async Task CreateOfferAsync(OfferHelpFormInputModel input, [CallerMemberName] string callerMethodName = "")
        {
            var helpTask = new HelpTask
            {
                Title = input.Title,
                Description = input.Description,
                Location = input.Location,
                ImageName = input.ImageName,
                Type = input.TaskType,
                Creator = input.Creator,
            };

            // get selected items by their id's.
            var availableItems = this.itemService.GetAll().Where(i => input.AvailableItemsId.Contains(i.Id)).ToList();

            // add available items to database.
            foreach (var item in availableItems)
            {
                helpTask.Items.Add(new HelpTaskItems()
                {
                    ItemId = item.Id,
                    ItemUseType = ItemUseType.Available,
                });
            }

            await this.helpTaskRepo.AddAsync(helpTask);
            await this.helpTaskRepo.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var helpTask = await this.helpTaskRepo.All()
                .Where(ht => ht.Id == id)
                .FirstOrDefaultAsync();

            this.helpTaskRepo.Delete(helpTask);

            await this.helpTaskRepo.SaveChangesAsync();
        }

        public ICollection<T> GetAllByUsername<T>(string username)
        {
            return this.helpTaskRepo.AllAsNoTracking()
                .Where(x => x.Creator.UserName == username)
                .To<T>()
                .ToList();
        }

        public ICollection<T> GetAllOfType<T>(TaskType taskType)
        {
            return this.helpTaskRepo.AllAsNoTracking()
                .Where(x => x.Type == taskType)
                .To<T>()
                .ToList();
        }

        public TitleAndCreatorUsernameDTO GetTitleAndCreatorUsernameById(int id)
        {
            return this.helpTaskRepo.AllAsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new TitleAndCreatorUsernameDTO
                {
                     Title = x.Title, 
                     Username = x.Creator.UserName,
                })
                .FirstOrDefault();
        }
    }
}
