namespace AutoReasoningGUI
{
    partial class Form2
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
            prevPage = new Button();
            SuspendLayout();
            // 
            // prevPage
            // 
            prevPage.Location = new Point(12, 426);
            prevPage.Name = "prevPage";
            prevPage.Size = new Size(110, 23);
            prevPage.TabIndex = 0;
            prevPage.Text = "Previous Page";
            prevPage.UseVisualStyleBackColor = true;
            prevPage.Click += prevPage_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 461);
            Controls.Add(prevPage);
            MaximumSize = new Size(800, 500);
            MinimumSize = new Size(800, 500);
            Name = "Form2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Reasoning";
            FormClosing += Form2_FormClosing;
            ResumeLayout(false);
        }

        #endregion

        private Button prevPage;
    }
}