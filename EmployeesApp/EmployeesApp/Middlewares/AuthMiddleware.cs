namespace EmployeesApp.Web.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        var path = httpContext.Request.Path;
        if (path.HasValue && (path.Value.StartsWith("/Employee") || path.Value.StartsWith("/Todo")))
        {
            if (httpContext.Session.GetString("token") == null)
            {
                httpContext.Response.Redirect("/Login");
            }
        }
        return _next(httpContext);
    }
}



