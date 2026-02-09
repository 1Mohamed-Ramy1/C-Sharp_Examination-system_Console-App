using MID_PROJ.Pages;

namespace MID_PROJ.Routes;

public class Router
{
    private readonly Dictionary<string, Route> _routes = new();
    private string _currentPath = "";

    public void Register(string path, Func<Page> factory)
    {
        _routes[path] = new Route
        {
            PageFactory = factory,
        };
    }

    public void Navigate(string path)
    {
        if (!_routes.TryGetValue(path, out var route))
        {
            Console.WriteLine($"Page {path} not found");
            RerouteCurrent();
            return;
        }
        _currentPath = path;
        var page = route.PageFactory();
        Console.Clear();
        page.Display();
        page.HandleInput(this);
    }

    public void Start(string startPath)
    {
        Navigate(startPath);
    }

    public void RerouteCurrent()
    {
        Navigate(_currentPath);
    }
}
