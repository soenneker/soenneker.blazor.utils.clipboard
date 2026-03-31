using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Soenneker.Asyncs.Initializers;
using Soenneker.Blazor.Utils.Clipboard.Abstract;
using Soenneker.Blazor.Utils.Clipboard.Dtos;
using Soenneker.Blazor.Utils.Clipboard.Enums;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;

namespace Soenneker.Blazor.Utils.Clipboard;

/// <inheritdoc cref="IClipboardInterop"/>
public sealed class ClipboardInterop : IClipboardInterop
{
    private const string _modulePath = "Soenneker.Blazor.Utils.Clipboard/js/clipboardinterop.js";
    private const string _jsHasClipboard = "ClipboardInterop.hasClipboard";
    private const string _jsGetReadPermissionState = "ClipboardInterop.getReadPermissionState";
    private const string _jsGetWritePermissionState = "ClipboardInterop.getWritePermissionState";
    private const string _jsReadText = "ClipboardInterop.readText";
    private const string _jsWriteText = "ClipboardInterop.writeText";
    private const string _jsRead = "ClipboardInterop.read";
    private const string _jsWrite = "ClipboardInterop.write";

    private readonly IJSRuntime _jsRuntime;
    private readonly IResourceLoader _resourceLoader;
    private readonly AsyncInitializer _initializer;
    private readonly CancellationScope _cancellationScope = new();

    private bool? _hasClipboardCache;

    public ClipboardInterop(IJSRuntime jsRuntime, IResourceLoader resourceLoader)
    {
        _jsRuntime = jsRuntime;
        _resourceLoader = resourceLoader;
        _initializer = new AsyncInitializer(Initialize);
    }

    private async ValueTask Initialize(CancellationToken token)
    {
        _ = await _resourceLoader.ImportModule(_modulePath, token);
    }

    private async ValueTask EnsureInitialized(CancellationToken cancellationToken)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
            await _initializer.Init(linked);
    }

    private static ClipboardPermissionState ParsePermissionState(string state)
    {
        return state switch
        {
            "granted" => ClipboardPermissionState.Granted,
            "denied" => ClipboardPermissionState.Denied,
            "prompt" => ClipboardPermissionState.Prompt,
            _ => ClipboardPermissionState.Unsupported
        };
    }

    public async ValueTask<bool> HasClipboard(CancellationToken cancellationToken = default)
    {
        if (_hasClipboardCache.HasValue)
            return _hasClipboardCache.Value;

        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            try
            {
                await EnsureInitialized(linked);
                var result = await _jsRuntime.InvokeAsync<bool>(_jsHasClipboard, linked);
                _hasClipboardCache = result;
                return result;
            }
            catch
            {
                _hasClipboardCache = false;
                return false;
            }
        }
    }

    public async ValueTask<ClipboardPermissionState> GetReadPermissionState(CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await EnsureInitialized(linked);
            var state = await _jsRuntime.InvokeAsync<string>(_jsGetReadPermissionState, linked);
            return ParsePermissionState(state);
        }
    }

    public async ValueTask<ClipboardPermissionState> GetWritePermissionState(CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await EnsureInitialized(linked);
            var state = await _jsRuntime.InvokeAsync<string>(_jsGetWritePermissionState, linked);
            return ParsePermissionState(state);
        }
    }

    public async ValueTask<string> ReadText(CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await EnsureInitialized(linked);
            return await _jsRuntime.InvokeAsync<string>(_jsReadText, linked);
        }
    }

    public async ValueTask WriteText(string? text, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await EnsureInitialized(linked);
            await _jsRuntime.InvokeVoidAsync(_jsWriteText, linked, text ?? "");
        }
    }

    public async ValueTask<IReadOnlyList<ClipboardItemDto>> Read(CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await EnsureInitialized(linked);
            var list = await _jsRuntime.InvokeAsync<List<ClipboardItemDto>>(_jsRead, linked);
            return list ?? [];
        }
    }

    public async ValueTask Write(IEnumerable<ClipboardItemDto> items, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await EnsureInitialized(linked);
            await _jsRuntime.InvokeVoidAsync(_jsWrite, linked, items);
        }
    }

    public async ValueTask Clear(CancellationToken cancellationToken = default)
    {
        await WriteText("", cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _resourceLoader.DisposeModule(_modulePath);
        await _initializer.DisposeAsync();
        await _cancellationScope.DisposeAsync();
    }
}
