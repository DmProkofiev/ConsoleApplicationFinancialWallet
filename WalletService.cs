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
        public static async Task<decimal> CurrentBalanceAsync(int choiceWallet)
        {
            using (var context = new ApplicationContext())
            {
                decimal startBalance = await context.Wallets.Where(w => w.Id == choiceWallet).Select(w => w.StartBalance).FirstOrDefaultAsync();

                decimal income = await context.Transactions.Where(t => t.WalletId == choiceWallet && (int)t.Type == 1).SumAsync(t => t.Amount);

                decimal expense = await context.Transactions.Where(t => t.WalletId == choiceWallet && (int)t.Type == 2).SumAsync(t => t.Amount);

                decimal currentBalance = startBalance + income - expense;
                Console.WriteLine($"Ваш текущий остаток по счету {currentBalance}");
                return currentBalance;
            }
        }
        public static async Task PayWalletAsync(int choiceWallet) //Совершение платежа
        {
            Console.WriteLine("Назначение платежного поручения");
            string description = Console.ReadLine();

            Console.WriteLine("Сумма");
            string inputAmount = Console.ReadLine();

            if (decimal.TryParse(inputAmount, out decimal amount) && amount > 0)
            {
                decimal currentBalance = await CurrentBalanceAsync(choiceWallet);

                if (currentBalance < amount)
                {
                    Console.WriteLine("Недостаточно средств на кошельке.");
                    return;
                }

                using (var context = new ApplicationContext())
                {
                    var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.Id == choiceWallet);

                    var transaction = new Transaction
                    {
                        WalletId = choiceWallet,
                        Date = DateTime.Now,
                        Amount = amount,
                        Description = description,
                        Type = Transaction.TransactionType.Expense
                    };

                    context.Transactions.Add(transaction);
                    await context.SaveChangesAsync();

                    Console.WriteLine($"Платеж выполнен. Ваш баланс: {wallet.StartBalance}");
                }
                return;
            }
            Console.WriteLine("Некорректная сумма");
        }
    }
}
