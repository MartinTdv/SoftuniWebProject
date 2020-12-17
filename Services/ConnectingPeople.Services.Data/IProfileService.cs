using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ConnectingPeople.Common.Enums;
using ConnectingPeople.Web.ViewModels.Profile;

namespace ConnectingPeople.Services.Data
{
    public interface IProfileService
    {
        ProfileViewModel GetProfileInfoByName(string username);

        ProfileViewModel GetProfileInfoByNameWithSearch(string username, string search, TaskType type, bool? isActive);
    }
}
