using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace ASPViewComponent.ViewComponents;

public class TimerViewComponent : ViewComponent
{
    private readonly ITime _time;

    public TimerViewComponent(ITime time)
    {
        //HtmlContentViewComponentResult
        //ContentViewComponentResult
        //ViewViewComponentResult
        _time = time;
    }
    
    public IViewComponentResult Invoke(bool inculdeSeconds)
    {
        //return Content(_time.GetTime(inculdeSeconds));
        return new HtmlContentViewComponentResult(
            new HtmlString($"<div style=\"color:red\">{_time.GetTime(inculdeSeconds)}</div>"));
    }
}