using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace AcademicWork.Serilog
{
    public class AcademicworkHttpRequestEnricherMiddleware
    {
        public const string AwCorrelationid = "X-AW-CorrelationId";
        private readonly RequestDelegate _next;

        public AcademicworkHttpRequestEnricherMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var originalCorrelationId = context.Request
                                               .Headers[AwCorrelationid]
                                               .FirstOrDefault();

            var correlationId = originalCorrelationId ?? Guid.NewGuid().ToString();

            if (originalCorrelationId == null)
            {
                context.Request.Headers.Add(AwCorrelationid, new StringValues(correlationId));
            }

            using (LogContext.PushProperty("UserIp", context.Connection.RemoteIpAddress))
            using (LogContext.PushProperty(AwCorrelationid, correlationId))
            {
                return _next(context);
            }
        }
    }
}