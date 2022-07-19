using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Friends
{
    public class ImageAddRequest
    {
        //public int Id { get; set; }

        public int TypeId { get; set; }
        public string Url { get; set; }
    }
}
