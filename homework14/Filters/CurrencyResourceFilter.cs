using homework14.Data;
using homework14.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace homework14.Filters;

public class CurrencyResourceFilter : IResourceFilter
{
    private readonly ApplicationContext _context;


    public CurrencyResourceFilter(ApplicationContext context)
    {
        _context = context;
    }
    
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        string currencyCode = context.HttpContext.Request.Headers["Currency"];
        if (string.IsNullOrEmpty(currencyCode))
        {
            currencyCode = "USD";
        }
        var currency = _context.Currencies.FirstOrDefault(x => x.Code == currencyCode);
        if (currency is null)
        {
            context.Result = new BadRequestObjectResult($"Currency {currencyCode} is invalid");
            return;
        }
    }

    public void OnResourceExecuted(ResourceExecutedContext context) { }
}