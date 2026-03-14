using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce_Proj.Models;

namespace storeProject.Models
{
    public class FavoriteItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product product { get; set; }
        public int FavouriteId { get; set; }
        public Favourite Favourite { get; set; }
    }
}
