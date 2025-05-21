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

namespace AutoReasoningGUI
{
    public partial class FormulaForm: Form
    {
        private Form2 form2;
        public FormulaForm(Form2 form2)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.form2 = form2;

            //// formula dropdown - to be potentially used later
            //var formulaDict = new Dictionary<string, Type>
            //{
            //    ["True"] = typeof(True),
            //    ["False"] = typeof(False),
            //    //["FluentIsSet"] = typeof(FluentIsSet),
            //    ["Not"] = typeof(Not),
            //    ["And"] = typeof(And),
            //    ["Or"] = typeof(Or),
            //    ["Implies"] = typeof(Implies),
            //    ["Is Equivalent To"] = typeof(Equivalent)
            //};
            //var bs = new BindingSource(formulaDict, null);
        }
    }
}
