using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiteDB;
using CashFlow.Models;

namespace CashFlow
{
    public partial class mainScrn : Form
    {
        public mainScrn()
        {
            InitializeComponent();
            //customize theme
            // ApllyGridTheme(bunifuDataGridView1);

            //set the chart theming
            bunifuDatavizAdvanced1.colorSet.Add(col1.BackColor);
            bunifuDatavizAdvanced1.colorSet.Add(col2.BackColor);
            bunifuDatavizAdvanced1.colorSet.Add(col3.BackColor);

            //TxtCategory.Clear();
            foreach (var item in DbContext.GetInstance().GetCollection<IncomeCategory>("income_categories").FindAll())
                GridMIncome.Rows[GridMIncome.Rows.Add(item.Name)].Tag= item;

            foreach (var item in DbContext.GetInstance().GetCollection<IncomeCategory>("expense_categories").FindAll())
                GridMExpenses.Rows[GridMExpenses.Rows.Add(item.Name)].Tag = item;

            foreach (var item in DbContext.GetInstance().GetCollection<IncomeCategory>("accounts").FindAll())
                GridMAccounts.Rows[GridMAccounts.Rows.Add(item.Name)].Tag = item;

            ReloadIncome();
            ReloadExpenses();
        }

        void ReloadIncome()
        {
            GridIncome.Rows.Clear();

             var incomeTransactions = DbContext.GetInstance().GetCollection<IncomeTransaction>("income_transactions")
                .Include(x => x.Account)
                .Include(x => x.Category)
                .FindAll();

            //Filter the Search
            incomeTransactions = incomeTransactions.Where(r => r.PaymentFrom.ToLower().Contains(TxtSearch.Text.ToLower())
            || r.Desciption.ToLower().Contains(TxtSearch.Text.ToLower())
            || r.TransactionCode.ToLower().Contains(TxtSearch.Text.ToLower())
            );


            //Filter Durations
            incomeTransactions = incomeTransactions.Where(r => r.Date.Year == inputDate.Value.Year && r.Date.Month == inputDate.Value.Month);
            if(InputDuration.SelectedIndex==0) 
                incomeTransactions = incomeTransactions.Where(r => r.Date.Day == inputDate.Value.Day);

            //Filter Categories
            if (InputCategory.SelectedIndex > 0)
                incomeTransactions = incomeTransactions.Where(r => r.Category.Id == DbContext.GetInstance().GetCollection<IncomeCategory>("income_categories").FindAll().ToList()[InputCategory.SelectedIndex-1].Id);

            //Filter Account
            if (InputAccount.SelectedIndex > 0)
                incomeTransactions = incomeTransactions.Where(r => r.Account.Id == DbContext.GetInstance().GetCollection<Account>("accounts").FindAll().ToList()[InputAccount.SelectedIndex-1].Id);

            LblTotalInc.Text = "Total:     "+ incomeTransactions.Sum(r => r.Amount).ToString();
            IncomeNo.Text = incomeTransactions.Sum(r => r.Amount).ToString();


            foreach (var item in incomeTransactions)
            {
                GridIncome.Rows.Add(new object[]
                {
                    "  "+ item.TransactionCode,
                    item.PaymentFrom,
                    item.Desciption,
                    item.Category.Name,
                    item.Account.Name,
                    item.Amount.ToString("N0")+"     "
                });
            }
        }

