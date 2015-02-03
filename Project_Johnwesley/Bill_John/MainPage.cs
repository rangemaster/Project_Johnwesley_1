using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bill_John
{
    public partial class MainPage : Form
    {
        private static string DataLocation = "Data";
        private static string MapYears = "Years";
        private static string DataYearFileName = "Years.xls";
        private static string DataMonthFileName = "Months.xls";
        public MainPage()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            LoadYears();
            LoadMonthsSelectedYear();
            this.Feedback_lb.ForeColor = Color.Red;
            this.Feedback_lb.Text = "Make your choice";
            this.Feedback_lb.TextAlign = ContentAlignment.MiddleCenter;
            DefaultEditValues();
        }
        private void DefaultEditValues()
        {
            HideEditYearPanel();
            HideEditMonthPanel();
            this.Edit_Year_Feedback_lb.Text = "Make your changes";
            this.Edit_Month_Feedback_tx.Text = "Make your changes";
        }
        #region Loading
        private void LoadYears()
        {
            if (!Directory.Exists(DataLocation))
                Directory.CreateDirectory(DataLocation);
            if (!File.Exists(DataLocation + "/" + MapYears))
            {
                Directory.CreateDirectory(DataLocation + "/" + MapYears);
                StreamWriter writer = new StreamWriter(DataLocation + "/" + MapYears + "/" + DataYearFileName);
                writer.WriteLine("2015");
                writer.Close();
            }
            StreamReader reader = new StreamReader(DataLocation + "/" + MapYears + "/" + DataYearFileName);
            while (reader.Peek() >= 0)
                this.Years_bx.Items.Add(reader.ReadLine());
            this.Years_bx.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void LoadMonthsSelectedYear()
        {
            int index = Years_bx.SelectedIndex;
            string selectedYear = Years_bx.Items[index].ToString();
            if (!File.Exists(DataMonthFileName))
            {
                StreamWriter writer = new StreamWriter(DataMonthFileName);
                writer.WriteLine("Januari");
                writer.Close();
            }
            StreamReader reader = new StreamReader(DataMonthFileName);
            this.Months_bx.Items.Add("Januari");
            this.Months_bx.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        #endregion

        #region Buttons
        #region Year
        private void Add_Year_bn_Click(object sender, EventArgs e)
        { }
        private void Edit_Year_bn_Click(object sender, EventArgs e)
        { EditSelectedYear(); }
        private void Remove_Year_bn_Click(object sender, EventArgs e)
        { }
        private void Close_Year_bn_Click(object sender, EventArgs e)
        { HideEditYearPanel(); }
        private void Save_Year_bn_Click(object sender, EventArgs e)
        { SaveEditYear(); }
        #endregion
        #region Month
        private void Add_Month_bn_Click(object sender, EventArgs e)
        { }
        private void Edit_Month_bn_Click(object sender, EventArgs e)
        { EditSelectedMonth(); }
        private void Remove_Month_bn_Click(object sender, EventArgs e)
        { }
        private void Close_Month_bn_Click(object sender, EventArgs e)
        { DefaultEditValues(); }
        private void Save_Month_bn_Click(object sender, EventArgs e)
        { SaveEditMonth(); }
        #endregion
        private void Open_bn_Click(object sender, EventArgs e)
        { }
        #endregion
        #region Functions
        private void EditSelectedYear()
        {
            if (Edit(Years_bx, Edit_Year_tx))
            { ShowEditYearPanel(); }
        }
        private void EditSelectedMonth()
        {
            if (Edit(Months_bx, Edit_Month_tx))
            { ShowEditMonthPanel(); }
        }
        private void SaveEditYear()
        {
            int index = Years_bx.SelectedIndex;
            if (index != -1)
                ComboboxSet(Years_bx, index, Edit_Year_tx.Text);
            else
                Debug.WriteLine("Save Edit Year: Index = -1 (" + index + ")");
            HideEditYearPanel();
        }
        private void SaveEditMonth()
        {
            int index = Months_bx.SelectedIndex;
            ComboboxSet(Months_bx, index, Edit_Month_tx.Text);
            HideEditMonthPanel();
        }
        private bool Edit(ComboBox cbx, TextBox tbx)
        {
            int index = cbx.SelectedIndex;
            if (index != -1)
            { tbx.Text = cbx.Items[index].ToString(); }
            else
            { Debug.WriteLine("Edit: Index = -1 (" + index + ")"); return false; }
            return true;
        }
        private void ComboboxSet(ComboBox box, int index, object obj)
        {
            box.Items.RemoveAt(index);
            box.Items.Insert(index, obj);
            box.Update();
        }
        private void ShowEditYearPanel()
        {
            this.Year_Panel.Visible = true;
            HideEditMonthPanel();
        }
        private void HideEditYearPanel()
        { this.Year_Panel.Visible = false; }
        private void ShowEditMonthPanel()
        {
            this.Month_Panel.Visible = true;
            HideEditYearPanel();
        }
        private void HideEditMonthPanel()
        { this.Month_Panel.Visible = false; }
        #endregion
    }
}
