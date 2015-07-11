using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TaxCalculator
{
    public partial class import : Form
    {
        public import()
        {
            InitializeComponent();
           
            load();
        }

        private void load()
        {
            if(Form1.No_button==1)
            {
                pictureBox1.Image = Properties.Resources._2;
            }
            else if (Form1.No_button == 2)
            {
                pictureBox1.Image = Properties.Resources._1;
            }
            DataTable dt = Form1.dtname;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string temp=dt.Rows[i][0].ToString().Replace("$","");
                comboBox1.Items.Add(temp);
            }
            comboBox1.SelectedIndex = 0;
            button1.DialogResult = DialogResult.OK;
            button2.DialogResult = DialogResult.Cancel;
        }

        public string selectedT
        {
            get { return comboBox1.SelectedItem.ToString(); }
            set { comboBox1.SelectedItem = value; }
        }

    }
}
