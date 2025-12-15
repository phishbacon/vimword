using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using vimword.AddIn;
using vimword.Vimulator;
using vimword.Vimulator.Modes;
using Office = Microsoft.Office.Core;
using UserControl = vimword.Src.VimStatusDisplay.UserControl;

namespace vimword
{
    /// <summary>
    /// Main entry point for the VSTO Word Add-in.
    /// Handles lifecycle, dependency injection setup, and UI initialization.
    /// </summary>
    public partial class ThisAddIn
    {
        private ServiceProvider _services;
        private KeyboardListener _keyboardListener;
        private IVimMachine _vimMachine;
        private UserControl _vimStatusDisplay;
        public CustomTaskPane _vimStatusPane;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            _vimStatusDisplay = new UserControl();
            _vimStatusPane = this.CustomTaskPanes.Add(_vimStatusDisplay, "vimword");
            _vimStatusPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionBottom;
            _vimStatusPane.Height = 60;
            _vimStatusPane.VisibleChanged += new EventHandler(VimStatusPane_VisibleChanged);
            _vimStatusPane.Visible = true;

            // Setup dependency injection container
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<Microsoft.Office.Interop.Word.Application>(Globals.ThisAddIn.Application);
            
            // Register all Vim modes - injected as IEnumerable<IVimMode>
            services.AddSingleton<IVimMode, InsertMode>();
            services.AddSingleton<IVimMode, NormalMode>();
            services.AddSingleton<IVimMode, VisualMode>();
            
            services.AddSingleton<IVimMachine, VimMachine>();
            services.AddSingleton<KeyboardListener>();

            _services = services.BuildServiceProvider();

            _vimMachine = _services.GetRequiredService<IVimMachine>();
            _keyboardListener = _services.GetRequiredService<KeyboardListener>();
            _keyboardListener.Install();

            _vimMachine.PropertyChanged += VimMachine_PropertyChanged;
            
            // Update cursor position when user clicks or moves cursor in Word
            this.Application.WindowSelectionChange += Application_WindowSelectionChange;
        }
        
        private void Application_WindowSelectionChange(Selection sel)
        {
            // Update cursor position in status bar when selection changes
            _vimMachine.UpdateCursorPosition();
        }

        private void VimMachine_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IVimMachine.CurrentMode))
            {
                _vimStatusDisplay.vimModeText.Text = Constants.ModeText[(int)_vimMachine.CurrentMode];
            }
            else if (e.PropertyName == nameof(IVimMachine.KeyBuffer))
            {
                // Update the keys label with the current key buffer (e.g., "5w")
                _vimStatusDisplay.keys.Text = _vimMachine.KeyBuffer;
            }
            else if (e.PropertyName == nameof(IVimMachine.CurrentLine))
            {
                _vimStatusDisplay.lineLabel.Text = _vimMachine.CurrentLine.ToString();
            }
            else if (e.PropertyName == nameof(IVimMachine.CurrentColumn))
            {
                _vimStatusDisplay.colLabel.Text = _vimMachine.CurrentColumn.ToString();
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            _keyboardListener.Uninstall();
        }

        private void VimStatusPane_VisibleChanged(object sender, System.EventArgs e)
        {
            if (_vimStatusPane.Visible)
            {
                Globals.Ribbons.RibbonManager.toggleButton.Label = "Hide Vim Display";
            }
            else
            {
                Globals.Ribbons.RibbonManager.toggleButton.Label = "Show Vim Display";
            }

            Globals.Ribbons.RibbonManager.toggleButton.Checked = _vimStatusPane.Visible;
        }

        #region VSTO generated code

        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
