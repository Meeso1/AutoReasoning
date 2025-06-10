using Logic;
using Logic.Problem.Models;
using Logic.Queries.Models;
using Logic.States.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AutoReasoningGUI
{
    public partial class Form2 : Form
    {
        private Form1 form1;
        //private FormulaForm formulaForm;
        private StateGroup? _states;
        private int? _budget;
        private Query? _query;
        private ActionProgram? _actionProgram;
        public Form2(Form1 form1)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.form1 = form1;

            // show fluents list
            fluentListBox.DataSource = form1.Fluents;
            fluentListBox.SelectedIndex = -1;

            // show actions list
            actionListBox.DataSource = form1.ActionNames;
            actionListBox.SelectedIndex = -1;

            // show statements list
            statementListBox.DataSource = form1.Statements;
            statementListBox.SelectedIndex = -1;

            // query type selection
            queryTypeComboBox.DataSource = Enum.GetValues(typeof(QueryType));
            queryTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            // query class selection

            // assign handler to event
            queryClassComboBox.SelectedIndexChanged += QueryClassComboBox_SelectedIndexChanged;
            queryTypeComboBox.SelectedIndexChanged += QueryTypeComboBox_SelectedIndexChanged;

            queryClassComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            var dict = new Dictionary<string, Type>
            {
                ["Executable"] = typeof(ExecutableQuery),
                ["Accessible"] = typeof(AccessibleQuery),
                ["Affordable"] = typeof(AffordableQuery),
            };
            var bs = new BindingSource(dict, null);
            queryClassComboBox.DisplayMember = "Key";
            queryClassComboBox.ValueMember = "Value";
            queryClassComboBox.DataSource = bs;

            queryClassComboBox.SelectedIndex = 0;

            // invoke at start to initialize
            QueryClassComboBox_SelectedIndexChanged(queryClassComboBox, EventArgs.Empty);
            QueryTypeComboBox_SelectedIndexChanged(queryTypeComboBox, EventArgs.Empty);

        }

        private void prevPage_Click(object sender, EventArgs e)
        {
            queryResultValueLabel.Text = "";
            this.Visible = false;
            form1.Location = this.Location;
            form1.Size = this.Size;
            form1.Visible = true;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            form1.Close();
        }

        public void PrepareToShow()
        {
            fluentListBox.SelectedIndex = -1;
            actionListBox.SelectedIndex = -1;
            statementListBox.SelectedIndex = -1;
        }

        private void QueryClassComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            queryResultValueLabel.Text = "";
            var combo = (System.Windows.Forms.ComboBox)sender;
            var selectedType = (Type)combo.SelectedValue;

            var isBudget = selectedType == typeof(AffordableQuery);
            budgetNumericUpDown.Enabled = isBudget;
            budgetLabel.Enabled = isBudget;
            if (isBudget)
                _budget = (int)budgetNumericUpDown.Value;

            var isAccessible = selectedType == typeof(AccessibleQuery);
            createFormulaButton.Enabled = isAccessible;
            formulaTextBox.Enabled = isAccessible;
            formulaLabel.Enabled = isAccessible;
            formulaValidationLabel.Visible = false;
        }

        private void QueryTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            queryResultValueLabel.Text = "";
        }

        private void createFormulaButton_Click(object sender, EventArgs e)
        {
            queryResultValueLabel.Text = "";
            //formulaForm = new FormulaForm(this);
            //formulaForm.Show();
            this.formulaErrorProvider.SetError(formulaTextBox, "");
            Formula? parsedFormula;
            IReadOnlyList<string>? errors = null;
            string expression = formulaTextBox.Text.Trim().ToLower();

            if (!form1.App.FormulaParser.TryParse(
                            expression,
                            form1.Fluents.ToDictionary(f => f.Name),
                            out parsedFormula,
                            out errors))
            {
                string combinedError = string.Join(Environment.NewLine, errors);
                this.formulaErrorProvider.SetError(formulaTextBox, combinedError);
                formulaValidationLabel.Text = "INVALID";
                formulaValidationLabel.ForeColor = Color.Red;
                formulaValidationLabel.Visible = true;
                return;
            }

            formulaValidationLabel.Text = "VALID";
            formulaValidationLabel.ForeColor = Color.Green;
            formulaValidationLabel.Visible = true;

            _states = form1.App.FormulaReducer.Reduce(parsedFormula);
        }

        private void executeQueryButton_Click(object sender, EventArgs e)
        {
            // validate formula expression input in accessible query case
            var queryClass = (Type)queryClassComboBox.SelectedValue;

            if (queryClass == typeof(AccessibleQuery))
                createFormulaButton_Click(sender, e);

            if (queryClass == typeof(AffordableQuery))
            {
                _budget = (int)budgetNumericUpDown.Value;
            }


            _actionProgram = CreateActionProgram();
            _query = CreateQuery();
            if (_query == null)
            {
                queryResultValueLabel.Text = "";
                return;
            }

            QueryResult? result;
            EvaluateQueryResult evalResult;
            // not implemented yet - try usage to avoid runtime exceptions
            try
            {
                evalResult = form1.App.EvaluateQuery(_query);
                if (evalResult.IsValid)
                {
                    result = evalResult.Success;
                }
                else
                {
                    MessageBox.Show($"{evalResult.Errors}", "Error creating model",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (NotImplementedException)
            {
                evalResult = new(null, false, ["Not implemented."]);
            }

            queryResultValueLabel.Text = evalResult.Success switch
            {
                null => "NOT IMPLEMENTED",
                QueryResult.Consequence => "TRUE",
                QueryResult.NotConsequence => "FALSE",
                QueryResult.Unconsistent => "DOMAIN IS INCONSISTENT",
                _ => "NOT IMPLEMENTED QUERY RESULT (?)"
            };

            _states = null;
        }

        private Query CreateQuery()
        {
            var selectedType = (Type)queryClassComboBox.SelectedValue;
            var queryType = (QueryType)queryTypeComboBox.SelectedItem;

            if (_actionProgram == null)
            {
                MessageBox.Show($"Cannot create action program. Some actions do not have at least 1 statement specified.",
                    "Error creating action program", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                queryResultValueLabel.Text = "";
                return null;
            }

            if (selectedType == typeof(ExecutableQuery))
            {
                return new ExecutableQuery(queryType, _actionProgram);
            }
            else if (selectedType == typeof(AccessibleQuery))
            {
                if (_states == null)
                {
                    MessageBox.Show($"Cannot create Accessible query. Formula field above has invalid input.",
                        "Error creating query", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    queryResultValueLabel.Text = "";
                    return null;
                }
                return new AccessibleQuery(queryType, _actionProgram, _states);
            }
            else if (selectedType == typeof(AffordableQuery))
            {
                if (_budget == null)
                {
                    MessageBox.Show($"Cannot create Affordable query. Budget not specified.",
                        "Error creating query", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    queryResultValueLabel.Text = "";
                    return null;
                }
                return new AffordableQuery(queryType, _actionProgram, (uint)_budget);
            }
            else
            {
                queryResultValueLabel.Text = "";
                throw new InvalidOperationException("Query type not implemented.");
            }
        }

        private void addActionToProgramButton_Click(object sender, EventArgs e)
        {
            int index = actionListBox.SelectedIndex;
            if (index != ListBox.NoMatches)
            {
                var item = actionListBox.Items[index];
                actionProgramCheckedListBox.Items.Add(item);
            }

            queryResultValueLabel.Text = "";
        }

        private void removeActionFromProgramButton_Click(object sender, EventArgs e)
        {
            List<int> checkedIndices = new();

            foreach (int index in actionProgramCheckedListBox.CheckedIndices)
            {
                checkedIndices.Add(index);
            }

            checkedIndices.Reverse();
            foreach (int index in checkedIndices)
            {
                actionProgramCheckedListBox.Items.RemoveAt(index);
            }

            queryResultValueLabel.Text = "";
        }

        private ActionProgram CreateActionProgram()
        {
            var actionsDict = form1.App.ProblemDependent.Problem.Actions;

            var orderedActions = new List<Logic.Problem.Models.Action>();

            foreach (string name in actionProgramCheckedListBox.Items)
            {
                if (actionsDict.TryGetValue(name, out var action))
                {
                    orderedActions.Add(action);
                }
                else
                {
                    return null;
                }
            }

            return new ActionProgram(orderedActions);
        }

        private void actionListBox_MouseDown(object sender, MouseEventArgs e)
        {
            int index = actionListBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                var item = actionListBox.Items[index];
                DoDragDrop(item, DragDropEffects.Copy);
            }
        }

        private void actionProgramCheckedListBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(string)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void actionProgramCheckedListBox_DragDrop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(string)) as string;
            if (!string.IsNullOrEmpty(data))
            {
                actionProgramCheckedListBox.Items.Add(data);
            }
        }

        private void selectAllActionsFromProgramButton_Click(object sender, EventArgs e)
        {
            actionProgramCheckedListBox.Focus();

            if (actionProgramCheckedListBox.Items.Count == 0) return;

            bool allChecked = true;

            for (int i = 0; i < actionProgramCheckedListBox.Items.Count; i++)
            {
                if (!actionProgramCheckedListBox.GetItemChecked(i))
                {
                    allChecked = false;
                    break;
                }
            }

            for (int i = 0; i < actionProgramCheckedListBox.Items.Count; i++)
            {
                actionProgramCheckedListBox.SetItemChecked(i, !allChecked);
            }
        }

        private void actionListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                addActionToProgramButton_Click(sender, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void actionProgramCheckedListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                removeActionFromProgramButton_Click(sender, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        private void budgetNumericUpDown_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(budgetNumericUpDown.Text))
            {
                budgetNumericUpDown.Value = budgetNumericUpDown.Minimum;
            }
        }
    }
}
