using System.Diagnostics;

namespace E_CommerceApi.Middlewares
{
    public class HandleRequestsMiddleware
    {
        private readonly RequestDelegate _next;
        private int _cnt = 0;
        private DateTime _start = DateTime.Now;
        public HandleRequestsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            _cnt++;
            if (DateTime.Now.Subtract(_start).Seconds <= 10)
            {
                if (_cnt < 50)
                {
                    await _next(context);
                }
                else
                {
                    context.Response.StatusCode = 500;
                    context.Response.BodyWriter.WriteAsync(System.Text.Encoding.UTF8.GetBytes("Too many requests. Please try after some time."));
                }
            }
            else
            {
                _start = DateTime.Now;
                _cnt = 1;
                await _next(context);
            }
        }

    }
}
