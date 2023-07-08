namespace star_wars.Web;

public static class ConfigureServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddRazorPages();

        return services;
    }
}