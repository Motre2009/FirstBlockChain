using FirstBlockChain.Models;
using FirstBlockChain.Services;

namespace FirstBlockChain;

internal class Program
{
    static void Main(string[] args)
    {
        var blockChain = new BlockChain(2);
        var displayService = new DisplayService();
        var transactionService = new TransactionService();

        var listOfTransactions = new List<Transaction>
        {
            transactionService.CreateTransaction("Alice", "Bob", 10),
            transactionService.CreateTransaction("Bob", "Charlie", 5),
            transactionService.CreateTransaction("Charlie", "Dave", 2)
        };

        Console.WriteLine("------------MENU-------------");
        Console.WriteLine("1. Create a new transaction");
        Console.WriteLine("2. Add new block");
        Console.WriteLine("3. Display the blockchain");
        Console.WriteLine("4. Is the blockchain valid?");
        Console.WriteLine("5. Exit");
        var enter = Console.ReadLine();

        while (enter != "5")
        {
            switch (enter)
            {
                case "1":
                    Console.WriteLine("Enter sender:");
                    var sender = Console.ReadLine();
                    Console.WriteLine("Enter receiver:");
                    var receiver = Console.ReadLine();
                    Console.WriteLine("Enter amount:");
                    var amount = decimal.Parse(Console.ReadLine());
                    var transaction = transactionService.CreateTransaction(sender, receiver, amount);
                    listOfTransactions.Add(transaction);
                    break;
                case "2":
                    blockChain.CreateTransaction(listOfTransactions);
                    listOfTransactions.Clear();
                    break;
                case "3":
                    displayService.DisplayBlockChain(blockChain);
                    break;
                case "4":
                    Console.WriteLine($"Is valid: {blockChain.IsChainValid()}");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
            Console.WriteLine("------------MENU-------------");
            Console.WriteLine("1. Create a new transaction");
            Console.WriteLine("2. Add new block");
            Console.WriteLine("3. Display the blockchain");
            Console.WriteLine("4. Is valid the block?");
            Console.WriteLine("5. Exit");
            enter = Console.ReadLine();
        }
    }
}
