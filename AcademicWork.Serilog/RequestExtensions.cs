using System.Linq;
using Microsoft.AspNetCore.Http;

namespace AcademicWork.Serilog
{
    public static class RequestExtensions
    {
        public static string GetAwCorrelationId(this HttpRequest request)
        {
            return request.Headers[AcademicworkHttpRequestEnricherMiddleware.AwCorrelationid].FirstOrDefault();
        }
    }
}
