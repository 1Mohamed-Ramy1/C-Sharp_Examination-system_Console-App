using MID_PROJ.Routes;

namespace MID_PROJ.Pages;
public abstract class Page{    
    public abstract void Display();
     public abstract void HandleInput(Router router);
}