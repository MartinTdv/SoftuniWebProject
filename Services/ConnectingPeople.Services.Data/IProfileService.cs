using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ConnectingPeople.Common.Enums;
using ConnectingPeople.Data.Models;
using ConnectingPeople.Web.ViewModels.HelpTasks;
using ConnectingPeople.Web.ViewModels.Profile;

namespace ConnectingPeople.Services.Data
{
    public interface IProfileService
    {
        T MapUserInfoById<T>(string id);

        ProfileViewModel GetProfileInfoByName(string username);

        ProfileViewModel GetProfileInfoByNameWithSearch(string username, string search, TaskType type, bool? isActive);

        string GetUserIdByUsername(string username);

        ICollection<FinishedTasksToCommentViewModel> GetUserTasksToComment(string id);
    }
}
