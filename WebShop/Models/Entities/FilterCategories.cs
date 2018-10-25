using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebShop.Models.Entities
{
    [Table("tblFilterCategories")]
    public class FilterCategory
    {
        [ForeignKey("FilterNameOf"), Key, Column(Order = 0)]
        public int FilterNameId { get; set; }
        [Display(Name = "FilterName")]
        public virtual FilterName FilterNameOf { get; set; }
        [ForeignKey("FilterValueOf"), Key, Column(Order = 1)]
        public int FilterValueId { get; set; }
        [Display(Name = "FilterValue")]
        public virtual FilterValue FilterValueOf { get; set; }
        [ForeignKey("CategoryOf"), Key, Column(Order = 2)]
        public int CategoryId { get; set; }
        [Display(Name = "Category")]
        public virtual Category CategoryOf { get; set; }
    }
}