        //EXPENSES
        void ReloadExpenses()
        {
            GridExpense.Rows.Clear();

            var expenseTransactions = DbContext.GetInstance().GetCollection<ExpenseTransaction>("expense_transactions")
                                      .Include(x => x.Account)
                                    .Include(x => x.Category)
                                      .FindAll();


            //Filter the Search
            expenseTransactions = expenseTransactions.Where(r => r.PaymentTo.ToLower().Contains(TxtSearch.Text.ToLower())
            || r.Desciption.ToLower().Contains(TxtSearch.Text.ToLower())
            || r.TransactionCode.ToLower().Contains(TxtSearch.Text.ToLower())
            );

            //Filter Durations
            expenseTransactions = expenseTransactions.Where(r => r.Date.Year == inputDate.Value.Year && r.Date.Month == inputDate.Value.Month);
            if (InputDuration.SelectedIndex == 0)
                expenseTransactions = expenseTransactions.Where(r => r.Date.Day == inputDate.Value.Day);

            //Filter Categories
            if (InputCategory.SelectedIndex > 0) 
                expenseTransactions = expenseTransactions.Where(r => r.Category.Id == DbContext.GetInstance().GetCollection<ExpenseCategory>("expense_categories").FindAll().ToList()[InputCategory.SelectedIndex - 1].Id);

            //Filter Account
            if (InputAccount.SelectedIndex > 0) 
                expenseTransactions = expenseTransactions.Where(r => r.Account.Id == DbContext.GetInstance().GetCollection<Account>("accounts").FindAll().ToList()[InputAccount.SelectedIndex - 1].Id);


            LblTotalExpense.Text = "Total:     " + expenseTransactions.Sum(r => r.Amount).ToString();
            ExpenditureNo.Text = expenseTransactions.Sum(r => r.Amount).ToString();

            foreach (var item in expenseTransactions)
            {
                GridExpense.Rows.Add(new object[]
                {
                    "  "+ item.TransactionCode,
                    item.PaymentTo,
                    item.Desciption,
                    item.Category.Name,
                    item.Account.Name,
                    item.Amount.ToString("N0")+"     "
                });
            }
        }

        /*
        void ApllyGridTheme(Bunifu.UI.WinForms.BunifuDataGridView grid)
        { 
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.DimGray;

            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor= Color.DimGray;
        }
        */

        //move indicator
        void MoveIndicator(Control btn)
        {
            indicator.Left = btn.Left;
            indicator.Width = btn.Width;
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            //update indicator
            MoveIndicator((Control)sender);
            bunifuPages.SetPage("Dashboard");
           
        }

        private void btnIncome_Click(object sender, EventArgs e)
        {
            //update indicator
            MoveIndicator((Control)sender);
            bunifuPages.SetPage("Income");
        }

        private void btnExpenses_Click(object sender, EventArgs e)
        {
            //update indicator
            MoveIndicator((Control)sender);
            bunifuPages.SetPage("Expenses");
        }


        private void btnReports_Click(object sender, EventArgs e)
        {
            //update indicator
            MoveIndicator((Control)sender);
            bunifuPages.SetPage("Reports");
        }
        private void btnSettings_Click(object sender, EventArgs e)
        {
            //update indicator
            MoveIndicator((Control)sender);
            bunifuPages.SetPage("Settings");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void indicator_Click(object sender, EventArgs e)
        {

        }

        private void bunifuDatavizAdvanced2_Load_1(object sender, EventArgs e)
        {

        }

        void RenderMonthChart()
        {
            Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.Canvas canvas = new Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.Canvas();

            //Series
            Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.DataPoint income = new Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.DataPoint(Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced._type.Bunifu_column);
            Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.DataPoint expenses = new Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.DataPoint(Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced._type.Bunifu_column);
            Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.DataPoint balance = new Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.DataPoint(Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced._type.Bunifu_spline);

            //Add random Data
            Random ran = new Random();
            for(int i = 0; i <= 30; i++)
            {
                income.addLabely(i.ToString(), ran.Next(20, 500));
                expenses.addLabely(i.ToString(), ran.Next(0, 100));
                balance.addLabely(i.ToString(), ran.Next(100, 1000));
            }

            canvas.addData(income);
            canvas.addData(expenses);
            canvas.addData(balance);

            //reder the chart
            bunifuDatavizAdvanced1.Render(canvas);
        }

        void RenderIncomeChart()
        {
            Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.Canvas canvas = new Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.Canvas();

            //Series
            Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.DataPoint outlook = new Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.DataPoint(Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced._type.Bunifu_pie);

            //Add random Data
            outlook.addLabely("Salary", 100000);
            outlook.addLabely("Commission", 50000);
            outlook.addLabely("Freelance", 2000);
            outlook.addLabely("SocialMedia", 20000);


            canvas.addData(outlook);

            //reder the chart
            bunifuDatavizAdvanced2.Render(canvas);
        }

        void RenderExpenseChart()
        {
            Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.Canvas canvas = new Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.Canvas();

            //Series
            Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.DataPoint outlook = new Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced.DataPoint(Bunifu.Dataviz.WinForms.BunifuDatavizAdvanced._type.Bunifu_pie);

            //Add random Data
            outlook.addLabely("Rent", 100000);
            outlook.addLabely("Food", 50000);
            outlook.addLabely("Bills", 2000);
            outlook.addLabely("Internet", 20000);
            outlook.addLabely("Fuel", 20000);

            canvas.addData(outlook);

            //reder the chart
            bunifuDatavizAdvanced3.Render(canvas);
        }


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            timer1.Stop();
            RenderMonthChart();
            RenderIncomeChart();
            RenderExpenseChart();
        }

