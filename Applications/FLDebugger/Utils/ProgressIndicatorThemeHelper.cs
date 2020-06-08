using Byt3.WindowsForms.CustomControls;
using FLDebugger.Forms;

namespace FLDebugger.Utils
{
    public static class ProgressIndicatorThemeHelper
    {

        public static void ApplyTheme(ProgressIndicator indicator)
        {
            FLScriptEditor.RegisterDefaultTheme(indicator.gbSubTask);
            FLScriptEditor.RegisterDefaultTheme(indicator.lblStatus);
            FLScriptEditor.RegisterDefaultTheme(indicator.mainProgressPanel);
            FLScriptEditor.RegisterDefaultTheme(indicator.panelMain);
            FLScriptEditor.RegisterDefaultTheme(indicator.pbProgress);
            FLScriptEditor.RegisterDefaultTheme(indicator.subTaskPanel);
        }
    }
}