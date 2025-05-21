using Logic.States.Models;
using Logic.States;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.CompilerServices;
using Logic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Logic.Problem.Models;

namespace AutoReasoningGUI
{
    public partial class Form1 : Form // TODO: Zrobiæ usuwanie wybranych statement
    {
        public App App { get; } = new();
        private List<Fluent> _fluents = new List<Fluent>();
        private List<string> _actionNames = new List<string>();
        private List<string> _statements = new List<string>();
        private List<Formula> _alwaysStatements = new List<Formula>();
        private Dictionary<Fluent, bool> _initialFluents = new Dictionary<Fluent, bool>();
        private List<ActionStatement> _actionStatements = new List<ActionStatement>();
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
            Formula? parsedFormula;
            Formula formula;
            Fluent fluent;
            ActionStatement actionStatement;
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
                    string oppositeStatement = $"initially {(startValue ? "not " : "")}{fluentName}";
                    if (!_statements.Contains(statement))
                    {
                        fluent = _fluents.FirstOrDefault(f => f.Name == fluentName);
                        if (!_statements.Contains(oppositeStatement))
                        {
                            _statements.Add(statement);
                            _initialFluents.Add(fluent, startValue);
                        } else
                        {
                            _statements.Remove(oppositeStatement);
                            _initialFluents.Remove(fluent);
                            _statements.Add(statement);
                            _initialFluents.Add(fluent, startValue);
                        }
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
                            out parsedFormula,
                            out errors))
                    {
                        combinedError = string.Join(Environment.NewLine, errors);
                        this.errorProvider1.SetError(alwaysTextBox, combinedError);
                        return;
                    }
                    formula = parsedFormula;
                    statement = $"always {expression}";
                    if (!_statements.Contains(statement))
                    {
                        _statements.Add(statement);
                        _alwaysStatements.Add(formula);
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
                            out parsedFormula,
                            out errors))
                    {
                        combinedError = string.Join(Environment.NewLine, errors);
                        this.errorProvider1.SetError(impossibleTextBox, combinedError);
                        return;
                    }
                    formula = parsedFormula;
                    statement = $"impossible {actionName} if {expression}";
                    if (!_statements.Contains(statement))
                    {
                        _statements.Add(statement);
                        ActionCondition actionImpossible = new ActionCondition(formula);
                        actionStatement = new ActionStatement(actionName, actionImpossible);
                        _actionStatements.Add(actionStatement);
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
                        formula = new True();
                    } else
                    {
                        if (!App.FormulaParser.TryParse(
                                expression,
                                _fluents.ToDictionary(f => f.Name),
                                out parsedFormula,
                                out errors))
                        {
                            combinedError = string.Join(Environment.NewLine, errors);
                            this.errorProvider1.SetError(causesTextBox2, combinedError);
                            return;
                        }
                        formula = parsedFormula;
                        statement = $"{actionName} causes {(isTrue ? "" : "not ")}{fluentName} if {expression} costs {cost}";
                    }
                    if (!_statements.Contains(statement))
                    {
                        _statements.Add(statement);
                        fluent = _fluents.FirstOrDefault(f => f.Name == fluentName);
                        ActionEffect actionCauses = new ActionEffect(formula, fluent, isTrue, (int)cost);
                        actionStatement = new ActionStatement(actionName, actionCauses);
                        _actionStatements.Add(actionStatement);
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
                        formula = new True();
                    }
                    else
                    {
                        if (!App.FormulaParser.TryParse(
                                expression,
                                _fluents.ToDictionary(f => f.Name),
                                out parsedFormula,
                                out errors))
                        {
                            combinedError = string.Join(Environment.NewLine, errors);
                            this.errorProvider1.SetError(releasesTextBox2, combinedError);
                            return;
                        }
                        formula = parsedFormula;
                        statement = $"{actionName} releases {fluentName} if {expression} costs {cost}";
                    }
                    if (!_statements.Contains(statement))
                    {
                        _statements.Add(statement);
                        fluent = _fluents.FirstOrDefault(f => f.Name == fluentName);
                        ActionRelease actionRelease = new ActionRelease(formula, fluent, (int)cost);
                        actionStatement = new ActionStatement(actionName, actionRelease);
                        _actionStatements.Add(actionStatement);
                        UpdateStatementsList();
                    }
                    break;
            }
        }

        private void removeStatementsButton_Click(object sender, EventArgs e)
        {
            _statements.Clear();
            _actionStatements.Clear();
            _initialFluents.Clear();
            _alwaysStatements.Clear();
            UpdateStatementsList();
        }
    }
}
