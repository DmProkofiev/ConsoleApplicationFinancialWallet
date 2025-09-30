using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationFinancialWallet.Model
{
    [Table("Wallets")]
    public class Wallet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Currency {  get; set; }
        public decimal StartBalance { get; set; }
        public Wallet(int id, string name, string currency, decimal startBalance)
        {
            Id = id;
            Name = name;
            Currency = currency;
            StartBalance = startBalance;
        }
    }
}
