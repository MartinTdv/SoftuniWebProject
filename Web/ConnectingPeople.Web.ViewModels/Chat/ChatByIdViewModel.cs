using ConnectingPeople.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectingPeople.Web.ViewModels.Chat
{
    public class ChatByIdViewModel : IMapFrom<Data.Chat>
    {
        public string helpTaskId { get; set; }

        public string TaskCreatorUsername { get; set; }

        public string OthersideUsername { get; set; }

        public string About { get; set; }

        public string HelpTaskPartnerId { get; set; }

        public DateTime? HelpTaskDeletedOn { get; set; }

        public ICollection<MessageViewModel> Messages { get; set; }
    }
}
