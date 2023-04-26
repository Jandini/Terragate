using Microsoft.AspNetCore.Mvc.Filters;

namespace Terragate.Api.Controllers
{
    public class InfrastructureIdAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            AppContext.SetData("InfrastructureId", context.RouteData.Values["id"] as string);
            base.OnActionExecuting(context);
        }
    }
}
