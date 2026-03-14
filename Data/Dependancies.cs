namespace E_Commerce_Proj.Data
{
    public static class Dependancies 
    {
        public static IServiceCollection AddSwaggerSettings(this IServiceCollection services)
        {
            services.AddSwaggerGen();
            return services;
        }

        public static IServiceCollection AddDbContextSettings(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();
            return services;
        }
    }
}
