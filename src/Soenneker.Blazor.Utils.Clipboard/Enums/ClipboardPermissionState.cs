namespace Soenneker.Blazor.Utils.Clipboard.Enums;

/// <summary>
/// Result of querying clipboard read or write permission.
/// </summary>
public enum ClipboardPermissionState
{
    /// <summary>Permission granted.</summary>
    Granted,

    /// <summary>Permission denied.</summary>
    Denied,

    /// <summary>Permission not yet requested; user may be prompted.</summary>
    Prompt,

    /// <summary>Permission query not supported (e.g. in Firefox/Safari).</summary>
    Unsupported
}
