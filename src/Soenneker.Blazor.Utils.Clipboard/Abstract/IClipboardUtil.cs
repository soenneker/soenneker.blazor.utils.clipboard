using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Blazor.Utils.Clipboard.Dtos;
using Soenneker.Blazor.Utils.Clipboard.Enums;

namespace Soenneker.Blazor.Utils.Clipboard.Abstract;

/// <summary>
/// High-level clipboard utility for Blazor applications. Wraps <see cref="IClipboardInterop"/> for read/write and permission detection.
/// </summary>
public interface IClipboardUtil
{
    /// <summary>
    /// Returns true if the clipboard API is available (e.g. secure context).
    /// </summary>
    ValueTask<bool> HasClipboard(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current clipboard-read permission state.
    /// </summary>
    ValueTask<ClipboardPermissionState> GetReadPermissionState(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current clipboard-write permission state.
    /// </summary>
    ValueTask<ClipboardPermissionState> GetWritePermissionState(CancellationToken cancellationToken = default);

    /// <summary>
    /// Reads plain text from the clipboard.
    /// </summary>
    ValueTask<string> ReadText(CancellationToken cancellationToken = default);

    /// <summary>
    /// Attempts to read plain text from the clipboard. Returns (true, text) on success or (false, null) on permission denied or error.
    /// </summary>
    ValueTask<(bool Success, string? Text)> TryReadText(CancellationToken cancellationToken = default);

    /// <summary>
    /// Writes plain text to the clipboard.
    /// </summary>
    ValueTask WriteText(string? text, CancellationToken cancellationToken = default);

    /// <summary>
    /// Attempts to write plain text to the clipboard. Returns true on success, false on permission denied or error.
    /// </summary>
    ValueTask<bool> TryWriteText(string? text, CancellationToken cancellationToken = default);

    /// <summary>
    /// Copies the given text to the clipboard (alias for <see cref="WriteText"/>).
    /// </summary>
    ValueTask CopyText(string? text, CancellationToken cancellationToken = default);

    /// <summary>
    /// Writes plain and optional HTML to the clipboard as a single item (e.g. for pasting into rich editors).
    /// </summary>
    ValueTask CopyPlainAndHtml(string plainText, string? html = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reads all clipboard items with all available MIME types (e.g. text/plain, text/html, image/png).
    /// </summary>
    ValueTask<IReadOnlyList<ClipboardItemDto>> Read(CancellationToken cancellationToken = default);

    /// <summary>
    /// Writes one or more clipboard items with multiple MIME types (e.g. text/plain + text/html, or image/png as data URL).
    /// </summary>
    ValueTask Write(IEnumerable<ClipboardItemDto> items, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears the clipboard by writing empty content.
    /// </summary>
    ValueTask Clear(CancellationToken cancellationToken = default);
}
