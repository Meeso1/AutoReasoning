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
            alwaysPanel = new Panel();
            alwaysTextBox = new TextBox();
            alwaysLabel = new Label();
            impossiblePanel = new Panel();
            impossibleTextBox = new TextBox();
            impossibleIfLabel = new Label();
            impossibleActionComboBox = new ComboBox();
            impossibleLabel = new Label();
            initiallyPanel = new Panel();
            initiallyTextBox = new TextBox();
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
            alwaysPanel.SuspendLayout();
            impossiblePanel.SuspendLayout();
            initiallyPanel.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // nextPage
            // 
            nextPage.Location = new Point(797, 568);
            nextPage.Margin = new Padding(3, 4, 3, 4);
            nextPage.Name = "nextPage";
            nextPage.Size = new Size(86, 31);
            nextPage.TabIndex = 0;
            nextPage.Text = "Next Page";
            nextPage.UseVisualStyleBackColor = true;
            nextPage.Click += nextPage_Click;
            // 
            // contFluentsActions
            // 
            contFluentsActions.BorderStyle = BorderStyle.FixedSingle;
            contFluentsActions.IsSplitterFixed = true;
            contFluentsActions.Location = new Point(235, 36);
            contFluentsActions.Margin = new Padding(3, 4, 3, 4);
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
            contFluentsActions.Size = new Size(254, 219);
            contFluentsActions.SplitterDistance = 101;
            contFluentsActions.SplitterWidth = 5;
            contFluentsActions.TabIndex = 1;
            // 
            // isInertialCheckBox
            // 
            isInertialCheckBox.AutoSize = true;
            isInertialCheckBox.Checked = true;
            isInertialCheckBox.CheckState = CheckState.Checked;
            isInertialCheckBox.Location = new Point(30, 63);
            isInertialCheckBox.Margin = new Padding(3, 4, 3, 4);
            isInertialCheckBox.Name = "isInertialCheckBox";
            isInertialCheckBox.Size = new Size(91, 24);
            isInertialCheckBox.TabIndex = 3;
            isInertialCheckBox.Text = "Is Inertial";
            isInertialCheckBox.UseVisualStyleBackColor = true;
            // 
            // fluentLabel
            // 
            fluentLabel.AutoSize = true;
            fluentLabel.Location = new Point(87, 0);
            fluentLabel.Name = "fluentLabel";
            fluentLabel.Size = new Size(85, 20);
            fluentLabel.TabIndex = 2;
            fluentLabel.Text = "Add fluents";
            // 
            // addFluentButton
            // 
            addFluentButton.Location = new Point(161, 41);
            addFluentButton.Margin = new Padding(3, 4, 3, 4);
            addFluentButton.Name = "addFluentButton";
            addFluentButton.Size = new Size(86, 31);
            addFluentButton.TabIndex = 1;
            addFluentButton.Text = "Add";
            addFluentButton.UseVisualStyleBackColor = true;
            addFluentButton.Click += addFluentButton_Click;
            // 
            // fluentTextBox
            // 
            fluentTextBox.Location = new Point(3, 24);
            fluentTextBox.Margin = new Padding(3, 4, 3, 4);
            fluentTextBox.Name = "fluentTextBox";
            fluentTextBox.Size = new Size(150, 27);
            fluentTextBox.TabIndex = 0;
            // 
            // actionLabel
            // 
            actionLabel.AutoSize = true;
            actionLabel.Location = new Point(82, 0);
            actionLabel.Name = "actionLabel";
            actionLabel.Size = new Size(90, 20);
            actionLabel.TabIndex = 4;
            actionLabel.Text = "Add Actions";
            // 
            // addActionButton
            // 
            addActionButton.Location = new Point(161, 60);
            addActionButton.Margin = new Padding(3, 4, 3, 4);
            addActionButton.Name = "addActionButton";
            addActionButton.Size = new Size(86, 31);
            addActionButton.TabIndex = 3;
            addActionButton.Text = "Add";
            addActionButton.UseVisualStyleBackColor = true;
            addActionButton.Click += addActionButton_Click;
            // 
            // actionTextBox
            // 
            actionTextBox.Location = new Point(3, 39);
            actionTextBox.Margin = new Padding(3, 4, 3, 4);
            actionTextBox.Name = "actionTextBox";
            actionTextBox.Size = new Size(150, 27);
            actionTextBox.TabIndex = 2;
            // 
            // fluentActionListContainer
            // 
            fluentActionListContainer.IsSplitterFixed = true;
            fluentActionListContainer.Location = new Point(496, 36);
            fluentActionListContainer.Margin = new Padding(3, 4, 3, 4);
            fluentActionListContainer.Name = "fluentActionListContainer";
            // 
            // fluentActionListContainer.Panel1
            // 
            fluentActionListContainer.Panel1.Controls.Add(fluentCheckedListBox);
            // 
            // fluentActionListContainer.Panel2
            // 
            fluentActionListContainer.Panel2.Controls.Add(actionCheckedListBox);
            fluentActionListContainer.Size = new Size(386, 219);
            fluentActionListContainer.SplitterDistance = 194;
            fluentActionListContainer.SplitterWidth = 5;
            fluentActionListContainer.TabIndex = 2;
            // 
            // fluentCheckedListBox
            // 
            fluentCheckedListBox.Dock = DockStyle.Fill;
            fluentCheckedListBox.FormattingEnabled = true;
            fluentCheckedListBox.Location = new Point(0, 0);
            fluentCheckedListBox.Margin = new Padding(3, 4, 3, 4);
            fluentCheckedListBox.Name = "fluentCheckedListBox";
            fluentCheckedListBox.Size = new Size(194, 219);
            fluentCheckedListBox.TabIndex = 1;
            fluentCheckedListBox.KeyDown += fluentCheckedListBox_KeyDown;
            // 
            // actionCheckedListBox
            // 
            actionCheckedListBox.Dock = DockStyle.Fill;
            actionCheckedListBox.FormattingEnabled = true;
            actionCheckedListBox.Location = new Point(0, 0);
            actionCheckedListBox.Margin = new Padding(3, 4, 3, 4);
            actionCheckedListBox.Name = "actionCheckedListBox";
            actionCheckedListBox.Size = new Size(187, 219);
            actionCheckedListBox.TabIndex = 0;
            actionCheckedListBox.KeyDown += actionCheckedListBox_KeyDown;
            // 
            // fluentListLabel
            // 
            fluentListLabel.AutoSize = true;
            fluentListLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            fluentListLabel.Location = new Point(563, 12);
            fluentListLabel.Name = "fluentListLabel";
            fluentListLabel.Size = new Size(64, 20);
            fluentListLabel.TabIndex = 0;
            fluentListLabel.Text = "Fluents:";
            // 
            // actionListLabel
            // 
            actionListLabel.AutoSize = true;
            actionListLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            actionListLabel.Location = new Point(759, 12);
            actionListLabel.Name = "actionListLabel";
            actionListLabel.Size = new Size(66, 20);
            actionListLabel.TabIndex = 0;
            actionListLabel.Text = "Actions:";
            // 
            // removeFluentsButton
            // 
            removeFluentsButton.Location = new Point(496, 263);
            removeFluentsButton.Margin = new Padding(3, 4, 3, 4);
            removeFluentsButton.Name = "removeFluentsButton";
            removeFluentsButton.Size = new Size(195, 31);
            removeFluentsButton.TabIndex = 2;
            removeFluentsButton.Text = "Remove Selected Fluents";
            removeFluentsButton.UseVisualStyleBackColor = true;
            removeFluentsButton.Click += removeFluentsButton_Click;
            // 
            // removeActionsButton
            // 
            removeActionsButton.Location = new Point(696, 263);
            removeActionsButton.Margin = new Padding(3, 4, 3, 4);
            removeActionsButton.Name = "removeActionsButton";
            removeActionsButton.Size = new Size(186, 31);
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
            typeOfStatementComboBox.Margin = new Padding(3, 4, 3, 4);
            typeOfStatementComboBox.Name = "typeOfStatementComboBox";
            typeOfStatementComboBox.Size = new Size(249, 28);
            typeOfStatementComboBox.TabIndex = 4;
            typeOfStatementComboBox.SelectedIndexChanged += typeOfStatementComboBox_SelectedIndexChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(releasesPanel);
            panel1.Controls.Add(causesPanel);
            panel1.Controls.Add(alwaysPanel);
            panel1.Controls.Add(impossiblePanel);
            panel1.Controls.Add(initiallyPanel);
            panel1.Controls.Add(addStatementButton);
            panel1.Controls.Add(typeOfStatementComboBox);
            panel1.Location = new Point(14, 301);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(249, 297);
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
            releasesPanel.Location = new Point(0, 28);
            releasesPanel.Margin = new Padding(3, 4, 3, 4);
            releasesPanel.Name = "releasesPanel";
            releasesPanel.Size = new Size(249, 238);
            releasesPanel.TabIndex = 8;
            releasesPanel.Visible = false;
            // 
            // releasesFluentComboBox
            // 
            releasesFluentComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            releasesFluentComboBox.FormattingEnabled = true;
            releasesFluentComboBox.Location = new Point(30, 65);
            releasesFluentComboBox.Margin = new Padding(3, 4, 3, 4);
            releasesFluentComboBox.Name = "releasesFluentComboBox";
            releasesFluentComboBox.Size = new Size(190, 28);
            releasesFluentComboBox.TabIndex = 7;
            // 
            // releasesNumericUpDown
            // 
            releasesNumericUpDown.Location = new Point(30, 201);
            releasesNumericUpDown.Margin = new Padding(3, 4, 3, 4);
            releasesNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            releasesNumericUpDown.Name = "releasesNumericUpDown";
            releasesNumericUpDown.Size = new Size(191, 27);
            releasesNumericUpDown.TabIndex = 6;
            releasesNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            releasesNumericUpDown.ValueChanged += releasesNumericUpDown_ValueChanged;
            // 
            // releasesCostLabel
            // 
            releasesCostLabel.AutoSize = true;
            releasesCostLabel.Location = new Point(103, 181);
            releasesCostLabel.Name = "releasesCostLabel";
            releasesCostLabel.Size = new Size(42, 20);
            releasesCostLabel.TabIndex = 5;
            releasesCostLabel.Text = "costs";
            // 
            // releasesTextBox2
            // 
            releasesTextBox2.Location = new Point(30, 131);
            releasesTextBox2.Margin = new Padding(3, 4, 3, 4);
            releasesTextBox2.Multiline = true;
            releasesTextBox2.Name = "releasesTextBox2";
            releasesTextBox2.PlaceholderText = "Expression";
            releasesTextBox2.Size = new Size(190, 45);
            releasesTextBox2.TabIndex = 4;
            // 
            // releasesIfLabel
            // 
            releasesIfLabel.AutoSize = true;
            releasesIfLabel.Location = new Point(114, 109);
            releasesIfLabel.Name = "releasesIfLabel";
            releasesIfLabel.Size = new Size(18, 20);
            releasesIfLabel.TabIndex = 3;
            releasesIfLabel.Text = "if";
            // 
            // releasesActionComboBox
            // 
            releasesActionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            releasesActionComboBox.FormattingEnabled = true;
            releasesActionComboBox.Location = new Point(30, 7);
            releasesActionComboBox.Margin = new Padding(3, 4, 3, 4);
            releasesActionComboBox.Name = "releasesActionComboBox";
            releasesActionComboBox.Size = new Size(190, 28);
            releasesActionComboBox.TabIndex = 1;
            // 
            // releasesLabel
            // 
            releasesLabel.AutoSize = true;
            releasesLabel.Location = new Point(97, 41);
            releasesLabel.Name = "releasesLabel";
            releasesLabel.Size = new Size(62, 20);
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
            causesPanel.Location = new Point(0, 28);
            causesPanel.Margin = new Padding(3, 4, 3, 4);
            causesPanel.Name = "causesPanel";
            causesPanel.Size = new Size(249, 238);
            causesPanel.TabIndex = 8;
            causesPanel.Visible = false;
            // 
            // causesTextBox1
            // 
            causesTextBox1.Location = new Point(30, 60);
            causesTextBox1.Margin = new Padding(3, 4, 3, 4);
            causesTextBox1.Multiline = true;
            causesTextBox1.Name = "causesTextBox1";
            causesTextBox1.PlaceholderText = "Expression";
            causesTextBox1.Size = new Size(190, 45);
            causesTextBox1.TabIndex = 7;
            // 
            // causesNumericUpDown
            // 
            causesNumericUpDown.Location = new Point(30, 201);
            causesNumericUpDown.Margin = new Padding(3, 4, 3, 4);
            causesNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            causesNumericUpDown.Name = "causesNumericUpDown";
            causesNumericUpDown.Size = new Size(191, 27);
            causesNumericUpDown.TabIndex = 6;
            causesNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            causesNumericUpDown.ValueChanged += causesNumericUpDown_ValueChanged;
            // 
            // casuesCostLabel
            // 
            casuesCostLabel.AutoSize = true;
            casuesCostLabel.Location = new Point(103, 181);
            casuesCostLabel.Name = "casuesCostLabel";
            casuesCostLabel.Size = new Size(42, 20);
            casuesCostLabel.TabIndex = 5;
            casuesCostLabel.Text = "costs";
            // 
            // causesTextBox2
            // 
            causesTextBox2.Location = new Point(30, 135);
            causesTextBox2.Margin = new Padding(3, 4, 3, 4);
            causesTextBox2.Multiline = true;
            causesTextBox2.Name = "causesTextBox2";
            causesTextBox2.PlaceholderText = "Expression";
            causesTextBox2.Size = new Size(190, 45);
            causesTextBox2.TabIndex = 4;
            // 
            // causesIfLabel
            // 
            causesIfLabel.AutoSize = true;
            causesIfLabel.Location = new Point(111, 111);
            causesIfLabel.Name = "causesIfLabel";
            causesIfLabel.Size = new Size(18, 20);
            causesIfLabel.TabIndex = 3;
            causesIfLabel.Text = "if";
            // 
            // causesActionComboBox
            // 
            causesActionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            causesActionComboBox.FormattingEnabled = true;
            causesActionComboBox.Location = new Point(30, 7);
            causesActionComboBox.Margin = new Padding(3, 4, 3, 4);
            causesActionComboBox.Name = "causesActionComboBox";
            causesActionComboBox.Size = new Size(190, 28);
            causesActionComboBox.TabIndex = 1;
            // 
            // causesLabel
            // 
            causesLabel.AutoSize = true;
            causesLabel.Location = new Point(94, 36);
            causesLabel.Name = "causesLabel";
            causesLabel.Size = new Size(52, 20);
            causesLabel.TabIndex = 0;
            causesLabel.Text = "causes";
            // 
            // alwaysPanel
            // 
            alwaysPanel.Controls.Add(alwaysTextBox);
            alwaysPanel.Controls.Add(alwaysLabel);
            alwaysPanel.Dock = DockStyle.Fill;
            alwaysPanel.Location = new Point(0, 28);
            alwaysPanel.Margin = new Padding(3, 4, 3, 4);
            alwaysPanel.Name = "alwaysPanel";
            alwaysPanel.Size = new Size(249, 238);
            alwaysPanel.TabIndex = 8;
            alwaysPanel.Visible = false;
            // 
            // alwaysTextBox
            // 
            alwaysTextBox.Location = new Point(30, 61);
            alwaysTextBox.Margin = new Padding(3, 4, 3, 4);
            alwaysTextBox.Multiline = true;
            alwaysTextBox.Name = "alwaysTextBox";
            alwaysTextBox.PlaceholderText = "Expression";
            alwaysTextBox.Size = new Size(190, 117);
            alwaysTextBox.TabIndex = 1;
            // 
            // alwaysLabel
            // 
            alwaysLabel.AutoSize = true;
            alwaysLabel.Location = new Point(97, 17);
            alwaysLabel.Name = "alwaysLabel";
            alwaysLabel.Size = new Size(53, 20);
            alwaysLabel.TabIndex = 0;
            alwaysLabel.Text = "always";
            // 
            // impossiblePanel
            // 
            impossiblePanel.Controls.Add(impossibleTextBox);
            impossiblePanel.Controls.Add(impossibleIfLabel);
            impossiblePanel.Controls.Add(impossibleActionComboBox);
            impossiblePanel.Controls.Add(impossibleLabel);
            impossiblePanel.Dock = DockStyle.Fill;
            impossiblePanel.Location = new Point(0, 28);
            impossiblePanel.Margin = new Padding(3, 4, 3, 4);
            impossiblePanel.Name = "impossiblePanel";
            impossiblePanel.Size = new Size(249, 238);
            impossiblePanel.TabIndex = 8;
            impossiblePanel.Visible = false;
            // 
            // impossibleTextBox
            // 
            impossibleTextBox.Location = new Point(30, 120);
            impossibleTextBox.Margin = new Padding(3, 4, 3, 4);
            impossibleTextBox.Multiline = true;
            impossibleTextBox.Name = "impossibleTextBox";
            impossibleTextBox.PlaceholderText = "Expression";
            impossibleTextBox.Size = new Size(190, 67);
            impossibleTextBox.TabIndex = 3;
            // 
            // impossibleIfLabel
            // 
            impossibleIfLabel.AutoSize = true;
            impossibleIfLabel.Location = new Point(114, 96);
            impossibleIfLabel.Name = "impossibleIfLabel";
            impossibleIfLabel.Size = new Size(18, 20);
            impossibleIfLabel.TabIndex = 2;
            impossibleIfLabel.Text = "if";
            // 
            // impossibleActionComboBox
            // 
            impossibleActionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            impossibleActionComboBox.FormattingEnabled = true;
            impossibleActionComboBox.Location = new Point(30, 61);
            impossibleActionComboBox.Margin = new Padding(3, 4, 3, 4);
            impossibleActionComboBox.Name = "impossibleActionComboBox";
            impossibleActionComboBox.Size = new Size(190, 28);
            impossibleActionComboBox.TabIndex = 1;
            // 
            // impossibleLabel
            // 
            impossibleLabel.AutoSize = true;
            impossibleLabel.Location = new Point(90, 17);
            impossibleLabel.Name = "impossibleLabel";
            impossibleLabel.Size = new Size(81, 20);
            impossibleLabel.TabIndex = 0;
            impossibleLabel.Text = "Impossible";
            // 
            // initiallyPanel
            // 
            initiallyPanel.Controls.Add(initiallyTextBox);
            initiallyPanel.Controls.Add(initiallyLabel);
            initiallyPanel.Dock = DockStyle.Fill;
            initiallyPanel.Location = new Point(0, 28);
            initiallyPanel.Margin = new Padding(3, 4, 3, 4);
            initiallyPanel.Name = "initiallyPanel";
            initiallyPanel.Size = new Size(249, 238);
            initiallyPanel.TabIndex = 8;
            initiallyPanel.Visible = false;
            // 
            // initiallyTextBox
            // 
            initiallyTextBox.Location = new Point(32, 66);
            initiallyTextBox.Margin = new Padding(3, 4, 3, 4);
            initiallyTextBox.Multiline = true;
            initiallyTextBox.Name = "initiallyTextBox";
            initiallyTextBox.PlaceholderText = "Expression";
            initiallyTextBox.Size = new Size(190, 107);
            initiallyTextBox.TabIndex = 5;
            // 
            // initiallyLabel
            // 
            initiallyLabel.AutoSize = true;
            initiallyLabel.Location = new Point(97, 22);
            initiallyLabel.Name = "initiallyLabel";
            initiallyLabel.Size = new Size(57, 20);
            initiallyLabel.TabIndex = 0;
            initiallyLabel.Text = "initially";
            // 
            // addStatementButton
            // 
            addStatementButton.Dock = DockStyle.Bottom;
            addStatementButton.Location = new Point(0, 266);
            addStatementButton.Margin = new Padding(3, 4, 3, 4);
            addStatementButton.Name = "addStatementButton";
            addStatementButton.Size = new Size(249, 31);
            addStatementButton.TabIndex = 6;
            addStatementButton.Text = "Add Statement";
            addStatementButton.UseVisualStyleBackColor = true;
            addStatementButton.Click += addStatementButton_Click;
            // 
            // statementsCheckedListBox
            // 
            statementsCheckedListBox.FormattingEnabled = true;
            statementsCheckedListBox.Location = new Point(3, 23);
            statementsCheckedListBox.Margin = new Padding(3, 4, 3, 4);
            statementsCheckedListBox.Name = "statementsCheckedListBox";
            statementsCheckedListBox.Size = new Size(419, 202);
            statementsCheckedListBox.TabIndex = 6;
            statementsCheckedListBox.KeyDown += statementsCheckedListBox_KeyDown;
            // 
            // panel2
            // 
            panel2.Controls.Add(statementsLabel);
            panel2.Controls.Add(removeStatementsButton);
            panel2.Controls.Add(statementsCheckedListBox);
            panel2.Location = new Point(353, 301);
            panel2.Margin = new Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(426, 297);
            panel2.TabIndex = 7;
            // 
            // statementsLabel
            // 
            statementsLabel.AutoSize = true;
            statementsLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            statementsLabel.Location = new Point(173, 0);
            statementsLabel.Name = "statementsLabel";
            statementsLabel.Size = new Size(93, 20);
            statementsLabel.TabIndex = 8;
            statementsLabel.Text = "Statements:";
            // 
            // removeStatementsButton
            // 
            removeStatementsButton.Dock = DockStyle.Bottom;
            removeStatementsButton.Location = new Point(0, 266);
            removeStatementsButton.Margin = new Padding(3, 4, 3, 4);
            removeStatementsButton.Name = "removeStatementsButton";
            removeStatementsButton.Size = new Size(426, 31);
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
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(896, 615);
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
            Margin = new Padding(3, 4, 3, 4);
            MaximumSize = new Size(1712, 1982);
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
            alwaysPanel.ResumeLayout(false);
            alwaysPanel.PerformLayout();
            impossiblePanel.ResumeLayout(false);
            impossiblePanel.PerformLayout();
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
        private TextBox initiallyTextBox;
    }
}
