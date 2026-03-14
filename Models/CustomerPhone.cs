using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storeProject.Models
{
    public class CustomerPhone
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public int CustomerId { get; set; }
        public Customer customer { get; set; }
    }
}
