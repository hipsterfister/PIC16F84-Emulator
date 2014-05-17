namespace PIC16F84_Emulator.GUI.Forms
{
    partial class PulseGeneratorForm
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
            this.togglePortAButton = new System.Windows.Forms.Button();
            this.portACaptionLabel = new System.Windows.Forms.Label();
            this.portBCaptionLabel = new System.Windows.Forms.Label();
            this.togglePortBButton = new System.Windows.Forms.Button();
            this.intervalABox = new System.Windows.Forms.TextBox();
            this.intervalBBox = new System.Windows.Forms.TextBox();
            this.seperatorLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // togglePortAButton
            // 
            this.togglePortAButton.Location = new System.Drawing.Point(175, 23);
            this.togglePortAButton.Name = "togglePortAButton";
            this.togglePortAButton.Size = new System.Drawing.Size(75, 23);
            this.togglePortAButton.TabIndex = 0;
            this.togglePortAButton.Text = "Activate";
            this.togglePortAButton.UseVisualStyleBackColor = true;
            this.togglePortAButton.Click += new System.EventHandler(this.togglePortAButton_Click);
            // 
            // portACaptionLabel
            // 
            this.portACaptionLabel.AutoSize = true;
            this.portACaptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portACaptionLabel.Location = new System.Drawing.Point(5, 8);
            this.portACaptionLabel.Name = "portACaptionLabel";
            this.portACaptionLabel.Size = new System.Drawing.Size(77, 13);
            this.portACaptionLabel.TabIndex = 1;
            this.portACaptionLabel.Text = "Port A Pulse";
            // 
            // portBCaptionLabel
            // 
            this.portBCaptionLabel.AutoSize = true;
            this.portBCaptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portBCaptionLabel.Location = new System.Drawing.Point(5, 96);
            this.portBCaptionLabel.Name = "portBCaptionLabel";
            this.portBCaptionLabel.Size = new System.Drawing.Size(77, 13);
            this.portBCaptionLabel.TabIndex = 2;
            this.portBCaptionLabel.Text = "Port B Pulse";
            // 
            // togglePortBButton
            // 
            this.togglePortBButton.Location = new System.Drawing.Point(175, 113);
            this.togglePortBButton.Name = "togglePortBButton";
            this.togglePortBButton.Size = new System.Drawing.Size(75, 23);
            this.togglePortBButton.TabIndex = 3;
            this.togglePortBButton.Text = "Activate";
            this.togglePortBButton.UseVisualStyleBackColor = true;
            this.togglePortBButton.Click += new System.EventHandler(this.togglePortBButton_Click);
            // 
            // intervalABox
            // 
            this.intervalABox.Location = new System.Drawing.Point(69, 25);
            this.intervalABox.Name = "intervalABox";
            this.intervalABox.Size = new System.Drawing.Size(100, 20);
            this.intervalABox.TabIndex = 4;
            this.intervalABox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.intervalABox_KeyDown);
            // 
            // intervalBBox
            // 
            this.intervalBBox.Location = new System.Drawing.Point(69, 115);
            this.intervalBBox.Name = "intervalBBox";
            this.intervalBBox.Size = new System.Drawing.Size(100, 20);
            this.intervalBBox.TabIndex = 5;
            this.intervalBBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.intervalBBox_KeyDown);
            // 
            // seperatorLabel
            // 
            this.seperatorLabel.AutoSize = true;
            this.seperatorLabel.BackColor = System.Drawing.Color.Transparent;
            this.seperatorLabel.Location = new System.Drawing.Point(0, 78);
            this.seperatorLabel.MaximumSize = new System.Drawing.Size(260, 15);
            this.seperatorLabel.Name = "seperatorLabel";
            this.seperatorLabel.Size = new System.Drawing.Size(259, 15);
            this.seperatorLabel.TabIndex = 6;
            this.seperatorLabel.Text = "________________________________________________";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Interval [ms]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Interval [ms]";
            // 
            // PulseGeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 180);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.seperatorLabel);
            this.Controls.Add(this.intervalBBox);
            this.Controls.Add(this.intervalABox);
            this.Controls.Add(this.togglePortBButton);
            this.Controls.Add(this.portBCaptionLabel);
            this.Controls.Add(this.portACaptionLabel);
            this.Controls.Add(this.togglePortAButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PulseGeneratorForm";
            this.Text = "PulseGeneratorForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button togglePortAButton;
        private System.Windows.Forms.Label portACaptionLabel;
        private System.Windows.Forms.Label portBCaptionLabel;
        private System.Windows.Forms.Button togglePortBButton;
        private System.Windows.Forms.TextBox intervalABox;
        private System.Windows.Forms.TextBox intervalBBox;
        private System.Windows.Forms.Label seperatorLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}