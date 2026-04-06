using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Soenneker.Blazor.Utils.Clipboard.Abstract;
using Soenneker.Blazor.Utils.Clipboard.Dtos;
using Soenneker.Blazor.Utils.Clipboard.Enums;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;

namespace Soenneker.Blazor.Utils.Clipboard;

/// <inheritdoc cref="IClipboardInterop"/>
public sealed class ClipboardInterop : IClipboardInterop
{
    private const string _modulePath = "/_content/Soenneker.Blazor.Utils.Clipboard/js/clipboardinterop.js";

    private readonly IModuleImportUtil _moduleImportUtil;
    private readonly CancellationScope _cancellationScope = new();

    private bool? _hasClipboardCache;

    public ClipboardInterop(IModuleImportUtil moduleImportUtil)
    {
        _moduleImportUtil = moduleImportUtil;
    }

    private static ClipboardPermissionState ParsePermissionState(string? state)
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
                IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
                var result = await module.InvokeAsync<bool>("hasClipboard", linked);

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
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            var state = await module.InvokeAsync<string>("getReadPermissionState", linked);

            return ParsePermissionState(state);
        }
    }

    public async ValueTask<ClipboardPermissionState> GetWritePermissionState(CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            var state = await module.InvokeAsync<string>("getWritePermissionState", linked);

            return ParsePermissionState(state);
        }
    }

    public async ValueTask<string> ReadText(CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            return await module.InvokeAsync<string>("readText", linked);
        }
    }

    public async ValueTask WriteText(string? text, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("writeText", linked, text ?? "");
        }
    }

    public async ValueTask<IReadOnlyList<ClipboardItemDto>> Read(CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            var list = await module.InvokeAsync<List<ClipboardItemDto>>("read", linked);

            return list ?? [];
        }
    }

    public async ValueTask Write(IEnumerable<ClipboardItemDto> items, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("write", linked, items);
        }
    }

    public ValueTask Clear(CancellationToken cancellationToken = default)
    {
        return WriteText("", cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _moduleImportUtil.DisposeContentModule(_modulePath);
        await _cancellationScope.DisposeAsync();
    }
}