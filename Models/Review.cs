using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storeProject.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string ReviewTaxt { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        public int? CustomerId { get; set; }
        public Customer? customer { get; set; }
        public int ProductId { get; set; }
        public Product product { get; set; }
    }
}
