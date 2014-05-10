namespace PIC16F84_Emulator.GUI.Forms
{
    partial class SpecialValueForm
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
            this.wRegisterLabel = new System.Windows.Forms.Label();
            this.programCounterLabel = new System.Windows.Forms.Label();
            this.timer0Label = new System.Windows.Forms.Label();
            this.stackLabel = new System.Windows.Forms.Label();
            this.stackIndex0Label = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // wRegisterLabel
            // 
            this.wRegisterLabel.AutoSize = true;
            this.wRegisterLabel.Location = new System.Drawing.Point(50, 13);
            this.wRegisterLabel.Name = "wRegisterLabel";
            this.wRegisterLabel.Size = new System.Drawing.Size(60, 13);
            this.wRegisterLabel.TabIndex = 0;
            this.wRegisterLabel.Text = "W-Register";
            // 
            // programCounterLabel
            // 
            this.programCounterLabel.AutoSize = true;
            this.programCounterLabel.Location = new System.Drawing.Point(50, 36);
            this.programCounterLabel.Name = "programCounterLabel";
            this.programCounterLabel.Size = new System.Drawing.Size(86, 13);
            this.programCounterLabel.TabIndex = 1;
            this.programCounterLabel.Text = "Program Counter";
            // 
            // timer0Label
            // 
            this.timer0Label.AutoSize = true;
            this.timer0Label.Location = new System.Drawing.Point(50, 59);
            this.timer0Label.Name = "timer0Label";
            this.timer0Label.Size = new System.Drawing.Size(42, 13);
            this.timer0Label.TabIndex = 2;
            this.timer0Label.Text = "Timer 0";
            // 
            // stackLabel
            // 
            this.stackLabel.AutoSize = true;
            this.stackLabel.Location = new System.Drawing.Point(50, 148);
            this.stackLabel.Name = "stackLabel";
            this.stackLabel.Size = new System.Drawing.Size(35, 13);
            this.stackLabel.TabIndex = 3;
            this.stackLabel.Text = "Stack";
            // 
            // stackIndex0Label
            // 
            this.stackIndex0Label.AutoSize = true;
            this.stackIndex0Label.Location = new System.Drawing.Point(50, 93);
            this.stackIndex0Label.Name = "stackIndex0Label";
            this.stackIndex0Label.Size = new System.Drawing.Size(42, 13);
            this.stackIndex0Label.TabIndex = 4;
            this.stackIndex0Label.Text = "0 (ToS)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(50, 208);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "7";
            // 
            // SpecialValueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 233);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.stackIndex0Label);
            this.Controls.Add(this.stackLabel);
            this.Controls.Add(this.timer0Label);
            this.Controls.Add(this.programCounterLabel);
            this.Controls.Add(this.wRegisterLabel);
            this.Name = "SpecialValueForm";
            this.Text = "SpecialValueForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label wRegisterLabel;
        private System.Windows.Forms.Label programCounterLabel;
        private System.Windows.Forms.Label timer0Label;
        private System.Windows.Forms.Label stackLabel;
        private System.Windows.Forms.Label stackIndex0Label;
        private System.Windows.Forms.Label label7;
    }
}