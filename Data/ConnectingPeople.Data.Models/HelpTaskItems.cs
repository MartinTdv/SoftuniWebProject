using ConnectingPeople.Data.Common.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Threading.Tasks;

namespace ConnectingPeople.Data.Models
{
    public class HelpTaskItems
    {
        public int HelpTaskId { get; set; }

        public HelpTask HelpTask { get; set; }

        public int ItemId { get; set; }

        public Item Item { get; set; }

        public ItemUseType ItemUseType { get; set; }
    }
}
