using Microsoft.AspNetCore.Builder;

namespace AcademicWork.Serilog
{
    public static class AcademicworkMiddlewareExtensions
    {
        public static IApplicationBuilder UseAcademicworkHttpRequestEnricher(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AcademicworkHttpRequestEnricherMiddleware>();
        }
        public static IApplicationBuilder UseAcademicworkErrorEnricher(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SerilogMiddleware>();
        }
    }
}