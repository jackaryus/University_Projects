using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapEditor2.GUI.Forms
{
    public partial class NewMapForm : Form
    {

        public String MapName;
        public int MapWidth, MapHeight;

        public NewMapForm()
        {
            InitializeComponent();
        }

        private void NewMapForm_Load(object sender, EventArgs e)
        {

        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.MapName = this.MapNameField.Text;
            this.MapWidth = Convert.ToInt32(this.MapWidthField.Value);
            this.MapHeight = Convert.ToInt32(this.MapHeightField.Value);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
