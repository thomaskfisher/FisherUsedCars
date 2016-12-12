using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FisherUsedCars.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        [DisplayName("User ID")]
        public int userID { get; set; }

        [Required(ErrorMessage = "An email address is required")]
        [DisplayName("Email")]
        [EmailAddress]
        public string email { get; set; }

        [Required(ErrorMessage = "A password is required")]
        [DisplayName("Password")]
        [StringLength(50, MinimumLength = 7, ErrorMessage = "The maximum length is 50, the minimum is 7")]
        [PasswordPropertyText]
        public string password { get; set; }

        [Required(ErrorMessage = "Your first name is required")]
        [DisplayName("First Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The maximum length is 50, the minimum is 2")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "Your last name is required")]
        [DisplayName("Last Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The maximum length is 50, the minimum is 2")]
        public string lastName { get; set; }
    }
}