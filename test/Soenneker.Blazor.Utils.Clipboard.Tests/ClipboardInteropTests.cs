using Soenneker.Blazor.Utils.Clipboard.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Blazor.Utils.Clipboard.Tests;

[Collection("Collection")]
public sealed class ClipboardInteropTests : FixturedUnitTest
{
    private readonly IClipboardInterop _clipboardInterop;

    public ClipboardInteropTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _clipboardInterop = Resolve<IClipboardInterop>(true);
    }

    [Fact]
    public void Resolves_IClipboardInterop()
    {
        Assert.NotNull(_clipboardInterop);
    }

    [Fact]
    public void Resolves_IClipboardUtil()
    {
        var util = Resolve<IClipboardUtil>(true);
        Assert.NotNull(util);
    }
}
