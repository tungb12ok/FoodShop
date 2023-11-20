﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodShop_SWP.Models.EF
{
    [Table("tb_News")]
    public class News : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "The blog title must not be blank")]
        [StringLength(150)]
        public string Title { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        //[AllowHtml]
        public string? Detail { get; set; }
        public string? Image { get; set; }
        public int? CategoryId { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public string? SeoKeywords { get; set; }
        public bool IsActive { get; set; }
        public virtual Category? Category { get; set; }
    }
}