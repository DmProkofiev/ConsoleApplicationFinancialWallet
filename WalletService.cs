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

            if (decimal.TryParse(inputAmount, out decimal amount))
            {
                using (var context = new ApplicationContext())
                {
                    // Пример: определить следующий Id через цикл (не рекомендуется)
                    int newId = 1;
                    var allIds = context.Transactions.Select(t => t.Id).ToList();
                    int maxId = 0;
                    for (int i = 0; i < allIds.Count; i++)
                    {
                        if (allIds[i] > maxId) maxId = allIds[i];
                    }
                    newId = maxId + 1;

                    //var payment = new Transaction
                    //{
                    //    Id = newId, 
                    //    WalletId = choiceWallet,
                    //    Date = DateTime.Now,
                    //    Amount = amount,
                    //    Description = description,
                    //    Type = Transaction.TransactionType.Expense
                    //};

                    //context.Transactions.Add(payment);
                    //context.SaveChanges();
                }
                return;
            }
            Console.WriteLine("Некорректная сумма.");
        }
    }
}
