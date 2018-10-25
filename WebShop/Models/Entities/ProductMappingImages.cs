using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebShop.Models.Entities
{
    [Table("tblProductMappingImages")]
    public class ProductMappingImage
    {
        [ForeignKey("Product"),Key, Column(Order = 0)]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [ForeignKey("ProductImage"),Key, Column(Order = 1)]
        public int ImageId { get; set; }
        public ProductImage ProductImage { get; set; }
        public int Priority { get; set; }


    }
}