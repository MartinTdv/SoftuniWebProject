using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectingPeople.Data.Models
{
    public class UserChat
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int ChatId { get; set; }

        public Chat Chat { get; set; }
    }
}
