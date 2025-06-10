using Logic.States.Models;
using Logic.States;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.CompilerServices;
using Logic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Logic.Problem.Models;
using System.ComponentModel;
using System.Xml.Linq;

namespace AutoReasoningGUI
{
    public partial class Form1 : Form
    {
        public sealed class DisplayItem
        {
            public object Value { get; }
            public string DisplayText { get; }

            public DisplayItem(object value, string displayText)
            {
                Value = value;
                DisplayText = displayText;
            }

            public override string ToString()
            {
                return DisplayText;
            }

            public override bool Equals(object obj)
            {
                if (obj is DisplayItem other)
                {
                    return string.Equals(this.DisplayText, other.DisplayText, StringComparison.Ordinal);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return DisplayText?.GetHashCode() ?? 0;
            }
        }

        public App App { get; } = new();
        public BindingList<Fluent> Fluents = new();
        public BindingList<string> ActionNames = new();
        public BindingList<DisplayItem> Statements = new();
        private List<Formula> _alwaysStatements = new List<Formula>();
        private List<ValueStatement> _valueStatements = new List<ValueStatement>();
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
            form2.PrepareToShow();
            form2.Visible = true;

            App.SetModel(Fluents.ToDictionary(f => f.Name), _actionStatements, _valueStatements, _alwaysStatements);
        }

        private void addFluentButton_Click(object sender, EventArgs e)
        {
            var fluentName = this.fluentTextBox.Text.ToLower();
            fluentName = string.Concat(fluentName.Where(c => !char.IsWhiteSpace(c)));
            var isInertial = this.isInertialCheckBox.Checked;
            var newFluent = new Fluent(fluentName, isInertial);
            if (!Fluents.Contains(newFluent) && fluentName != "" && !Fluents.Any(f => f.Name == newFluent.Name))
            {
                Fluents.Add(newFluent);
                this.fluentTextBox.Text = null;
                this.isInertialCheckBox.Checked = true;
                UpdateFluentList();
            }
        }

        private void UpdateFluentList()
        {
            this.fluentCheckedListBox.Items.Clear();
            this.releasesFluentComboBox.Items.Clear();
            foreach (var fluent in Fluents)
            {
                var fluentName = fluent.Name;
                var isInertialString = fluent.IsInertial ? "INERTIAL" : "NOT INERTIAL";
                this.fluentCheckedListBox.Items.Add($"{fluentName} {isInertialString}");
                if (fluent.IsInertial == true)
                {
                    this.releasesFluentComboBox.Items.Add($"{fluentName}");
                }
            }
        }

        private void addActionButton_Click(object sender, EventArgs e)
        {
            var actionName = this.actionTextBox.Text.ToUpper();
            actionName = string.Concat(actionName.Where(c => !char.IsWhiteSpace(c)));
            if (!ActionNames.Contains(actionName) && actionName != "")
            {
                ActionNames.Add(actionName);
                this.actionTextBox.Text = null;
                UpdateActionList();
            }
        }

        private void UpdateActionList() // TODO:
        {
            this.actionCheckedListBox.Items.Clear();
            this.impossibleActionComboBox.Items.Clear();
            this.causesActionComboBox.Items.Clear();
            this.releasesActionComboBox.Items.Clear();
            foreach (string action in ActionNames)
            {
                this.actionCheckedListBox.Items.Add(action);
                this.impossibleActionComboBox.Items.Add(action);
                this.causesActionComboBox.Items.Add(action);
                this.releasesActionComboBox.Items.Add(action);
            }
        }

        private void UpdateStatementsList()
        {
            _valueStatements.Clear();
            _alwaysStatements.Clear();
            _actionStatements.Clear();
            statementsCheckedListBox.Items.Clear();
            foreach (DisplayItem statement in Statements)
            {
                statementsCheckedListBox.Items.Add(statement);
                object statementType = statement.Value;
                string statementText = statement.DisplayText;
                statementText = statementText.Trim();
                string firstWord = statementText.Split(' ')[0];
                if (statementType is Formula initiallyStatement && firstWord == "initially")
                {
                    _valueStatements.Add(new AfterStatement(new ActionProgram([]), initiallyStatement));
                }
                else if (statementType is ValueStatement valueStatement)
                {
                    _valueStatements.Add(valueStatement);
                }
                else if (statementType is Formula alwaysStatement && firstWord == "always")
                {
                    _alwaysStatements.Add(alwaysStatement);
                }
                else if (statementType is ActionStatement actionStatement)
                {
                    _actionStatements.Add(actionStatement);
                }
            }
        }

