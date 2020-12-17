using ConnectingPeople.Data.Models;
using ConnectingPeople.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectingPeople.Web.ViewModels.Forms
{
    public class SelectedItemsInputModel : IMapFrom<Item>
    {
        public int Id { get; set; }

        public string NameInCyrillic { get; set; }

        public string FAIconClass { get; set; }

        public bool SelectedAsMandatory { get; set; }

        public bool SelectedAsHelpful { get; set; }

        public bool SelectedAsAvailable { get; set; }
    }
}
