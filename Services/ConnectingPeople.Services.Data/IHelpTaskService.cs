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

        Task StartHelpTask(int helpTaskId, string partnerId);

        ICollection<StartedTasksViewModel> GetUserAllStartedTasks(string username);
    }
}
