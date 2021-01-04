namespace ConnectingPeople.Services.Data
{
using System;
using System.Collections.Generic;
using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
using System.Threading.Tasks;

using AutoMapper;
    using ConnectingPeople.Common.Enums;
    using ConnectingPeople.Data.Common.Repositories;
using ConnectingPeople.Data.Models;
    using ConnectingPeople.Services.Mapping;
    using ConnectingPeople.Web.ViewModels.HelpTasks;
    using ConnectingPeople.Web.ViewModels.Profile;

    public class ProfileService : IProfileService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;

        public ProfileService(IDeletableEntityRepository<ApplicationUser> repository)
        {
            this.userRepo = repository;
        }

        public ProfileViewModel GetProfileInfoByName(string username)
        {
             return this.userRepo.AllAsNoTracking()
                .Where(p => p.UserName == username)
                .To<ProfileViewModel>()
                .FirstOrDefault();
        }

        public ProfileViewModel GetProfileInfoByNameWithSearch(string username, string search, TaskType type, bool? isActive)
        {
            var profile = this.userRepo.AllAsNoTracking()
                .Where(p => p.UserName == username)
                .To<ProfileViewModel>()
                .FirstOrDefault();
            
            profile.HelpTasks = profile.HelpTasks.Where(ht => ht.Type == type && ht.Title.Contains(search)).ToList();
            if (isActive != null)
            {
                profile.HelpTasks = (bool) isActive ?
                    profile.HelpTasks.Where(ht => ht.Rating == null) :
                    profile.HelpTasks.Where(ht => ht.Rating != null);
            }
            return profile;
        }

        public string GetUserIdByUsername(string username)
        {
            return this.userRepo.AllAsNoTracking()
                .Where(x => x.UserName == username)
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        public T MapUserInfoById<T>(string id)
        {
            return this.userRepo.AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public ICollection<FinishedTasksToCommentViewModel> GetUserTasksToComment(string id)
        {
            var tasksToCommentSeparated = this.userRepo.AllAsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    Username = x.UserName,
                    helpTasksToCommentAsCreator = x.HelpTasks
                        .Where(c => c.RatingId != null && c.Rating.CreatorComment == null)
                        .Select(c => new
                        {
                            c.Rating.CreatorComment,
                            c.Rating.OthersideComment,
                            c.Title,
                            c.Type,
                            c.RatingId,
                            c.ModifiedOn,
                            PartnerUsername = c.Partner.UserName,
                        }),
                    helpTasksToCommentAsPartner = x.HelpTasksAsPartner
                        .Where(c => c.RatingId != null && c.Rating.OthersideComment == null)
                        .Select(c => new
                        {
                            c.Rating.CreatorComment,
                            c.Rating.OthersideComment,
                            c.Title,
                            c.Type,
                            c.RatingId,
                            c.ModifiedOn,
                            CreatorUsername = c.Creator.UserName,
                        }),
                })
                .FirstOrDefault();
            List<FinishedTasksToCommentViewModel> tasksToComment = new List<FinishedTasksToCommentViewModel>();
            foreach (var task in tasksToCommentSeparated.helpTasksToCommentAsCreator)
            {
                tasksToComment.Add(new FinishedTasksToCommentViewModel
                {
                    Title = task.Title,
                    Type = task.Type,
                    RatingId = (int)task.RatingId,
                    IsPostMine = true,
                    ModifiedOn = (DateTime) task.ModifiedOn,
                    CurrentUserName = tasksToCommentSeparated.Username,
                    OthersideUserName = task.PartnerUsername,
                });
            }

            foreach (var task in tasksToCommentSeparated.helpTasksToCommentAsPartner)
            {
                tasksToComment.Add(new FinishedTasksToCommentViewModel
                {
                    Title = task.Title,
                    Type = task.Type,
                    RatingId = (int)task.RatingId,
                    IsPostMine = false,
                    ModifiedOn = (DateTime) task.ModifiedOn,
                    CurrentUserName = tasksToCommentSeparated.Username,
                    OthersideUserName = task.CreatorUsername,
                });
            }

            return tasksToComment;
        }
    }
}
