using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Addresses
{
    public class AddressAddRequest
    {
       [Required] // attributes that live inside the data annotations
        [StringLength(200, MinimumLength =2)]
        public string LineOne { get; set; }
        [Required]
        [Range(2,80)]
        public int SuiteNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string City { get; set; }
        [Required]
        [StringLength(50)]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public double Lat { get; set; }
        [Required]
        public double Long { get; set; }
    }
}

