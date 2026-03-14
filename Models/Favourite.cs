using storeProject.Models;

namespace E_Commerce_Proj.Models
{
    public class Favourite
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer customer { get; set; }
        public ICollection<FavoriteItem> FavoriteItems { get; set; }
    }
}
