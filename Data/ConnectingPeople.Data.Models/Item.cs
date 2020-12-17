
namespace ConnectingPeople.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using ConnectingPeople.Data.Common.Models;

    public class Item : BaseDeletableModel<int>
    {
        public string NameInCyrillic { get; set; }

        public string FAIconClass { get; set; }

        public ICollection<HelpTaskItems> HelpTasks { get; set; } = new List<HelpTaskItems>();
    }
}
