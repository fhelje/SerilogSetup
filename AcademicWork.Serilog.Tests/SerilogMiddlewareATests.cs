using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using Shouldly;
using Xunit;

namespace AcademicWork.Serilog.Tests
{
    public class SerilogMiddlewareTests
    {
        [Fact]
        public void ShouldLogExceptionWithExtraInfoWhen500StatusCode()
        {
            var bodyStream = new MemoryStream();
            var sw = new StreamWriter(bodyStream);
            sw.Write("{ \"data\": \"data\" }");
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Sink(new TestCorrelatorSink())
                .CreateLogger();

            using (TestCorrelator.CreateContext())
            {
                var sut = new SerilogMiddleware(innerHttpContext =>
                {
                    logger.Information("Test");
                    var logEvent = TestCorrelator.GetLogEventsFromCurrentContext().First();
                    VerifyLogExceptionProperiesExists(logEvent);
                    VerifyLogMessage(logEvent);
                    return Task.FromResult(0);
                });
                Setup500HttpContextWithApplicationJson(bodyStream);
            }
        }

        [Fact]
        public void ShouldLogInformationWhenNot500StatusCode()
        {
            Assert.True(false, "NotImplemented");
        }

        [Fact]
        public void ShouldLogInformationWhenNoQS()
        {
            Assert.True(false, "NotImplemented");
        }

        [Fact]
        public void ShouldLogInformationWhenNoHeaders()
        {
            Assert.True(false, "NotImplemented");
        }

        [Fact]
        public void ShouldAddFormDataWhen500StatuscodeAndPostForm()
        {
            Assert.True(false, "NotImplemented");
        }

        private void VerifyLogMessage(LogEvent logEvent)
        {
            logEvent.RenderMessage().ShouldStartWith("HTTP Get /Path responded 500 in ");
        }

        private static void VerifyLogExceptionProperiesExists(LogEvent logEvent)
        {
            logEvent.Level.ShouldBe(LogEventLevel.Error);
            logEvent.Properties.ContainsKey("RequestQueryString").ShouldBeTrue();
            logEvent.Properties.ContainsKey("RequestHeaders").ShouldBeTrue();
            logEvent.Properties.ContainsKey("RequestForm").ShouldBeFalse();
            logEvent.Properties.ContainsKey("RequestHost").ShouldBeTrue();
            logEvent.Properties.ContainsKey("RequestProtocol").ShouldBeTrue();
            logEvent.Properties.ContainsKey("StatusCode").ShouldBeTrue();
            logEvent.Properties.ContainsKey("Elapsed").ShouldBeTrue();
        }

        private static void Setup500HttpContextWithApplicationJson(Stream bodyStream)
        {
            var context = new DefaultHttpContext();
            context.Response.StatusCode = 500;
            context.Request.Method = "Get";
            context.Request.Path = "/Path";
            context.Request.QueryString = new QueryString("?a=1");
            context.Request.Headers["Test"] = "test";
            context.Request.Host = new HostString("localhost", 5000);
            context.Request.Protocol = "http";
            context.Request.Body = bodyStream;
            context.Request.ContentType = "application/json";
        }

        private static void Setup200HttpContextWithApplicationJson(Stream bodyStream)
        {
            var context = new DefaultHttpContext();
            context.Response.StatusCode = 200;
            context.Request.Method = "Get";
            context.Request.Path = "/Path";
            context.Request.QueryString = new QueryString("?a=1");
            context.Request.Headers["Test"] = "test";
            context.Request.Host = new HostString("localhost", 5000);
            context.Request.Protocol = "http";
            context.Request.Body = bodyStream;
            context.Request.ContentType = "application/json";
        }
    }
}