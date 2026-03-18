namespace E_CommerceApi.Middlewares
{
    public class CounterMiddleware
    {
        private readonly RequestDelegate _next;
        private static long Counter = 0;

        public CounterMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            Counter++;
            await _next(context);
            Console.WriteLine($"Now The Number Of Requsets Is: {Counter}");
        }
    }
}
