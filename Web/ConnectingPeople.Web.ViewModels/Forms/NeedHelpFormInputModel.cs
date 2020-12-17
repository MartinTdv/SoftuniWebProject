
namespace ConnectingPeople.Web.ViewModels.Forms
{
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Text;

using ConnectingPeople.Common.Enums;
using ConnectingPeople.Data.Common.Enums;
using ConnectingPeople.Data.Models;

using Microsoft.AspNetCore.Http;
public class NeedHelpFormInputModel
    {
        private const string stringLengthErrorMessage = "Полето \"{0}\" трябва да бъде между {2} и {1} символа.";
        private const string requiredFieldErrorMessage = "Полето \"{0}\" e задължително!";

        [Required(ErrorMessage = requiredFieldErrorMessage)]
        [StringLength(160, ErrorMessage = stringLengthErrorMessage, MinimumLength = 10)]
        [Display(Name = "Желая помощ за:")]
        public string Title { get; set; }

        [Required(ErrorMessage = requiredFieldErrorMessage)]
        [StringLength(950, ErrorMessage = stringLengthErrorMessage, MinimumLength = 10)]
        [Display(Name = "Описание:")]
        public string Description { get; set; }

        [Required(ErrorMessage = requiredFieldErrorMessage)]
        [StringLength(50, ErrorMessage = stringLengthErrorMessage, MinimumLength = 5)]
        [Display(Name = "Местоположениe:")]
        public string Location { get; set; }

        [Display(Name = "Снимка:")]
        public IFormFile Image { get; set; }

        public string ImageName { get; set; }

        public ICollection<int> AvailableItemsId { get; set; }

        public ICollection<int> MandatoryItemsId { get; set; }

        public ICollection<int> HelpfulItemsId { get; set; }

        public IList<SelectedItemsInputModel> Items { get; set; }

        public ApplicationUser Creator { get; set; }

        public TaskType TaskType { get; set; }
    }
}
