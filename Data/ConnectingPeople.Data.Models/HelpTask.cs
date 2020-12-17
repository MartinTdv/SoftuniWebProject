
namespace ConnectingPeople.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using ConnectingPeople.Common.Enums;
    using ConnectingPeople.Data.Common.Models;

    using Microsoft.AspNetCore.Http;

    public class HelpTask : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(100)]
        [MinLength(5)]
        public string Title { get; set; }

        [Required]
        [MaxLength(950)]
        [MinLength(10)]
        public string Description { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Location { get; set; }

        public string ImageName { get; set; }

        [Range(0, 1)]
        public TaskType Type { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        public int? RatingId { get; set; }

        public ICollection<HelpTaskItems> Items { get; set; } = new List<HelpTaskItems>();
    }
}
