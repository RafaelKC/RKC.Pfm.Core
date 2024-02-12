using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace RKC.Pfm.Core.Infrastructure.Authentication;

public class LoggedOutUserMiddleware
{
    private readonly RequestDelegate _next;

    public LoggedOutUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext ctx)
    {
        var authorizationHeader = ctx.Request.Headers[HeaderNames.Authorization];
        if (!string.IsNullOrWhiteSpace(authorizationHeader))
        {
            var token = authorizationHeader.ToString().Split(" ").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(token))
            {
                ctx.Response.Clear();
                ctx.Response.ContentType = "text/plain";
                ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                // await ctx.Response.WriteAsync("Invalid User Key");
                // return;
            }
        }
        await _next.Invoke(ctx);
    }
}