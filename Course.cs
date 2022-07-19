using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.CodeChallenge
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int SeasonTermId { get; set; }

        public int TeacherId { get; set; }

        public SeasonTerm Seasons { get; set;  }

        public Teacher Teachers { get; set; }

        public List<Student> Students { get; set; }
    }
}
