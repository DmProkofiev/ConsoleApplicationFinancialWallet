using ConsoleApplicationFinancialWallet.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ConsoleApplicationFinancialWallet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.OutputEncoding = Encoding.GetEncoding(866);
            Console.InputEncoding = Encoding.GetEncoding(866);

            ApplicationContext context = new ApplicationContext();
            WalletService walletService = new WalletService(context);
            TransactionService transactionService = new TransactionService(context);
            
            Console.WriteLine("Консольное Приложение Financial Wallet для учета личных финансов");
            Console.WriteLine("Разработчик Прокофьев Дмитрий Леонидович");

            while (true)
            {
                int choiceWallet = GetWallet(context);
                UseWallet(context, choiceWallet);
            }
        }

        public static int GetWallet(ApplicationContext context)
        {
            //context.Database.CanConnect();

            var wallets = context.Wallets.ToList();

            for (int i = 0; i < wallets.Count; i++)
            {
                var wallet = wallets[i];
                Console.WriteLine($"{wallets[i].Id}. {wallet.Name}");
            }
  
            while (true)
            {
                Console.WriteLine($"введите значение в диапазоне: 1..{wallets.Count}");
                var inputUser = Console.ReadLine();
                if (int.TryParse(inputUser, out int index))
                {
                    if (index >= 1 && index <= wallets.Count)
                    {
                        return wallets[index - 1].Id; 
                    }
                }

                Console.WriteLine("Некорректный выбор. Попробуйте ещё раз");
            }
        }

        public static void UseWallet(ApplicationContext context, int choiceWallet)
        {
            var wallet = context.Wallets.FirstOrDefault(w => w.Id == choiceWallet);
            
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Ваш кошелек: {wallet.Name}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Валюта: {wallet.Currency}");
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("\n--- Меню ---");
                Console.WriteLine("Выберите действие: ");

                Console.WriteLine("1. Текущий баланс");
                Console.WriteLine("2. Транзакции");
                Console.WriteLine("3. Показать зачисления");
                Console.WriteLine("4. Показать расходы");
                Console.WriteLine("5. Показать доходы за текущий месяц");
                Console.WriteLine("6. Показать расходы за текущий месяц");
                Console.WriteLine("7. Совершить платежное поручение");
                Console.WriteLine("8. Выбрать кошелек");
                Console.ResetColor();

                string choiceMenu = Console.ReadLine();

                switch (choiceMenu)
                {
                    case "1":
                        WalletService.CurrentBalance(choiceWallet);
                        break;
                    case "2":
                        TransactionService.TransactionWallet(choiceWallet);
                        break;
                    case "3":
                        TransactionService.IncomeWallet(choiceWallet);
                        break;
                    case "4":
                        TransactionService.ExpenseWallet(choiceWallet);
                        break;
                    case "5":
                        TransactionService.IncomeWalletPerMonth(choiceWallet);
                        break;
                    case "6":
                        TransactionService.ExpenseWalletPerMonth(choiceWallet);
                        break;
                    case "7":
                        Console.WriteLine("Для совершения платежного поручения укажите сумму платежа, назначение");
                        WalletService.PayWallet(choiceWallet);
                        break;
                    case "8":
                        Console.WriteLine("Выберите другой кошелек");
                        return;
                    default:

                        break;
                }
            }
        }
    }
}
