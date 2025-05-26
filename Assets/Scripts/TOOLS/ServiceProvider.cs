using System;
using System.Collections.Generic;

public static class ServiceProvider
{
    private static readonly Dictionary<Type, object> Services = new();

    public static void SetService(Type type, object service, bool overrideIfFound = false)
    {
        if (!Services.TryAdd(type, service) && overrideIfFound)
            Services[type] = service;
    }

    public static void SetService<T>(T service, bool overrideIfFound = false)
    {
        if (!Services.TryAdd(typeof(T), service) && overrideIfFound)
            Services[typeof(T)] = service;
    }

    public static object GetService(Type type)
    {
        return Services.GetValueOrDefault(type);
    }

    public static bool TryGetService<T>(out T service) where T : class
    {
        if (Services.TryGetValue(typeof(T), out var serviceObject) && serviceObject is T tService)
        {
            service = tService;
            return true;
        }

        service = null;
        return false;
    }

    public static void RemoveService<T>()
    {
        Services.Remove(typeof(T));
    }

    public static void RemoveService(Type type)
    {
        Services.Remove(type);
    }
}
