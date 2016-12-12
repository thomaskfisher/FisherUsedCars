using FisherUsedCars.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FisherUsedCars.DAL
{
    public class FisherUsedCarsContext : DbContext
    {
        public FisherUsedCarsContext()
            : base("FisherUsedCarsContext")
        {

        }

        public DbSet<Users> User { get; set; }
        public DbSet<Vehicles> Vehicles { get; set; }
    }
}