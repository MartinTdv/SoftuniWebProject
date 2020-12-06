// ReSharper disable VirtualMemberCallInConstructor
namespace ConnectingPeople.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using ConnectingPeople.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        //User info

        [Required]
        [MaxLength(14)]
        [MinLength(3)]
        public override string UserName { get => base.UserName; set => base.UserName = value; }

        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(40)]
        [MinLength(2)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string City { get; set; }

        [MaxLength(600)]
        public string Description { get; set; }

        public string ImageName { get; set; }

        public ICollection<HelpTask> HelpTasks { get; set; }


        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
