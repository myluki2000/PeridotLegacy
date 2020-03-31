using System.Windows.Forms;

namespace PeridotEngine.Engine.Editor.UI
{
    /// <summary>
    /// This class adds on to the functionality provided in System.Windows.Forms.ToolStrip.
    /// Source: https://blogs.msdn.microsoft.com/rickbrew/2006/01/09/how-to-enable-click-through-for-net-2-0-toolstrip-and-menustrip/
    /// </summary>
    public class ToolStripEx : ToolStrip
    {
        private const int WM_MOUSEACTIVATE = 0x21;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEACTIVATE && this.CanFocus && !this.Focused)
                this.Focus();

            base.WndProc(ref m);
        }
    }
}