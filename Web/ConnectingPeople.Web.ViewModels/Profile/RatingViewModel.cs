using ConnectingPeople.Data.Models;
using ConnectingPeople.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectingPeople.Web.ViewModels.Profile
{
    public class RatingViewModel : IMapFrom<Rating>
    {
        public string OthersideUsername { get; set; }

        public string OthersideComment { get; set; }

        public int OthersideRating { get; set; }

        public string CreatorName { get; set; }

        public string CreatorComment { get; set; }

        public string CreatorRating { get; set; }
    }
}
