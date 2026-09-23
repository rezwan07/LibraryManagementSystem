using LibraryManagementSystem.Core.Interfaces;
using LibraryManagementSystem.Infrastructure.Data;
using LibraryManagementSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<LibraryDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                options.UseSqlServer(connectionString, sqloptions =>
                {
                sqloptions.MigrationsAssembly(

                    typeof(LibraryDbContext).Assembly.FullName);

                sqloptions.EnableRetryOnFailure(

                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null);

                });

                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });

            services.AddScoped(
                typeof(IGenericRepository<>),
                typeof(GenericRepository<>)
            );

            services.AddScoped<IBookRepository, BookRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
              
        }
    }
}
