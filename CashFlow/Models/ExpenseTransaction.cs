using System;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace CashFlow.Models
{
    public class ExpenseTransaction
    {
        public int Id { get; set; }

        public string TransactionCode { get; set; }

        public string PaymentTo { get; set; }

        public string Desciption { get; set; }

        [BsonRef("accounts")]
        public Account Account { get; set; }

        [BsonRef("expense_categories")]
        public ExpenseCategory Category { get; set; }

        public double Amount { get; set; }

        public double TransactionCost { get; set; }

        public DateTime Date { get; set; }
    }
}
