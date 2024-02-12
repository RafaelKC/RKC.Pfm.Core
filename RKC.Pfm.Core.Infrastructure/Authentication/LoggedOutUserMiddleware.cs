using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Net.Http.Headers;
using RKC.Pfm.Core.Infrastructure.Authentication.Consts;
using RKC.Pfm.Core.Infrastructure.Supabse;

namespace RKC.Pfm.Core.Infrastructure.Authentication;

public class LoggedOutUserMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDistributedCache _distributedCache;
    private readonly ISupabseClient _supabseClient;

    public LoggedOutUserMiddleware(RequestDelegate next, IDistributedCache distributedCache, ISupabseClient supabseClient)
    {
        _next = next;
        _distributedCache = distributedCache;
        _supabseClient = supabseClient;
    }

    public async Task Invoke(HttpContext ctx)
    {
        var authorizationHeader = ctx.Request.Headers[HeaderNames.Authorization];
        if (!string.IsNullOrWhiteSpace(authorizationHeader))
        {
            var arguments = authorizationHeader.ToString().Split(" ");
            if (arguments.Length > 1)
            {
                var token = arguments.LastOrDefault();
                
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var tokenIsInvalid =
                        await _distributedCache.GetStringAsync(AuthenticationConfig.GetTokenCacheKey(token));
                    if (!string.IsNullOrWhiteSpace(tokenIsInvalid))
                    {
                        ctx.Response.Clear();
                        ctx.Response.ContentType = "text/plain";
                        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await ctx.Response.WriteAsync("Invalid User Key");
                        return;
                    }

                    if (_supabseClient.Auth.CurrentSession is null)
                    {
                        ctx.Response.Clear();
                        ctx.Response.ContentType = "text/plain";
                        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await ctx.Response.WriteAsync("Invalid User Key");
                        return;
                    }
                }
            }
        }
        await _next.Invoke(ctx);
    }
}