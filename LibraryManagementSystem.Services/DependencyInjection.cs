using LibraryManagementSystem.Services.Implementations;
using LibraryManagementSystem.Services.Interfaces;
using LibraryManagementSystem.Services.Mappings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddScoped<IBookService, BookService>();

            return services;
        }
    }
}
