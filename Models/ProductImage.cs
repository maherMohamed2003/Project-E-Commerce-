using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storeProject.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public int ProductId { get; set; }
        public Product product { get; set; }
    }
}
