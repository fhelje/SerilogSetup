using System.Linq;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using Shouldly;
using Xunit;

namespace AcademicWork.Serilog.Tests
{
    public class EventTypeEnricherTests
    {
        [Fact]
        public void ShouldContainPropertyEventTypeWhenEventTypeEnricherIsUsed()
        {
            var logger = new LoggerConfiguration()
                                .Enrich.With<EventTypeEnricher>()
                                .WriteTo.Sink(new TestCorrelatorSink())
                                .CreateLogger();
            using (TestCorrelator.CreateContext())
            {
                logger.Information("Test {id}", 1);

                var first = TestCorrelator.GetLogEventsFromCurrentContext().First();
                first.Properties.ContainsKey("EventType").ShouldBeTrue();

            }
        }
        [Fact]
        public void ShouldContainPropertyEventTypeWhenEventTypeEnricherIsUsedWithEmptyLog()
        {
            var logger = new LoggerConfiguration()
                .Enrich.With<EventTypeEnricher>()
                .WriteTo.Sink(new TestCorrelatorSink())
                .CreateLogger();
            using (TestCorrelator.CreateContext())
            {
                logger.Information("");

                var first = TestCorrelator.GetLogEventsFromCurrentContext().First();
                first.Properties.ContainsKey("EventType").ShouldBeTrue();

            }
        }
    }
}
