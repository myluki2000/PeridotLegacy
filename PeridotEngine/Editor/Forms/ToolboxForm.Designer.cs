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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpSolids = new System.Windows.Forms.TabPage();
            this.tpEntities = new System.Windows.Forms.TabPage();
            this.lvSolids = new System.Windows.Forms.ListView();
            this.tabControl1.SuspendLayout();
            this.tpSolids.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpSolids);
            this.tabControl1.Controls.Add(this.tpEntities);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(321, 682);
            this.tabControl1.TabIndex = 0;
            // 
            // tpSolids
            // 
            this.tpSolids.Controls.Add(this.lvSolids);
            this.tpSolids.Location = new System.Drawing.Point(4, 22);
            this.tpSolids.Name = "tpSolids";
            this.tpSolids.Padding = new System.Windows.Forms.Padding(3);
            this.tpSolids.Size = new System.Drawing.Size(313, 656);
            this.tpSolids.TabIndex = 0;
            this.tpSolids.Text = "Solids";
            this.tpSolids.UseVisualStyleBackColor = true;
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
            // lvSolids
            // 
            this.lvSolids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSolids.Location = new System.Drawing.Point(3, 3);
            this.lvSolids.Name = "lvSolids";
            this.lvSolids.Size = new System.Drawing.Size(307, 650);
            this.lvSolids.TabIndex = 0;
            this.lvSolids.UseCompatibleStateImageBehavior = false;
            // 
            // ToolboxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 682);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ToolboxForm";
            this.Text = "ToolboxForm";
            this.tabControl1.ResumeLayout(false);
            this.tpSolids.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpSolids;
        private System.Windows.Forms.TabPage tpEntities;
        private System.Windows.Forms.ListView lvSolids;
    }
}