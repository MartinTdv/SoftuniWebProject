using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectingPeople.Web.Models
{
    public class Message
    {
        public string ConnectionId { get; set; }

        public string User { get; set; }

        public string Text { get; set; }
    }
}