        private void removeFluentsButton_Click(object sender, EventArgs e)
        {
            for (int i = fluentCheckedListBox.Items.Count - 1; i >= 0; i--)
            {
                if (fluentCheckedListBox.GetItemChecked(i))
                {
                    string fluentName = Fluents.ElementAt(i).Name;
                    Fluents.RemoveAt(i);
                    for (int j = Statements.Count - 1; j >= 0; j--)
                    {
                        string statement = Statements[j].DisplayText;
                        if (statement.Contains(" " + fluentName) || statement.Contains(fluentName + " "))
                        {
                            Statements.RemoveAt(j);
                        }
                    }
                }
            }
            UpdateFluentList();
            UpdateStatementsList();
        }

        private void removeActionsButton_Click(object sender, EventArgs e) // TODO:
        {
            for (int i = actionCheckedListBox.Items.Count - 1; i >= 0; i--)
            {
                if (actionCheckedListBox.GetItemChecked(i))
                {
                    string actionName = ActionNames.ElementAt(i);
                    ActionNames.RemoveAt(i);
                    for (int j = Statements.Count - 1; j >= 0; j--)
                    {
                        if (Statements[j].Value is ActionStatement actionStatement)
                        {
                            if (actionStatement.ActionName == actionName)
                            {
                                Statements.RemoveAt(j);
                            }
                        }
                    }
                }
            }
            UpdateActionList();
            UpdateStatementsList();
        }

