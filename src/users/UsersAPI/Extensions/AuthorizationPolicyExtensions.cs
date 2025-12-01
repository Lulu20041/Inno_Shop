namespace UsersAPI.Extensions
{
    public static class AuthorizationPolicyExtensions
    {
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireClaim("userRole", "1")); // Admin role is 1

                options.AddPolicy("UserOnly", policy =>
                    policy.RequireClaim("userRole", "0")); // User role is 0

                options.AddPolicy("UserOrAdmin", policy =>
                    policy.RequireClaim("userRole", "0", "1")); // Either User or Admin
            });

            return services;
        }
    }
}
