namespace AutoReasoningGUI
{
    public partial class Form1 : Form
    {
        private Form2 form2;
        public Form1()
        {
            InitializeComponent();
        }

        private void nextPage_Click(object sender, EventArgs e)
        {
            if (form2 == null)
            {
                form2 = new Form2(this);
                form2.Show();
            }
            // save input from form1 first
            this.Visible = false;
            form2.Visible = true;
        }

        private void addFluentButton_Click(object sender, EventArgs e)
        {

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
    }
}
