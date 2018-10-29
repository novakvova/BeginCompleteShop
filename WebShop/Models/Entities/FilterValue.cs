using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebShop.Models.Entities
{
    [Table("tblFilterValue")]
    public class FilterValue
    {
        [Key]
        public int Id { get; set; }
        [StringLength(maximumLength: 258)]
        [Display(Name = "FilterValue")]
        public string Name { get; set; }
        public virtual ICollection<Filter> Filters { get; set; }
    }
}