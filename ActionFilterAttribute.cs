using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WeatherAPI;

public class ActionFilterAttribute : Attribute, IActionFilter
{
    private readonly string _name;
    
    public ActionFilterAttribute(string name)
    {
        _name = name;
    }
    
    public void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine($"OnActionExecuting - {_name}");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        Console.WriteLine($"OnActionExecuted - {_name}");
    }
}