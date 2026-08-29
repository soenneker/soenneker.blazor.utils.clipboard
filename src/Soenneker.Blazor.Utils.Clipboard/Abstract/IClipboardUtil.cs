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
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>true if the browser Clipboard API is available; otherwise, false.</returns>
    ValueTask<bool> HasClipboard(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current clipboard-read permission state.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested clipboard Permission State.</returns>
    ValueTask<ClipboardPermissionState> GetReadPermissionState(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current clipboard-write permission state.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested clipboard Permission State.</returns>
    ValueTask<ClipboardPermissionState> GetWritePermissionState(CancellationToken cancellationToken = default);

    /// <summary>
    /// Reads plain text from the clipboard.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the text returned by read Text.</returns>
    ValueTask<string> ReadText(CancellationToken cancellationToken = default);

    /// <summary>
    /// Attempts to read plain text from the clipboard. Returns (true, text) on success or (false, null) on permission denied or error.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested (bool Success, string Text).</returns>
    ValueTask<(bool Success, string? Text)> TryReadText(CancellationToken cancellationToken = default);

    /// <summary>
    /// Writes plain text to the clipboard.
    /// </summary>
    /// <param name="text">Text to read, write, or transform.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the write text operation is complete.</returns>
    ValueTask WriteText(string? text, CancellationToken cancellationToken = default);

    /// <summary>
    /// Attempts to write plain text to the clipboard. Returns true on success, false on permission denied or error.
    /// </summary>
    /// <param name="text">Text to read, write, or transform.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>true if the requested update was applied; otherwise, false.</returns>
    ValueTask<bool> TryWriteText(string? text, CancellationToken cancellationToken = default);

    /// <summary>
    /// Copies the given text to the clipboard (alias for <see cref="WriteText"/>).
    /// </summary>
    /// <param name="text">Text to read, write, or transform.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the copy text operation is complete.</returns>
    ValueTask CopyText(string? text, CancellationToken cancellationToken = default);

    /// <summary>
    /// Writes plain and optional HTML to the clipboard as a single item (e.g. for pasting into rich editors).
    /// </summary>
    /// <param name="plainText">Plain text to encrypt, hash, or compare.</param>
    /// <param name="html">Rendered page HTML to inspect.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the copy plain and html operation is complete.</returns>
    ValueTask CopyPlainAndHtml(string plainText, string? html = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reads all clipboard items with all available MIME types (e.g. text/plain, text/html, image/png).
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the collection returned by read.</returns>
    ValueTask<IReadOnlyList<ClipboardItemDto>> Read(CancellationToken cancellationToken = default);

    /// <summary>
    /// Writes one or more clipboard items with multiple MIME types (e.g. text/plain + text/html, or image/png as data URL).
    /// </summary>
    /// <param name="items">items to inspect or update.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the write operation is complete.</returns>
    ValueTask Write(IEnumerable<ClipboardItemDto> items, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears the clipboard by writing empty content.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the Clipboard has been cleared.</returns>
    ValueTask Clear(CancellationToken cancellationToken = default);
}
