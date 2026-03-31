[![](https://img.shields.io/nuget/v/soenneker.blazor.utils.clipboard.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.utils.clipboard/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.utils.clipboard/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.utils.clipboard/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.blazor.utils.clipboard.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.utils.clipboard/)
[![](https://img.shields.io/badge/Demo-Live-blueviolet?style=for-the-badge&logo=github)](https://soenneker.github.io/soenneker.blazor.utils.clipboard)

# Soenneker.Blazor.Utils.Clipboard

Blazor library for reading and writing the clipboard in the browser. Uses the Clipboard API (HTTPS or localhost). Supports plain text, HTML, images, permission checks, and no-throw helpers.

---

## Install

```bash
dotnet add package Soenneker.Blazor.Utils.Clipboard
```

---

## Setup

Register clipboard services (e.g. in `Program.cs`):

```csharp
builder.Services.AddClipboardAsScoped();
```

Inject `IClipboardUtil` where you need it:

```csharp
@inject IClipboardUtil Clipboard
```

---

## Usage

### Check if clipboard is available

Use this when you might not be in a secure context (e.g. prerender, some hosts). Result is cached.

```csharp
if (await Clipboard.HasClipboard())
{
    await Clipboard.CopyText("Hello");
}
```

### Read and write plain text

```csharp
// Write
await Clipboard.WriteText("Hello, world");
await Clipboard.CopyText("Same as WriteText");  // alias

// Read (may throw if permission denied)
string text = await Clipboard.ReadText();
```

### Try read / try write (no throw)

Use these to avoid try/catch when you only care about success or failure:

```csharp
// Try read: (success, text)
var (ok, text) = await Clipboard.TryReadText();
if (ok && !string.IsNullOrEmpty(text))
    Console.WriteLine(text);

// Try write: true if copied, false on error
bool copied = await Clipboard.TryWriteText("Hello");
if (!copied)
    ShowMessage("Could not copy to clipboard");
```

### Permission state

Check read/write permission state (where supported; returns `Unsupported` in e.g. Firefox/Safari):

```csharp
var readState = await Clipboard.GetReadPermissionState();   // Granted, Denied, Prompt, Unsupported
var writeState = await Clipboard.GetWritePermissionState();
```

### Clear clipboard

```csharp
await Clipboard.Clear();
```

### Rich content: plain + HTML

Useful when pasting into rich editors (plain fallback + HTML):

```csharp
await Clipboard.CopyPlainAndHtml("Fallback text", "<p>Rich <b>HTML</b></p>");
```

### Read/write multiple types (text, HTML, images)

For full control, use `Read()` and `Write()` with `ClipboardItemDto`. Each item has a dictionary of MIME type → content (text or data URL for images).

```csharp
// Read all items and types from clipboard
IReadOnlyList<ClipboardItemDto> items = await Clipboard.Read();
foreach (var item in items)
{
    foreach (var (mimeType, content) in item.Types)
    {
        if (content.StartsWith("data:image/"))
            // image as data URL
        else
            // text (e.g. text/plain, text/html)
    }
}

// Write custom items
await Clipboard.Write(new[]
{
    ClipboardItems.CreatePlainAndHtml("Plain", "<p>HTML</p>"),
    ClipboardItems.CreateImage("data:image/png;base64,...")
});
```