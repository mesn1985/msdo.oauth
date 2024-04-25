using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace msdo.middleware.httpRequest
{
    public class IncomingCorrelationIdMiddleware(RequestDelegate next)
    {
        private const string _correlationIdHeader = "X-Correlation-Id";
        private const string correlationIdLogPropertyname = "CorrelationId";

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers.TryGetValue(_correlationIdHeader, out StringValues correlationIds);
            var correlationId = correlationIds.FirstOrDefault() ?? Guid.NewGuid().ToString();

            using (LogContext.PushProperty(correlationIdLogPropertyname, correlationId))
            {
                context.Items["Correlation-Id"] = correlationId;
                await next(context);
            }

        }


    }
}
