using Microsoft.Office.Interop.Word;
using System.Windows.Forms;

namespace vimword.Vimulator.Modes
{
    internal class NormalMode : IVimMode
    {
        public Constants.Modes Mode
        {
            get
            {
                return Constants.Modes.NORMAL;
            }
        }
        public bool HandleKey(Keys key)
        {
            switch (key)
            {
                case Keys.H:
                    Globals.ThisAddIn.Application.Selection.MoveLeft(WdUnits.wdCharacter, 1);
                    return true;
                case Keys.L:
                    Globals.ThisAddIn.Application.Selection.MoveRight(WdUnits.wdCharacter, 1);
                    return true;
                case Keys.K:
                    Globals.ThisAddIn.Application.Selection.MoveUp(WdUnits.wdLine, 1);
                    return true;
                case Keys.J:
                    Globals.ThisAddIn.Application.Selection.MoveDown(WdUnits.wdLine, 1);
                    return true;
                default:
                    return true;
            }
        }
    }
}
