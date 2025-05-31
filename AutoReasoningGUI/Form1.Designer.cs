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
            components = new System.ComponentModel.Container();
            nextPage = new Button();
            contFluentsActions = new SplitContainer();
            isInertialCheckBox = new CheckBox();
            fluentLabel = new Label();
            addFluentButton = new Button();
            fluentTextBox = new TextBox();
            actionLabel = new Label();
            addActionButton = new Button();
            actionTextBox = new TextBox();
            fluentActionListContainer = new SplitContainer();
            fluentCheckedListBox = new CheckedListBox();
            actionCheckedListBox = new CheckedListBox();
            fluentListLabel = new Label();
            actionListLabel = new Label();
            removeFluentsButton = new Button();
            removeActionsButton = new Button();
            typeOfStatementComboBox = new ComboBox();
            panel1 = new Panel();
            releasesPanel = new Panel();
            releasesFluentComboBox = new ComboBox();
            releasesNumericUpDown = new NumericUpDown();
            releasesCostLabel = new Label();
            releasesTextBox2 = new TextBox();
            releasesIfLabel = new Label();
            releasesActionComboBox = new ComboBox();
            releasesLabel = new Label();
            causesPanel = new Panel();
            causesTextBox1 = new TextBox();
            causesNumericUpDown = new NumericUpDown();
            casuesCostLabel = new Label();
            causesTextBox2 = new TextBox();
            causesIfLabel = new Label();
            causesActionComboBox = new ComboBox();
            causesLabel = new Label();
            impossiblePanel = new Panel();
            impossibleTextBox = new TextBox();
            impossibleIfLabel = new Label();
            impossibleActionComboBox = new ComboBox();
            impossibleLabel = new Label();
            alwaysPanel = new Panel();
            alwaysTextBox = new TextBox();
            alwaysLabel = new Label();
            initiallyPanel = new Panel();
            initiallyFluentComboBox = new ComboBox();
            InitialCheckBox = new CheckBox();
            initiallyLabel = new Label();
            addStatementButton = new Button();
            statementsCheckedListBox = new CheckedListBox();
            panel2 = new Panel();
            statementsLabel = new Label();
            removeStatementsButton = new Button();
            errorProvider1 = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)contFluentsActions).BeginInit();
            contFluentsActions.Panel1.SuspendLayout();
            contFluentsActions.Panel2.SuspendLayout();
            contFluentsActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)fluentActionListContainer).BeginInit();
            fluentActionListContainer.Panel1.SuspendLayout();
            fluentActionListContainer.Panel2.SuspendLayout();
            fluentActionListContainer.SuspendLayout();
            panel1.SuspendLayout();
            releasesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)releasesNumericUpDown).BeginInit();
            causesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)causesNumericUpDown).BeginInit();
            impossiblePanel.SuspendLayout();
            alwaysPanel.SuspendLayout();
            initiallyPanel.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
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
            contFluentsActions.IsSplitterFixed = true;
            contFluentsActions.Location = new Point(206, 27);
            contFluentsActions.Name = "contFluentsActions";
            contFluentsActions.Orientation = Orientation.Horizontal;
            // 
            // contFluentsActions.Panel1
            // 
            contFluentsActions.Panel1.Controls.Add(isInertialCheckBox);
            contFluentsActions.Panel1.Controls.Add(fluentLabel);
            contFluentsActions.Panel1.Controls.Add(addFluentButton);
            contFluentsActions.Panel1.Controls.Add(fluentTextBox);
            // 
            // contFluentsActions.Panel2
            // 
            contFluentsActions.Panel2.Controls.Add(actionLabel);
            contFluentsActions.Panel2.Controls.Add(addActionButton);
            contFluentsActions.Panel2.Controls.Add(actionTextBox);
            contFluentsActions.Size = new Size(222, 164);
            contFluentsActions.SplitterDistance = 76;
            contFluentsActions.TabIndex = 1;
            // 
            // isInertialCheckBox
            // 
            isInertialCheckBox.AutoSize = true;
            isInertialCheckBox.Checked = true;
            isInertialCheckBox.CheckState = CheckState.Checked;
            isInertialCheckBox.Location = new Point(26, 47);
            isInertialCheckBox.Name = "isInertialCheckBox";
            isInertialCheckBox.Size = new Size(73, 19);
            isInertialCheckBox.TabIndex = 3;
            isInertialCheckBox.Text = "Is Inertial";
            isInertialCheckBox.UseVisualStyleBackColor = true;
            // 
            // fluentLabel
            // 
            fluentLabel.AutoSize = true;
            fluentLabel.Location = new Point(76, 0);
            fluentLabel.Name = "fluentLabel";
            fluentLabel.Size = new Size(68, 15);
            fluentLabel.TabIndex = 2;
            fluentLabel.Text = "Add fluents";
            // 
            // addFluentButton
            // 
            addFluentButton.Location = new Point(141, 31);
            addFluentButton.Name = "addFluentButton";
            addFluentButton.Size = new Size(75, 23);
            addFluentButton.TabIndex = 1;
            addFluentButton.Text = "Add";
            addFluentButton.UseVisualStyleBackColor = true;
            addFluentButton.Click += addFluentButton_Click;
            // 
            // fluentTextBox
            // 
            fluentTextBox.Location = new Point(3, 18);
            fluentTextBox.Name = "fluentTextBox";
            fluentTextBox.Size = new Size(132, 23);
            fluentTextBox.TabIndex = 0;
            // 
            // actionLabel
            // 
            actionLabel.AutoSize = true;
            actionLabel.Location = new Point(72, 0);
            actionLabel.Name = "actionLabel";
            actionLabel.Size = new Size(72, 15);
            actionLabel.TabIndex = 4;
            actionLabel.Text = "Add Actions";
            // 
            // addActionButton
            // 
            addActionButton.Location = new Point(141, 45);
            addActionButton.Name = "addActionButton";
            addActionButton.Size = new Size(75, 23);
            addActionButton.TabIndex = 3;
            addActionButton.Text = "Add";
            addActionButton.UseVisualStyleBackColor = true;
            addActionButton.Click += addActionButton_Click;
            // 
            // actionTextBox
            // 
            actionTextBox.Location = new Point(3, 29);
            actionTextBox.Name = "actionTextBox";
            actionTextBox.Size = new Size(132, 23);
            actionTextBox.TabIndex = 2;
            // 
            // fluentActionListContainer
            // 
            fluentActionListContainer.IsSplitterFixed = true;
            fluentActionListContainer.Location = new Point(434, 27);
            fluentActionListContainer.Name = "fluentActionListContainer";
            // 
            // fluentActionListContainer.Panel1
            // 
            fluentActionListContainer.Panel1.Controls.Add(fluentCheckedListBox);
            // 
            // fluentActionListContainer.Panel2
            // 
            fluentActionListContainer.Panel2.Controls.Add(actionCheckedListBox);
            fluentActionListContainer.Size = new Size(338, 164);
            fluentActionListContainer.SplitterDistance = 170;
            fluentActionListContainer.TabIndex = 2;
            // 
            // fluentCheckedListBox
            // 
            fluentCheckedListBox.Dock = DockStyle.Fill;
            fluentCheckedListBox.FormattingEnabled = true;
            fluentCheckedListBox.Location = new Point(0, 0);
            fluentCheckedListBox.Name = "fluentCheckedListBox";
            fluentCheckedListBox.Size = new Size(170, 164);
            fluentCheckedListBox.TabIndex = 1;
            fluentCheckedListBox.KeyDown += fluentCheckedListBox_KeyDown;
            // 
            // actionCheckedListBox
            // 
            actionCheckedListBox.Dock = DockStyle.Fill;
            actionCheckedListBox.FormattingEnabled = true;
            actionCheckedListBox.Location = new Point(0, 0);
            actionCheckedListBox.Name = "actionCheckedListBox";
            actionCheckedListBox.Size = new Size(164, 164);
            actionCheckedListBox.TabIndex = 0;
            actionCheckedListBox.KeyDown += actionCheckedListBox_KeyDown;
            // 
            // fluentListLabel
            // 
            fluentListLabel.AutoSize = true;
            fluentListLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            fluentListLabel.Location = new Point(493, 9);
            fluentListLabel.Name = "fluentListLabel";
            fluentListLabel.Size = new Size(50, 15);
            fluentListLabel.TabIndex = 0;
            fluentListLabel.Text = "Fluents:";
            // 
            // actionListLabel
            // 
            actionListLabel.AutoSize = true;
            actionListLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            actionListLabel.Location = new Point(664, 9);
            actionListLabel.Name = "actionListLabel";
            actionListLabel.Size = new Size(51, 15);
            actionListLabel.TabIndex = 0;
            actionListLabel.Text = "Actions:";
            // 
            // removeFluentsButton
            // 
            removeFluentsButton.Location = new Point(434, 197);
            removeFluentsButton.Name = "removeFluentsButton";
            removeFluentsButton.Size = new Size(171, 23);
            removeFluentsButton.TabIndex = 2;
            removeFluentsButton.Text = "Remove Selected Fluents";
            removeFluentsButton.UseVisualStyleBackColor = true;
            removeFluentsButton.Click += removeFluentsButton_Click;
            // 
            // removeActionsButton
            // 
            removeActionsButton.Location = new Point(609, 197);
            removeActionsButton.Name = "removeActionsButton";
            removeActionsButton.Size = new Size(163, 23);
            removeActionsButton.TabIndex = 3;
            removeActionsButton.Text = "Remove Selected Actions";
            removeActionsButton.UseVisualStyleBackColor = true;
            removeActionsButton.Click += removeActionsButton_Click;
            // 
            // typeOfStatementComboBox
            // 
            typeOfStatementComboBox.Dock = DockStyle.Top;
            typeOfStatementComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            typeOfStatementComboBox.FormattingEnabled = true;
            typeOfStatementComboBox.Items.AddRange(new object[] { "Initial Fluent Value", "Always", "Impossible Action", "Action Causes", "Action Releases" });
            typeOfStatementComboBox.Location = new Point(0, 0);
            typeOfStatementComboBox.Name = "typeOfStatementComboBox";
            typeOfStatementComboBox.Size = new Size(218, 23);
            typeOfStatementComboBox.TabIndex = 4;
            typeOfStatementComboBox.SelectedIndexChanged += typeOfStatementComboBox_SelectedIndexChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(releasesPanel);
            panel1.Controls.Add(causesPanel);
            panel1.Controls.Add(impossiblePanel);
            panel1.Controls.Add(alwaysPanel);
            panel1.Controls.Add(initiallyPanel);
            panel1.Controls.Add(addStatementButton);
            panel1.Controls.Add(typeOfStatementComboBox);
            panel1.Location = new Point(12, 226);
            panel1.Name = "panel1";
            panel1.Size = new Size(218, 223);
            panel1.TabIndex = 5;
            // 
            // releasesPanel
            // 
            releasesPanel.Controls.Add(releasesFluentComboBox);
            releasesPanel.Controls.Add(releasesNumericUpDown);
            releasesPanel.Controls.Add(releasesCostLabel);
            releasesPanel.Controls.Add(releasesTextBox2);
            releasesPanel.Controls.Add(releasesIfLabel);
            releasesPanel.Controls.Add(releasesActionComboBox);
            releasesPanel.Controls.Add(releasesLabel);
            releasesPanel.Dock = DockStyle.Fill;
            releasesPanel.Location = new Point(0, 23);
            releasesPanel.Name = "releasesPanel";
            releasesPanel.Size = new Size(218, 177);
            releasesPanel.TabIndex = 8;
            releasesPanel.Visible = false;
            // 
            // releasesFluentComboBox
            // 
            releasesFluentComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            releasesFluentComboBox.FormattingEnabled = true;
            releasesFluentComboBox.Location = new Point(26, 49);
            releasesFluentComboBox.Name = "releasesFluentComboBox";
            releasesFluentComboBox.Size = new Size(167, 23);
            releasesFluentComboBox.TabIndex = 7;
            // 
            // releasesNumericUpDown
            // 
            releasesNumericUpDown.Location = new Point(26, 151);
            releasesNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            releasesNumericUpDown.Name = "releasesNumericUpDown";
            releasesNumericUpDown.Size = new Size(167, 23);
            releasesNumericUpDown.TabIndex = 6;
            releasesNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // releasesCostLabel
            // 
            releasesCostLabel.AutoSize = true;
            releasesCostLabel.Location = new Point(90, 136);
            releasesCostLabel.Name = "releasesCostLabel";
            releasesCostLabel.Size = new Size(34, 15);
            releasesCostLabel.TabIndex = 5;
            releasesCostLabel.Text = "costs";
            // 
            // releasesTextBox2
            // 
            releasesTextBox2.Location = new Point(26, 98);
            releasesTextBox2.Multiline = true;
            releasesTextBox2.Name = "releasesTextBox2";
            releasesTextBox2.PlaceholderText = "Expression";
            releasesTextBox2.Size = new Size(167, 35);
            releasesTextBox2.TabIndex = 4;
            // 
            // releasesIfLabel
            // 
            releasesIfLabel.AutoSize = true;
            releasesIfLabel.Location = new Point(100, 82);
            releasesIfLabel.Name = "releasesIfLabel";
            releasesIfLabel.Size = new Size(14, 15);
            releasesIfLabel.TabIndex = 3;
            releasesIfLabel.Text = "if";
            // 
            // releasesActionComboBox
            // 
            releasesActionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            releasesActionComboBox.FormattingEnabled = true;
            releasesActionComboBox.Location = new Point(26, 5);
            releasesActionComboBox.Name = "releasesActionComboBox";
            releasesActionComboBox.Size = new Size(167, 23);
            releasesActionComboBox.TabIndex = 1;
            // 
            // releasesLabel
            // 
            releasesLabel.AutoSize = true;
            releasesLabel.Location = new Point(85, 31);
            releasesLabel.Name = "releasesLabel";
            releasesLabel.Size = new Size(48, 15);
            releasesLabel.TabIndex = 0;
            releasesLabel.Text = "releases";
            // 
            // causesPanel
            // 
            causesPanel.Controls.Add(causesTextBox1);
            causesPanel.Controls.Add(causesNumericUpDown);
            causesPanel.Controls.Add(casuesCostLabel);
            causesPanel.Controls.Add(causesTextBox2);
            causesPanel.Controls.Add(causesIfLabel);
            causesPanel.Controls.Add(causesActionComboBox);
            causesPanel.Controls.Add(causesLabel);
            causesPanel.Dock = DockStyle.Fill;
            causesPanel.Location = new Point(0, 23);
            causesPanel.Name = "causesPanel";
            causesPanel.Size = new Size(218, 177);
            causesPanel.TabIndex = 8;
            causesPanel.Visible = false;
            // 
            // causesTextBox1
            // 
            causesTextBox1.Location = new Point(26, 45);
            causesTextBox1.Multiline = true;
            causesTextBox1.Name = "causesTextBox1";
            causesTextBox1.PlaceholderText = "Expression";
            causesTextBox1.Size = new Size(167, 35);
            causesTextBox1.TabIndex = 7;
            // 
            // causesNumericUpDown
            // 
            causesNumericUpDown.Location = new Point(26, 151);
            causesNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            causesNumericUpDown.Name = "causesNumericUpDown";
            causesNumericUpDown.Size = new Size(167, 23);
            causesNumericUpDown.TabIndex = 6;
            causesNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // casuesCostLabel
            // 
            casuesCostLabel.AutoSize = true;
            casuesCostLabel.Location = new Point(90, 136);
            casuesCostLabel.Name = "casuesCostLabel";
            casuesCostLabel.Size = new Size(34, 15);
            casuesCostLabel.TabIndex = 5;
            casuesCostLabel.Text = "costs";
            // 
            // causesTextBox2
            // 
            causesTextBox2.Location = new Point(26, 101);
            causesTextBox2.Multiline = true;
            causesTextBox2.Name = "causesTextBox2";
            causesTextBox2.PlaceholderText = "Expression";
            causesTextBox2.Size = new Size(167, 35);
            causesTextBox2.TabIndex = 4;
            // 
            // causesIfLabel
            // 
            causesIfLabel.AutoSize = true;
            causesIfLabel.Location = new Point(97, 83);
            causesIfLabel.Name = "causesIfLabel";
            causesIfLabel.Size = new Size(14, 15);
            causesIfLabel.TabIndex = 3;
            causesIfLabel.Text = "if";
            // 
            // causesActionComboBox
            // 
            causesActionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            causesActionComboBox.FormattingEnabled = true;
            causesActionComboBox.Location = new Point(26, 5);
            causesActionComboBox.Name = "causesActionComboBox";
            causesActionComboBox.Size = new Size(167, 23);
            causesActionComboBox.TabIndex = 1;
            // 
            // causesLabel
            // 
            causesLabel.AutoSize = true;
            causesLabel.Location = new Point(82, 27);
            causesLabel.Name = "causesLabel";
            causesLabel.Size = new Size(42, 15);
            causesLabel.TabIndex = 0;
            causesLabel.Text = "causes";
            // 
            // impossiblePanel
            // 
            impossiblePanel.Controls.Add(impossibleTextBox);
            impossiblePanel.Controls.Add(impossibleIfLabel);
            impossiblePanel.Controls.Add(impossibleActionComboBox);
            impossiblePanel.Controls.Add(impossibleLabel);
            impossiblePanel.Dock = DockStyle.Fill;
            impossiblePanel.Location = new Point(0, 23);
            impossiblePanel.Name = "impossiblePanel";
            impossiblePanel.Size = new Size(218, 177);
            impossiblePanel.TabIndex = 8;
            impossiblePanel.Visible = false;
            // 
            // impossibleTextBox
            // 
            impossibleTextBox.Location = new Point(26, 90);
            impossibleTextBox.Multiline = true;
            impossibleTextBox.Name = "impossibleTextBox";
            impossibleTextBox.PlaceholderText = "Expression";
            impossibleTextBox.Size = new Size(167, 51);
            impossibleTextBox.TabIndex = 3;
            // 
            // impossibleIfLabel
            // 
            impossibleIfLabel.AutoSize = true;
            impossibleIfLabel.Location = new Point(100, 72);
            impossibleIfLabel.Name = "impossibleIfLabel";
            impossibleIfLabel.Size = new Size(14, 15);
            impossibleIfLabel.TabIndex = 2;
            impossibleIfLabel.Text = "if";
            // 
            // impossibleActionComboBox
            // 
            impossibleActionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            impossibleActionComboBox.FormattingEnabled = true;
            impossibleActionComboBox.Location = new Point(26, 46);
            impossibleActionComboBox.Name = "impossibleActionComboBox";
            impossibleActionComboBox.Size = new Size(167, 23);
            impossibleActionComboBox.TabIndex = 1;
            // 
            // impossibleLabel
            // 
            impossibleLabel.AutoSize = true;
            impossibleLabel.Location = new Point(79, 13);
            impossibleLabel.Name = "impossibleLabel";
            impossibleLabel.Size = new Size(64, 15);
            impossibleLabel.TabIndex = 0;
            impossibleLabel.Text = "Impossible";
            // 
            // alwaysPanel
            // 
            alwaysPanel.Controls.Add(alwaysTextBox);
            alwaysPanel.Controls.Add(alwaysLabel);
            alwaysPanel.Dock = DockStyle.Fill;
            alwaysPanel.Location = new Point(0, 23);
            alwaysPanel.Name = "alwaysPanel";
            alwaysPanel.Size = new Size(218, 177);
            alwaysPanel.TabIndex = 8;
            alwaysPanel.Visible = false;
            // 
            // alwaysTextBox
            // 
            alwaysTextBox.Location = new Point(26, 46);
            alwaysTextBox.Multiline = true;
            alwaysTextBox.Name = "alwaysTextBox";
            alwaysTextBox.PlaceholderText = "Expression";
            alwaysTextBox.Size = new Size(167, 95);
            alwaysTextBox.TabIndex = 1;
            // 
            // alwaysLabel
            // 
            alwaysLabel.AutoSize = true;
            alwaysLabel.Location = new Point(85, 13);
            alwaysLabel.Name = "alwaysLabel";
            alwaysLabel.Size = new Size(42, 15);
            alwaysLabel.TabIndex = 0;
            alwaysLabel.Text = "always";
            // 
            // initiallyPanel
            // 
            initiallyPanel.Controls.Add(initiallyFluentComboBox);
            initiallyPanel.Controls.Add(InitialCheckBox);
            initiallyPanel.Controls.Add(initiallyLabel);
            initiallyPanel.Dock = DockStyle.Fill;
            initiallyPanel.Location = new Point(0, 23);
            initiallyPanel.Name = "initiallyPanel";
            initiallyPanel.Size = new Size(218, 177);
            initiallyPanel.TabIndex = 8;
            initiallyPanel.Visible = false;
            // 
            // initiallyFluentComboBox
            // 
            initiallyFluentComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            initiallyFluentComboBox.FormattingEnabled = true;
            initiallyFluentComboBox.Location = new Point(26, 59);
            initiallyFluentComboBox.Name = "initiallyFluentComboBox";
            initiallyFluentComboBox.Size = new Size(157, 23);
            initiallyFluentComboBox.TabIndex = 9;
            // 
            // InitialCheckBox
            // 
            InitialCheckBox.AutoSize = true;
            InitialCheckBox.Checked = true;
            InitialCheckBox.CheckState = CheckState.Checked;
            InitialCheckBox.Location = new Point(79, 100);
            InitialCheckBox.Name = "InitialCheckBox";
            InitialCheckBox.Size = new Size(49, 19);
            InitialCheckBox.TabIndex = 9;
            InitialCheckBox.Text = "True";
            InitialCheckBox.UseVisualStyleBackColor = true;
            // 
            // initiallyLabel
            // 
            initiallyLabel.AutoSize = true;
            initiallyLabel.Location = new Point(79, 28);
            initiallyLabel.Name = "initiallyLabel";
            initiallyLabel.Size = new Size(45, 15);
            initiallyLabel.TabIndex = 0;
            initiallyLabel.Text = "initially";
            // 
            // addStatementButton
            // 
            addStatementButton.Dock = DockStyle.Bottom;
            addStatementButton.Location = new Point(0, 200);
            addStatementButton.Name = "addStatementButton";
            addStatementButton.Size = new Size(218, 23);
            addStatementButton.TabIndex = 6;
            addStatementButton.Text = "Add Statement";
            addStatementButton.UseVisualStyleBackColor = true;
            addStatementButton.Click += addStatementButton_Click;
            // 
            // statementsCheckedListBox
            // 
            statementsCheckedListBox.FormattingEnabled = true;
            statementsCheckedListBox.Location = new Point(3, 17);
            statementsCheckedListBox.Name = "statementsCheckedListBox";
            statementsCheckedListBox.Size = new Size(367, 166);
            statementsCheckedListBox.TabIndex = 6;
            statementsCheckedListBox.KeyDown += statementsCheckedListBox_KeyDown;
            // 
            // panel2
            // 
            panel2.Controls.Add(statementsLabel);
            panel2.Controls.Add(removeStatementsButton);
            panel2.Controls.Add(statementsCheckedListBox);
            panel2.Location = new Point(309, 226);
            panel2.Name = "panel2";
            panel2.Size = new Size(373, 223);
            panel2.TabIndex = 7;
            // 
            // statementsLabel
            // 
            statementsLabel.AutoSize = true;
            statementsLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            statementsLabel.Location = new Point(151, 0);
            statementsLabel.Name = "statementsLabel";
            statementsLabel.Size = new Size(75, 15);
            statementsLabel.TabIndex = 8;
            statementsLabel.Text = "Statements:";
            // 
            // removeStatementsButton
            // 
            removeStatementsButton.Dock = DockStyle.Bottom;
            removeStatementsButton.Location = new Point(0, 200);
            removeStatementsButton.Name = "removeStatementsButton";
            removeStatementsButton.Size = new Size(373, 23);
            removeStatementsButton.TabIndex = 7;
            removeStatementsButton.Text = "Remove Selected Statements";
            removeStatementsButton.UseVisualStyleBackColor = true;
            removeStatementsButton.Click += removeStatementsButton_Click;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 461);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(removeActionsButton);
            Controls.Add(removeFluentsButton);
            Controls.Add(actionListLabel);
            Controls.Add(fluentActionListContainer);
            Controls.Add(fluentListLabel);
            Controls.Add(contFluentsActions);
            Controls.Add(nextPage);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximumSize = new Size(1500, 1498);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Reasoning";
            contFluentsActions.Panel1.ResumeLayout(false);
            contFluentsActions.Panel1.PerformLayout();
            contFluentsActions.Panel2.ResumeLayout(false);
            contFluentsActions.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)contFluentsActions).EndInit();
            contFluentsActions.ResumeLayout(false);
            fluentActionListContainer.Panel1.ResumeLayout(false);
            fluentActionListContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)fluentActionListContainer).EndInit();
            fluentActionListContainer.ResumeLayout(false);
            panel1.ResumeLayout(false);
            releasesPanel.ResumeLayout(false);
            releasesPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)releasesNumericUpDown).EndInit();
            causesPanel.ResumeLayout(false);
            causesPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)causesNumericUpDown).EndInit();
            impossiblePanel.ResumeLayout(false);
            impossiblePanel.PerformLayout();
            alwaysPanel.ResumeLayout(false);
            alwaysPanel.PerformLayout();
            initiallyPanel.ResumeLayout(false);
            initiallyPanel.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
        private CheckBox isInertialCheckBox;
        private SplitContainer fluentActionListContainer;
        private Label fluentListLabel;
        private Label actionListLabel;
        private CheckedListBox fluentCheckedListBox;
        private CheckedListBox actionCheckedListBox;
        private Button removeFluentsButton;
        private Button removeActionsButton;
        private ComboBox typeOfStatementComboBox;
        private Panel panel1;
        private Button addStatementButton;
        private CheckedListBox statementsCheckedListBox;
        private Panel panel2;
        private Button removeStatementsButton;
        private Label statementsLabel;
        private Panel initiallyPanel;
        private Label initiallyLabel;
        private CheckBox InitialCheckBox;
        private ComboBox initiallyFluentComboBox;
        private Panel alwaysPanel;
        private TextBox alwaysTextBox;
        private Label alwaysLabel;
        private Panel impossiblePanel;
        private Label impossibleIfLabel;
        private ComboBox impossibleActionComboBox;
        private Label impossibleLabel;
        private TextBox impossibleTextBox;
        private Panel causesPanel;
        private ComboBox causesActionComboBox;
        private Label causesLabel;
        private Label causesIfLabel;
        private NumericUpDown causesNumericUpDown;
        private Label casuesCostLabel;
        private TextBox causesTextBox2;
        private Panel releasesPanel;
        private ComboBox releasesActionComboBox;
        private Label releasesLabel;
        private NumericUpDown releasesNumericUpDown;
        private Label releasesCostLabel;
        private TextBox releasesTextBox2;
        private Label releasesIfLabel;
        private ComboBox releasesFluentComboBox;
        private ErrorProvider errorProvider1;
        private TextBox causesTextBox1;
    }
}
