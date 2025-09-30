using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationFinancialWallet
{
    public class TransactionService
    {
        public TransactionService(ApplicationContext context)
        {
            _context = context;
        }
        private readonly ApplicationContext _context;

        public static void TransactionWallet(int choiceWallet) //Транзакции
        {
            using (var context = new ApplicationContext())
            {
                var Transactions = context.Transactions.Where(t => t.WalletId == choiceWallet).OrderByDescending(t => t.Date).ToList();

                for (int i = 0; i < Transactions.Count; i++)
                {
                    var transaction = Transactions[i];
                    Console.WriteLine($"{transaction.Date:dd.MM.yyyy} | {transaction.Amount} | {transaction.Description}");
                }
            }
        }
        public static void IncomeWallet(int choiceWallet) //Доходы
        {
            using (var context = new ApplicationContext())
            {
                var Transactions = context.Transactions.Where(t => t.WalletId == choiceWallet).OrderByDescending(t => t.Date).ToList();

                //var Income = Transactions.Where(t => t.Type == Transaction.TransactionType.Income).ToList(); 
                var Income = Transactions.Where(t => (int)t.Type == 1).ToList();

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
        }
        public static void ExpenseWallet(int choiceWallet) //Расходы
        {
            using (var context = new ApplicationContext())
            {
                var Transactions = context.Transactions.Where(t => t.WalletId == choiceWallet).OrderByDescending(t => t.Date).ToList();

                var Expense = Transactions.Where(t => (int)t.Type == 2).ToList();

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
            }
        }
        public static void IncomeWalletPerMonth(int choiceWallet) //Доходы за текущий месяц
        {
            using (var context = new ApplicationContext())
            {
                DateTime now = DateTime.Now;
                int thisYear = now.Year;
                int thisMonth = now.Month;

                var Income = context.Transactions.Where(t => t.WalletId == choiceWallet && (int)t.Type == 1 && t.Date.Year == thisYear && t.Date.Month == thisMonth).OrderByDescending(t => t.Date).ToList();

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
        }
        public static void ExpenseWalletPerMonth(int choiceWallet) //Расходы за текущий месяц
        {
            using (var context = new ApplicationContext())
            {
                DateTime now = DateTime.Now;
                int thisYear = now.Year;
                int thisMonth = now.Month;

                var Expense = context.Transactions.Where(t => t.WalletId == choiceWallet && (int)t.Type == 2 && t.Date.Year == thisYear && t.Date.Month == thisMonth).OrderByDescending(t => t.Date).ToList();

                Console.WriteLine($"Доходы за {thisMonth:00}.{thisYear}:");

                if (Expense.Count == 0)
                {
                    Console.WriteLine("Расходов за текущий месяц нет");
                    return;
                }

                for (int i = 0; i < Expense.Count; i++)
                {
                    var transaction = Expense[i];
                    Console.WriteLine($"{transaction.Date:dd.MM.yyyy} | {transaction.Amount} | {transaction.Description}");
                }

                decimal AmountExpense = Expense.Sum(t => t.Amount);

                Console.WriteLine($"Общая сумма расходов за месяц: {AmountExpense}");
            }
        }
    }
}
