namespace MID_PROJ.Services;
using MID_PROJ.Models;

public static class AdminManager
{
    private static readonly User _adminUser = new()
    {
        Id =0,
        Username = "admin",
        Email = "admin",
        Password = "admin",
        IsAdmin = true
    };

    public static User GetAdmin()
    {
        return _adminUser;
    }

    public static bool IsAdmin(string email, string password)
    {
        return email.Equals(_adminUser.Email, StringComparison.OrdinalIgnoreCase)
               && password == _adminUser.Password;
    }
}