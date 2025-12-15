namespace vimword.Src.VimStatusDisplay
{
    partial class UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.statusTable = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.keys = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.vimModeTable = new System.Windows.Forms.TableLayoutPanel();
            this.vimModeText = new System.Windows.Forms.Label();
            this.vimModeLabelConst = new System.Windows.Forms.Label();
            this.lineLabelConst = new System.Windows.Forms.Label();
            this.lineLabel = new System.Windows.Forms.Label();
            this.colLabelConst = new System.Windows.Forms.Label();
            this.colLabel = new System.Windows.Forms.Label();
            this.statusTable.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.vimModeTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusTable
            // 
            this.statusTable.AutoSize = true;
            this.statusTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.statusTable.ColumnCount = 3;
            this.statusTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.statusTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.statusTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.statusTable.Controls.Add(this.tableLayoutPanel4, 2, 0);
            this.statusTable.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.statusTable.Controls.Add(this.vimModeTable, 0, 0);
            this.statusTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusTable.Location = new System.Drawing.Point(0, 0);
            this.statusTable.Name = "statusTable";
            this.statusTable.RowCount = 1;
            this.statusTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.statusTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.statusTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.statusTable.Size = new System.Drawing.Size(455, 28);
            this.statusTable.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tableLayoutPanel4.Controls.Add(this.keys, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(305, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(147, 22);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // keys
            // 
            this.keys.AutoSize = true;
            this.keys.Location = new System.Drawing.Point(3, 0);
            this.keys.Name = "keys";
            this.keys.Size = new System.Drawing.Size(0, 13);
            this.keys.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.colLabel, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.colLabelConst, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.lineLabel, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.lineLabelConst, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(154, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(145, 22);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // vimModeTable
            // 
            this.vimModeTable.AutoSize = true;
            this.vimModeTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.vimModeTable.ColumnCount = 2;
            this.vimModeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.vimModeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.vimModeTable.Controls.Add(this.vimModeText, 1, 0);
            this.vimModeTable.Controls.Add(this.vimModeLabelConst, 0, 0);
            this.vimModeTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vimModeTable.Location = new System.Drawing.Point(3, 3);
            this.vimModeTable.Name = "vimModeTable";
            this.vimModeTable.RowCount = 1;
            this.vimModeTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.vimModeTable.Size = new System.Drawing.Size(145, 22);
            this.vimModeTable.TabIndex = 0;
            // 
            // vimModeText
            // 
            this.vimModeText.AutoSize = true;
            this.vimModeText.Location = new System.Drawing.Point(75, 0);
            this.vimModeText.Name = "vimModeText";
            this.vimModeText.Size = new System.Drawing.Size(53, 13);
            this.vimModeText.TabIndex = 1;
            this.vimModeText.Text = "NORMAL";
            // 
            // vimModeLabelConst
            // 
            this.vimModeLabelConst.AutoSize = true;
            this.vimModeLabelConst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vimModeLabelConst.Location = new System.Drawing.Point(3, 0);
            this.vimModeLabelConst.Name = "vimModeLabelConst";
            this.vimModeLabelConst.Size = new System.Drawing.Size(62, 13);
            this.vimModeLabelConst.TabIndex = 0;
            this.vimModeLabelConst.Text = "VimMode:";
            this.vimModeLabelConst.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lineLabelConst
            // 
            this.lineLabelConst.AutoSize = true;
            this.lineLabelConst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lineLabelConst.Location = new System.Drawing.Point(3, 0);
            this.lineLabelConst.Name = "lineLabelConst";
            this.lineLabelConst.Size = new System.Drawing.Size(35, 13);
            this.lineLabelConst.TabIndex = 1;
            this.lineLabelConst.Text = "Line:";
            this.lineLabelConst.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lineLabel
            // 
            this.lineLabel.AutoSize = true;
            this.lineLabel.Location = new System.Drawing.Point(44, 0);
            this.lineLabel.Name = "lineLabel";
            this.lineLabel.Size = new System.Drawing.Size(13, 13);
            this.lineLabel.TabIndex = 2;
            this.lineLabel.Text = "1";
            // 
            // colLabelConst
            // 
            this.colLabelConst.AutoSize = true;
            this.colLabelConst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colLabelConst.Location = new System.Drawing.Point(63, 0);
            this.colLabelConst.Name = "colLabelConst";
            this.colLabelConst.Size = new System.Drawing.Size(29, 13);
            this.colLabelConst.TabIndex = 3;
            this.colLabelConst.Text = "Col:";
            this.colLabelConst.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // colLabel
            // 
            this.colLabel.AutoSize = true;
            this.colLabel.Location = new System.Drawing.Point(98, 0);
            this.colLabel.Name = "colLabel";
            this.colLabel.Size = new System.Drawing.Size(13, 13);
            this.colLabel.TabIndex = 4;
            this.colLabel.Text = "1";
            // 
            // UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusTable);
            this.Name = "UserControl";
            this.Size = new System.Drawing.Size(455, 28);
            this.Load += new System.EventHandler(this.VimStatusDisplay_Load);
            this.statusTable.ResumeLayout(false);
            this.statusTable.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.vimModeTable.ResumeLayout(false);
            this.vimModeTable.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel statusTable;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel vimModeTable;
        private System.Windows.Forms.Label vimModeLabelConst;
        public System.Windows.Forms.Label vimModeText;
        public System.Windows.Forms.Label keys;
        public System.Windows.Forms.Label lineLabel;
        private System.Windows.Forms.Label lineLabelConst;
        public System.Windows.Forms.Label colLabel;
        private System.Windows.Forms.Label colLabelConst;
    }
}
