using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace Middleware.LoggingErrorInFile
{
    public class LoggingError
    {
        private readonly RequestDelegate _requestDelegate;

        public LoggingError(RequestDelegate requestDelegate)
        {
            requestDelegate = requestDelegate ?? throw new ArgumentNullException(nameof(requestDelegate));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                throw new Exception("Mistake occured while processing");
                await _requestDelegate.Invoke(context);
            }
            catch (Exception e)
            {
                using (FileStream fstream = new FileStream("errors.txt", FileMode.OpenOrCreate))
                {
                    byte[] buffer = Encoding.Default.GetBytes(e.Message);
                    
                    await fstream.WriteAsync(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
