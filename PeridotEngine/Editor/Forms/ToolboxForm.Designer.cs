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
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpSolids = new System.Windows.Forms.TabPage();
            this.lvSolids = new System.Windows.Forms.ListView();
            this.tpEntities = new System.Windows.Forms.TabPage();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnCursor = new System.Windows.Forms.ToolStripButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpSolids.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(321, 24);
            this.panel1.TabIndex = 0;
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
            // 
            // tpEntities
            // 
            this.tpEntities.Location = new System.Drawing.Point(4, 22);
            this.tpEntities.Name = "tpEntities";
            this.tpEntities.Padding = new System.Windows.Forms.Padding(3);
            this.tpEntities.Size = new System.Drawing.Size(313, 656);
            this.tpEntities.TabIndex = 1;
            this.tpEntities.Text = "Entities";
            this.tpEntities.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
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
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpSolids.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnCursor;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpSolids;
        private System.Windows.Forms.ListView lvSolids;
        private System.Windows.Forms.TabPage tpEntities;
    }
}