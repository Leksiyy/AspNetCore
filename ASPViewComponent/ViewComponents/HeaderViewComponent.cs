using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace ASPViewComponent.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        string htmlContent = String.Empty;
        using (StreamReader reader = new StreamReader("Files/header.html"))
        {
            htmlContent = await reader.ReadToEndAsync();
        }
        return new HtmlContentViewComponentResult(new HtmlString(htmlContent));
    }
}