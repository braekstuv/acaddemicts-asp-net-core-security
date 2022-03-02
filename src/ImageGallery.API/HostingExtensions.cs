using ImageGallery.API.Entities;
using ImageGallery.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ImageGallery.API;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
         .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:44318";
                options.Audience = "imagegalleryapi";
            });

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

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    // ensure generic 500 status code on fault.
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError; ;
                    await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                });
            });
            // The default HSTS value is 30 days. You may want to change this for
            // production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }
}
