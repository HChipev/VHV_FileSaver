﻿using Microsoft.EntityFrameworkCore;
using VHV_FileSaver.Data;
using VHV_FileSaver.Api.Filter;
using VHV_FileSaver.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VHV_FileSaver.Data.Repository;
using VHV_FileSaver.Services.Abstract;
using VHV_FileSaver.Services.Implementation;
using System.Reflection;
using VHV_FileSaver.ViewModels.UserModels.UserProfiles;
using Microsoft.AspNetCore.Identity;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterDbContext(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection")));

        services.AddIdentity<User, Role>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
        if (environment.IsDevelopment())
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider = "Default";
            });
        }
        return services;
    }


    public static IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScopedServiceTypes(typeof(TokenService).Assembly, typeof(IService));
        services.AddAutoMapper(typeof(UserProfile));
        if (environment.IsDevelopment())
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("*")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .WithExposedHeaders("*");
                });
            });
        }
        return services;
    }

    public static IServiceCollection ConfigureAuth(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });
        services.AddAuthorization();
        return services;
    }

    public static IServiceCollection RegisterFilters(this IServiceCollection services)
    {

        services.AddControllers(options =>
        {
            options.Filters.Add<CustomExceptionFilter>();
        });

        return services;
    }

    private static IServiceCollection AddScopedServiceTypes(this IServiceCollection services, Assembly assembly, Type fromType)
    {
        var serviceTypes = assembly.GetTypes()
            .Where(x => !string.IsNullOrEmpty(x.Namespace) && x.IsClass && !x.IsAbstract && fromType.IsAssignableFrom(x))
            .Select(x => new
            {
                Interface = x.GetInterface($"I{x.Name}"),
                Implementation = x
            });
        foreach (var serviceType in serviceTypes)
        {
            services.AddScoped(serviceType.Interface, serviceType.Implementation);
        }
        return services;
    }
}
