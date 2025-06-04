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
            prevPage.Location = new Point(14, 568);
            prevPage.Margin = new Padding(3, 4, 3, 4);
            prevPage.Name = "prevPage";
            prevPage.Size = new Size(126, 31);
            prevPage.TabIndex = 0;
            prevPage.Text = "Previous Page";
            prevPage.UseVisualStyleBackColor = true;
            prevPage.Click += prevPage_Click;
            // 
            // fluentListLabel
            // 
            fluentListLabel.AutoSize = true;
            fluentListLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            fluentListLabel.Location = new Point(71, 12);
            fluentListLabel.Name = "fluentListLabel";
            fluentListLabel.Size = new Size(64, 20);
            fluentListLabel.TabIndex = 1;
            fluentListLabel.Text = "Fluents:";
            // 
            // actionListLabel
            // 
            actionListLabel.AutoSize = true;
            actionListLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            actionListLabel.Location = new Point(246, 12);
            actionListLabel.Name = "actionListLabel";
            actionListLabel.Size = new Size(66, 20);
            actionListLabel.TabIndex = 2;
            actionListLabel.Text = "Actions:";
            // 
            // statementsPanel
            // 
            statementsPanel.Controls.Add(statementListBox);
            statementsPanel.Location = new Point(17, 392);
            statementsPanel.Margin = new Padding(3, 4, 3, 4);
            statementsPanel.Name = "statementsPanel";
            statementsPanel.Size = new Size(391, 168);
            statementsPanel.TabIndex = 3;
            // 
            // statementListBox
            // 
            statementListBox.Dock = DockStyle.Fill;
            statementListBox.FormattingEnabled = true;
            statementListBox.Location = new Point(0, 0);
            statementListBox.Margin = new Padding(3, 4, 3, 4);
            statementListBox.Name = "statementListBox";
            statementListBox.Size = new Size(391, 168);
            statementListBox.TabIndex = 0;
            // 
            // statementsLabel
            // 
            statementsLabel.AutoSize = true;
            statementsLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            statementsLabel.Location = new Point(22, 353);
            statementsLabel.Name = "statementsLabel";
            statementsLabel.Size = new Size(93, 20);
            statementsLabel.TabIndex = 9;
            statementsLabel.Text = "Statements:";
            // 
            // queryTypeComboBox
            // 
            queryTypeComboBox.FormattingEnabled = true;
            queryTypeComboBox.Location = new Point(737, 23);
            queryTypeComboBox.Margin = new Padding(3, 4, 3, 4);
            queryTypeComboBox.Name = "queryTypeComboBox";
            queryTypeComboBox.Size = new Size(135, 28);
            queryTypeComboBox.TabIndex = 10;
            // 
            // queryTypeLabel
            // 
            queryTypeLabel.AutoSize = true;
            queryTypeLabel.Location = new Point(600, 82);
            queryTypeLabel.Name = "queryTypeLabel";
            queryTypeLabel.Size = new Size(130, 20);
            queryTypeLabel.TabIndex = 11;
            queryTypeLabel.Text = "Select Query Type:";
            // 
            // queryClassComboBox
            // 
            queryClassComboBox.FormattingEnabled = true;
            queryClassComboBox.Location = new Point(736, 79);
            queryClassComboBox.Margin = new Padding(3, 4, 3, 4);
            queryClassComboBox.Name = "queryClassComboBox";
            queryClassComboBox.Size = new Size(138, 28);
            queryClassComboBox.TabIndex = 12;
            // 
            // queryClassLabel
            // 
            queryClassLabel.AutoSize = true;
            queryClassLabel.Location = new Point(635, 26);
            queryClassLabel.Name = "queryClassLabel";
            queryClassLabel.Size = new Size(95, 20);
            queryClassLabel.TabIndex = 13;
            queryClassLabel.Text = "Select Query:";
            // 
            // budgetNumericUpDown
            // 
            budgetNumericUpDown.Location = new Point(737, 131);
            budgetNumericUpDown.Margin = new Padding(3, 4, 3, 4);
            budgetNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            budgetNumericUpDown.Name = "budgetNumericUpDown";
            budgetNumericUpDown.Size = new Size(137, 27);
            budgetNumericUpDown.TabIndex = 14;
            budgetNumericUpDown.Value = new decimal(new int[] { 10, 0, 0, 0 });
            budgetNumericUpDown.Validating += budgetNumericUpDown_Validating;
            // 
            // budgetLabel
            // 
            budgetLabel.AutoSize = true;
            budgetLabel.Location = new Point(619, 133);
            budgetLabel.Name = "budgetLabel";
            budgetLabel.Size = new Size(112, 20);
            budgetLabel.TabIndex = 15;
            budgetLabel.Text = "Specify Budget:";
            // 
            // executeQueryButton
            // 
            executeQueryButton.Location = new Point(609, 529);
            executeQueryButton.Margin = new Padding(3, 4, 3, 4);
            executeQueryButton.Name = "executeQueryButton";
            executeQueryButton.Size = new Size(273, 31);
            executeQueryButton.TabIndex = 16;
            executeQueryButton.Text = "Execute Query";
            executeQueryButton.UseVisualStyleBackColor = true;
            executeQueryButton.Click += executeQueryButton_Click;
            // 
            // queryResultLabel
            // 
            queryResultLabel.AutoSize = true;
            queryResultLabel.Location = new Point(641, 573);
            queryResultLabel.Name = "queryResultLabel";
            queryResultLabel.Size = new Size(52, 20);
            queryResultLabel.TabIndex = 17;
            queryResultLabel.Text = "Result:";
            // 
            // writeQueryResultLabel
            // 
            writeQueryResultLabel.AutoSize = true;
            writeQueryResultLabel.Location = new Point(725, 569);
            writeQueryResultLabel.Name = "writeQueryResultLabel";
            writeQueryResultLabel.Size = new Size(0, 20);
            writeQueryResultLabel.TabIndex = 18;
            // 
            // createFormulaButton
            // 
            createFormulaButton.Location = new Point(626, 360);
            createFormulaButton.Margin = new Padding(3, 4, 3, 4);
            createFormulaButton.Name = "createFormulaButton";
            createFormulaButton.Size = new Size(243, 31);
            createFormulaButton.TabIndex = 19;
            createFormulaButton.Text = "Validate Formula";
            createFormulaButton.UseVisualStyleBackColor = true;
            createFormulaButton.Click += createFormulaButton_Click;
            // 
            // formulaTextBox
            // 
            formulaTextBox.Location = new Point(626, 208);
            formulaTextBox.Margin = new Padding(3, 4, 3, 4);
            formulaTextBox.Multiline = true;
            formulaTextBox.Name = "formulaTextBox";
            formulaTextBox.Size = new Size(241, 147);
            formulaTextBox.TabIndex = 20;
            // 
            // formulaErrorProvider
            // 
            formulaErrorProvider.ContainerControl = this;
            // 
            // formulaLabel
            // 
            formulaLabel.AutoSize = true;
            formulaLabel.Location = new Point(623, 184);
            formulaLabel.Name = "formulaLabel";
            formulaLabel.Size = new Size(146, 20);
            formulaLabel.TabIndex = 21;
            formulaLabel.Text = "Type Formula below:";
            // 
            // formulaValidationLabel
            // 
            formulaValidationLabel.AutoSize = true;
            formulaValidationLabel.Location = new Point(725, 395);
            formulaValidationLabel.Name = "formulaValidationLabel";
            formulaValidationLabel.Size = new Size(49, 20);
            formulaValidationLabel.TabIndex = 22;
            formulaValidationLabel.Text = "VALID";
            formulaValidationLabel.Visible = false;
            // 
            // queryResultValueLabel
            // 
            queryResultValueLabel.AutoSize = true;
            queryResultValueLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 238);
            queryResultValueLabel.Location = new Point(713, 573);
            queryResultValueLabel.Name = "queryResultValueLabel";
            queryResultValueLabel.Size = new Size(0, 20);
            queryResultValueLabel.TabIndex = 23;
            // 
            // fluentsPanel
            // 
            fluentsPanel.Controls.Add(fluentListBox);
            fluentsPanel.Location = new Point(22, 36);
            fluentsPanel.Margin = new Padding(3, 4, 3, 4);
            fluentsPanel.Name = "fluentsPanel";
            fluentsPanel.Size = new Size(158, 296);
            fluentsPanel.TabIndex = 24;
            // 
            // fluentListBox
            // 
            fluentListBox.Dock = DockStyle.Fill;
            fluentListBox.FormattingEnabled = true;
            fluentListBox.Location = new Point(0, 0);
            fluentListBox.Margin = new Padding(3, 4, 3, 4);
            fluentListBox.Name = "fluentListBox";
            fluentListBox.Size = new Size(158, 296);
            fluentListBox.TabIndex = 0;
            // 
            // actionsPanel
            // 
            actionsPanel.Controls.Add(actionListBox);
            actionsPanel.Location = new Point(199, 36);
            actionsPanel.Margin = new Padding(3, 4, 3, 4);
            actionsPanel.Name = "actionsPanel";
            actionsPanel.Size = new Size(158, 279);
            actionsPanel.TabIndex = 25;
            // 
            // actionListBox
            // 
            actionListBox.Dock = DockStyle.Fill;
            actionListBox.FormattingEnabled = true;
            actionListBox.Location = new Point(0, 0);
            actionListBox.Margin = new Padding(3, 4, 3, 4);
            actionListBox.Name = "actionListBox";
            actionListBox.Size = new Size(158, 279);
            actionListBox.TabIndex = 0;
            actionListBox.KeyDown += actionListBox_KeyDown;
            actionListBox.MouseDown += actionListBox_MouseDown;
            // 
            // actionProgramPanel
            // 
            actionProgramPanel.Controls.Add(actionProgramCheckedListBox);
            actionProgramPanel.Location = new Point(376, 36);
            actionProgramPanel.Margin = new Padding(3, 4, 3, 4);
            actionProgramPanel.Name = "actionProgramPanel";
            actionProgramPanel.Size = new Size(179, 279);
            actionProgramPanel.TabIndex = 25;
            // 
            // actionProgramCheckedListBox
            // 
            actionProgramCheckedListBox.AllowDrop = true;
            actionProgramCheckedListBox.Dock = DockStyle.Fill;
            actionProgramCheckedListBox.FormattingEnabled = true;
            actionProgramCheckedListBox.Location = new Point(0, 0);
            actionProgramCheckedListBox.Margin = new Padding(3, 4, 3, 4);
            actionProgramCheckedListBox.Name = "actionProgramCheckedListBox";
            actionProgramCheckedListBox.Size = new Size(179, 279);
            actionProgramCheckedListBox.TabIndex = 0;
            actionProgramCheckedListBox.DragDrop += actionProgramCheckedListBox_DragDrop;
            actionProgramCheckedListBox.DragEnter += actionProgramCheckedListBox_DragEnter;
            actionProgramCheckedListBox.KeyDown += actionProgramCheckedListBox_KeyDown;
            // 
            // actionProgramLabel
            // 
            actionProgramLabel.AutoSize = true;
            actionProgramLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            actionProgramLabel.Location = new Point(413, 12);
            actionProgramLabel.Name = "actionProgramLabel";
            actionProgramLabel.Size = new Size(124, 20);
            actionProgramLabel.TabIndex = 26;
            actionProgramLabel.Text = "Action Program:";
            // 
            // removeActionFromProgramButton
            // 
            removeActionFromProgramButton.Location = new Point(376, 353);
            removeActionFromProgramButton.Margin = new Padding(3, 4, 3, 4);
            removeActionFromProgramButton.Name = "removeActionFromProgramButton";
            removeActionFromProgramButton.Size = new Size(179, 31);
            removeActionFromProgramButton.TabIndex = 28;
            removeActionFromProgramButton.Text = "Remove selected action(s)";
            removeActionFromProgramButton.UseVisualStyleBackColor = true;
            removeActionFromProgramButton.Click += removeActionFromProgramButton_Click;
            // 
            // addActionToProgramButton
            // 
            addActionToProgramButton.Location = new Point(199, 319);
            addActionToProgramButton.Margin = new Padding(3, 4, 3, 4);
            addActionToProgramButton.Name = "addActionToProgramButton";
            addActionToProgramButton.Size = new Size(158, 31);
            addActionToProgramButton.TabIndex = 29;
            addActionToProgramButton.Text = "Add selected action";
            addActionToProgramButton.UseVisualStyleBackColor = true;
            addActionToProgramButton.Click += addActionToProgramButton_Click;
            // 
            // selectAllActionsFromProgramButton
            // 
            selectAllActionsFromProgramButton.Location = new Point(376, 319);
            selectAllActionsFromProgramButton.Margin = new Padding(3, 4, 3, 4);
            selectAllActionsFromProgramButton.Name = "selectAllActionsFromProgramButton";
            selectAllActionsFromProgramButton.Size = new Size(179, 31);
            selectAllActionsFromProgramButton.TabIndex = 30;
            selectAllActionsFromProgramButton.Text = "Select all actions";
            selectAllActionsFromProgramButton.UseVisualStyleBackColor = true;
            selectAllActionsFromProgramButton.Click += selectAllActionsFromProgramButton_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(894, 604);
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
            Margin = new Padding(3, 4, 3, 4);
            MaximumSize = new Size(912, 651);
            MinimumSize = new Size(912, 651);
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