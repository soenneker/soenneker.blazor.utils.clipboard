using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Blazor.Utils.Clipboard.Dtos;
using Soenneker.Blazor.Utils.Clipboard.Enums;

namespace Soenneker.Blazor.Utils.Clipboard.Abstract;

/// <summary>
/// Blazor interop for the browser Clipboard API (read/write text and arbitrary types, permission detection).
/// </summary>
public interface IClipboardInterop : IAsyncDisposable
{
    /// <summary>
    /// Returns true if the clipboard API is available (e.g. secure context). Use before calling read/write in non-browser or insecure contexts.
    /// </summary>
    ValueTask<bool> HasClipboard(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current clipboard-read permission state. Returns <see cref="ClipboardPermissionState.Unsupported"/> in browsers that do not support the permission query (e.g. Firefox, Safari).
    /// </summary>
    ValueTask<ClipboardPermissionState> GetReadPermissionState(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current clipboard-write permission state. Returns <see cref="ClipboardPermissionState.Unsupported"/> in browsers that do not support the permission query.
    /// </summary>
    ValueTask<ClipboardPermissionState> GetWritePermissionState(CancellationToken cancellationToken = default);

    /// <summary>
    /// Reads plain text from the clipboard. Requires user permission in the browser for read.
    /// </summary>
    ValueTask<string> ReadText(CancellationToken cancellationToken = default);

    /// <summary>
    /// Writes plain text to the clipboard.
    /// </summary>
    ValueTask WriteText(string? text, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reads all clipboard items with all available MIME types (e.g. text/plain, text/html, image/png). Requires user permission for read.
    /// Binary content is returned as data URLs (e.g. data:image/png;base64,...).
    /// </summary>
    ValueTask<IReadOnlyList<ClipboardItemDto>> Read(CancellationToken cancellationToken = default);

    /// <summary>
    /// Writes one or more clipboard items. Each item can have multiple MIME types; use data URLs for binary (e.g. image/png).
    /// </summary>
    ValueTask Write(IEnumerable<ClipboardItemDto> items, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears the clipboard by writing empty content.
    /// </summary>
    ValueTask Clear(CancellationToken cancellationToken = default);
}