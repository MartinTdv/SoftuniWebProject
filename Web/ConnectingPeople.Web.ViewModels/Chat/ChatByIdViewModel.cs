using ConnectingPeople.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectingPeople.Web.ViewModels.Chat
{
    public class ChatByIdViewModel : IMapFrom<Data.Chat>
    {
        public string About { get; set; }

        public ICollection<MessageViewModel> Messages { get; set; }
    }
}
