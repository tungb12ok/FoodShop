using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodShop_SWP.Models.EF
{
    public class Role
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(250)]
        public string RoleName { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public bool status { get; set; }

      //  public ICollection<User> Users { get; set; }
    }
}
