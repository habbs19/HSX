using Microsoft.AspNetCore.Mvc.Filters;

namespace HSX.Infrastructure.Filters;

public class UpdateUserLastActiveFilter : IAsyncActionFilter
{

    public UpdateUserLastActiveFilter()
    {

    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();
        if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

       
    }
}
