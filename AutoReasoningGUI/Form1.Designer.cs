namespace AutoReasoningGUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            nextPage = new Button();
            contFluentsActions = new SplitContainer();
            fluentLabel = new Label();
            addFluentButton = new Button();
            fluentTextBox = new TextBox();
            costActionLabel = new Label();
            numericUpDownAction = new NumericUpDown();
            actionLabel = new Label();
            addActionButton = new Button();
            actionTextBox = new TextBox();
            ((System.ComponentModel.ISupportInitialize)contFluentsActions).BeginInit();
            contFluentsActions.Panel1.SuspendLayout();
            contFluentsActions.Panel2.SuspendLayout();
            contFluentsActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownAction).BeginInit();
            SuspendLayout();
            // 
            // nextPage
            // 
            nextPage.Location = new Point(697, 426);
            nextPage.Name = "nextPage";
            nextPage.Size = new Size(75, 23);
            nextPage.TabIndex = 0;
            nextPage.Text = "Next Page";
            nextPage.UseVisualStyleBackColor = true;
            nextPage.Click += nextPage_Click;
            // 
            // contFluentsActions
            // 
            contFluentsActions.BorderStyle = BorderStyle.FixedSingle;
            contFluentsActions.Location = new Point(550, 12);
            contFluentsActions.Name = "contFluentsActions";
            contFluentsActions.Orientation = Orientation.Horizontal;
            // 
            // contFluentsActions.Panel1
            // 
            contFluentsActions.Panel1.Controls.Add(fluentLabel);
            contFluentsActions.Panel1.Controls.Add(addFluentButton);
            contFluentsActions.Panel1.Controls.Add(fluentTextBox);
            // 
            // contFluentsActions.Panel2
            // 
            contFluentsActions.Panel2.Controls.Add(costActionLabel);
            contFluentsActions.Panel2.Controls.Add(numericUpDownAction);
            contFluentsActions.Panel2.Controls.Add(actionLabel);
            contFluentsActions.Panel2.Controls.Add(addActionButton);
            contFluentsActions.Panel2.Controls.Add(actionTextBox);
            contFluentsActions.Size = new Size(222, 209);
            contFluentsActions.SplitterDistance = 76;
            contFluentsActions.TabIndex = 1;
            contFluentsActions.SplitterMoved += contFluentsActions_SplitterMoved;
            // 
            // fluentLabel
            // 
            fluentLabel.AutoSize = true;
            fluentLabel.Location = new Point(76, 16);
            fluentLabel.Name = "fluentLabel";
            fluentLabel.Size = new Size(68, 15);
            fluentLabel.TabIndex = 2;
            fluentLabel.Text = "Add fluents";
            // 
            // addFluentButton
            // 
            addFluentButton.Location = new Point(141, 34);
            addFluentButton.Name = "addFluentButton";
            addFluentButton.Size = new Size(75, 23);
            addFluentButton.TabIndex = 1;
            addFluentButton.Text = "Add";
            addFluentButton.UseVisualStyleBackColor = true;
            addFluentButton.Click += addFluentButton_Click;
            // 
            // fluentTextBox
            // 
            fluentTextBox.Location = new Point(3, 35);
            fluentTextBox.Name = "fluentTextBox";
            fluentTextBox.Size = new Size(132, 23);
            fluentTextBox.TabIndex = 0;
            // 
            // costActionLabel
            // 
            costActionLabel.AutoSize = true;
            costActionLabel.Location = new Point(54, 67);
            costActionLabel.Name = "costActionLabel";
            costActionLabel.Size = new Size(31, 15);
            costActionLabel.TabIndex = 6;
            costActionLabel.Text = "Cost";
            // 
            // numericUpDownAction
            // 
            numericUpDownAction.Location = new Point(3, 85);
            numericUpDownAction.Name = "numericUpDownAction";
            numericUpDownAction.Size = new Size(132, 23);
            numericUpDownAction.TabIndex = 5;
            numericUpDownAction.ValueChanged += numericUpDownAction_ValueChanged;
            // 
            // actionLabel
            // 
            actionLabel.AutoSize = true;
            actionLabel.Location = new Point(76, 8);
            actionLabel.Name = "actionLabel";
            actionLabel.Size = new Size(72, 15);
            actionLabel.TabIndex = 4;
            actionLabel.Text = "Add Actions";
            // 
            // addActionButton
            // 
            addActionButton.Location = new Point(142, 63);
            addActionButton.Name = "addActionButton";
            addActionButton.Size = new Size(75, 23);
            addActionButton.TabIndex = 3;
            addActionButton.Text = "Add";
            addActionButton.UseVisualStyleBackColor = true;
            addActionButton.Click += addActionButton_Click;
            // 
            // actionTextBox
            // 
            actionTextBox.Location = new Point(3, 41);
            actionTextBox.Name = "actionTextBox";
            actionTextBox.Size = new Size(132, 23);
            actionTextBox.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 461);
            Controls.Add(contFluentsActions);
            Controls.Add(nextPage);
            MaximumSize = new Size(800, 500);
            MinimumSize = new Size(800, 500);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Reasoning";
            contFluentsActions.Panel1.ResumeLayout(false);
            contFluentsActions.Panel1.PerformLayout();
            contFluentsActions.Panel2.ResumeLayout(false);
            contFluentsActions.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)contFluentsActions).EndInit();
            contFluentsActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numericUpDownAction).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button nextPage;
        private SplitContainer contFluentsActions;
        private TextBox fluentTextBox;
        private TextBox actionTextBox;
        private Button addFluentButton;
        private Button addActionButton;
        private Label fluentLabel;
        private Label actionLabel;
        private NumericUpDown numericUpDownAction;
        private Label costActionLabel;
    }
}
