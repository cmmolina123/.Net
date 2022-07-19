using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Automobiles
{
    public class AutomobileUpdateRequest : AutomobileAddRequest
    {
        public int Id { get; set; }
    }
}
