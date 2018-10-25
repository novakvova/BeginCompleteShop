using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebShop.Models.Entities
{
    [Table("tblFilterName")]
    public class FilterName
    {
        [Key]
        public int Id { get; set; }
        [StringLength(maximumLength: 258)]
        [Display(Name = "FilterName")]
        public string Name { get; set; }
    }
}