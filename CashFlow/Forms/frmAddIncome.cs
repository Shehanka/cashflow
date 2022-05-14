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
using CashFlow.Forms;

namespace CashFlow.Forms
{
    public partial class frmAddIncome : Form
    {
        public frmAddIncome(DateTime date)
        {
            InitializeComponent();
            //Load the category and accounts
            //TxtCategory.Clear();
            foreach (var item in DbContext.GetInstance().GetCollection<IncomeCategory>("income_categories").FindAll())
            {
                TxtCategory.Items.Add(item.Name);
                TxtCategory.SelectedIndex = 0;
            }

            //load accounts
            //TxtAccount.Clear();
            foreach (var item in DbContext.GetInstance().GetCollection<Account>("accounts").FindAll())
            {
                TxtAccount.Items.Add(item.Name);
                TxtAccount.SelectedIndex = 0;
            }

            TxtDate.Value = date;
        }

        private void frmAddIncome_Load(object sender, EventArgs e)
        {

        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void bunifuSeparator1_Click(object sender, EventArgs e)
        {

        }

        private void BtnAddIncome_Click(object sender, EventArgs e)
        {
            //validate the form
            if (TxtFrom.Text.Trim().Length == 0) 
            {
                bunifuSnackbar1.Show(this, "Validation Error", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 2000);
                TxtFrom.Focus();
                return;
            }
            double amount = 0;
            try
            {
                amount = double.Parse(TxtAmount.Text.Trim());
                if (amount <= 0)
                {
                    bunifuSnackbar1.Show(this, "Invalid Amount", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 2000);
                    TxtAmount.Focus();
                    return ;
                }
            }
            catch (Exception)
            {
                bunifuSnackbar1.Show(this, "Invalid Amount", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 2000);
                TxtAmount.Focus();
                return;
            }

            //save the transaction
            if(MessageBox.Show("Confirm receve amount" +amount+ " from "+TxtFrom.Text,"MoneyBoss",MessageBoxButtons.YesNoCancel)!=DialogResult.Yes)
            {
                bunifuSnackbar1.Show(this, "Transaction Cancelled", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 2000);
                return;
            }

            //complete transaction
            IncomeTransaction incomeTransaction = new IncomeTransaction()
            {
                Account = DbContext.GetInstance().GetCollection<Account>("accounts").FindAll().ToList()[TxtAccount.SelectedIndex],
                Category = DbContext.GetInstance().GetCollection<IncomeCategory>("income_categories").FindAll().ToList()[TxtAccount.SelectedIndex],
                Amount = amount,
                Date = TxtDate.Value,
                Desciption = TxtDesc.Text.Trim(),
                PaymentFrom = TxtFrom.Text.Trim(),
                TransactionCode = TxtCode.Text.Trim()
            };

            DbContext.GetInstance().GetCollection<IncomeTransaction>("income_transactions").Insert(incomeTransaction);
            bunifuSnackbar1.Show(this, "Successfull", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 2000);
            CloseForm.Start();
        }

        private void CloseForm_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
