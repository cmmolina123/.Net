using Sabio.Models.Domain.Automobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Automobiles
{
    public class AutomobileAddRequest
    {
        
        public string Make { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public string Color { get; set; }

        public string Type { get; set; }

        public AutoImageAddRequest PrimaryImage { get; set; }

    }
}
