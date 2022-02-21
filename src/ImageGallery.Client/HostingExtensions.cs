using Microsoft.Net.Http.Headers;

namespace ImageGallery.Client;

public static class HostingExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews()
             .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

        // create an HttpClient used for accessing the API
        builder.Services.AddHttpClient("APIClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:44366/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        });
    }
}
