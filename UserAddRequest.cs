using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Users
{
    public class UserAddRequest
    {
        [Required] 
        [StringLength(200, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Email { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string AvatarUrl { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string TenantId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Password { get; set; }

        // public string PasswordConfirm { get; set; }

        //public DateTime DateCreated { get; set; }

        //public DateTime DateModified { get; set; }
    }
}
