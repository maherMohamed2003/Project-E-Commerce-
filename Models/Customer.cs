using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce_Proj.Models;

namespace storeProject.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public bool IsEmailVerified { get; set; } = false;
        public string EmailToken { get; set; }
        public Cart cart {  get; set; }
        public Favourite Favourite {  get; set; }
        public int RoleId { get; set; }
        public ICollection<Role> roles { get; set; }
        public ICollection<Order> orders { get; set; }
        public ICollection<Review> reviews { get; set; }
        public ICollection<FeedBack> feedBacks { get; set; }

    }
}
