using Byt3.ExtPP.Base.settings;

namespace Byt3.ExtPP.Base
{
    /// <summary>
    /// AbstractLinePlugin but with fixed plugin type toggle
    /// </summary>
    public abstract class AbstractLineAfterPlugin : AbstractLinePlugin
    {
        /// <summary>
        /// Specifies the plugin type. Fullscript or Line Script
        /// </summary>
        public override PluginType PluginTypeToggle { get; } = PluginType.LINE_PLUGIN_AFTER;
    }
}