namespace PIC16F84_Emulator.GUI.Forms
{
    partial class PICEmulatorForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PICEmulatorForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dateiÖffnenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dateiSchließenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ansichtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showControlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showRegisterMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iOControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.specialValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extrasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOM3AusgabeAktivierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.ansichtToolStripMenuItem,
            this.extrasToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1221, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiÖffnenToolStripMenuItem,
            this.dateiSchließenToolStripMenuItem});
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.dateiToolStripMenuItem.Text = "Datei";
            // 
            // dateiÖffnenToolStripMenuItem
            // 
            this.dateiÖffnenToolStripMenuItem.Name = "dateiÖffnenToolStripMenuItem";
            this.dateiÖffnenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.dateiÖffnenToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.dateiÖffnenToolStripMenuItem.Text = "Datei öffnen";
            this.dateiÖffnenToolStripMenuItem.Click += new System.EventHandler(this.showOpenFileDialog);
            // 
            // dateiSchließenToolStripMenuItem
            // 
            this.dateiSchließenToolStripMenuItem.Enabled = false;
            this.dateiSchließenToolStripMenuItem.Name = "dateiSchließenToolStripMenuItem";
            this.dateiSchließenToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.dateiSchließenToolStripMenuItem.Text = "Datei schließen";
            this.dateiSchließenToolStripMenuItem.Click += new System.EventHandler(this.dateiSchließenToolStripMenuItem_Click);
            // 
            // ansichtToolStripMenuItem
            // 
            this.ansichtToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showControlsToolStripMenuItem,
            this.showRegisterMapToolStripMenuItem,
            this.listingToolStripMenuItem,
            this.iOControlToolStripMenuItem,
            this.specialValuesToolStripMenuItem});
            this.ansichtToolStripMenuItem.Enabled = false;
            this.ansichtToolStripMenuItem.Name = "ansichtToolStripMenuItem";
            this.ansichtToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.ansichtToolStripMenuItem.Text = "Ansicht";
            // 
            // showControlsToolStripMenuItem
            // 
            this.showControlsToolStripMenuItem.Name = "showControlsToolStripMenuItem";
            this.showControlsToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.showControlsToolStripMenuItem.Text = "Controls";
            this.showControlsToolStripMenuItem.Click += new System.EventHandler(this.showControlsToolStripMenuItem_Click);
            // 
            // showRegisterMapToolStripMenuItem
            // 
            this.showRegisterMapToolStripMenuItem.Name = "showRegisterMapToolStripMenuItem";
            this.showRegisterMapToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.showRegisterMapToolStripMenuItem.Text = "Register Map";
            this.showRegisterMapToolStripMenuItem.Click += new System.EventHandler(this.showRegisterMapToolStripMenuItem_Click);
            // 
            // listingToolStripMenuItem
            // 
            this.listingToolStripMenuItem.Name = "listingToolStripMenuItem";
            this.listingToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.listingToolStripMenuItem.Text = "Listing";
            this.listingToolStripMenuItem.Click += new System.EventHandler(this.listingToolStripMenuItem_Click);
            // 
            // iOControlToolStripMenuItem
            // 
            this.iOControlToolStripMenuItem.Name = "iOControlToolStripMenuItem";
            this.iOControlToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.iOControlToolStripMenuItem.Text = "I/O Control";
            this.iOControlToolStripMenuItem.Click += new System.EventHandler(this.iOControlToolStripMenuItem_Click);
            // 
            // specialValuesToolStripMenuItem
            // 
            this.specialValuesToolStripMenuItem.Name = "specialValuesToolStripMenuItem";
            this.specialValuesToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.specialValuesToolStripMenuItem.Text = "Special Values";
            this.specialValuesToolStripMenuItem.Click += new System.EventHandler(this.specialValuesToolStripMenuItem_Click);
            // 
            // extrasToolStripMenuItem
            // 
            this.extrasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cOM3AusgabeAktivierenToolStripMenuItem});
            this.extrasToolStripMenuItem.Name = "extrasToolStripMenuItem";
            this.extrasToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.extrasToolStripMenuItem.Text = "Extras";
            // 
            // cOM3AusgabeAktivierenToolStripMenuItem
            // 
            this.cOM3AusgabeAktivierenToolStripMenuItem.Name = "cOM3AusgabeAktivierenToolStripMenuItem";
            this.cOM3AusgabeAktivierenToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.cOM3AusgabeAktivierenToolStripMenuItem.Text = "COM3 Ausgabe aktivieren";
            this.cOM3AusgabeAktivierenToolStripMenuItem.Click += new System.EventHandler(this.cOM3AusgabeAktivierenToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog";
            this.openFileDialog1.Filter = "Programme | *.LST";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // PICEmulatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PIC16F84_Emulator.Properties.Resources.background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1221, 601);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "PICEmulatorForm";
            this.Text = "PICEmulatorForm";
            this.ClientSizeChanged += new System.EventHandler(this.PICEmulatorForm_onClientSizeChanged);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dateiÖffnenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ansichtToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showControlsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showRegisterMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dateiSchließenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iOControlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem specialValuesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extrasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cOM3AusgabeAktivierenToolStripMenuItem;
    }
}