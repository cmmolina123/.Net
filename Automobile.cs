using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Automobiles
{
    public class Automobile
    {
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public string Color { get; set; }

        public string Type { get; set; }

        public AutoImage PrimaryImage { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public List<AutoFeature> Features { get; set; }
    }
}
