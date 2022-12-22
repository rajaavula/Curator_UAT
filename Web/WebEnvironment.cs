using Cortex.Core.Utilities.Extensions;

namespace LeadingEdge.Curator.Web
{
    public static class WebEnvironment
    {
#if ENVIRONMENT_DEVELOPMENT
        public const string EnvironmentName = "Development";
#elif ENVIRONMENT_STAGING
        public const string EnvironmentName = "Staging";
#elif ENVIRONMENT_PRODUCTION
        public const string EnvironmentName = "Production";
#else
        public const string EnvironmentName = "Development";
#endif

        public static bool IsDevelopment()
        {
            return EnvironmentName.EqualsIgnoreCase("Development");
        }

        public static bool IsStaging()
        {
            return EnvironmentName.EqualsIgnoreCase("Staging");
        }

        public static bool IsProduction()
        {
            return EnvironmentName.EqualsIgnoreCase("Production");
        }

        public static bool Include(params string[] environments)
        {
            return EnvironmentName.InIgnoreCase(environments);
        }

        public static bool Exclude(params string[] environments)
        {
            return !EnvironmentName.InIgnoreCase(environments);
        }
    }
}