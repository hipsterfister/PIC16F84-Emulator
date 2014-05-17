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
            this.executedCyclesLabel = new System.Windows.Forms.Label();
            this.simulatedTimeLabel = new System.Windows.Forms.Label();
            this.simulatedTimeValueLabel = new System.Windows.Forms.Label();
            this.executedCyclesValueLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // wRegisterLabel
            // 
            this.wRegisterLabel.AutoSize = true;
            this.wRegisterLabel.Location = new System.Drawing.Point(50, 63);
            this.wRegisterLabel.Name = "wRegisterLabel";
            this.wRegisterLabel.Size = new System.Drawing.Size(60, 13);
            this.wRegisterLabel.TabIndex = 0;
            this.wRegisterLabel.Text = "W-Register";
            // 
            // programCounterLabel
            // 
            this.programCounterLabel.AutoSize = true;
            this.programCounterLabel.Location = new System.Drawing.Point(50, 86);
            this.programCounterLabel.Name = "programCounterLabel";
            this.programCounterLabel.Size = new System.Drawing.Size(86, 13);
            this.programCounterLabel.TabIndex = 1;
            this.programCounterLabel.Text = "Program Counter";
            // 
            // timer0Label
            // 
            this.timer0Label.AutoSize = true;
            this.timer0Label.Location = new System.Drawing.Point(50, 109);
            this.timer0Label.Name = "timer0Label";
            this.timer0Label.Size = new System.Drawing.Size(42, 13);
            this.timer0Label.TabIndex = 2;
            this.timer0Label.Text = "Timer 0";
            // 
            // stackLabel
            // 
            this.stackLabel.AutoSize = true;
            this.stackLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stackLabel.Location = new System.Drawing.Point(207, 86);
            this.stackLabel.Name = "stackLabel";
            this.stackLabel.Size = new System.Drawing.Size(47, 13);
            this.stackLabel.TabIndex = 3;
            this.stackLabel.Text = "STACK";
            // 
            // stackIndex0Label
            // 
            this.stackIndex0Label.AutoSize = true;
            this.stackIndex0Label.Location = new System.Drawing.Point(207, 16);
            this.stackIndex0Label.Name = "stackIndex0Label";
            this.stackIndex0Label.Size = new System.Drawing.Size(42, 13);
            this.stackIndex0Label.TabIndex = 4;
            this.stackIndex0Label.Text = "0 (ToS)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(207, 156);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "7";
            // 
            // executedCyclesLabel
            // 
            this.executedCyclesLabel.AutoSize = true;
            this.executedCyclesLabel.Location = new System.Drawing.Point(50, 145);
            this.executedCyclesLabel.Name = "executedCyclesLabel";
            this.executedCyclesLabel.Size = new System.Drawing.Size(86, 13);
            this.executedCyclesLabel.TabIndex = 12;
            this.executedCyclesLabel.Text = "Executed Cycles";
            // 
            // simulatedTimeLabel
            // 
            this.simulatedTimeLabel.AutoSize = true;
            this.simulatedTimeLabel.Location = new System.Drawing.Point(50, 167);
            this.simulatedTimeLabel.Name = "simulatedTimeLabel";
            this.simulatedTimeLabel.Size = new System.Drawing.Size(79, 13);
            this.simulatedTimeLabel.TabIndex = 13;
            this.simulatedTimeLabel.Text = "Simulated Time";
            // 
            // simulatedTimeValueLabel
            // 
            this.simulatedTimeValueLabel.AutoSize = true;
            this.simulatedTimeValueLabel.Location = new System.Drawing.Point(5, 167);
            this.simulatedTimeValueLabel.Name = "simulatedTimeValueLabel";
            this.simulatedTimeValueLabel.Size = new System.Drawing.Size(0, 13);
            this.simulatedTimeValueLabel.TabIndex = 15;
            // 
            // executedCyclesValueLabel
            // 
            this.executedCyclesValueLabel.AutoSize = true;
            this.executedCyclesValueLabel.Location = new System.Drawing.Point(5, 145);
            this.executedCyclesValueLabel.Name = "executedCyclesValueLabel";
            this.executedCyclesValueLabel.Size = new System.Drawing.Size(0, 13);
            this.executedCyclesValueLabel.TabIndex = 14;
            // 
            // SpecialValueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 187);
            this.Controls.Add(this.simulatedTimeValueLabel);
            this.Controls.Add(this.executedCyclesValueLabel);
            this.Controls.Add(this.simulatedTimeLabel);
            this.Controls.Add(this.executedCyclesLabel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.stackIndex0Label);
            this.Controls.Add(this.stackLabel);
            this.Controls.Add(this.timer0Label);
            this.Controls.Add(this.programCounterLabel);
            this.Controls.Add(this.wRegisterLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
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
        private System.Windows.Forms.Label executedCyclesLabel;
        private System.Windows.Forms.Label simulatedTimeLabel;
        private System.Windows.Forms.Label simulatedTimeValueLabel;
        private System.Windows.Forms.Label executedCyclesValueLabel;
    }
}