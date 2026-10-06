using Microsoft.JSInterop;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Blazor.Utils.ModuleImport.Dtos;

internal sealed class SmokeModuleImport(IJSObjectReference module) : IModuleImportUtil
{
    public ValueTask<ModuleImportItem> GetContentModule(string path, CancellationToken cancellationToken = default) => throw new NotSupportedException();
    public ValueTask<ModuleImportItem> GetExternalModule(string url, CancellationToken cancellationToken = default) => throw new NotSupportedException();
    public ValueTask<IJSObjectReference> GetContentModuleReference(string path, CancellationToken cancellationToken = default) => ValueTask.FromResult(module);
    public ValueTask<IJSObjectReference> GetExternalModuleReference(string url, CancellationToken cancellationToken = default) => ValueTask.FromResult(module);
    public ValueTask<bool> DisposeContentModule(string name) => ValueTask.FromResult(true);
    public ValueTask<bool> DisposeExternalModule(string url) => ValueTask.FromResult(true);
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
