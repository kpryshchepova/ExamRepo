namespace EmployeesApp.Web.Models;

public class UserAuthModel
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public UserAuthModel(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}
