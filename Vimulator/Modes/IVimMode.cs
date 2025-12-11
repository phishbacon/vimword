using System.Windows.Forms;

namespace vimword.Vimulator.Modes
{
    public interface IVimMode
    {
        Constants.Modes Mode { get; }
        bool HandleKey(Keys key);
    }
}
