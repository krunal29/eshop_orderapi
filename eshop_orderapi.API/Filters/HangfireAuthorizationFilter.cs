using Hangfire.Dashboard;
using eshop_orderapi.Business.Helpers;

namespace eshop_orderapi.API.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return new AppSettings().EnableHangfire;
        }
    }
}