        private void typeOfStatementComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            initiallyPanel.Visible = false;
            afterPanel.Visible = false;
            observablePanel.Visible = false;
            alwaysPanel.Visible = false;
            impossiblePanel.Visible = false;
            causesPanel.Visible = false;
            releasesPanel.Visible = false;
            string selectedItem = typeOfStatementComboBox.SelectedItem.ToString();
            switch (selectedItem)
            {
                case "":
                    return;
                case "Initially":
                    initiallyPanel.Visible = true;
                    break;
                case "After":
                    afterPanel.Visible = true;
                    break;
                case "Observable":
                    observablePanel.Visible = true;
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

        private void addStatementButton_Click(object sender, EventArgs e) // TODO
        {
            if (typeOfStatementComboBox.SelectedItem == null) return;

            this.errorProvider1.SetError(initiallyTextBox, "");
            this.errorProvider1.SetError(afterTextBox, "");
            this.errorProvider1.SetError(observableTextBox, "");
            this.errorProvider1.SetError(alwaysTextBox, "");
            this.errorProvider1.SetError(impossibleTextBox, "");
            this.errorProvider1.SetError(causesTextBox1, "");
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
            DisplayItem displayItem;
            IReadOnlyList<string>? errors = null;
            string combinedError = "";
            switch (selectedItem)
            {
                case "":
                    return;
                case "Initially":
                    expression = initiallyTextBox.Text.ToString();
                    expression = expression.Trim().ToLower();
                    if (expression == "")
                    {
                        return;
                    }
                    if (!App.FormulaParser.TryParse(
                            expression,
                            Fluents.ToDictionary(f => f.Name),
                            out parsedFormula,
                            out errors))
                    {
                        combinedError = string.Join(Environment.NewLine, errors);
                        this.errorProvider1.SetError(initiallyTextBox, combinedError);
                        return;
                    }
                    formula = parsedFormula;
                    statement = $"initially {expression}";
                    displayItem = new DisplayItem(formula, statement);
                    if (!Statements.Contains(displayItem))
                    {
                        Statements.Add(displayItem);
                        UpdateStatementsList();
                        initiallyTextBox.Clear();
                    }
                    break;
                case "After":

                case "Observable":

                case "Always":
                    expression = alwaysTextBox.Text.ToString();
                    expression = expression.Trim().ToLower();
                    if (expression == "")
                    {
                        return;
                    }
                    if (!App.FormulaParser.TryParse(
                            expression,
                            Fluents.ToDictionary(f => f.Name),
                            out parsedFormula,
                            out errors))
                    {
                        combinedError = string.Join(Environment.NewLine, errors);
                        this.errorProvider1.SetError(alwaysTextBox, combinedError);
                        return;
                    }
                    formula = parsedFormula;
                    statement = $"always {expression}";
                    displayItem = new DisplayItem(formula, statement);
                    if (!Statements.Contains(displayItem))
                    {
                        Statements.Add(displayItem);
                        UpdateStatementsList();
                        alwaysTextBox.Clear();
                    }
                    break;
                case "Impossible Action":
                    if (impossibleActionComboBox.SelectedItem == null)
                    {
                        return;
                    }
                    actionName = impossibleActionComboBox.SelectedItem.ToString();
                    expression = impossibleTextBox.Text.ToString();
                    expression = expression.Trim().ToLower();
                    if (expression == "")
                    {
                        return;
                    }
                    if (!App.FormulaParser.TryParse(
                            expression,
                            Fluents.ToDictionary(f => f.Name),
                            out parsedFormula,
                            out errors))
                    {
                        combinedError = string.Join(Environment.NewLine, errors);
                        this.errorProvider1.SetError(impossibleTextBox, combinedError);
                        return;
                    }
                    formula = parsedFormula;
                    statement = $"impossible {actionName} if {expression}";
                    ActionCondition actionImpossible = new ActionCondition(formula);
                    actionStatement = new ActionStatement(actionName, actionImpossible);
                    displayItem = new DisplayItem(actionStatement, statement);
                    if (!Statements.Contains(displayItem))
                    {
                        Statements.Add(displayItem);
                        UpdateStatementsList();
                        impossibleTextBox.Clear();
                    }
                    break;
                case "Action Causes":
                    if (causesActionComboBox.SelectedItem == null)
                    {
                        return;
                    }
                    string causesExpression = causesTextBox1.Text.ToString();
                    causesExpression = causesExpression.Trim().ToLower();
                    if (causesExpression == "")
                    {
                        return;
                    }
                    if (!App.FormulaParser.TryParse(
                                causesExpression,
                                Fluents.ToDictionary(f => f.Name),
                                out parsedFormula,
                                out errors))
                    {
                        combinedError = string.Join(Environment.NewLine, errors);
                        this.errorProvider1.SetError(causesTextBox1, combinedError);
                        return;
                    }
                    Formula causesFormula = parsedFormula;
                    actionName = causesActionComboBox.SelectedItem.ToString();
                    expression = causesTextBox2.Text.ToString();
                    expression = expression.Trim().ToLower();
                    causesNumericUpDown_ValueChanged(sender, e);
                    cost = causesNumericUpDown.Value;
                    if (expression == "")
                    {
                        statement = $"{actionName} causes {causesExpression} costs {cost}";
                        formula = new True();
                    }
                    else
                    {
                        if (!App.FormulaParser.TryParse(
                                expression,
                                Fluents.ToDictionary(f => f.Name),
                                out parsedFormula,
                                out errors))
                        {
                            combinedError = string.Join(Environment.NewLine, errors);
                            this.errorProvider1.SetError(causesTextBox2, combinedError);
                            return;
                        }
                        formula = parsedFormula;
                        statement = $"{actionName} causes {causesExpression} if {expression} costs {cost}";
                    }
                    ActionEffect actionCauses = new ActionEffect(formula, causesFormula, (int)cost);
                    actionStatement = new ActionStatement(actionName, actionCauses);
                    displayItem = new DisplayItem(actionStatement, statement);
                    if (!Statements.Contains(displayItem))
                    {
                        Statements.Add(displayItem);
                        UpdateStatementsList();
                        causesTextBox1.Clear();
                        causesTextBox2.Clear();
                        
                        causesNumericUpDown.Value = 1;
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
                    expression = expression.Trim().ToLower();
                    releasesNumericUpDown_ValueChanged(sender, e);
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
                                Fluents.ToDictionary(f => f.Name),
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
                    fluent = Fluents.FirstOrDefault(f => f.Name == fluentName);
                    ActionRelease actionRelease = new ActionRelease(formula, fluent, (int)cost);
                    actionStatement = new ActionStatement(actionName, actionRelease);
                    displayItem = new DisplayItem(actionStatement, statement);
                    if (!Statements.Contains(displayItem))
                    {
                        Statements.Add(displayItem);
                        UpdateStatementsList();
                        releasesTextBox2.Clear();
                        releasesNumericUpDown.Value = 1;
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
                    Statements.RemoveAt(i);
                }
            }
            UpdateStatementsList();
        }

        private void fluentCheckedListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                removeFluentsButton_Click(sender, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void actionCheckedListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                removeActionsButton_Click(sender, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void statementsCheckedListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                removeStatementsButton_Click(sender, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void releasesNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(releasesNumericUpDown.Text))
            {
                releasesNumericUpDown.Value = releasesNumericUpDown.Minimum;
            }
        }

        private void causesNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(causesNumericUpDown.Text))
            {
                causesNumericUpDown.Value = causesNumericUpDown.Minimum;
            }
        }
    }
}
