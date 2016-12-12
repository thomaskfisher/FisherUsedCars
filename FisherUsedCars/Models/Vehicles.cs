using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FisherUsedCars.Models
{
    [Table("Vehicles")]
    public class Vehicles
    {
        [Key]
        [DisplayName("Vehicle ID")]
        public int vehicleID { get; set; }

        [Required(ErrorMessage = "The vehicle make is required")]
        [DisplayName("Make")]
        [StringLength(50, MinimumLength =2, ErrorMessage = "The maximum length is 50, the minimum is 2")]
        public string make { get; set; }

        [Required(ErrorMessage = "The vehicle model is required")]
        [DisplayName("Model")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The maximum length is 50, the minimum is 2")]
        public string model { get; set; }

        [Required(ErrorMessage = "The vehicle model year is required")]
        [DisplayName("Year")]
        public int year { get; set; }

        [Required(ErrorMessage = "The vehicle mileage is required")]
        [DisplayName("Mileage")]
        public int mileage { get; set; }

        [Required(ErrorMessage = "The vehicle price is required")]
        [DisplayName("Price")]
        public int price { get; set; }

        [DisplayName("Trim or Package")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The maximum number of characters is 50")]
        public string trim { get; set; }
    }
}