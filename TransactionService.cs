using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplicationFinancialWallet.Model;

namespace ConsoleApplicationFinancialWallet
{
    public class TransactionService
    {
        public TransactionService(ApplicationContext context)
        {
            _context = context;
        }
        private readonly ApplicationContext _context;

        public async Task TransactionWalletAsync(int choiceWallet) //Транзакции
        {

            var transactions = await _context.Transactions.Where(t => t.WalletId == choiceWallet).OrderByDescending(t => t.Date).ToListAsync();

            for (int i = 0; i < transactions.Count; i++)
            {
                var transaction = transactions[i];
                Console.WriteLine($"{transaction.Date:dd.MM.yyyy} | {transaction.Amount} | {transaction.Description}");
            }
            Console.WriteLine($"DEBUG: transactions.Count = {transactions.Count}");

        }
        public async Task IncomeWalletAsync(int choiceWallet) //Доходы
        {
            var Transactions = await _context.Transactions.Where(t => t.WalletId == choiceWallet).OrderByDescending(t => t.Date).ToListAsync();

            var Income = Transactions.Where(t => t.Type == Transaction.TransactionType.Income).ToList(); 

            for (int i = 0; i < Income.Count; i++)
            {
                var transaction = Income[i];
                Console.WriteLine($"{transaction.Date:dd.MM.yyyy} | {transaction.Amount} | {transaction.Description}");
            }

            decimal AmountIncome = 0;
            for (int i = 0; i < Income.Count; i++)
            {
                AmountIncome += Income[i].Amount;
            }

            Console.WriteLine($"Общая сумма доходов: {AmountIncome}");

        }
        public async Task ExpenseWalletAsync(int choiceWallet) //Расходы
        {
            var Transactions = await _context.Transactions.Where(t => t.WalletId == choiceWallet).OrderByDescending(t => t.Date).ToListAsync();

            var Expense = Transactions.Where(t => t.Type == Transaction.TransactionType.Expense).ToList();

            for (int i = 0; i < Expense.Count; i++)
            {
                var transaction = Expense[i];
                Console.WriteLine($"{transaction.Date:dd.MM.yyyy} | {transaction.Amount} | {transaction.Description}");
            }

            decimal AmountExpense = 0;
            for (int i = 0; i < Expense.Count; i++)
            {
                AmountExpense += Expense[i].Amount;
            }

            Console.WriteLine($"Общая сумма расходов: {AmountExpense}");
            //Console.WriteLine($" transactions.Count = {Expense.Count}"); для отлдаки

        }
        public async Task IncomeWalletPerMonthAsync(int choiceWallet) //Доходы за текущий месяц
        {

            DateTime now = DateTime.Now;
            int thisYear = now.Year;
            int thisMonth = now.Month;

            var Transactions = await _context.Transactions.Where(t => t.WalletId == choiceWallet && t.Date.Year == thisYear && t.Date.Month == thisMonth).ToListAsync();

            var Income = Transactions.Where(t => t.Type == Transaction.TransactionType.Income).ToList();

            Console.WriteLine($"Доходы за {thisMonth:00}.{thisYear}:");

            if (Income.Count == 0)
            {
                Console.WriteLine("Доходов за текущий месяц нет");
                return;
            }

            for (int i = 0; i < Income.Count; i++)
            {
                var transaction = Income[i];
                Console.WriteLine($"{transaction.Date:dd.MM.yyyy} | {transaction.Amount} | {transaction.Description}");
            }

            decimal AmountIncome = Income.Sum(t => t.Amount);
            Console.WriteLine($"Общая сумма доходов за месяц: {AmountIncome}");

        }
        public async Task ExpenseWalletPerMonthAsync(int choiceWallet) //Расходы за текущий месяц
        {

            DateTime now = DateTime.Now;
            int thisYear = now.Year;
            int thisMonth = now.Month;

            var Transactions = await _context.Transactions.Where(t => t.WalletId == choiceWallet && t.Date.Year == thisYear && t.Date.Month == thisMonth).ToListAsync();

            var Expense = Transactions.Where(t => t.Type == Transaction.TransactionType.Expense).ToList();

            if (Expense.Count == 0)
            {
                Console.WriteLine("Расходов за текущий месяц нет");
                return;
            }
            Console.WriteLine($"Расходы за {thisMonth:00}.{thisYear}:");
            for (int i = 0; i < Expense.Count; i++)
            {
                var transaction = Expense[i];
                Console.WriteLine($"{transaction.Date:dd.MM.yyyy} | {transaction.Amount} | {transaction.Description}");
            }

            decimal AmountExpense = Expense.Sum(t => t.Amount);

            Console.WriteLine($"Общая сумма расходов за месяц: {AmountExpense}");

        }
        public async Task ThreeExpenseWalletPerMonthAsync(int choiceWallet) //Три самых больших траты за указанный месяц 
        {
            {
                DateTime now = DateTime.Now;
                int thisYear = now.Year;
                int thisMonth = now.Month;

                var Transactions = await _context.Transactions.Where(t => t.WalletId == choiceWallet && t.Date.Year == thisYear && t.Date.Month == thisMonth).ToListAsync();

                var Expense = Transactions.Where(t => t.Type == Transaction.TransactionType.Expense).ToList();

                if (Expense.Count == 0)
                {
                    Console.WriteLine("Расходов за текущий месяц нет");
                    return;
                }

                Transaction[] topExpense = new Transaction[3];

                for (int i = 0; i < Expense.Count; i++)
                {
                    var current = Expense[i];

                    for (int position = 0; position < 3; position++)
                    {
                        if (topExpense[position] == null || current.Amount > topExpense[position].Amount)
                        {
                            for (int y = 2; y > position; y--)
                            {
                                topExpense[y] = topExpense[y - 1];
                            }
                            topExpense[position] = current;
                            break;
                        }
                    }
                }
                Console.WriteLine($"Три самых больших траты за {thisMonth:00}.{thisYear}:");

                for (int i = 0; i < 3; i++)
                {
                    if (topExpense[i] != null)
                    {
                        Console.WriteLine($"{i + 1}. {topExpense[i].Date:dd.MM.yyyy} | {topExpense[i].Amount} | {topExpense[i].Description}");
                    }
                }
            }

        }
    }
}
