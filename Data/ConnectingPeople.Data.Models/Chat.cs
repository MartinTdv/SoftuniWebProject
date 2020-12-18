using ConnectingPeople.Data.Common.Models;
using ConnectingPeople.Data.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ConnectingPeople.Data
{
    public class Chat : BaseDeletableModel<int>
    {
        public Chat()
        {
            this.Users = new List<UserChat>();
            this.Messages = new List<Message>();
        }
        public int HelpTaskId { get; set; }
        
        public HelpTask HelpTask { get; set; }

        public string About { get; set; }

        public string TaskCreatorUsername { get; set; }

        public string TaskCreatorConnectionId { get; set; }

        public string OthersideUsername { get; set; }

        public string OthersideConnectionId { get; set; }

        public string ChatGroupName { get; set; }

        public ICollection<UserChat> Users { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
