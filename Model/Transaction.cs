using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationFinancialWallet.Model
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public TransactionType Type { get; set; }
        public int WalletId { get; set; }

        public enum TransactionType
        {
            Income,
            Expense
        }
        public Transaction(int id, decimal amount, string description, int walletId )
        {
            Id = id;
            Amount = amount;
            Description = description;
            WalletId = walletId;
        }
        public Transaction()
        {
        }
    }
}
