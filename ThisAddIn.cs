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

            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IVimMode, InsertMode>();
            services.AddSingleton<IVimMode, NormalMode>();
            services.AddSingleton<IVimMode, VisualMode>();
            services.AddSingleton<IVimMachine, VimMachine>();
            services.AddSingleton<KeyboardListener>();

            _services = services.BuildServiceProvider();

            _vimMachine = _services.GetRequiredService<IVimMachine>();
            _keyboardListener = _services.GetRequiredService<KeyboardListener>();
            _keyboardListener.Install();

            _vimMachine.ModeChanged += VimMachine_ModeChanged;
        }

        private void VimMachine_ModeChanged(object sender, PropertyChangedEventArgs e)
        {
            _vimStatusDisplay.vimModeText.Text = Constants.ModeText[(int)_vimMachine.CurrentMode];
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
