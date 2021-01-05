using ConnectingPeople.Common.Enums;
using ConnectingPeople.Data.Models;
using ConnectingPeople.Services.Data.Models;
using ConnectingPeople.Web.ViewModels.Forms;
using ConnectingPeople.Web.ViewModels.HelpTasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConnectingPeople.Services.Data
{
    public interface IHelpTaskService
    {
        Task CreateNeedAsync(NeedHelpFormInputModel input, string callerMethodName = null);

        Task CreateOfferAsync(OfferHelpFormInputModel input, string callerMethodName = null);

        ICollection<T> GetAllByUsername<T>(string username);

        ICollection<T> GetAllOfType<T>(TaskType taskType);

        Task Delete(int id);

        TitleAndCreatorUsernameDTO GetTitleAndCreatorUsernameById(int id);

        Task StartHelpTaskAsync(int helpTaskId, string partnerId);

        Task FinishHelpTaskAsync(int rating, string comment, string username, int helpTaskId);

        ICollection<StartedTasksViewModel> GetUserAllStartedTasks(string username);

        CreatorAndPartnerUsernamesAndTitleDTO GetHelpTaskById(int id);

        RateFormPageDTO GetRateFormPageDTOByRatingId(int id);

        Task AddCommentAndRating(int ratingId, string currentUserName, string comment, int rating);
    }
}
