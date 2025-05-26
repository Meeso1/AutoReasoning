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
            string expression = formulaTextBox.Text.Trim();

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
            _query = CreateQuery();
            if (_query == null)
            {
                queryResultValueLabel.Text = "";
                return;
            }
                
            bool result;

            // not implemented yet - try usage to avoid runtime exceptions
            try
            {
                result = form1.App.QueryEvaluator.Evaluate(_query);
            }
            catch (NotImplementedException)
            {
                result = false;
            }

            queryResultValueLabel.Text = result.ToString().ToUpper();
        }

        private Query CreateQuery()
        {
            var selectedType = (Type)queryClassComboBox.SelectedValue;
            var queryType = (QueryType)queryTypeComboBox.SelectedItem;

            if (form1.App.ActionProgram == null)
            {
                MessageBox.Show($"Cannot create query. Action program is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                queryResultValueLabel.Text = "";
                return null;
            }

            if (selectedType == typeof(ExecutableQuery))
            {
                return new ExecutableQuery(queryType, form1.App.ActionProgram);
            }
            else if (selectedType == typeof(AccessibleQuery))
            {
                if (_states == null)
                {
                    MessageBox.Show($"Cannot create Accessible query. Formula field above has invalid input.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    queryResultValueLabel.Text = "";
                    return null;
                }
                return new AccessibleQuery(queryType, form1.App.ActionProgram, _states);
            }
            else if (selectedType == typeof(AffordableQuery))
            {
                if (_budget == null)
                {
                    MessageBox.Show($"Cannot create Affordable query. Budget not specified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    queryResultValueLabel.Text = "";
                    return null;
                }
                return new AffordableQuery(queryType, form1.App.ActionProgram, (int)_budget);
            }
            else
            {
                queryResultValueLabel.Text = "";
                throw new InvalidOperationException("Query type not implemented.");
            }
        }
    }
}
