using EventsReminderApp.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EventsReminderApp.MVC.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration )
        {
            services.AddDbContext<EventsReminderAppContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("EventsDb")));

            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<EventsReminderAppContext>();

            services.AddScoped<EventsReminderAppContext>();
        }

    }
}
