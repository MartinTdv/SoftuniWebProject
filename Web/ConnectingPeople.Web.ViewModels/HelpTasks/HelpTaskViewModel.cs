using ConnectingPeople.Common.Enums;
using ConnectingPeople.Data.Models;
using ConnectingPeople.Services.Mapping;
using ConnectingPeople.Web.ViewModels.Profile;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectingPeople.Web.ViewModels.HelpTasks
{
    public class HelpTaskViewModel : IMapFrom<HelpTask>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string ImageName { get; set; }

        public DateTime CreatedOn { get; set; }

        public RatingViewModel Rating { get; set; }

        public string CreatorUserName { get; set; }

        public ICollection<HelpTaskItemsViewModel> Items { get; set; }

        public TaskType Type { get; set; }
    }
}
