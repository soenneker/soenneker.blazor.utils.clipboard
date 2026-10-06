using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.JSInterop;

internal sealed class SmokeModule(string json) : IJSObjectReference
{
    public object?[]? Arguments { get; private set; }

    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, object?[]? args) =>
        InvokeAsync<TValue>(identifier, CancellationToken.None, args);

    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
    {
        Arguments = args;
        if (identifier is "write" or "dispose") return ValueTask.FromResult(default(TValue)!);
        if (typeof(TValue) != typeof(JsonElement)) throw new InvalidOperationException("Interop must request raw JSON before applying generated metadata.");
        using var document = JsonDocument.Parse(json);
        return ValueTask.FromResult((TValue)(object)document.RootElement.Clone());
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
