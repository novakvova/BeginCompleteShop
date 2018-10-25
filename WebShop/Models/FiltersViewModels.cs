using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebShop.Models
{
    public class FilterNameViewModel
    {
        public int? Id { get; set; }
        [Display(Name = "FilterName")]
        public string Name { get; set; }
    }
    public class FilterGroupViewModel
    {
        public int FilterNameId { get; set; }
        [Display(Name = "FilterValue")]
        public string FilterValue { get; set; }
    }
    public class FilterProductViewModel
    {
        public int? Id { get; set; }
        public int FilterNameId { get; set; }
        public int FilterValueId { get; set; }
        public int ProductId { get; set; }  
    }
    public class FilterCategoryViewModel
    {
        public int? Id { get; set; }
        [Display(Name = "FilterName")]
        public int FilterNameId { get; set; }
        [Display(Name = "FilterValue")]
        public int FilterValueId { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
    }
}