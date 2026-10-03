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
    /// <param name="plainText">The text to place on the clipboard.</param>
    /// <returns>The newly created clipboard Item Dto.</returns>
    public static ClipboardItemDto CreateText(string plainText)
    {
        return new ClipboardItemDto
        {
            Types = { ["text/plain"] = plainText ?? "" }
        };
    }

    /// <summary>
    /// Creates a single item with text/plain and optionally text/html (e.g. for pasting into rich editors).
    /// </summary>
    /// <param name="plainText">The plain-text representation used by destinations that do not accept HTML.</param>
    /// <param name="html">The HTML representation, or <see langword="null"/> to include only plain text.</param>
    /// <returns>The newly created clipboard Item Dto.</returns>
    public static ClipboardItemDto CreatePlainAndHtml(string plainText, string? html = null)
    {
        ClipboardItemDto item = CreateText(plainText);
        if (!string.IsNullOrEmpty(html))
            item.Types["text/html"] = html;
        return item;
    }

    /// <summary>
    /// Creates a single item for an image. Value must be a data URL (e.g. data:image/png;base64,...).
    /// </summary>
    /// <param name="dataUrl">Image as data URL.</param>
    /// <param name="mimeType">MIME type, e.g. image/png; defaults to image/png if not inferred from data URL.</param>
    /// <returns>The newly created clipboard Item Dto.</returns>
    public static ClipboardItemDto CreateImage(string dataUrl, string mimeType = "image/png")
    {
        return new ClipboardItemDto
        {
            Types = { [mimeType] = dataUrl ?? "" }
        };
    }
}
