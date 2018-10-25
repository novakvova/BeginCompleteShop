using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebShop.Models.Entities
{
    [Table("tblfilters")]
    public class Filter
    {
        [ForeignKey("FilterNameOf"), Key, Column(Order = 0)]
        public int FilterNameId { get; set; }
        public virtual FilterName FilterNameOf { get; set; }
        [ForeignKey("FilterValueOf"), Key, Column(Order = 1)]
        public int FilterValueId { get; set; }
        public virtual FilterValue FilterValueOf { get; set; }
        [ForeignKey("ProductOf"), Key, Column(Order = 2)]
        public int ProductId { get; set; }
        public virtual Product ProductOf { get; set; }
    }
}