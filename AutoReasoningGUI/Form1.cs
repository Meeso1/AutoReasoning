using Logic.States.Models;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.CompilerServices;

namespace AutoReasoningGUI
{
    public partial class Form1 : Form
    {
        private List<Fluent> _fluents = new List<Fluent>();
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
            //var isInertialString = isInertial ? "INERTIAL" : "NOT INERTIAL";
            if (!_fluents.Contains(newFluent) && fluentName != "")
            {
                _fluents.Add(newFluent);
                //this.fluentCheckedListBox.Items.Add($"{fluentName} {isInertialString}");
                this.fluentTextBox.Text = null;
                this.isInertialCheckBox.Checked = true;
                UpdateFluentList();
            }
        }

        private void UpdateFluentList()
        {
            this.fluentCheckedListBox.Items.Clear();
            foreach (var fluent in _fluents)
            {
                var fluentName = fluent.Name;
                var isInertialString = fluent.IsInertial ? "INERTIAL" : "NOT INERTIAL";
                this.fluentCheckedListBox.Items.Add($"{fluentName} {isInertialString}");
            }
        }

        private void addActionButton_Click(object sender, EventArgs e)
        {

        }

        private void contFluentsActions_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void numericUpDownAction_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void fluentCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

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
    }
}
