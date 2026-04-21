using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storeProject.Models
{
    public class Shipping
    {
        public int Id { get; set; }
        public string ShippingCarrier { get; set; }
        public string TrackingNumber { get; set; }
        public string ShippingStatus { get; set; }
        public DateTime EstimatedDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public int OrderId { get; set; }
        public Order order { get; set; }
    }
}
