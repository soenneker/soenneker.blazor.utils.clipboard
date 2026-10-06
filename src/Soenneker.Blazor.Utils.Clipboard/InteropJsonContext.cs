using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Soenneker.Blazor.Utils.Clipboard.Dtos;

namespace Soenneker.Blazor.Utils.Clipboard;

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web)]
[JsonSerializable(typeof(List<ClipboardItemDto>))]
[JsonSerializable(typeof(IEnumerable<ClipboardItemDto>))]
internal partial class InteropJsonContext : JsonSerializerContext;
