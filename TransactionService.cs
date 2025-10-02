using ConsoleApplicationFinancialWallet.Model;
using Microsoft.EntityFrameworkCore;

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
            var transactions = await _context.Transactions.Where(t => t.WalletId == choiceWallet).OrderByDescending(t => t.Date).ToListAsync();

            var income = transactions.Where(t => t.Type == Transaction.TransactionType.Income).ToList();

            for (int i = 0; i < income.Count; i++)
            {
                var transaction = income[i];
                Console.WriteLine($"{transaction.Date:dd.MM.yyyy} | {transaction.Amount} | {transaction.Description}");
            }

            decimal AmountIncome = 0;
            for (int i = 0; i < income.Count; i++)
            {
                AmountIncome += income[i].Amount;
            }

            Console.WriteLine($"Общая сумма доходов: {AmountIncome}");

        }
        public async Task ExpenseWalletAsync(int choiceWallet) //Расходы
        {
            var transactions = await _context.Transactions.Where(t => t.WalletId == choiceWallet).OrderByDescending(t => t.Date).ToListAsync();

            var expense = transactions.Where(t => t.Type == Transaction.TransactionType.Expense).ToList();

            for (int i = 0; i < expense.Count; i++)
            {
                var transaction = expense[i];
                Console.WriteLine($"{transaction.Date:dd.MM.yyyy} | {transaction.Amount} | {transaction.Description}");
            }

            decimal AmountExpense = 0;
            for (int i = 0; i < expense.Count; i++)
            {
                AmountExpense += expense[i].Amount;
            }

            Console.WriteLine($"Общая сумма расходов: {AmountExpense}");
            //Console.WriteLine($" transactions.Count = {Expense.Count}"); для отлдаки

        }
        public async Task IncomeWalletPerMonthAsync(int choiceWallet) //Доходы за текущий месяц
        {

            DateTime now = DateTime.Now;
            int thisYear = now.Year;
            int thisMonth = now.Month;

            var transactions = await _context.Transactions.Where(t => t.WalletId == choiceWallet && t.Date.Year == thisYear && t.Date.Month == thisMonth).ToListAsync();

            var income = transactions.Where(t => t.Type == Transaction.TransactionType.Income).ToList();

            Console.WriteLine($"Доходы за {thisMonth:00}.{thisYear}:");

            if (income.Count == 0)
            {
                Console.WriteLine("Доходов за текущий месяц нет");
                return;
            }

            for (int i = 0; i < income.Count; i++)
            {
                var transaction = income[i];
                Console.WriteLine($"{transaction.Date:dd.MM.yyyy} | {transaction.Amount} | {transaction.Description}");
            }

            decimal AmountIncome = income.Sum(t => t.Amount);
            Console.WriteLine($"Общая сумма доходов за месяц: {AmountIncome}");

        }
        public async Task ExpenseWalletPerMonthAsync(int choiceWallet) //Расходы за текущий месяц
        {

            DateTime now = DateTime.Now;
            int thisYear = now.Year;
            int thisMonth = now.Month;

            var transactions = await _context.Transactions.Where(t => t.WalletId == choiceWallet && t.Date.Year == thisYear && t.Date.Month == thisMonth).ToListAsync();

            var expense = transactions.Where(t => t.Type == Transaction.TransactionType.Expense).ToList();

            if (expense.Count == 0)
            {
                Console.WriteLine("Расходов за текущий месяц нет");
                return;
            }
            Console.WriteLine($"Расходы за {thisMonth:00}.{thisYear}:");
            for (int i = 0; i < expense.Count; i++)
            {
                var transaction = expense[i];
                Console.WriteLine($"{transaction.Date:dd.MM.yyyy} | {transaction.Amount} | {transaction.Description}");
            }

            decimal AmountExpense = expense.Sum(t => t.Amount);

            Console.WriteLine($"Общая сумма расходов за месяц: {AmountExpense}");

        }
        public async Task ThreeExpenseWalletPerMonthAsync(int choiceWallet) //Три самых больших траты за указанный месяц 
        {
            {
                DateTime now = DateTime.Now;
                int thisYear = now.Year;
                int thisMonth = now.Month;

                var transactions = await _context.Transactions.Where(t => t.WalletId == choiceWallet && t.Date.Year == thisYear && t.Date.Month == thisMonth).ToListAsync();

                var expense = transactions.Where(t => t.Type == Transaction.TransactionType.Expense).ToList();

                if (expense.Count == 0)
                {
                    Console.WriteLine("Расходов за текущий месяц нет");
                    return;
                }

                Transaction[] topExpense = new Transaction[3];

                for (int i = 0; i < expense.Count; i++)
                {
                    var current = expense[i];

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
