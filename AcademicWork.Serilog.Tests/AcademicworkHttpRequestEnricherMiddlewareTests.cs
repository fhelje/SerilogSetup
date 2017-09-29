using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using Shouldly;
using Xunit;

namespace AcademicWork.Serilog.Tests
{
    public class AcademicworkHttpRequestEnricherMiddlewareTests
    {
        [Fact]
        public void ShouldPushRemoteIpAdressAsPropertyOnLogEvent()
        {

            const string ipString = "10.10.0.0";
            var logger = new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .WriteTo.Sink(new TestCorrelatorSink())
                            .CreateLogger();

            using (TestCorrelator.CreateContext())
            {
                var sut = new AcademicworkHttpRequestEnricherMiddleware(innerHttpContext =>
                {
                    logger.Information("Test");
                    var events = TestCorrelator.GetLogEventsFromCurrentContext();
                    events.First().Properties["UserIp"].ToString().ShouldContain(ipString);
                    return Task.FromResult(0);
                });
                HttpContext context = new DefaultHttpContext();
                context.Request.Headers["X-AW-CorrelationId"] = "1";
                context.Connection.RemoteIpAddress = IPAddress.Parse(ipString);
                    
                sut.Invoke(context);

            }
        }

        [Fact]
        public void ShouldPushAwCorrelationIdFromRequestHeadersToLogEventProperties()
        {
            const string ipString = "10.10.0.0";
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Sink(new TestCorrelatorSink())
                .CreateLogger();

            using (TestCorrelator.CreateContext())
            {
                var sut = new AcademicworkHttpRequestEnricherMiddleware(innerHttpContext =>
                {
                    logger.Information("Test");
                    var events = TestCorrelator.GetLogEventsFromCurrentContext();
                    events.First().Properties[AcademicworkHttpRequestEnricherMiddleware.AwCorrelationid].ToString().ShouldBe("\"1\"");
                    return Task.FromResult(0);
                });
                HttpContext context = new DefaultHttpContext();
                context.Request.Headers["X-AW-CorrelationId"] = "1";
                context.Connection.RemoteIpAddress = IPAddress.Parse(ipString);

                sut.Invoke(context);

            }
        }

        [Fact]
        public void ShouldCreateNewAwCorrelationIdWhenMissingInRequestHeaders()
        {
            const string ipString = "10.10.0.0";
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Sink(new TestCorrelatorSink())
                .CreateLogger();

            using (TestCorrelator.CreateContext())
            {
                var sut = new AcademicworkHttpRequestEnricherMiddleware(innerHttpContext =>
                {
                    logger.Information("Test");
                    var logEvent = TestCorrelator.GetLogEventsFromCurrentContext().First();
                    logEvent.Properties.ContainsKey(AcademicworkHttpRequestEnricherMiddleware.AwCorrelationid);
                    logEvent.Properties[AcademicworkHttpRequestEnricherMiddleware.AwCorrelationid].ToString().Length.ShouldBeGreaterThan(36);
                    return Task.FromResult(0);
                });
                HttpContext context = new DefaultHttpContext();
                context.Connection.RemoteIpAddress = IPAddress.Parse(ipString);

                sut.Invoke(context);

            }
        }
    }
}