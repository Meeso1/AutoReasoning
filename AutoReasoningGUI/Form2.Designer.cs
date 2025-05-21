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
            components = new System.ComponentModel.Container();
            prevPage = new Button();
            fluentActionListContainer = new SplitContainer();
            fluentListBox = new ListBox();
            actionListBox = new ListBox();
            fluentListLabel = new Label();
            actionListLabel = new Label();
            statementsPanel = new Panel();
            statementListBox = new ListBox();
            statementsLabel = new Label();
            queryTypeComboBox = new ComboBox();
            queryTypeLabel = new Label();
            queryClassComboBox = new ComboBox();
            queryClassLabel = new Label();
            budgetNumericUpDown = new NumericUpDown();
            budgetLabel = new Label();
            executeQueryButton = new Button();
            queryResultLabel = new Label();
            writeQueryResultLabel = new Label();
            createFormulaButton = new Button();
            formulaTextBox = new TextBox();
            formulaErrorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)fluentActionListContainer).BeginInit();
            fluentActionListContainer.Panel1.SuspendLayout();
            fluentActionListContainer.Panel2.SuspendLayout();
            fluentActionListContainer.SuspendLayout();
            statementsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)budgetNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)formulaErrorProvider).BeginInit();
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
            // fluentActionListContainer
            // 
            fluentActionListContainer.IsSplitterFixed = true;
            fluentActionListContainer.Location = new Point(12, 30);
            fluentActionListContainer.Name = "fluentActionListContainer";
            // 
            // fluentActionListContainer.Panel1
            // 
            fluentActionListContainer.Panel1.Controls.Add(fluentListBox);
            // 
            // fluentActionListContainer.Panel2
            // 
            fluentActionListContainer.Panel2.Controls.Add(actionListBox);
            fluentActionListContainer.Size = new Size(346, 218);
            fluentActionListContainer.SplitterDistance = 170;
            fluentActionListContainer.TabIndex = 1;
            // 
            // fluentListBox
            // 
            fluentListBox.Dock = DockStyle.Fill;
            fluentListBox.FormattingEnabled = true;
            fluentListBox.Location = new Point(0, 0);
            fluentListBox.Name = "fluentListBox";
            fluentListBox.Size = new Size(170, 218);
            fluentListBox.TabIndex = 0;
            // 
            // actionListBox
            // 
            actionListBox.Dock = DockStyle.Fill;
            actionListBox.FormattingEnabled = true;
            actionListBox.Location = new Point(0, 0);
            actionListBox.Name = "actionListBox";
            actionListBox.Size = new Size(172, 218);
            actionListBox.TabIndex = 0;
            // 
            // fluentListLabel
            // 
            fluentListLabel.AutoSize = true;
            fluentListLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            fluentListLabel.Location = new Point(72, 9);
            fluentListLabel.Name = "fluentListLabel";
            fluentListLabel.Size = new Size(50, 15);
            fluentListLabel.TabIndex = 1;
            fluentListLabel.Text = "Fluents:";
            // 
            // actionListLabel
            // 
            actionListLabel.AutoSize = true;
            actionListLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            actionListLabel.Location = new Point(243, 9);
            actionListLabel.Name = "actionListLabel";
            actionListLabel.Size = new Size(51, 15);
            actionListLabel.TabIndex = 2;
            actionListLabel.Text = "Actions:";
            // 
            // statementsPanel
            // 
            statementsPanel.Controls.Add(statementListBox);
            statementsPanel.Location = new Point(15, 270);
            statementsPanel.Name = "statementsPanel";
            statementsPanel.Size = new Size(342, 150);
            statementsPanel.TabIndex = 3;
            // 
            // statementListBox
            // 
            statementListBox.Dock = DockStyle.Fill;
            statementListBox.FormattingEnabled = true;
            statementListBox.Location = new Point(0, 0);
            statementListBox.Name = "statementListBox";
            statementListBox.Size = new Size(342, 150);
            statementListBox.TabIndex = 0;
            // 
            // statementsLabel
            // 
            statementsLabel.AutoSize = true;
            statementsLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            statementsLabel.Location = new Point(146, 252);
            statementsLabel.Name = "statementsLabel";
            statementsLabel.Size = new Size(75, 15);
            statementsLabel.TabIndex = 9;
            statementsLabel.Text = "Statements:";
            // 
            // queryTypeComboBox
            // 
            queryTypeComboBox.FormattingEnabled = true;
            queryTypeComboBox.Location = new Point(643, 17);
            queryTypeComboBox.Name = "queryTypeComboBox";
            queryTypeComboBox.Size = new Size(121, 23);
            queryTypeComboBox.TabIndex = 10;
            // 
            // queryTypeLabel
            // 
            queryTypeLabel.AutoSize = true;
            queryTypeLabel.Location = new Point(533, 20);
            queryTypeLabel.Name = "queryTypeLabel";
            queryTypeLabel.Size = new Size(104, 15);
            queryTypeLabel.TabIndex = 11;
            queryTypeLabel.Text = "Select Query Type:";
            // 
            // queryClassComboBox
            // 
            queryClassComboBox.FormattingEnabled = true;
            queryClassComboBox.Location = new Point(644, 59);
            queryClassComboBox.Name = "queryClassComboBox";
            queryClassComboBox.Size = new Size(121, 23);
            queryClassComboBox.TabIndex = 12;
            // 
            // queryClassLabel
            // 
            queryClassLabel.AutoSize = true;
            queryClassLabel.Location = new Point(561, 62);
            queryClassLabel.Name = "queryClassLabel";
            queryClassLabel.Size = new Size(76, 15);
            queryClassLabel.TabIndex = 13;
            queryClassLabel.Text = "Select Query:";
            // 
            // budgetNumericUpDown
            // 
            budgetNumericUpDown.Location = new Point(645, 98);
            budgetNumericUpDown.Name = "budgetNumericUpDown";
            budgetNumericUpDown.Size = new Size(120, 23);
            budgetNumericUpDown.TabIndex = 14;
            budgetNumericUpDown.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // budgetLabel
            // 
            budgetLabel.AutoSize = true;
            budgetLabel.Location = new Point(545, 100);
            budgetLabel.Name = "budgetLabel";
            budgetLabel.Size = new Size(89, 15);
            budgetLabel.TabIndex = 15;
            budgetLabel.Text = "Specify Budget:";
            // 
            // executeQueryButton
            // 
            executeQueryButton.Location = new Point(533, 397);
            executeQueryButton.Name = "executeQueryButton";
            executeQueryButton.Size = new Size(239, 23);
            executeQueryButton.TabIndex = 16;
            executeQueryButton.Text = "Execute Query";
            executeQueryButton.UseVisualStyleBackColor = true;
            executeQueryButton.Click += executeQueryButton_Click;
            // 
            // queryResultLabel
            // 
            queryResultLabel.AutoSize = true;
            queryResultLabel.Location = new Point(548, 430);
            queryResultLabel.Name = "queryResultLabel";
            queryResultLabel.Size = new Size(42, 15);
            queryResultLabel.TabIndex = 17;
            queryResultLabel.Text = "Result:";
            // 
            // writeQueryResultLabel
            // 
            writeQueryResultLabel.AutoSize = true;
            writeQueryResultLabel.Location = new Point(634, 427);
            writeQueryResultLabel.Name = "writeQueryResultLabel";
            writeQueryResultLabel.Size = new Size(0, 15);
            writeQueryResultLabel.TabIndex = 18;
            // 
            // createFormulaButton
            // 
            createFormulaButton.Location = new Point(548, 244);
            createFormulaButton.Name = "createFormulaButton";
            createFormulaButton.Size = new Size(213, 23);
            createFormulaButton.TabIndex = 19;
            createFormulaButton.Text = "Create Formula";
            createFormulaButton.UseVisualStyleBackColor = true;
            createFormulaButton.Click += createFormulaButton_Click;
            // 
            // formulaTextBox
            // 
            formulaTextBox.Location = new Point(548, 127);
            formulaTextBox.Multiline = true;
            formulaTextBox.Name = "formulaTextBox";
            formulaTextBox.Size = new Size(211, 111);
            formulaTextBox.TabIndex = 20;
            // 
            // formulaErrorProvider
            // 
            formulaErrorProvider.ContainerControl = this;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 461);
            Controls.Add(formulaTextBox);
            Controls.Add(createFormulaButton);
            Controls.Add(writeQueryResultLabel);
            Controls.Add(queryResultLabel);
            Controls.Add(executeQueryButton);
            Controls.Add(budgetLabel);
            Controls.Add(budgetNumericUpDown);
            Controls.Add(queryClassLabel);
            Controls.Add(queryClassComboBox);
            Controls.Add(queryTypeLabel);
            Controls.Add(queryTypeComboBox);
            Controls.Add(statementsLabel);
            Controls.Add(statementsPanel);
            Controls.Add(actionListLabel);
            Controls.Add(fluentListLabel);
            Controls.Add(fluentActionListContainer);
            Controls.Add(prevPage);
            MaximumSize = new Size(800, 500);
            MinimumSize = new Size(800, 500);
            Name = "Form2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Reasoning";
            FormClosing += Form2_FormClosing;
            fluentActionListContainer.Panel1.ResumeLayout(false);
            fluentActionListContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)fluentActionListContainer).EndInit();
            fluentActionListContainer.ResumeLayout(false);
            statementsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)budgetNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)formulaErrorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button prevPage;
        private SplitContainer fluentActionListContainer;
        private Label fluentListLabel;
        private Label actionListLabel;
        private ListBox fluentListBox;
        private ListBox actionListBox;
        private Panel statementsPanel;
        private ListBox statementListBox;
        private Label statementsLabel;
        private ComboBox queryTypeComboBox;
        private Label queryTypeLabel;
        private ComboBox queryClassComboBox;
        private Label queryClassLabel;
        private NumericUpDown budgetNumericUpDown;
        private Label budgetLabel;
        private Button executeQueryButton;
        private Label queryResultLabel;
        private Label writeQueryResultLabel;
        private Button createFormulaButton;
        private TextBox formulaTextBox;
        private ErrorProvider formulaErrorProvider;
    }
}