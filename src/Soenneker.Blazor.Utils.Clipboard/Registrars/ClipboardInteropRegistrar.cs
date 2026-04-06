using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blazor.Utils.Clipboard.Abstract;
using Soenneker.Blazor.Utils.ModuleImport.Registrars;

namespace Soenneker.Blazor.Utils.Clipboard.Registrars;

/// <summary>
/// Registration for clipboard interop and util services.
/// </summary>
public static class ClipboardInteropRegistrar
{
    /// <summary>
    /// Adds <see cref="IClipboardInterop"/> and <see cref="IClipboardUtil"/> as scoped services.
    /// Registers <see cref="Soenneker.Blazor.Utils.ModuleImport.Abstract.IModuleImportUtil"/> as scoped when needed.
    /// </summary>
    public static IServiceCollection AddClipboardAsScoped(this IServiceCollection services)
    {
        services.AddModuleImportUtilAsScoped()
                .TryAddScoped<IClipboardInterop, ClipboardInterop>();
        services.TryAddScoped<IClipboardUtil, ClipboardUtil>();

        return services;
    }
}