//using FoodShop_SWP.Models;
//using FoodShop_SWP.Models.EF;

//namespace FoodShop_SWP.Repository
//{
//    public class CategoryRepository : ICategoryRepository
//    {
//        private readonly ShopFoodWebContext _context;
//        public CategoryRepository(ShopFoodWebContext context)
//        {
//            _context = context;
//        }
//        public IEnumerable<Category> GetAllCategories()
//        {
//            return _context.Categories;
//        }

//        public Category GetCategory(int id)
//        {
//            return _context.Categories.Find(id);
//        }
//    }
//}
