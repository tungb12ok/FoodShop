using FoodShop_SWP.Models.EF;

namespace FoodShop_SWP.Models
{
    public class ProductViewModel
    {
        public Product? Product { get; set; }
        public ProductCategory? ProductCategory { get; set; }
    }
}
