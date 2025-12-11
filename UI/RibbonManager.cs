using Microsoft.Office.Tools.Ribbon;

namespace vimword
{
    public partial class RibbonManager
    {
        private void ManageVimStatusDisplayRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void toggleButton_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn._vimStatusPane.Visible = ((RibbonToggleButton)sender).Checked;
        }
    }
}
