using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodShop_SWP.Models.EF
{
    [Table("tb_User")]   
    
    public class User
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(250)]
        public string Email { get; set; } = null!;
        public bool? Enable { get; set; }
        [StringLength(250)]
        public string Firrtname { get; set; } = null!;
        public bool Gender { get; set; }
        [StringLength(250)]
        public string? ImageUrl { get; set; }
        public bool? IsLockout { get; set; }
        [StringLength(250)]
        public string Lastname { get; set; } = null!;
        [StringLength(250)]
        public string Password { get; set; } = null!;
        public int? Role { get; set; }

    }
}
