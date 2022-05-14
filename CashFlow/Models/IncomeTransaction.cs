using System;
using LiteDB;

namespace CashFlow.Models
{
    public class IncomeTransaction
    {

        public int Id { get; set; }

        public string TransactionCode { get; set; }

        public string PaymentFrom { get; set; }

        public string Desciption { get; set; }

        [BsonRef("accounts")]
        public Account Account { get; set; }

        [BsonRef("income_categories")]
        public IncomeCategory Category { get; set; }

        public double Amount { get; set; }

        public double TransactionCost { get; set; }

        public DateTime Date { get; set; }
    }
}
