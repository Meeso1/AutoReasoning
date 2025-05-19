using Logic.States.Models;
using Logic.States;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.CompilerServices;
using Logic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AutoReasoningGUI
{
    public partial class Form1 : Form
    {
        public App App { get; } = new();
        private List<Fluent> _fluents = new List<Fluent>();
        private List<string> _actionNames = new List<string>();
        private List<string> _statements = new List<string>(); // TODO: rozbiæ to na kilka list
        private Form2 form2;
        public Form1()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            form2 = new Form2(this);
            form2.Show();
            form2.Visible = false;
        }

        private void nextPage_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            form2.Location = this.Location;
            form2.Size = this.Size;
            form2.Visible = true;
        }

        private void addFluentButton_Click(object sender, EventArgs e)
        {
            var fluentName = this.fluentTextBox.Text;
            fluentName = string.Concat(fluentName.Where(c => !char.IsWhiteSpace(c)));
            var isInertial = this.isInertialCheckBox.Checked;
            var newFluent = new Fluent(fluentName, isInertial);
            if (!_fluents.Contains(newFluent) && fluentName != "")
            {
                _fluents.Add(newFluent);
                this.fluentTextBox.Text = null;
                this.isInertialCheckBox.Checked = true;
                UpdateFluentList();
            }
        }

        private void UpdateFluentList()
        {
            this.fluentCheckedListBox.Items.Clear();
            this.initiallyFluentComboBox.Items.Clear();
            this.causesFluentComboBox.Items.Clear();
            this.releasesFluentComboBox.Items.Clear();
            foreach (var fluent in _fluents)
            {
                var fluentName = fluent.Name;
                var isInertialString = fluent.IsInertial ? "INERTIAL" : "NOT INERTIAL";
                this.fluentCheckedListBox.Items.Add($"{fluentName} {isInertialString}");
                this.initiallyFluentComboBox.Items.Add($"{fluentName}");
                this.causesFluentComboBox.Items.Add($"{fluentName}");
                this.releasesFluentComboBox.Items.Add($"{fluentName}");
            }
        }

        private void addActionButton_Click(object sender, EventArgs e)
        {
            var actionName = this.actionTextBox.Text;
            actionName = string.Concat(actionName.Where(c => !char.IsWhiteSpace(c)));
            if (!_actionNames.Contains(actionName) && actionName != "")
            {
                _actionNames.Add(actionName);
                this.actionTextBox.Text = null;
                UpdateActionList();
            }
        }

        private void UpdateActionList()
        {
            this.actionCheckedListBox.Items.Clear();
            this.impossibleActionComboBox.Items.Clear();
            this.causesActionComboBox.Items.Clear();
            this.releasesActionComboBox.Items.Clear();
            foreach (string action in _actionNames)
            {
                this.actionCheckedListBox.Items.Add(action);
                this.impossibleActionComboBox.Items.Add(action);
                this.causesActionComboBox.Items.Add(action);
                this.releasesActionComboBox.Items.Add(action);
            }
        }

        private void UpdateStatementsList()
        {
            this.statementsCheckedListBox.Items.Clear();
            foreach (string statement in _statements)
            {
                this.statementsCheckedListBox.Items.Add(statement);
            }
        }
        private void removeFluentsButton_Click(object sender, EventArgs e)
        {
            for (int i = fluentCheckedListBox.Items.Count - 1; i >= 0; i--)
            {
                if (fluentCheckedListBox.GetItemChecked(i))
                {
                    _fluents.RemoveAt(i);
                }
            }
            UpdateFluentList();
        }

        private void removeActionsButton_Click(object sender, EventArgs e)
        {
            for (int i = actionCheckedListBox.Items.Count - 1; i >= 0; i--)
            {
                if (actionCheckedListBox.GetItemChecked(i))
                {
                    _actionNames.RemoveAt(i);
                }
            }
            UpdateActionList();
        }

        private void typeOfStatementComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            initiallyPanel.Visible = false;
            alwaysPanel.Visible = false;
            impossiblePanel.Visible = false;
            causesPanel.Visible = false;
            releasesPanel.Visible = false;
            string selectedItem = typeOfStatementComboBox.SelectedItem.ToString();
            switch (selectedItem)
            {
                case "":
                    return;
                case "Initial Fluent Value":
                    initiallyPanel.Visible = true;
                    break;
                case "Always":
                    alwaysPanel.Visible = true;
                    break;
                case "Impossible Action":
                    impossiblePanel.Visible = true;
                    break;
                case "Action Causes":
                    causesPanel.Visible = true;
                    break;
                case "Action Releases":
                    releasesPanel.Visible = true;
                    break;
            }
        }

        private void addStatementButton_Click(object sender, EventArgs e)
        {
            this.errorProvider1.SetError(alwaysTextBox, "");
            this.errorProvider1.SetError(impossibleTextBox, "");
            this.errorProvider1.SetError(causesTextBox2, "");
            this.errorProvider1.SetError(releasesTextBox2, "");
            string selectedItem = typeOfStatementComboBox.SelectedItem.ToString();
            string fluentName = "";
            string actionName = "";
            string expression = "";
            string statement = "";
            decimal cost;
            IReadOnlyList<string>? errors = null;
            string combinedError = "";
            switch (selectedItem)
            {
                case "":
                    return;
                case "Initial Fluent Value":
                    if (initiallyFluentComboBox.SelectedItem == null)
                    {
                        return;
                    }
                    else
                    {
                        fluentName = initiallyFluentComboBox.SelectedItem.ToString();
                    }
                    bool startValue = InitialCheckBox.Checked;
                    statement = $"initially {(startValue ? "" : "not ")}{fluentName}";
                    if (!_statements.Contains(statement))
                    {
                        _statements.Add(statement);
                        UpdateStatementsList();
                    }
                    break;
                case "Always":
                    expression = alwaysTextBox.Text.ToString();
                    expression = expression.TrimStart();
                    if (expression == "")
                    {
                        return;
                    }
                    if(!App.FormulaParser.TryParse(
                            expression,
                            _fluents.ToDictionary(f => f.Name),
                            out _,
                            out errors))
                    {
                        combinedError = string.Join(Environment.NewLine, errors);
                        this.errorProvider1.SetError(alwaysTextBox, combinedError);
                        return;
                    }
                    statement = $"always {expression}";
                    if (!_statements.Contains(statement))
                    {
                        _statements.Add(statement);
                        UpdateStatementsList();
                    }
                    break;
                case "Impossible Action":
                    if (impossibleActionComboBox.SelectedItem == null)
                    {
                        return;
                    }
                    else
                    {
                        actionName = impossibleActionComboBox.SelectedItem.ToString();
                    }
                    expression = impossibleTextBox.Text.ToString();
                    expression = expression.TrimStart();
                    if (expression == "")
                    {
                        return;
                    }
                    if (!App.FormulaParser.TryParse(
                            expression,
                            _fluents.ToDictionary(f => f.Name),
                            out _,
                            out errors))
                    {
                        combinedError = string.Join(Environment.NewLine, errors);
                        this.errorProvider1.SetError(impossibleTextBox, combinedError);
                        return;
                    }
                    statement = $"impossible {actionName} if {expression}";
                    if (!_statements.Contains(statement))
                    {
                        _statements.Add(statement);
                        UpdateStatementsList();
                    }
                    break;
                case "Action Causes":
                    if (causesActionComboBox.SelectedItem == null || causesFluentComboBox.SelectedItem == null)
                    {
                        return;
                    }
                    actionName = causesActionComboBox.SelectedItem.ToString();
                    bool isTrue = causesCheckBox.Checked;
                    fluentName = causesFluentComboBox.SelectedItem.ToString();
                    expression = causesTextBox2.Text.ToString();
                    expression = expression.TrimStart();
                    cost = causesNumericUpDown.Value;
                    if (expression == "")
                    {
                        statement = $"{actionName} causes {(isTrue ? "" : "not ")}{fluentName} costs {cost}";
                    } else
                    {
                        if (!App.FormulaParser.TryParse(
                                expression,
                                _fluents.ToDictionary(f => f.Name),
                                out _,
                                out errors))
                        {
                            combinedError = string.Join(Environment.NewLine, errors);
                            this.errorProvider1.SetError(causesTextBox2, combinedError);
                            return;
                        }
                        statement = $"{actionName} causes {(isTrue ? "" : "not ")}{fluentName} if {expression} costs {cost}";
                    }
                    if (!_statements.Contains(statement))
                    {
                        _statements.Add(statement);
                        UpdateStatementsList();
                    }
                    break;
                case "Action Releases":
                    if (releasesActionComboBox.SelectedItem == null || releasesFluentComboBox.SelectedItem == null)
                    {
                        return;
                    }
                    actionName = releasesActionComboBox.SelectedItem.ToString();
                    fluentName = releasesFluentComboBox.SelectedItem.ToString();
                    expression = releasesTextBox2.Text.ToString();
                    expression = expression.TrimStart();
                    cost = releasesNumericUpDown.Value;
                    if (expression == "")
                    {
                        statement = $"{actionName} releases {fluentName} costs {cost}";
                    }
                    else
                    {
                        if (!App.FormulaParser.TryParse(
                                expression,
                                _fluents.ToDictionary(f => f.Name),
                                out _,
                                out errors))
                        {
                            combinedError = string.Join(Environment.NewLine, errors);
                            this.errorProvider1.SetError(releasesTextBox2, combinedError);
                            return;
                        }
                        statement = $"{actionName} releases {fluentName} if {expression} costs {cost}";
                    }
                    if (!_statements.Contains(statement))
                    {
                        _statements.Add(statement);
                        UpdateStatementsList();
                    }
                    break;
            }
        }

        private void removeStatementsButton_Click(object sender, EventArgs e)
        {
            for (int i = statementsCheckedListBox.Items.Count - 1; i >= 0; i--)
            {
                if (statementsCheckedListBox.GetItemChecked(i))
                {
                    _statements.RemoveAt(i);
                }
            }
            UpdateStatementsList();
        }
    }
}
