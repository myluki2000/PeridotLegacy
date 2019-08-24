using PeridotEngine.Editor.UI;

namespace PeridotEngine.Editor.Forms
{
    partial class ToolboxForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolboxForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.toolStrip1 = new PeridotEngine.Editor.UI.ToolStripEx();
            this.btnCursor = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpSolids = new System.Windows.Forms.TabPage();
            this.lvSolids = new System.Windows.Forms.ListView();
            this.tpEntities = new System.Windows.Forms.TabPage();
            this.lvEntities = new System.Windows.Forms.ListView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpSolids.SuspendLayout();
            this.tpEntities.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.nudWidth);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.nudHeight);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(321, 24);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(100, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Width";
            // 
            // nudWidth
            // 
            this.nudWidth.Location = new System.Drawing.Point(137, 2);
            this.nudWidth.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.Size = new System.Drawing.Size(67, 20);
            this.nudWidth.TabIndex = 7;
            this.nudWidth.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(211, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Height";
            // 
            // nudHeight
            // 
            this.nudHeight.Location = new System.Drawing.Point(251, 2);
            this.nudHeight.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.Size = new System.Drawing.Size(67, 20);
            this.nudHeight.TabIndex = 5;
            this.nudHeight.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // toolStrip1
            // 
            this.toolStrip1.ClickThrough = true;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCursor});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(321, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnCursor
            // 
            this.btnCursor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCursor.Image = ((System.Drawing.Image)(resources.GetObject("btnCursor.Image")));
            this.btnCursor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCursor.Name = "btnCursor";
            this.btnCursor.Size = new System.Drawing.Size(46, 22);
            this.btnCursor.Text = "Cursor";
            this.btnCursor.Click += new System.EventHandler(this.BtnCursor_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(321, 658);
            this.panel2.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpSolids);
            this.tabControl1.Controls.Add(this.tpEntities);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(321, 658);
            this.tabControl1.TabIndex = 3;
            // 
            // tpSolids
            // 
            this.tpSolids.Controls.Add(this.lvSolids);
            this.tpSolids.Location = new System.Drawing.Point(4, 22);
            this.tpSolids.Name = "tpSolids";
            this.tpSolids.Padding = new System.Windows.Forms.Padding(3);
            this.tpSolids.Size = new System.Drawing.Size(313, 632);
            this.tpSolids.TabIndex = 0;
            this.tpSolids.Text = "Solids";
            this.tpSolids.UseVisualStyleBackColor = true;
            // 
            // lvSolids
            // 
            this.lvSolids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSolids.Location = new System.Drawing.Point(3, 3);
            this.lvSolids.Name = "lvSolids";
            this.lvSolids.Size = new System.Drawing.Size(307, 626);
            this.lvSolids.TabIndex = 0;
            this.lvSolids.UseCompatibleStateImageBehavior = false;
            this.lvSolids.SelectedIndexChanged += new System.EventHandler(this.LvSolids_SelectedIndexChanged);
            // 
            // tpEntities
            // 
            this.tpEntities.Controls.Add(this.lvEntities);
            this.tpEntities.Location = new System.Drawing.Point(4, 22);
            this.tpEntities.Name = "tpEntities";
            this.tpEntities.Padding = new System.Windows.Forms.Padding(3);
            this.tpEntities.Size = new System.Drawing.Size(313, 632);
            this.tpEntities.TabIndex = 1;
            this.tpEntities.Text = "Entities";
            this.tpEntities.UseVisualStyleBackColor = true;
            // 
            // lvEntities
            // 
            this.lvEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvEntities.Location = new System.Drawing.Point(3, 3);
            this.lvEntities.Name = "lvEntities";
            this.lvEntities.Size = new System.Drawing.Size(307, 626);
            this.lvEntities.TabIndex = 0;
            this.lvEntities.UseCompatibleStateImageBehavior = false;
            // 
            // ToolboxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 682);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ToolboxForm";
            this.Text = "ToolboxForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpSolids.ResumeLayout(false);
            this.tpEntities.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpSolids;
        private System.Windows.Forms.ListView lvSolids;
        private System.Windows.Forms.TabPage tpEntities;
        private System.Windows.Forms.ListView lvEntities;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private PeridotEngine.Editor.UI.ToolStripEx toolStrip1;
        private System.Windows.Forms.ToolStripButton btnCursor;
    }
}