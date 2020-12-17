
namespace ConnectingPeople.Web.ViewModels.Profile
{
    using AutoMapper;
    using ConnectingPeople.Common.Enums;
    using ConnectingPeople.Data.Models;
    using ConnectingPeople.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    public class ActiveHelpTaskViewModel : IMapFrom<HelpTask>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string CreatorUserName { get; set; }

        public string Location { get; set; }

        public string ImageName { get; set; }

        public int? RatingId { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<HelpTaskItemsViewModel> Items { get; set; }

        public TaskType Type { get; set; }

    }
}