        private void AddIncomeBtn_Click(object sender, EventArgs e)
        {
            Form formBackground = new Form();
            try
            {
                using (Forms.frmAddIncome uu = new Forms.frmAddIncome(inputDate.Value))
                {
                    formBackground.StartPosition = FormStartPosition.Manual;
                    formBackground.FormBorderStyle = FormBorderStyle.None;
                    formBackground.Opacity = .50d;
                    formBackground.BackColor = Color.Black;
                    formBackground.WindowState = FormWindowState.Maximized;
                    formBackground.TopMost = true;
                    formBackground.Location = this.Location;
                    formBackground.ShowInTaskbar = false;
                    formBackground.Show();

                    uu.Owner = formBackground;
                    uu.ShowDialog();

                    formBackground.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                formBackground.Dispose();
            }
            ReloadIncome();
        }



        private void AddExpenseBtn_Click(object sender, EventArgs e)
        {
            Form formBackground = new Form();
            try
            {
                using (Forms.frmAddExpenses ee = new Forms.frmAddExpenses(inputDate.Value))
                {
                    formBackground.StartPosition = FormStartPosition.Manual;
                    formBackground.FormBorderStyle = FormBorderStyle.None;
                    formBackground.Opacity = .50d;
                    formBackground.BackColor = Color.Black;
                    formBackground.WindowState = FormWindowState.Maximized;
                    formBackground.TopMost = true;
                    formBackground.Location = this.Location;
                    formBackground.ShowInTaskbar = false;
                    formBackground.Show();

                    ee.Owner = formBackground;
                    ee.ShowDialog();

                    formBackground.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                formBackground.Dispose();
            }
            ReloadExpenses();
        }

        private void GridMIncome_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(GridMIncome.Rows[e.RowIndex].Tag != null)
            {
                //edit
                var incomeCategory = (IncomeCategory)GridMIncome.Rows[e.RowIndex].Tag;
                incomeCategory.Name = GridMIncome.Rows[e.RowIndex].Cells[0].Value.ToString();
                DbContext.GetInstance().GetCollection<IncomeCategory>("income_categories").Update(incomeCategory);
            }
            else
            {
                //new record
                int id = DbContext.GetInstance().GetCollection<IncomeCategory>("income_categories").Insert(
                    new IncomeCategory()
                    {
                        Name = GridMIncome.CurrentCell.Value.ToString()
                    }
                    );
                //update tag
                GridMIncome.Rows[e.RowIndex].Tag = DbContext.GetInstance().GetCollection<IncomeCategory>("income_categories").FindById(id);
            }
        }

        private void GridMExpenses_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (GridMExpenses.Rows[e.RowIndex].Tag != null)
            {
                //edit
                var expenseCategory = (ExpenseCategory)GridMExpenses.Rows[e.RowIndex].Tag;
                expenseCategory.Name = GridMExpenses.Rows[e.RowIndex].Cells[0].Value.ToString();
                DbContext.GetInstance().GetCollection<ExpenseCategory>("expense_categories").Update(expenseCategory);
            }
            else
            {
                //new record
                int id = DbContext.GetInstance().GetCollection<ExpenseCategory>("expense_categories").Insert(
                    new ExpenseCategory()
                    {
                        Name = GridMExpenses.CurrentCell.Value.ToString()
                    }
                    );
                //update tag
                GridMExpenses.Rows[e.RowIndex].Tag = DbContext.GetInstance().GetCollection<ExpenseCategory>("expense_categories").FindById(id);
            }
        }

