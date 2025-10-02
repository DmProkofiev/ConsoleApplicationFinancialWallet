using ConsoleApplicationFinancialWallet.Model;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApplicationFinancialWallet
{
    public class WalletService
    {
        public WalletService(ApplicationContext context)
        {
            _context = context;
        }

        private readonly ApplicationContext _context;

        public async Task<decimal> CurrentBalanceAsync(int walletId)
        {
            decimal startBalance = await _context.Wallets.Where(w => w.Id == walletId).Select(w => w.StartBalance).FirstOrDefaultAsync();

            decimal income = await _context.Transactions.Where(t => t.WalletId == walletId && t.Type == Transaction.TransactionType.Income).SumAsync(t => t.Amount);

            decimal expense = await _context.Transactions.Where(t => t.WalletId == walletId && t.Type == Transaction.TransactionType.Expense).SumAsync(t => t.Amount);

            decimal currentBalance = startBalance + income - expense;

            Console.WriteLine($"Текущий баланс кошелька: {currentBalance}");
            Console.WriteLine($"(Начальный: {startBalance} + Доходы: {income} - Расходы: {expense})");

            return currentBalance;
        }

        public async Task PayWalletAsync(int choiceWallet)
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

                var transaction = new Transaction
                {
                    WalletId = choiceWallet,
                    Date = DateTime.Today,
                    Amount = amount,
                    Description = description,
                    Type = Transaction.TransactionType.Expense
                };

                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                decimal newBalance = currentBalance - amount;
                Console.WriteLine($"Платеж выполнен. Ваш баланс: {newBalance}");
                return;
            }
            Console.WriteLine("Некорректная сумма");
        }
    }
}
