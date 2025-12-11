namespace vimword
{
    partial class RibbonManager : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public RibbonManager()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.ribbonGroup = this.Factory.CreateRibbonGroup();
            this.toggleButton = this.Factory.CreateRibbonToggleButton();
            this.tab1.SuspendLayout();
            this.ribbonGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.ribbonGroup);
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // ribbonGroup
            // 
            this.ribbonGroup.Items.Add(this.toggleButton);
            this.ribbonGroup.Label = "Vim Status Display Manager";
            this.ribbonGroup.Name = "ribbonGroup";
            // 
            // toggleButton
            // 
            this.toggleButton.Label = "Show Vim Display";
            this.toggleButton.Name = "toggleButton";
            this.toggleButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.toggleButton_Click);
            // 
            // RibbonManager
            // 
            this.Name = "RibbonManager";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.ManageVimStatusDisplayRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.ribbonGroup.ResumeLayout(false);
            this.ribbonGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup ribbonGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton toggleButton;
    }

    partial class ThisRibbonCollection
    {
        internal RibbonManager RibbonManager
        {
            get { return this.GetRibbon<RibbonManager>(); }
        }
    }
}