        private void GridMAccounts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (GridMAccounts.Rows[e.RowIndex].Tag != null)
            {
                //edit
                var account = (Account)GridMAccounts.Rows[e.RowIndex].Tag;
                account.Name = GridMAccounts.Rows[e.RowIndex].Cells[0].Value.ToString();
                DbContext.GetInstance().GetCollection<Account>("accounts").Update(account);
            }
            else
            {
                //new record
                int id = DbContext.GetInstance().GetCollection<Account>("accounts").Insert(
                    new Account()
                    {
                        Name = GridMAccounts.CurrentCell.Value.ToString()
                    }
                    );
                //update tag
                GridMAccounts.Rows[e.RowIndex].Tag = DbContext.GetInstance().GetCollection<Account>("accounts").FindById(id);
            }
        }

        private void inputDate_ValueChanged(object sender, EventArgs e)
        {
            ReloadIncome();
            ReloadExpenses();
        }

        private void IncomeReportBtn_Click(object sender, EventArgs e)
        {
            DataObject copyData = GridIncome.GetClipboardContent();
            if (copyData != null) Clipboard.SetDataObject(copyData);
            Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
            xlapp.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook xlWbook;
            Microsoft.Office.Interop.Excel.Worksheet xlsheet;
            object misdata = System.Reflection.Missing.Value;
            xlWbook = xlapp.Workbooks.Add(misdata);
            xlsheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWbook.Worksheets.get_Item(1);
            Microsoft.Office.Interop.Excel.Range xlr = (Microsoft.Office.Interop.Excel.Range)xlsheet.Cells[1, 1];
            xlr.Select();

            xlsheet.PasteSpecial(xlr, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
        }

        private void ExpenseReportBtn_Click(object sender, EventArgs e)
        {
            DataObject copyData = GridExpense.GetClipboardContent();
            if (copyData != null) Clipboard.SetDataObject(copyData);
            Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
            xlapp.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook xlWbook;
            Microsoft.Office.Interop.Excel.Worksheet xlsheet;
            object misdata = System.Reflection.Missing.Value;
            xlWbook = xlapp.Workbooks.Add(misdata);
            xlsheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWbook.Worksheets.get_Item(1);
            Microsoft.Office.Interop.Excel.Range xlr = (Microsoft.Office.Interop.Excel.Range)xlsheet.Cells[1, 1];
            xlr.Select();

            xlsheet.PasteSpecial(xlr, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
        }

        private void IncomeTotLbl_Click(object sender, EventArgs e)
        {
            /*
            var incomeTransactions = DbContext.GetInstance().GetCollection<IncomeTransaction>("income_transactions")
                .Include(x => x.Account)
                .Include(x => x.Category)
                .FindAll();

            LblTotalIncome.Text = "Total:     " + incomeTransactions.Sum(r => r.Amount).ToString();
            IncomeNo.Text = incomeTransactions.Sum(r => r.Amount).ToString();
            */
        }

        private void LblTotalExpense_Click(object sender, EventArgs e)
        {

        }

        private void LblTotalInc_Click(object sender, EventArgs e)
        {

        }
    }
}
