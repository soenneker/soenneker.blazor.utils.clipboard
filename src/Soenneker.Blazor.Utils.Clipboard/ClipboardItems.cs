using System.Collections.Generic;
using Soenneker.Blazor.Utils.Clipboard.Dtos;

namespace Soenneker.Blazor.Utils.Clipboard;

/// <summary>
/// Factory methods for common <see cref="ClipboardItemDto"/> shapes.
/// </summary>
public static class ClipboardItems
{
    /// <summary>
    /// Creates a single item with text/plain only.
    /// </summary>
    public static ClipboardItemDto CreateText(string plainText)
    {
        return new ClipboardItemDto
        {
            Types = new Dictionary<string, string> { ["text/plain"] = plainText ?? "" }
        };
    }

    /// <summary>
    /// Creates a single item with text/plain and optionally text/html (e.g. for pasting into rich editors).
    /// </summary>
    /// <param name="plainText">Plain text fallback; required.</param>
    /// <param name="html">HTML content; if null, only text/plain is included.</param>
    public static ClipboardItemDto CreatePlainAndHtml(string plainText, string? html = null)
    {
        var types = new Dictionary<string, string> { ["text/plain"] = plainText ?? "" };
        if (!string.IsNullOrEmpty(html))
            types["text/html"] = html;
        return new ClipboardItemDto { Types = types };
    }

    /// <summary>
    /// Creates a single item for an image. Value must be a data URL (e.g. data:image/png;base64,...).
    /// </summary>
    /// <param name="dataUrl">Image as data URL.</param>
    /// <param name="mimeType">MIME type, e.g. image/png; defaults to image/png if not inferred from data URL.</param>
    public static ClipboardItemDto CreateImage(string dataUrl, string mimeType = "image/png")
    {
        return new ClipboardItemDto
        {
            Types = new Dictionary<string, string> { [mimeType] = dataUrl ?? "" }
        };
    }
}
