using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vimword.Vimulator.Modes;

namespace vimword.Vimulator
{
    /// <summary>
    /// The Vim state machine that manages modes and delegates key handling.
    /// </summary>
    public interface IVimMachine : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the current Vim mode. Setting fires PropertyChanged event.
        /// </summary>
        Constants.Modes CurrentMode { get; set; }

        /// <summary>
        /// Handles a key press by routing to the current mode.
        /// Returns true if handled, false to pass to Word.
        /// </summary>
        bool HandleKey(Keys key);
        
        /// <summary>
        /// Gets or sets the current key buffer to display in UI (e.g., "5w").
        /// </summary>
        string KeyBuffer { get; set; }
        
        /// <summary>
        /// Gets the current line number (1-based).
        /// </summary>
        int CurrentLine { get; }
        
        /// <summary>
        /// Gets the current column number (1-based).
        /// </summary>
        int CurrentColumn { get; }
        
        /// <summary>
        /// Updates the cursor position from the Word application.
        /// Fires PropertyChanged for CurrentLine and CurrentColumn.
        /// </summary>
        void UpdateCursorPosition();
    }
}
