using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storeProject.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int customerId { get; set; }
        public Customer customer { get; set; }
    }
}
