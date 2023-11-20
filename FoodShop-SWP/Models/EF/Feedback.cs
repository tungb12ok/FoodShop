using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodShop_SWP.Models.EF
{
    [Table("tb_FeedBack")]
    public class Feedback : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nội dung không để trống")]
        [StringLength(150)]
        public string Content { get; set; }

        [ForeignKey("ProductFeedBack")]
        public int ProductID { get; set; }

        public virtual Product? product { get; set; }

        [ForeignKey("UserFeedBack")]
        public int UserID { get; set; }

        public virtual User? user { get; set; }
    }
}
