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
    public partial class frmAddExpenses : Form
    {
        public frmAddExpenses(DateTime date)
        {
            InitializeComponent();

            //TxtCategory.Clear();
            foreach (var item in DbContext.GetInstance().GetCollection<ExpenseCategory>("expense_categories").FindAll())
            {
                TxtCategory.Items.Add(item.Name);
                TxtCategory.SelectedIndex = 0;
            }

            //load accounts
            //TxtAccount.Clear();
            foreach (var item in DbContext.GetInstance().GetCollection<Account>("accounts").FindAll())
            {
                TxtAccounts.Items.Add(item.Name);
                TxtAccounts.SelectedIndex = 0;
            }

            TxtDate.Value = date;
        }

        private void frmAddExpenses_Load(object sender, EventArgs e)
        {

        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {

        }

        private void AddExpensesBtn_Click(object sender, EventArgs e)
        {
            //validate the form
            if (TxtTo.Text.Trim().Length == 0)
            {
                bunifuSnackbar1.Show(this, "Validation Error", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 2000);
                TxtTo.Focus();
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
                    return;
                }
            }
            catch (Exception)
            {
                bunifuSnackbar1.Show(this, "Invalid Amount", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 2000);
                TxtAmount.Focus();
                return;
            }

            //save the transaction
            if (MessageBox.Show("Confirm reverse amount" + amount + " from " + TxtTo.Text, "MoneyBoss", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
            {
                bunifuSnackbar1.Show(this, "Transaction Cancelled", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 2000);
                return;
            }

            //complete transaction
            ExpenseTransaction expenseTransaction = new ExpenseTransaction()
            {
                Account = DbContext.GetInstance().GetCollection<Account>("accounts").FindAll().ToList()[TxtAccounts.SelectedIndex],
                Category = DbContext.GetInstance().GetCollection<ExpenseCategory>("expense_categories").FindAll().ToList()[TxtCategory.SelectedIndex],
                Amount = amount,
                Date = TxtDate.Value,
                Desciption = TxtDesc.Text.Trim(),
                PaymentTo = TxtTo.Text.Trim(),
                TransactionCode = TxtTransacId.Text.Trim()
            };

            DbContext.GetInstance().GetCollection<ExpenseTransaction>("expense_transactions").Insert(expenseTransaction);
            bunifuSnackbar1.Show(this, "Successfull", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 2000);
            CloseForm.Start();
        }

        private void CloseForm_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
