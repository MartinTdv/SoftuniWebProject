using AutoMapper;
using ConnectingPeople.Data.Models;
using ConnectingPeople.Services.Mapping;
using ConnectingPeople.Web.ViewModels.HelpTasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectingPeople.Web.ViewModels.Profile
{
    public class ProfileViewModel : IMapFrom<ApplicationUser>
    {
        public string UserName { get; set; }

        public string ImageName { get; set; }

        public string Description { get; set; }

        public IEnumerable<HelpTaskViewModel> HelpTasks { get; set; }

    }
}
