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



# Tips and Tricks
### #1
Include `FisherUsedCarsContext db = new FisherUsedCarsContext();` at the top of every controller that you will want to access the database from (which is practically all controllers).
```C#
namespace FisherUsedCars.Controllers
{
    public class HomeController : Controller
    {
        FisherUsedCarsContext db = new FisherUsedCarsContext();
```
### #2
At the top of a view where you will be accessing data from the database, you will need to specify which model you are using. You will either use a single instance of the model `@model FisherUsedCars.Models.Vehicles` (such as when you are editing the details of a single record) or you will use multiple instances of the model `@model IEnumerable<FisherUsedCars.Models.Vehicles>` (like when you're viewing a list of the records in the database.
```C#
@model FisherUsedCars.Models.Vehicles

@{
    ViewBag.Title = "Details";
}
```
```C#
@model IEnumerable<FisherUsedCars.Models.Vehicles>

@{
    ViewBag.Title = "Index";
}
```

### #3
This is how you do a proper bootsrap button.
```HTML
<a class="btn btn-success" href="@Url.Action("AddBSISQuestion", "AddQuestion", null, null)">Ask A Question</a>
```

### #4
This is how you create a bootstrap error message (if there is one).
```HTML
@if (ViewBag.alert == true)
{
    <h4 class="alert alert-danger" role="alert">@ViewBag.error</h4>
}
```


# How to add User Authentication
### #1 Update the `web.config` files
In the `system.web` tag, comment out the authentication mode and replace it with this.
```C#
<system.web>
    <authentication mode="Forms">
      <forms loginUrl="~/Home/Login" timeout="2880"/>
    </authentication>
    <!--<authentication mode="None" />-->
    <compilation debug="true" targetFramework="4.5.1" />
    <httpRuntime targetFramework="4.5.1" />
  </system.web>
```
In the `system.webServer` tag, remove the name tag.
```C#
 <system.webServer>
    <modules>
      <!--<remove name="FormsAuthentication" />-->
    </modules>
  </system.webServer>
```

### #2 Modify the `startup.auth.cs` file under the `app_start` folder
Comment out everything in the `ConfigureAuth` block of code (this comments out all content in this file except for the structure)
```C#
namespace FisherUsedCars
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            //EVERYTHING IS COMMENTED OUT
        }
    }
}
```

### #3 Create these Login action methods within the home controller
```C#
public ActionResult Login()
{
    return View();
}

[HttpPost]
public ActionResult Login(FormCollection form, bool rememberMe = false)
{
    String email = form["Email address"].ToString();
    String password = form["Password"].ToString();

    bool validUser = db.User.Any(m => m.email.Equals(email));

    if(validUser)
    {
        Users tempUser = db.User.SingleOrDefault(m => m.email.Equals(email));

        int myUserID = tempUser.userID;
        Session["TESLA"] = myUserID;

        if(string.Equals(tempUser.password, password))
        {
            FormsAuthentication.SetAuthCookie(email, rememberMe);
            return RedirectToAction("Index", "Home"); //User is authenticated succesfully
        }
        else
        {
            ViewBag.error = "The password entered is incorrect.";
            ViewBag.alert = true;
             return View();
        }
   }
   else
   {
    ViewBag.error = "The email entered does not match any on file.";
    ViewBag.alert = true;
    return View();
    }
}
```

### #4 Create this Login view under the home folder
```HTML
@{
    ViewBag.Title = "Login";
}

<h2>Login</h2>
@if (ViewBag.alert == true)
{
    <h4 class="alert alert-danger" role="alert">@ViewBag.error</h4>
}

@using (Html.BeginForm("Login", "Home", FormMethod.Post))
{
    <label for="inputEmail">Email address</label>
    @Html.TextBox("Email address", "", new { type = "email", id = "inputEmail", @class = "form-control", placeholder = "Email Address", required = true, autofocus = true})
    <br />

    <label for="inputPassword">Password</label>
    @Html.Password("Password", "", new { type = "password", id = "inputPassword", @class = "form-control", placeholder = "Password", required = true})
    <br />
    
    <button class="btn btn-lg btn-primary" type="submit">Login</button>
}

<br /><br /><br />

<div>
    @Html.ActionLink("Create an Account", "Register", "Home", routeValues: null, htmlAttributes: new { id = "registerLink"})
</div>
```

### #5 Create these Register action methods within the home controller
```C#
public ActionResult Register()
{
    return View();
}

[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Register([Bind(Include = "userID, email, password, firstName, lastName")] Users User, FormCollection Form, bool rememberMe = false)
{
    if(ModelState.IsValid)
    {
        String email = Form["email"].ToString();
        db.User.Add(User);
        db.SaveChanges();

        FormsAuthentication.SetAuthCookie(email, rememberMe);

        int myUserID = User.userID;
        Session["TESLA"] = myUserID;

        return RedirectToAction("Index", "Home");
    }
    return View(User);
}
```

### #6 Create this Register view under the home folder
```HTML
@model FisherUsedCars.Models.Users

@{
    ViewBag.Title = "Register";
}

<h2>Register</h2>

@using (Html.BeginForm())
{
    @Html.AntiForgeryToken()

    <div class="form-horizontal">

        <hr />
        @Html.ValidationSummary(true, "", new { @class = "text-danger" })
        <div class="form-group">
            @Html.LabelFor(model => model.email, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.email, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.email, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.password, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.password, new { htmlAttributes = new { type = "password", @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.password, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.firstName, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.firstName, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.firstName, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(model => model.lastName, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.lastName, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.lastName, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Create" class="btn btn-default" />
            </div>
        </div>
    </div>
}
```

### #7 Modify the LogOff() action method in the defualt account controller
If you are using the pre-built login framework (aka you want it to say the user name on the top right of the screen and you want the user to be able to logout, but you’re not using the prebuilt login page) you need to modify the LogOff() action method in the AccountController. Comment out the two lines in there and change it to this.
```C#
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult LogOff()
{
    Session.Abandon();
    System.Web.Security.FormsAuthentication.SignOut();
    return RedirectToAction("Index", "Home");
    //AuthenticationManager.SignOut();
     //return RedirectToAction("Index", "Home");
}
```

### #8 Change the default links in the `_LoginPartial.cshtml` file to direct to your new login and register views in the home controller
```HTML
<ul class="nav navbar-nav navbar-right">
    <li>@Html.ActionLink("Register", "Register", "Home", routeValues: null, htmlAttributes: new { id = "registerLink" })</li>
    <li>@Html.ActionLink("Log in", "Login", "Home", routeValues: null, htmlAttributes: new { id = "loginLink" })</li>
</ul>
```

### Don't forget to add the tage `[Authorize]` before any action method you want to require a login
```C#
[Authorize]
public ActionResult Index()
{
    ...
}
```

### Don't foget to use your session key
In a view when you have a bunch of `@HTML.HiddenFor` helpers, you need to remember to hide the session.
```HTML
@Html.Hidden("degreeID", 5)
@Html.HiddenFor(m => m.degreeID)
@Html.HiddenFor(m => m.degreeQuestionID)
@Html.Hidden("userID", @Session["TESLA"])
@Html.Hidden("answer", @TempData["TempAnswer"])
```

# GitHub Written Tutorials
[Introduction to Views](https://github.com/byu-is-403/syllabus/blob/master/asp-dotnet-mvc/views.md)

[Entity Framework](https://github.com/byu-is-403/syllabus/blob/master/entity-framework/entity-framework.md)

[Entity Framework Tutorial](https://github.com/byu-is-403/syllabus/blob/master/entity-framework/entity-framework-tutorial.md)

[Entity Framework Tutorial Explanation](https://github.com/byu-is-403/syllabus/blob/master/entity-framework/tutorial-explanation.md)

[.NET HTML Forms](https://github.com/danmo91/HTML-Forms)

[Forms and Helpers](https://github.com/byu-is-403/syllabus/blob/master/entity-framework/form-helpers.md)

[Model Annotations and Validations](https://github.com/byu-is-403/syllabus/blob/master/entity-framework/annotations-and-validations.md)

[Dropdown List](https://github.com/byu-is-403/syllabus/blob/master/entity-framework/how-to-create-dropdown-list.md)

[Model within a Model](https://github.com/byu-is-403/syllabus/blob/master/entity-framework/model-within-model.md)

[Security](https://github.com/byu-is-403/syllabus/blob/master/entity-framework/security.md)

[Adding Authorization](https://github.com/byu-is-403/syllabus/blob/master/entity-framework/adding-authorization.md)

[How to Authenticate Using Google](https://github.com/byu-is-403/syllabus/blob/master/entity-framework/how-to-authenticate-using-google.md)

[How to Grab Info from Google Authentication](https://github.com/byu-is-403/syllabus/blob/master/entity-framework/google-authentication.md)

[User Identity](https://github.com/byu-is-403/syllabus/blob/master/entity-framework/user-identify.md)

[Pass User Login to View](https://github.com/byu-is-403/syllabus/blob/master/entity-framework/pass-user-login-to-view.md)

[Return to Previous URL](https://github.com/byu-is-403/syllabus/blob/master/entity-framework/return-to-previous-url.md)

[ASP.NET MVC 5 Deployment](https://github.com/byu-is-403/syllabus/blob/master/entity-framework/deployment.md)



# Video Tutorials
[#1 Creating an MS SQL Server Database and Tables using Visual Studio 2013](https://www.youtube.com/watch?v=jwarWeWadn4&feature=youtu.be&hd=1)

[#2 Create a default MVC ASP.NET application - explain app structure](https://www.youtube.com/watch?v=JZ4MoqeqzHo&feature=youtu.be&hd=1)

[#3 Default MVC Controllers and Views (_ViewStart, _Layout,  _LoginPartial)](https://www.youtube.com/watch?v=qOnYmyTRA2w&feature=youtu.be&hd=1)

[#4 How Razor Processes the Views (RenderBody)](https://www.youtube.com/watch?v=y5VgvAaYVaM&feature=youtu.be&hd=1)

[#5 Modifying a menu in a view and using Action and ActionLink to click on an Image and a link to take you to other views](https://www.youtube.com/watch?v=iT1PfwVq6ac&feature=youtu.be&hd=1)

[#6 Working with a Database - Creating the HardKnox Database - Part 1](https://www.youtube.com/watch?v=tTGzNxNLSbE&feature=youtu.be&hd=1)

[#7 Working with a Database - Web.config and the Connection String - Part 2](https://www.youtube.com/watch?v=ASTRxNwi0Go&feature=youtu.be&hd=1)

[#8 Work with a Database - Models - Part 3](https://www.youtube.com/watch?v=KGNyGRNL2sg&feature=youtu.be&hd=1)

[#9 Working with a Database - Creating the DbContext Class - Part 4](https://www.youtube.com/watch?v=5ayTV2_hmus&feature=youtu.be&hd=1)

[#10 Working with a Database - Showing the Data - Part 5](https://www.youtube.com/watch?v=vT7pe9_et2A&feature=youtu.be&hd=1)

[#11 Working with a Database - Showing only 1 record from a set - Part 6](https://www.youtube.com/watch?v=9ZyozL1FexE&feature=youtu.be&hd=1)

[#12 Working with a Database - Delete a record - Part 7](https://www.youtube.com/watch?v=9SiSxrrwY8c&feature=youtu.be&hd=1)

[#13 Working with a Database - Editing a record - Part 8](https://www.youtube.com/watch?v=s2KDKQO1Wm4&feature=youtu.be&hd=1)

[#14 Working with a Database - Model Validations and Scaffolding - Part 9](https://www.youtube.com/watch?v=BrdBqZ5bgRY&feature=youtu.be&hd=1)

[#15 Working with a Database - Create a Drop Down List - Part 10](https://www.youtube.com/watch?v=kSL94ZSqez0&feature=youtu.be&hd=1)

[#16 Model within a model (Part 1 of 2)](https://www.youtube.com/watch?v=_qtW1bWsy1E&feature=youtu.be&hd=1)

[#17 Model within a model (Part 2 of 2)](https://www.youtube.com/watch?v=4TkBwbgZIcE&feature=youtu.be&hd=1)

[#18 Login Screen via Authorize (works with Videos #16 and #17 Model within a model)](https://www.youtube.com/watch?v=aM5QKZSfbPQ&feature=youtu.be&hd=1)


