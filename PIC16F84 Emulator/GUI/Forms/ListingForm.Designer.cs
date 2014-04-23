namespace PIC16F84_Emulator
{
    partial class ListingForm
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
            this.listingBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listingBox
            // 
            this.listingBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listingBox.FormattingEnabled = true;
            this.listingBox.Location = new System.Drawing.Point(0, 0);
            this.listingBox.Name = "listingBox";
            this.listingBox.Size = new System.Drawing.Size(284, 262);
            this.listingBox.TabIndex = 0;
            // 
            // ListingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.listingBox);
            this.Name = "ListingForm";
            this.Text = "ListingForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listingBox;
    }
}