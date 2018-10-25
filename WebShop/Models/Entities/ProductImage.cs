using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebShop.Models.Entities
{
    [Table("tblProductImages")]
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "File")]
        [Required, StringLength(maximumLength:255)]
        [Index(IsUnique = true)]
        public string FileName { get; set; }
        public virtual ICollection<ProductMappingImage> ProductMappingImages { get; set; }
    }
}