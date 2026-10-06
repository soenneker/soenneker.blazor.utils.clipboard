using System.Text.Json;
using Soenneker.Blazor.Utils.Clipboard;

var items = JsonSerializer.Deserialize("""[{"types":{"text/plain":"copied text","text/html":"<b>copied text</b>"}}]""", InteropJsonContext.Default.ListClipboardItemDto)!;
Check(items.Count == 1 && items[0].Types["text/plain"] == "copied text", "clipboard item collection");
var json = JsonSerializer.SerializeToElement(items, InteropJsonContext.Default.IEnumerableClipboardItemDto);
Check(json[0].GetProperty("types").GetProperty("text/html").GetString() == "<b>copied text</b>", "clipboard write shape");

var module = new SmokeModule("""[{"types":{"text/plain":"copied text"}}]""");
await using var interop = new ClipboardInterop(new SmokeModuleImport(module));
Check((await interop.Read())[0].Types["text/plain"] == "copied text", "read boundary");
await interop.Write(items);
Check(module.Arguments is [JsonElement], "write crosses JS as generated JSON");

Console.WriteLine("Trimmed JSON smoke checks passed.");

static void Check(bool condition, string name)
{
    if (!condition) throw new InvalidOperationException(name);
}
