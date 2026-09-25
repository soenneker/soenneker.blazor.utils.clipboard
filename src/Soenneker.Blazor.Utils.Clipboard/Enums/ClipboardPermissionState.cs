using Soenneker.Gen.EnumValues;

namespace Soenneker.Blazor.Utils.Clipboard.Enums;

/// <summary>
/// Result of querying clipboard read or write permission.
/// </summary>
[EnumValue]
public sealed partial class ClipboardPermissionState
{
    /// <summary>Permission granted.</summary>
    public static readonly ClipboardPermissionState Granted = new(0);

    /// <summary>Permission denied.</summary>
    public static readonly ClipboardPermissionState Denied = new(1);

    /// <summary>Permission not yet requested; user may be prompted.</summary>
    public static readonly ClipboardPermissionState Prompt = new(2);

    /// <summary>Permission query not supported (e.g. in Firefox/Safari).</summary>
    public static readonly ClipboardPermissionState Unsupported = new(3);
}
