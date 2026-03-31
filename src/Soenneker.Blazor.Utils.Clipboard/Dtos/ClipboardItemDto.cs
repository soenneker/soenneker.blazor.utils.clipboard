using System.Collections.Generic;

namespace Soenneker.Blazor.Utils.Clipboard.Dtos;

/// <summary>
/// One clipboard item with one or more MIME types. Keys are MIME types (e.g. "text/plain", "text/html", "image/png");
/// values are either plain text or a data URL for binary (e.g. "data:image/png;base64,...").
/// </summary>
public sealed class ClipboardItemDto
{
    /// <summary>
    /// MIME type to content. For text types use the raw string; for images use a data URL (data:image/png;base64,...).
    /// </summary>
    public Dictionary<string, string> Types { get; set; } = new();
}
