using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Addresses
{
    public class Address : BaseAddress
    {
        //@Id int,
        //@LineOne nvarchar(50),
        //@SuiteNumber int,
        //@City nvarchar(50),
        //@State nvarchar(50),
        //@PostalCode nvarchar(50),
        //@IsActive bit,
        //@Lat float,
        //@Long float

        //public int Id { get; set; }
        public string LineOne { get; set; }

        public int SuiteNumber { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public bool IsActive { get; set; }

        public double Lat { get; set; }
        public double Long { get; set; }


    }
}