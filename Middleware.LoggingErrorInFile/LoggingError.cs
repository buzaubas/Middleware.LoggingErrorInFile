using static System.Net.Mime.MediaTypeNames;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Middleware.LoggingErrorInFile
{
    public class LoggingError
    {
        private readonly RequestDelegate requestDelegate;

        public LoggingError(RequestDelegate _requestDelegate)
        {
            requestDelegate = _requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                //throw new Exception("Mistake occured while processing");
                await requestDelegate.Invoke(context);
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
