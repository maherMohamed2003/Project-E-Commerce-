using E_Commerce_Proj.Abstracts.Feedback;
using E_Commerce_Proj.Reposetories.CategoryReposetories;
using E_Commerce_Proj.Reposetories.ProductReposetories;
using E_Commerce_Proj.Reposetories.ReviewReposetories;

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

        public static IServiceCollection AddReposetories(this IServiceCollection services)
        {
            services.AddScoped<IFeedbackRepo, FeedbackRepo>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<IReviewRepo, ReviewRepo>();
            return services;
        }
    }
}
