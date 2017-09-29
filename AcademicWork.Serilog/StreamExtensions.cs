using System.IO;
using System.Text;

namespace AcademicWork.Serilog
{
    public static class StreamExtensions
    {
        public static string GetAsString(this Stream body)
        {
            if (body.CanSeek)
            {
                body.Position = 0;
                using (StreamReader reader = new StreamReader(body, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            return "Cannot unwind body stream";
        }
    }
}