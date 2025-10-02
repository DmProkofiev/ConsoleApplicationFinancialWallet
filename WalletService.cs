using ConsoleApplicationFinancialWallet.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationFinancialWallet
{
    public class WalletService
    {
        public WalletService(ApplicationContext context)
        {
            _context = context;
        }

        private readonly ApplicationContext _context;
        public static decimal CurrentBalance(int choiceWallet) //текущтий баланс
        {
            using (var context = new ApplicationContext())
            {
                decimal StartBalance = context.Wallets.Where(w => w.Id == choiceWallet).Select(w => w.StartBalance).FirstOrDefault();

                decimal Income = context.Transactions.Where(t => t.WalletId == choiceWallet && (int)t.Type == 1).Sum(t => t.Amount);
                decimal Expense = context.Transactions.Where(t => t.WalletId == choiceWallet && (int)t.Type == 2).Sum(t => t.Amount);

                decimal currentBalance = StartBalance + Income - Expense;
                Console.WriteLine($"Ваш текущий остаток по счету {currentBalance}");
                return currentBalance;
            }
        }
        public static void PayWallet(int choiceWallet) //Совершение платежа
        {
            Console.WriteLine("Назначение платежного поручения");
            string description = Console.ReadLine();

            Console.WriteLine("Сумма");
            string inputAmount = Console.ReadLine();

            if (decimal.TryParse(inputAmount, out decimal amount) && amount > 0)
            {
                decimal currentBalance = CurrentBalance(choiceWallet);

                if (currentBalance < amount)
                {
                    Console.WriteLine("Недостаточно средств на кошельке.");
                    return;
                }

                using (var context = new ApplicationContext())
                {
                    var wallet = context.Wallets.FirstOrDefault(w => w.Id == choiceWallet);

                    var transaction = new Transaction
                    {
                        WalletId = choiceWallet,
                        Date = DateTime.Now,
                        Amount = amount,
                        Description = description,
                        Type = Transaction.TransactionType.Expense
                    };

                    context.Transactions.Add(transaction);
                    context.SaveChanges();

                    Console.WriteLine($"Платеж выполнен. Ваш баланс: {wallet.StartBalance}");
                }
                return;
            }
            Console.WriteLine("Некорректная сумма");
        }
    }
}
