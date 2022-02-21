using AutoMapper;
using ImageGallery.API.Entities;
using ImageGallery.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace ImageGallery.API;

public static class HostingExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
         .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // register the DbContext on the container, getting the connection string from
        // appSettings (note: use this during development; in a production environment,
        // it's better to store the connection string in an environment variable)
        builder.Services.AddDbContext<GalleryContext>(options =>
        {
            options.UseSqlServer(
                builder.Configuration["ConnectionStrings:ImageGalleryDBConnectionString"]);
        });

        // register the repository
        builder.Services.AddScoped<IGalleryRepository, GalleryRepository>();

        // register AutoMapper-related services
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}
