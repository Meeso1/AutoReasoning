using Logic;
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
        private FormulaForm formulaForm;
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
        }

        private void prevPage_Click(object sender, EventArgs e)
        {
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
            var combo = (System.Windows.Forms.ComboBox)sender;
            var selectedType = (Type)combo.SelectedValue;

            var isBudget = selectedType == typeof(AffordableQuery);
            budgetNumericUpDown.Enabled = isBudget;
            budgetLabel.Enabled = isBudget;

            var isAccessible = selectedType == typeof(AccessibleQuery);
            createFormulaButton.Enabled = isAccessible;
            formulaTextBox.Enabled = isAccessible;
            formulaLabel.Enabled = isAccessible;
            formulaValidationLabel.Visible = false;
        }

        private void createFormulaButton_Click(object sender, EventArgs e)
        {
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
        }

        private void executeQueryButton_Click(object sender, EventArgs e)
        {

        }
    }
}
