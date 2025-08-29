public static class ServiceLocator
{
    public static IServiceProvider? ServiceProvider { get; set; }

    public static T GetService<T>() where T : class
    {
        return ServiceProvider?.GetService(typeof(T)) as T;
    }
}