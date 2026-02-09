using MID_PROJ.Pages;
using MID_PROJ.Routes;
using MID_PROJ.Services;

namespace MID_PROJ;
public class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var router = new Router();
        router.Register("home", () => new HomePage());
        router.Register("main", () => new MainMenuPage());
        router.Register("login", () => new LoginPage());
        router.Register("register", () => new RegisterPage());
        router.Register("about", () => new AboutPage());
        router.Register("profile", () => new ProfilePage());
        router.Register("subjects", () => new SubjectsPage());
        router.Register("takeexam", () => new TakeExamPage());
        router.Register("history", () => new HistoryPage());
        router.Register("admin", () => new AdminPage());
        router.Register("createexam", () => new CreateExamPage());
        router.Register("editexam", () => new EditExamPage());
        router.Register("deleteexam", () => new DeleteExamPage());
        router.Register("statistics", () => new StatisticsPage());

        router.Start("home");
    }
}