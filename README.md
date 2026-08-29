[![](https://img.shields.io/nuget/v/soenneker.blazor.utils.clipboard.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.utils.clipboard/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.utils.clipboard/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.utils.clipboard/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.blazor.utils.clipboard.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.utils.clipboard/)
[![](https://img.shields.io/badge/Demo-Live-blueviolet?style=for-the-badge&logo=github)](https://soenneker.github.io/soenneker.blazor.utils.clipboard)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.utils.clipboard/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.utils.clipboard/actions/workflows/codeql.yml)

# Soenneker.Blazor.Utils.Clipboard

A scoped Blazor service for the browser Clipboard API, with plain-text helpers, rich clipboard items, permission queries, and failure-tolerant text operations.

## Installation

```bash
dotnet add package Soenneker.Blazor.Utils.Clipboard
```

```csharp
using Soenneker.Blazor.Utils.Clipboard.Registrars;

builder.Services.AddClipboardAsScoped();
```

Inject `IClipboardUtil` into a component or service:

```razor
@using Soenneker.Blazor.Utils.Clipboard.Abstract
@inject IClipboardUtil Clipboard

<button @onclick="Copy">Copy</button>

@code {
    private async Task Copy()
    {
        bool copied = await Clipboard.TryWriteText("Hello from Blazor");
        // Show feedback when copied is false.
    }
}
```

Clipboard access is a browser operation. Invoke it after interactive rendering and, for the broadest browser support, directly from a user action such as a click. The API normally requires HTTPS; localhost is treated as a secure context by browsers.

## Plain text

`WriteText` and `ReadText` expose browser failures such as denied permission or unavailable clipboard access. `TryWriteText` and `TryReadText` convert those failures into a result while still propagating cancellation.

```csharp
await Clipboard.WriteText("Order 1234");
string text = await Clipboard.ReadText();

var (read, optionalText) = await Clipboard.TryReadText();
if (read)
{
    // optionalText can be empty, but is non-null after a successful read.
}
```

`CopyText` is an alias for `WriteText`. Passing `null` writes an empty string. `Clear` does the same; browsers do not provide a separate clipboard-clear operation.

## Availability and permissions

```csharp
if (!await Clipboard.HasClipboard())
    return;

ClipboardPermissionState readState = await Clipboard.GetReadPermissionState();
ClipboardPermissionState writeState = await Clipboard.GetWritePermissionState();
```

`HasClipboard` checks for `navigator.clipboard` and caches the result for the scoped service. Permission queries can return `Unsupported`; that does not prove the operation will fail. Browser support and user-activation rules differ, so the actual read or write remains authoritative.

## Plain text and HTML

Provide both representations when copied content should retain formatting in rich editors and still paste sensibly elsewhere:

```csharp
await Clipboard.CopyPlainAndHtml(
    "Invoice total: $42.00",
    "<p>Invoice total: <strong>$42.00</strong></p>");
```

The library does not sanitize HTML. Only write HTML you trust, and treat HTML returned by `Read()` as untrusted input. Sanitize it before rendering it into a page.

## Multiple MIME types and images

`Read()` returns every representation exposed by each clipboard item. Text and `application/json` values are returned as strings; other values, including images, are returned as data URLs.

```csharp
IReadOnlyList<ClipboardItemDto> items = await Clipboard.Read();

await Clipboard.Write([
    ClipboardItems.CreatePlainAndHtml("Plain fallback", "<strong>Formatted</strong>"),
    ClipboardItems.CreateImage(pngDataUrl, "image/png")
]);
```

For custom content, construct a `ClipboardItemDto` whose `Types` dictionary maps MIME types to content. Use raw strings for text and data URLs for binary values. The destination browser decides which MIME types it accepts and may reject an entire write.

Clipboard contents can include passwords, tokens, personal data, and images. Avoid logging read values, retain them only as long as needed, and do not send them elsewhere without explicit user intent.
