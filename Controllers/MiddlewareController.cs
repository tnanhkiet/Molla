using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Web.SessionService;

public class MiddlewareController : Controller
{
    public Member member;
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (HttpContext.Session.Get<Member>("Member") == null && filterContext.RouteData.Values["action"].ToString() != "Login" && filterContext.RouteData.Values["action"].ToString() != "CheckMember")
        {
            filterContext.Result = new RedirectResult("/Admin/Login");
        }
        else
        {
            member = HttpContext.Session.Get<Member>("Member");
            // filterContext.Result = new RedirectResult("/Admin");

        }
        base.OnActionExecuting(filterContext);
    }
}