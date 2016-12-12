#Final Study Guide
This is the project I created to review for my IS 403 Final. Here are the steps and tips/tricks to be used as a resource for the fina.


# How to Setup the Database and Models
### #1 Create the Database and Tables
To create a new database, go to the server explorer, right click on Data Connections, and create a new SQL server database.
When building the tables, make sure to indicate that the ID field is an identity field and that the identiy seed starts at one.
Don't forget to enforce foreign key relationships.


### #2 Use the `web.config` file to create the connection string
Find that connection string by going to the server explorer, righ clicking on the data connection, and selecting properties.
Be sure to also change the name field in the connectionStrings tag.
```HTML
<connectionStrings>
    <add name="FisherUsedCarsContext" connectionString="Data Source=THOMASFISHECFE1\SQLEXPRESS;Initial Catalog=FisherUsedCars;Integrated Security=True;Pooling=False"
      providerName="System.Data.SqlClient" />
  </connectionStrings>
```


### #3 Create the models
When creating a user model (and table) you only need the `userID`, `email`, `password`, `firstName`, and `lastName`.
```C#
namespace FisherUsedCars.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int userID { get; set; }

        [EmailAddress]
        public string email { get; set; }

        [Required(ErrorMessage = "A password is required")]
        [DisplayName("Password")]
        [StringLength(50, MinimumLength = 7, ErrorMessage = "The maximum length is 50, the minimum is 7")]
        [PasswordPropertyText]
        public string password { get; set; }
    }
}
```


If you want to do a kind-of model within a model you only need one key.
```C#
namespace Project_1.Models
{
    public class DegreesAndQuestions
    {
        public int degreeID { get; set; }
        public string degreeName { get; set; }
        public string degreeCoordinator { get; set; }
        
        [Key]
        public int degreeQuestionID { get; set; }
        public int userID { get; set; }
        public string question { get; set; }
        public string answer { get; set; }

        public IEnumerable<DegreeQuestions> DegreeQuestion { get; set; }
        public IEnumerable<Degrees> Degree { get; set; }
    }
}
```


If you want to do a real model within a model you do this.
```C#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FisherUsedCars.Models
{
    public class CarsOwner
    {
        public Cars cars { get; set; }
        public Owner owner { get; set; }
    }
}
```

### #4 Create a new folder called DAL and insert a new class file called FisherUsedCarsContext
Make sure to get the two inheritence lines included.
Create a DbSet for each model.
```C#
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
```

### #5 Modify the `global.asax` file
Include the new line `Database.SetInitializer<FisherUsedCarsContext>(null);`
```C#
namespace FisherUsedCars
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer<FisherUsedCarsContext>(null);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
```
