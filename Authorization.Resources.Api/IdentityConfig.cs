using System;
namespace Authorization.Resources.Api
{
    public static class IdentityConfig
    {
        public static void ConfigureUserRequirements(Microsoft.AspNetCore.Identity.IdentityOptions options) {

            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 5;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            options.User.RequireUniqueEmail = false;
        }
    }
}
