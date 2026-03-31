using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Blazor.Utils.Clipboard.Abstract;
using Soenneker.Blazor.Utils.Clipboard.Dtos;
using Soenneker.Blazor.Utils.Clipboard.Enums;

namespace Soenneker.Blazor.Utils.Clipboard;

/// <inheritdoc cref="IClipboardUtil"/>
public sealed class ClipboardUtil : IClipboardUtil
{
    private readonly IClipboardInterop _clipboardInterop;

    public ClipboardUtil(IClipboardInterop clipboardInterop)
    {
        _clipboardInterop = clipboardInterop;
    }

    public ValueTask<bool> HasClipboard(CancellationToken cancellationToken = default)
        => _clipboardInterop.HasClipboard(cancellationToken);

    public ValueTask<ClipboardPermissionState> GetReadPermissionState(CancellationToken cancellationToken = default)
        => _clipboardInterop.GetReadPermissionState(cancellationToken);

    public ValueTask<ClipboardPermissionState> GetWritePermissionState(CancellationToken cancellationToken = default)
        => _clipboardInterop.GetWritePermissionState(cancellationToken);

    public ValueTask<string> ReadText(CancellationToken cancellationToken = default)
        => _clipboardInterop.ReadText(cancellationToken);

    public async ValueTask<(bool Success, string? Text)> TryReadText(CancellationToken cancellationToken = default)
    {
        try
        {
            string text = await _clipboardInterop.ReadText(cancellationToken);
            return (true, text);
        }
        catch
        {
            return (false, null);
        }
    }

    public ValueTask WriteText(string? text, CancellationToken cancellationToken = default)
        => _clipboardInterop.WriteText(text, cancellationToken);

    public async ValueTask<bool> TryWriteText(string? text, CancellationToken cancellationToken = default)
    {
        try
        {
            await _clipboardInterop.WriteText(text, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public ValueTask CopyText(string? text, CancellationToken cancellationToken = default)
        => _clipboardInterop.WriteText(text, cancellationToken);

    public ValueTask CopyPlainAndHtml(string plainText, string? html = null, CancellationToken cancellationToken = default)
        => _clipboardInterop.Write([ClipboardItems.CreatePlainAndHtml(plainText, html)], cancellationToken);

    public ValueTask<IReadOnlyList<ClipboardItemDto>> Read(CancellationToken cancellationToken = default)
        => _clipboardInterop.Read(cancellationToken);

    public ValueTask Write(IEnumerable<ClipboardItemDto> items, CancellationToken cancellationToken = default)
        => _clipboardInterop.Write(items, cancellationToken);

    public ValueTask Clear(CancellationToken cancellationToken = default)
        => _clipboardInterop.Clear(cancellationToken);
}
