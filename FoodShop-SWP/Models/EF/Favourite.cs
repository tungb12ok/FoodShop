using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodShop_SWP.Models.EF
{
    [Table("tb_Favourite")]
    public class Favourite : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("ProductFavourite")]
        public int ProductID { get; set; }

        public virtual Product? product { get; set; }

        [ForeignKey("UserFavourite")]
        public int UserID { get; set; }

        public virtual User? user { get; set; }
    }
}
