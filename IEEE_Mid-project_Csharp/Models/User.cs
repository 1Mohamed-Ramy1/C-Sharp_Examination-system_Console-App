namespace MID_PROJ.Models;
using System.Collections.Generic;
using MID_PROJ.Services;
using Newtonsoft.Json;

public class User : IIdentifiable
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
    public List<int> ExamHistory { get; set; }
    public DateTime RegistrationDate { get; set; }

    [JsonConstructor]
    public User()
    {
        Username = "";
        Email = "";
        Password = "";
        IsAdmin = false;
        ExamHistory = new List<int>();
        RegistrationDate = DateTime.Now;
    }

    public User(string username, string email, string password, bool isAdmin = false)
    {
        Username = username;
        Email = email;
        Password = password;
        IsAdmin = isAdmin;
        ExamHistory = new List<int>();
        RegistrationDate = DateTime.Now;
    }
}
