using Microsoft.AspNetCore.Http;
using Shouldly;
using Xunit;

namespace AcademicWork.Serilog.Tests
{
    public class RequestExtensionsTests
    {
        [Fact]
        public void ShouldReturnCorrelationIdIfExists()
        {
            HttpContext context = new DefaultHttpContext();
            context.Request.Headers["X-AW-CorrelationId"] = "1";
            context.Request.GetAwCorrelationId().ShouldBe("1");
        }
        [Fact]
        public void ShouldReturnNullWhenCorrelationIdIsMissing()
        {
            HttpContext context = new DefaultHttpContext();
            context.Request.GetAwCorrelationId().ShouldBeNull();
        }
    }
}