using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebShop.Models.Entities
{
    [Table("tblUserProfiles")]
    public class UserProfile
    {
        [ForeignKey("ApplicationUserOf"), Key]
        public int Id { get; set; }
        public string Image { get; set; }
        public string Phone { get; set; }
        [DataType(DataType.Date)]
        public DateTime ? DateOfBirth { get; set; }
        public ApplicationUser ApplicationUserOf { get; set; }
    }
}