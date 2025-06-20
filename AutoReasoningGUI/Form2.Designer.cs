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
            formulaLabel = new Label();
            formulaValidationLabel = new Label();
            queryResultValueLabel = new Label();
            fluentsPanel = new Panel();
            fluentListBox = new ListBox();
            actionsPanel = new Panel();
            actionListBox = new ListBox();
            actionProgramPanel = new Panel();
            actionProgramCheckedListBox = new CheckedListBox();
            actionProgramLabel = new Label();
            removeActionFromProgramButton = new Button();
            addActionToProgramButton = new Button();
            selectAllActionsFromProgramButton = new Button();
            statementsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)budgetNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)formulaErrorProvider).BeginInit();
            fluentsPanel.SuspendLayout();
            actionsPanel.SuspendLayout();
            actionProgramPanel.SuspendLayout();
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
            // fluentListLabel
            // 
            fluentListLabel.AutoSize = true;
            fluentListLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            fluentListLabel.Location = new Point(62, 9);
            fluentListLabel.Name = "fluentListLabel";
            fluentListLabel.Size = new Size(50, 15);
            fluentListLabel.TabIndex = 1;
            fluentListLabel.Text = "Fluents:";
            // 
            // actionListLabel
            // 
            actionListLabel.AutoSize = true;
            actionListLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            actionListLabel.Location = new Point(215, 9);
            actionListLabel.Name = "actionListLabel";
            actionListLabel.Size = new Size(51, 15);
            actionListLabel.TabIndex = 2;
            actionListLabel.Text = "Actions:";
            // 
            // statementsPanel
            // 
            statementsPanel.Controls.Add(statementListBox);
            statementsPanel.Location = new Point(15, 294);
            statementsPanel.Name = "statementsPanel";
            statementsPanel.Size = new Size(342, 126);
            statementsPanel.TabIndex = 3;
            // 
            // statementListBox
            // 
            statementListBox.Dock = DockStyle.Fill;
            statementListBox.FormattingEnabled = true;
            statementListBox.Location = new Point(0, 0);
            statementListBox.Name = "statementListBox";
            statementListBox.Size = new Size(342, 126);
            statementListBox.TabIndex = 0;
            // 
            // statementsLabel
            // 
            statementsLabel.AutoSize = true;
            statementsLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            statementsLabel.Location = new Point(19, 265);
            statementsLabel.Name = "statementsLabel";
            statementsLabel.Size = new Size(75, 15);
            statementsLabel.TabIndex = 9;
            statementsLabel.Text = "Statements:";
            // 
            // queryTypeComboBox
            // 
            queryTypeComboBox.FormattingEnabled = true;
            queryTypeComboBox.Location = new Point(645, 17);
            queryTypeComboBox.Name = "queryTypeComboBox";
            queryTypeComboBox.Size = new Size(119, 23);
            queryTypeComboBox.TabIndex = 10;
            // 
            // queryTypeLabel
            // 
            queryTypeLabel.AutoSize = true;
            queryTypeLabel.Location = new Point(525, 62);
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
            queryClassLabel.Location = new Point(556, 20);
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
            budgetNumericUpDown.Validating += budgetNumericUpDown_Validating;
            // 
            // budgetLabel
            // 
            budgetLabel.AutoSize = true;
            budgetLabel.Location = new Point(542, 100);
            budgetLabel.Name = "budgetLabel";
            budgetLabel.Size = new Size(89, 15);
            budgetLabel.TabIndex = 15;
            budgetLabel.Text = "Specify Budget:";
            // 
            // executeQueryButton
            // 
            executeQueryButton.Location = new Point(533, 385);
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
            queryResultLabel.Location = new Point(556, 426);
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
            createFormulaButton.Location = new Point(548, 270);
            createFormulaButton.Name = "createFormulaButton";
            createFormulaButton.Size = new Size(213, 23);
            createFormulaButton.TabIndex = 19;
            createFormulaButton.Text = "Validate Formula";
            createFormulaButton.UseVisualStyleBackColor = true;
            createFormulaButton.Click += createFormulaButton_Click;
            // 
            // formulaTextBox
            // 
            formulaTextBox.Location = new Point(548, 156);
            formulaTextBox.Multiline = true;
            formulaTextBox.Name = "formulaTextBox";
            formulaTextBox.Size = new Size(211, 111);
            formulaTextBox.TabIndex = 20;
            // 
            // formulaErrorProvider
            // 
            formulaErrorProvider.ContainerControl = this;
            // 
            // formulaLabel
            // 
            formulaLabel.AutoSize = true;
            formulaLabel.Location = new Point(545, 138);
            formulaLabel.Name = "formulaLabel";
            formulaLabel.Size = new Size(117, 15);
            formulaLabel.TabIndex = 21;
            formulaLabel.Text = "Type Formula below:";
            // 
            // formulaValidationLabel
            // 
            formulaValidationLabel.AutoSize = true;
            formulaValidationLabel.Location = new Point(634, 296);
            formulaValidationLabel.Name = "formulaValidationLabel";
            formulaValidationLabel.Size = new Size(38, 15);
            formulaValidationLabel.TabIndex = 22;
            formulaValidationLabel.Text = "VALID";
            formulaValidationLabel.Visible = false;
            // 
            // queryResultValueLabel
            // 
            queryResultValueLabel.AutoSize = true;
            queryResultValueLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 238);
            queryResultValueLabel.Location = new Point(604, 426);
            queryResultValueLabel.MaximumSize = new Size(150, 50);
            queryResultValueLabel.Name = "queryResultValueLabel";
            queryResultValueLabel.Size = new Size(0, 15);
            queryResultValueLabel.TabIndex = 23;
            // 
            // fluentsPanel
            // 
            fluentsPanel.Controls.Add(fluentListBox);
            fluentsPanel.Location = new Point(19, 27);
            fluentsPanel.Name = "fluentsPanel";
            fluentsPanel.Size = new Size(138, 222);
            fluentsPanel.TabIndex = 24;
            // 
            // fluentListBox
            // 
            fluentListBox.Dock = DockStyle.Fill;
            fluentListBox.FormattingEnabled = true;
            fluentListBox.Location = new Point(0, 0);
            fluentListBox.Name = "fluentListBox";
            fluentListBox.Size = new Size(138, 222);
            fluentListBox.TabIndex = 0;
            // 
            // actionsPanel
            // 
            actionsPanel.Controls.Add(actionListBox);
            actionsPanel.Location = new Point(174, 27);
            actionsPanel.Name = "actionsPanel";
            actionsPanel.Size = new Size(138, 209);
            actionsPanel.TabIndex = 25;
            // 
            // actionListBox
            // 
            actionListBox.Dock = DockStyle.Fill;
            actionListBox.FormattingEnabled = true;
            actionListBox.Location = new Point(0, 0);
            actionListBox.Name = "actionListBox";
            actionListBox.Size = new Size(138, 209);
            actionListBox.TabIndex = 0;
            actionListBox.KeyDown += actionListBox_KeyDown;
            actionListBox.MouseDown += actionListBox_MouseDown;
            // 
            // actionProgramPanel
            // 
            actionProgramPanel.Controls.Add(actionProgramCheckedListBox);
            actionProgramPanel.Location = new Point(329, 27);
            actionProgramPanel.Name = "actionProgramPanel";
            actionProgramPanel.Size = new Size(157, 209);
            actionProgramPanel.TabIndex = 25;
            // 
            // actionProgramCheckedListBox
            // 
            actionProgramCheckedListBox.AllowDrop = true;
            actionProgramCheckedListBox.Dock = DockStyle.Fill;
            actionProgramCheckedListBox.FormattingEnabled = true;
            actionProgramCheckedListBox.Location = new Point(0, 0);
            actionProgramCheckedListBox.Name = "actionProgramCheckedListBox";
            actionProgramCheckedListBox.Size = new Size(157, 209);
            actionProgramCheckedListBox.TabIndex = 0;
            actionProgramCheckedListBox.DragDrop += actionProgramCheckedListBox_DragDrop;
            actionProgramCheckedListBox.DragEnter += actionProgramCheckedListBox_DragEnter;
            actionProgramCheckedListBox.KeyDown += actionProgramCheckedListBox_KeyDown;
            // 
            // actionProgramLabel
            // 
            actionProgramLabel.AutoSize = true;
            actionProgramLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            actionProgramLabel.Location = new Point(361, 9);
            actionProgramLabel.Name = "actionProgramLabel";
            actionProgramLabel.Size = new Size(97, 15);
            actionProgramLabel.TabIndex = 26;
            actionProgramLabel.Text = "Action Program:";
            // 
            // removeActionFromProgramButton
            // 
            removeActionFromProgramButton.Location = new Point(329, 265);
            removeActionFromProgramButton.Name = "removeActionFromProgramButton";
            removeActionFromProgramButton.Size = new Size(157, 23);
            removeActionFromProgramButton.TabIndex = 28;
            removeActionFromProgramButton.Text = "Remove selected action(s)";
            removeActionFromProgramButton.UseVisualStyleBackColor = true;
            removeActionFromProgramButton.Click += removeActionFromProgramButton_Click;
            // 
            // addActionToProgramButton
            // 
            addActionToProgramButton.Location = new Point(174, 239);
            addActionToProgramButton.Name = "addActionToProgramButton";
            addActionToProgramButton.Size = new Size(138, 23);
            addActionToProgramButton.TabIndex = 29;
            addActionToProgramButton.Text = "Add selected action";
            addActionToProgramButton.UseVisualStyleBackColor = true;
            addActionToProgramButton.Click += addActionToProgramButton_Click;
            // 
            // selectAllActionsFromProgramButton
            // 
            selectAllActionsFromProgramButton.Location = new Point(329, 239);
            selectAllActionsFromProgramButton.Name = "selectAllActionsFromProgramButton";
            selectAllActionsFromProgramButton.Size = new Size(157, 23);
            selectAllActionsFromProgramButton.TabIndex = 30;
            selectAllActionsFromProgramButton.Text = "Select all actions";
            selectAllActionsFromProgramButton.UseVisualStyleBackColor = true;
            selectAllActionsFromProgramButton.Click += selectAllActionsFromProgramButton_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 459);
            Controls.Add(selectAllActionsFromProgramButton);
            Controls.Add(addActionToProgramButton);
            Controls.Add(removeActionFromProgramButton);
            Controls.Add(actionProgramLabel);
            Controls.Add(actionProgramPanel);
            Controls.Add(actionsPanel);
            Controls.Add(fluentsPanel);
            Controls.Add(queryResultValueLabel);
            Controls.Add(formulaValidationLabel);
            Controls.Add(formulaLabel);
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
            Controls.Add(prevPage);
            MaximumSize = new Size(800, 498);
            MinimumSize = new Size(800, 498);
            Name = "Form2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Reasoning";
            FormClosing += Form2_FormClosing;
            statementsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)budgetNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)formulaErrorProvider).EndInit();
            fluentsPanel.ResumeLayout(false);
            actionsPanel.ResumeLayout(false);
            actionProgramPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button prevPage;
        private Label fluentListLabel;
        private Label actionListLabel;
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
        private Label formulaLabel;
        private Label formulaValidationLabel;
        private Label queryResultValueLabel;
        private Panel fluentsPanel;
        private Label actionProgramLabel;
        private Panel actionProgramPanel;
        private Panel actionsPanel;
        private ListBox fluentListBox;
        private CheckedListBox actionProgramCheckedListBox;
        private Button removeActionFromProgramButton;
        private ListBox actionListBox;
        private Button addActionToProgramButton;
        private Button selectAllActionsFromProgramButton;
    }
}
