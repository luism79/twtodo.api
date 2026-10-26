using backend.services;
using backend.services.interfaces;

namespace backend.extensions
{
    public static class UserCurrentExtensions
    {
        public static IServiceCollection AddUserCurrent(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IUserCurrent, UserCurrent>();

            return services;
        }
    }
}
