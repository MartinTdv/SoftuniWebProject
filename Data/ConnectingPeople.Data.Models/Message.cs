using ConnectingPeople.Data.Common.Models;

namespace ConnectingPeople.Data
{
    public class Message : BaseDeletableModel<int>
    {
        public int ChatId { get; set; }

        public string SenderUsername { get; set; }

        public string Text { get; set; }
    }
}