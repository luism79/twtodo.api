using backend.repositories;
using backend.repositories.interfaces;

namespace backend.extensions
{
    public static class RepositoriesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ITaskRepository, TaskRepository>();
           
            return services;
        }
    }
}
