using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Users
{
    
    public class User : BaseUserV1 
    {
      /*@FirstName nvarchar(50)
    , @LastName nvarchar(50)
    , @Email nvarchar(50)
    , @AvatarUrl nvarchar(50)
    , @TenantId nvarchar(50)
    , @Password nvarchar(50)
    , @Id int OUTPUT*/

     //public int Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Email { get; set; }

    public string AvatarUrl { get; set; }

    public string TenantId { get; set; }

    // public string Password { get; set; }
     
    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    }
}
