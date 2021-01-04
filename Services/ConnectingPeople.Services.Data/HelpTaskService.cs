using ConnectingPeople.Common.Enums;
using ConnectingPeople.Data.Common.Enums;
using ConnectingPeople.Data.Common.Repositories;
using ConnectingPeople.Data.Models;
using ConnectingPeople.Services.Data.Models;
using ConnectingPeople.Services.Mapping;
using ConnectingPeople.Web.ViewModels.Forms;
using ConnectingPeople.Web.ViewModels.HelpTasks;
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
        private readonly IDeletableEntityRepository<Rating> ratingRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IItemService itemService;

        public HelpTaskService(
            IDeletableEntityRepository<HelpTask> helpTaskRepository,
            IDeletableEntityRepository<Rating> ratingRepository,
            UserManager<ApplicationUser> userManager,
            IItemService itemService)
        {
            this.helpTaskRepo = helpTaskRepository;
            this.ratingRepo = ratingRepository;
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
            var helpTask = this.helpTaskRepo.All()
                .Where(ht => ht.Id == id)
                .FirstOrDefault();

            this.helpTaskRepo.Delete(helpTask);
            await this.helpTaskRepo.SaveChangesAsync();
        }

        public async Task FinishHelpTaskAsync(int rating, string comment, string username, int helpTaskId)
        {
            var helpTask = this.helpTaskRepo.All()
                .Where(x => x.Id == helpTaskId)
                .Select(x => new
                {
                    x.Id,
                    x.RatingId,
                    OthersideUsername = x.Partner.UserName,
                    CreatorUsername = x.Creator.UserName,
                })
                .FirstOrDefault(x => x.Id == helpTaskId);

            var taskRating = new Rating();

            if (helpTask.CreatorUsername == username)
            {
                taskRating.CreatorComment = comment;
                taskRating.CreatorRating = rating;
                switch (rating > 7 ? "green" :
                        rating > 3 ? "yellow" : "red")
                {
                    case "green":
                        taskRating.CreatorRatingColorClass = "btn-success";
                        break;
                    case "yellow":
                        taskRating.CreatorRatingColorClass = "btn-warning";
                        break;
                    case "red":
                        taskRating.CreatorRatingColorClass = "btn-danger";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                taskRating.OthersideComment = comment;
                taskRating.OthersideRating = rating;
                switch (rating > 7 ? "green" :
                        rating > 3 ? "yellow" : "red")
                {
                    case "green":
                        taskRating.OthersideRatingColorClass = "btn-success";
                        break;
                    case "yellow":
                        taskRating.OthersideRatingColorClass = "btn-warning";
                        break;
                    case "red":
                        taskRating.OthersideRatingColorClass = "btn-danger";
                        break;
                    default:
                        break;
                }
            }
            taskRating.OthersideUsername = helpTask.OthersideUsername;
            taskRating.TaskId = helpTask.Id;
            await this.ratingRepo.AddAsync(taskRating);
            await this.ratingRepo.SaveChangesAsync();

            var ratingId = this.ratingRepo.AllAsNoTracking()
                .Where(x => x.TaskId == helpTask.Id)
                .Select(x => x.Id)
                .FirstOrDefault();

            this.helpTaskRepo.All()
                .FirstOrDefault(x => x.Id == helpTaskId)
                .RatingId = ratingId;
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
                .Where(x => x.Type == taskType && x.PartnerId == null)
                .To<T>()
                .ToList();
        }

        public CreatorAndPartnerUsernamesAndTitleDTO GetHelpTaskById(int id)
        {
            return this.helpTaskRepo.AllAsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new CreatorAndPartnerUsernamesAndTitleDTO
                {
                    CreatorUserName = x.Creator.UserName,
                    PartnerUserName = x.Partner.UserName,
                    Title = x.Title,
                })
                .FirstOrDefault();
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

        public ICollection<StartedTasksViewModel> GetUserAllStartedTasks(string username)
        {
            var startedTasks = this.helpTaskRepo.AllAsNoTracking()
                .Where(x => (x.Creator.UserName == username || x.Partner.UserName == username) && x.RatingId == null && x.PartnerId != null)
                .To<StartedTasksViewModel>()
                .ToList();

            return startedTasks;
        }

        public async Task StartHelpTaskAsync(int helpTaskId, string partnerId)
        {
            var helpTask = this.helpTaskRepo.All()
                .FirstOrDefault(x => x.Id == helpTaskId);
            helpTask.PartnerId = partnerId;
            await this.helpTaskRepo.SaveChangesAsync();
        }
    }
}
