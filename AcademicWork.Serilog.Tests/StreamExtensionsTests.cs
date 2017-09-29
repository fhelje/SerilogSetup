using System.IO;
using Xunit;
using Shouldly;

namespace AcademicWork.Serilog.Tests
{
    public class StreamExtensionsTests
    {
        [Fact]
        public void ShouldReturnStreamDataAsString()
        {
            using (var stream = new MemoryStream())
            {
                var sw = new StreamWriter(stream);
                sw.Write("Test");
                sw.Flush();
                stream.GetAsString().ShouldBe("Test");
            }
        }

        [Fact]
        public void ShouldReturnDescriptiveMessageIfStreamCantBeSeeked()
        {
            using (var stream = new MemoryStream())
            {
                var sw = new StreamWriter(stream);
                sw.Write("Test");
                sw.Flush();
                stream.Close();
                stream.GetAsString().ShouldBe("Cannot unwind body stream");
            }
        }
    }
}
