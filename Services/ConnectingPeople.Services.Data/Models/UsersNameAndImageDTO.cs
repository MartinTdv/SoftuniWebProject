using ConnectingPeople.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectingPeople.Services.Data.Models
{
    public class UsersNameAndImageDTO
    {
        public ICollection<ApplicationUser> User { get; set; }

        public string Username { get; set; }

        public string ImageName { get; set; }
    }
}
