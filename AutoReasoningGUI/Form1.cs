using Logic.States.Models;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.CompilerServices;

namespace AutoReasoningGUI
{
    public partial class Form1 : Form
    {
        private List<Fluent> _fluents = new List<Fluent>();
        private List<string> _actionNames = new List<string>();
        private List<string> _statements = new List<string>();
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
            foreach (var fluent in _fluents)
            {
                var fluentName = fluent.Name;
                var isInertialString = fluent.IsInertial ? "INERTIAL" : "NOT INERTIAL";
                this.fluentCheckedListBox.Items.Add($"{fluentName} {isInertialString}");
                this.initiallyFluentComboBox.Items.Add($"{fluentName}");
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
            string selectedItem = typeOfStatementComboBox.SelectedItem.ToString();
            switch (selectedItem)
            {
                case "":
                    return;
                case "Initial Fluent Value":
                    string fluentName = "";
                    if (initiallyFluentComboBox.SelectedItem == null)
                    {
                        return;
                    }
                    else
                    {
                        fluentName = initiallyFluentComboBox.SelectedItem.ToString();
                    }
                    bool startValue = InitialCheckBox.Checked;
                    string initialStatement = $"initially {(startValue ? "" : "not ")}{fluentName}";
                    if (!_statements.Contains(initialStatement))
                    {
                        _statements.Add(initialStatement);
                        UpdateStatementsList();
                    }
                    break;
                case "Always":
                    string expression = alwaysTextBox.Text.ToString();
                    expression = expression.TrimStart();
                    if (expression == "")
                    {
                        return;
                    }
                    // trzeba tutaj sprawdziæ czy expression jest poprawnie napisane
                    string alwaysStatement = $"always {expression}";
                    if (!_statements.Contains(alwaysStatement))
                    {
                        _statements.Add(alwaysStatement);
                        UpdateStatementsList();
                    }
                    break;
                case "Impossible Action":
                    string actionName = "";
                    if (impossibleActionComboBox.SelectedItem == null)
                    {
                        return;
                    }
                    else
                    {
                        actionName = impossibleActionComboBox.SelectedItem.ToString();
                    }
                    string impossibleExpression = impossibleTextBox.Text.ToString();
                    impossibleExpression = impossibleExpression.TrimStart();
                    if (impossibleExpression == "")
                    {
                        return;
                    }
                    // trzeba tutaj sprawdziæ czy expression jest poprawnie napisane
                    string impossibleStatement = $"impossible {actionName} if {impossibleExpression}";
                    if (!_statements.Contains(impossibleStatement))
                    {
                        _statements.Add(impossibleStatement);
                        UpdateStatementsList();
                    }
                    break;
                case "Action Causes":
                    string actionCausesName = "";
                    if (causesActionComboBox.SelectedItem == null)
                    {
                        return;
                    }
                    else
                    {
                        actionCausesName = causesActionComboBox.SelectedItem.ToString();
                    }
                    string causesExpression_1 = causesTextBox.Text.ToString();
                    causesExpression_1 = causesExpression_1.TrimStart();
                    string causesExpression_2 = causesTextBox2.Text.ToString();
                    causesExpression_2 = causesExpression_2.TrimStart();
                    if (causesExpression_1 == "")
                    {
                        return;
                    }
                    decimal costCauses = causesNumericUpDown.Value;
                    string causesStatement = "";
                    if (causesExpression_2 == "")
                    {
                        causesStatement = $"{actionCausesName} causes {causesExpression_1} costs {costCauses}";
                    } else
                    {
                        causesStatement = $"{actionCausesName} causes {causesExpression_1} if {causesExpression_2} costs {costCauses}";
                    }
                    if (!_statements.Contains(causesStatement))
                    {
                        _statements.Add(causesStatement);
                        UpdateStatementsList();
                    }
                    break;
                case "Action Releases":
                    string actionReleasesName = "";
                    if (releasesActionComboBox.SelectedItem == null)
                    {
                        return;
                    }
                    else
                    {
                        actionReleasesName = releasesActionComboBox.SelectedItem.ToString();
                    }
                    string releasesExpression_1 = releasesTextBox.Text.ToString();
                    releasesExpression_1 = releasesExpression_1.TrimStart();
                    string releasesExpression_2 = releasesTextBox2.Text.ToString();
                    releasesExpression_2 = releasesExpression_2.TrimStart();
                    if (releasesExpression_1 == "")
                    {
                        return;
                    }
                    decimal costReales = releasesNumericUpDown.Value;
                    string releasesStatement = "";
                    if (releasesExpression_2 == "")
                    {
                        releasesStatement = $"{actionReleasesName} releases {releasesExpression_2} costs {costReales}";
                    }
                    else
                    {
                        releasesStatement = $"{actionReleasesName} releases {releasesExpression_1} if {releasesExpression_2} costs {costReales}";
                    }
                    if (!_statements.Contains(releasesStatement))
                    {
                        _statements.Add(releasesStatement);
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
