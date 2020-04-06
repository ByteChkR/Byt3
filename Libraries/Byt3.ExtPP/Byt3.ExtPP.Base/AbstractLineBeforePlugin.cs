using Byt3.ExtPP.Base.settings;

namespace Byt3.ExtPP.Base
{
    /// <summary>
    /// AbstractLinePlugin but with fixed plugin type toggle
    /// </summary>
    public abstract class AbstractLineBeforePlugin : AbstractLinePlugin
    {

        public override PluginType PluginTypeToggle { get; } = PluginType.LINE_PLUGIN_BEFORE;
    }
}