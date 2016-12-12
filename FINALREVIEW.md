# Multiple Choise (14 questions, 30/100)

### #1 HTTPPost vs. HTTPGet
The HttpGet request attribute is what the browser issues each time someone clicks on a link and will be responsible for display the initial blank form.

This action method is used to render the view RsvpForm.cshtml and generate the form.

```C#
[HttpGet]
public ViewResult RsvpForm() 
{
  return View();
}
```

The HttpPost request attribute is responsible for receiving submitted data and deciding what to do with it.

The post action method is using Model Binding where the incoming data (guestResponse of type GuestResponse) is parsed and the key/value pairs in the Http request are used to populate the properties of the domain model type.

The names of the input elements are used to set the values of the properties in the instance of the model class which is then passed to the Post-enabled action method (the form fields values are automatically passed to the guestResponse object model properties)

```C#
[HttpPost]
public ViewResult RsvpForm(GuestResponse guestResponse) 
{
  return View("Thanks", guestResponse);
}
```

Both of the action methods will have the same name but MVC will make sure the correct action method is selected based upon whether the system is getting the data or the user clicked on the submit button
In order to use the model in the controller you need to make sure it is imported: `using FisherUsedCars.Models;`

### #2 Validations/Properties/Attributes
Here are some examples of model validations and properties

`[Key]`

`[Required(ErrorMessage = "Please enter your name")]`

`[PasswordPropertyText]`

`[DisplayName("First Name")]`

`[StringLength(50, MinimumLength = 5, ErrorMessage = "The maximum length is 50, the minimum is 5")]`

`[EmailAddress]`

These will always be directly above whatever attribute you are defining, such as `public int userID { get; set; }`


### #3 @HTML.HiddenFor
This lets you hide an attribute in a table from the user so that the user doesn't see it and can't edit it, but it is still sent to the controller.

You will always do this with IDs. You don't want to edit an ID because they are identities, but you'll always need to pass an ID to the control when trying to edit or change or update something.

```HTML
@Html.HiddenFor(m => m.degreeID)
@Html.HiddenFor(m => m.degreeQuestionID)
```

### #4 Strongly Typed 

Strongly typed helpers allow you to work directly with a model to retrieve data.

You must first include the model in the top of the view: `@model FisherUsedCars.Models.User`

Once the model is in place, the VIEW can now work with the fields in the model. You can explicity type the model.field or use a lambda expression:

`@Html.TextBoxFor(m => m.FirstName)`

OR

`@Html.TextBox("FirstName", Model.FirstName);`

Strongly typed helpers can also take advantage of meta data for the model

`@Html.Label("FirstName")` produces: `<label for="FirstName">First Name</label>`

where the model contains:
```C#
[DisplayName("First Name")]
public string FirstName  { get; set; }
```
### #5 Lambda Expressions
A lambda expression is away to write an anonymous function, i.e. a function without a name. What you have on the left side of the "arrow" are the function parameters, and what you have on the right side are the function body. Thus, (x => x.Name) logically translates to something like string Function(Data x) { return x.Name } the types string and Data will obviously vary and be derived from the context.
The `model => item.FirstName` means logically it would translate to string

```C#
Function(Model model) {
  return item.FirstName;
}
```
Basically this is a function with a parameter, but this parameter is not used.

### #6 ModelState.IsValid
The `ModelState.IsValid` property checks to see if all items pass validation.

You will add this C# statement in your controller and if the model does not validate then you will return the view for the user to fix the problems.

```C#
[HttpPost]
public ViewResult RsvpForm(GuestResponse guestResponse) 
{
  if (ModelState.IsValid) 
  {
    return View("Thanks", guestResponse);
  } 
  else 
  {
    //Validation Error
    return View();
  }
}
```

### #7 HTML.Actionlink
`Html.ActionLink` is a helper method. It include methods for links, text inputs, checkboxes, selections, etc.

ActionLink takes 2 parameters: (1)  Text to display in the link, and action method to perform when the text is clicked

```C#
@Html.ActionLink("Create an Account", "Register", "Home", null, null)
```

### #8 Authentication vs. Authorization
Authentication is determining who can access your system.

Authorization is figuring out what someone can access once they've been authenticated.

### #9 NuGet
You can add bootstrap and jquery to an empty view/project by using NuGet. NuGet is a manager that allows you to download packages.

Click on Tools | NuGet Package Manager | Manage NuGet Packages for Solution.

The Content folder will automatically be created and the necessary CSS files will be placed in it along with the necessary script files in the Scripts folder.

### #10 HTML Helpers
Some of the following HTML helpers that are built into the ASP.NET MVC framework are: `Html.BeginForm`, `Html.TextBox`, `Html.TextArea`, `Html.Password`, `Html.Hidden`, `Html.CheckBox`, `Html.RadioButton`, `Html.DropDownList` and `Html.ListBox` (all items are displayed without drop down), `Html.ValidationSummary` and `Html.ValidationMessage`.

`Html.BeginForm` helper writes an opening form tag. The form uses a POST method and the request is processed by the action method for the view.

```HTML
@using (Html.BeginForm())
{
    @Html.TextBox("Name");
    @Html.Password("Password");
    <input type="submit" value="Sign In">
}
```

`Html.Password` is a text box that allows for password entries and uses "*" for input.

```HTML
//Code in the View
@Html.Password("First")
```

`Html.Hidden` is a field designed to make it easy to bind to view data or model data and also used for storing a value.

```HTML
//Code in the View
@Html.Hidden("First")
```


# Programming (70/1000)

### #1
This is how you make a picture a link, you may or may not need the paragraph tag
```HTML
<p><a href="@Url.Action("About", "Home")"><img src="~/Content/Images/CowboyUpHorseshoe.png" height="180" width="300"/></a> </p>
```

### #2
Here is how you hard code a login. You will do all the steps to create a normal login (talked about on the other readme doc), but you will use this login action method instead of the other one.
```C#
[HttpPost]
public ActionResult Login(FormCollection form, bool rememberMe = false)
{
    String username = form["username"].ToString();
    String password = form["Password"].ToString();

    if (string.Equals(username, "byu") && (string.Equals(password, "cougars")))
    {
        FormsAuthentication.SetAuthCookie(username, rememberMe);
        return RedirectToAction("Index", "Home");
    }
    else
    {
        return View();
    }
}
```

### #3
This is how you would query the database in a controller to return the data for a single record.
```C#
IEnumerable<NewPlayer> player =
        db.Database.SqlQuery<NewPlayer>(
    "Select Team.teamID, Team.teamName, Player.playerID, " +
    "FROM Team, Player, Position " +
    "WHERE Team.teamID = Player.teamID");
```

This is how you would execute a sql command to delete a record.
```C#
db.Database.ExecuteSqlCommand("DELETE From Student WHERE Student.StudentID = " + id);
```


### #4
REMEMBER TO COMMENT CONTROLLERS AND INCLUDE YOUR NAME & DATE CREATED
