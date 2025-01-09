using Microsoft.AspNetCore.Mvc.Filters;

namespace ASPFilters.Filters;

// public class SimpleResourceFilter : IActionFilter
// {
//     public void OnActionExecuting(ActionExecutingContext context)
//     {
//         
//     }
//
//     public void OnActionExecuted(ActionExecutedContext context)
//     {
//         
//     }
// }

// public class SimpleAsyncResourceFilter : IAsyncResourceFilter
// {
//     public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
//     {
//         await next();
//     }
// }

public class SimpleResourceFilter : Attribute, IResourceFilter
{
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        //
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        context.HttpContext.Response.Cookies.Append("LastVisit", DateTime.Now.ToString());
    }
}