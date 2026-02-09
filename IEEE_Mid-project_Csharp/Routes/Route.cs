using MID_PROJ.Pages;
namespace MID_PROJ.Routes;
public class Route
{
    public required Func<Page> PageFactory { get; set; }
}
