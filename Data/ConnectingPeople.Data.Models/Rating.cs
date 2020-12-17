namespace ConnectingPeople.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using ConnectingPeople.Data.Common.Models;

    public class Rating : BaseDeletableModel<int>
    {
        [Range(0, 10)]
        public int CreatorRating { get; set; }

        public string CreatorRatingColorClass { get; set; }

        [MaxLength(160)]
        public string CreatorComment { get; set; }

        [MaxLength(14)]
        public string OthersideUsername { get; set; }

        [Range(0, 10)]
        public int OthersideRating { get; set; }

        public string OthersideRatingColorClass { get; set; }

        [MaxLength(160)]
        public string OthersideComment { get; set; }

        [Required]
        public int TaskId { get; set; }

        public HelpTask HelpTask { get; set; }
    }
}
