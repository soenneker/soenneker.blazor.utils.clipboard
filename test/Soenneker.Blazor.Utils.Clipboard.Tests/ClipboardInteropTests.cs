using Soenneker.Blazor.Utils.Clipboard.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Blazor.Utils.Clipboard.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class ClipboardInteropTests : HostedUnitTest
{
    private readonly IClipboardInterop _clipboardInterop;

    public ClipboardInteropTests(Host host) : base(host)
    {
        _clipboardInterop = Resolve<IClipboardInterop>(true);
    }

    [Test]
    public void Resolves_IClipboardInterop()
    {
        Assert.NotNull(_clipboardInterop);
    }

    [Test]
    public void Resolves_IClipboardUtil()
    {
        var util = Resolve<IClipboardUtil>(true);
        Assert.NotNull(util);
    }
}
