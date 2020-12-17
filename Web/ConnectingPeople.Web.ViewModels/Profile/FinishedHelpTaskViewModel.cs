using ConnectingPeople.Common.Enums;
using ConnectingPeople.Data.Models;
using ConnectingPeople.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectingPeople.Web.ViewModels.Profile
{
    public class FinishedHelpTaskViewModel : IMapFrom<HelpTask>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageName { get; set; }

        public TaskType Type { get; set; }

        public string OthersideUsername { get; set; }

        public string OthersideComment { get; set; }

        public int OthersideRating { get; set; }

        public string CreatorName { get; set; }

        public string CreatorComment { get; set; }

        public string CreatorRating { get; set; }
    }
}
