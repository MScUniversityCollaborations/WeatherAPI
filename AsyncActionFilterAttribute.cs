using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WeatherAPI;

public class ActionAsyncFilterAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _name;
    
    public ActionAsyncFilterAttribute(string name)
    {
        _name = name;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Console.WriteLine($"Before Async Execution - {_name}");
        await next();
        Console.WriteLine($"After Async Execution - {_name}");
    }
}