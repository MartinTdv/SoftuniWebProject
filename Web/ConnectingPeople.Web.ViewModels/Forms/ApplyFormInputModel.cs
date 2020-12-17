using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConnectingPeople.Web.ViewModels.Forms
{
    public class ApplyFormInputModel
    {
        public int HelpTaskId { get; set; }

        public string About { get; set; }

        public string TaskCreatorUsername { get; set; }

        public string TaskCreatorConnectionId { get; set; }

        public string OthersideUsername { get; set; }

        public string OthersideConnectionId { get; set; }

        [Display(Name = "Съобщение:")]
        public string MessageText { get; set; }
    }
}
