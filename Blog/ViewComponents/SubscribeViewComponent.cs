using Microsoft.AspNetCore.Mvc;

namespace Blog.ViewComponents;

public class SubscribeViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("Subscriber");
    }
}