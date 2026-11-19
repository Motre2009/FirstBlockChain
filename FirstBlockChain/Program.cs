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
        var walletService = new WalletService();

        var aliceWallet = walletService.CreateWallet("Alice");
        var bobWallet = walletService.CreateWallet("Bob");
        var charlieWallet = walletService.CreateWallet("Charlie");
        var wallets = new List<Wallet> { aliceWallet, bobWallet, charlieWallet };

        foreach (var wallet in wallets)
        {
            blockChain.AddBlock(new List<Transaction>(), wallet.Address);
        }

        var listOfTransactions = new List<Transaction>
        {
            transactionService.CreateTransaction(aliceWallet, bobWallet.Address, 10),
            transactionService.CreateTransaction(bobWallet, charlieWallet.Address, 5),
            transactionService.CreateTransaction(charlieWallet, aliceWallet.Address, 2)
        };

        Console.WriteLine("------------MENU-------------");
        Console.WriteLine("1. Create a new transaction");
        Console.WriteLine("2. Add new block");
        Console.WriteLine("3. Display the blockchain");
        Console.WriteLine("4. Is the blockchain valid?");
        Console.WriteLine("5. Check balance");
        Console.WriteLine("6. Exit");
        var enter = Console.ReadLine();

        while (enter != "6")
        {
            switch (enter)
            {
                case "1":
                    Console.WriteLine("Enter sender:");
                    var senderName = Console.ReadLine();
                    var sender = wallets.FirstOrDefault(w => w.Name == senderName);
                    if (sender == null && !string.IsNullOrWhiteSpace(senderName))
                    {
                        sender = walletService.CreateWallet(senderName);
                        wallets.Add(sender);
                    }
                    Console.WriteLine("Enter receiver:");
                    var receiverName = Console.ReadLine();
                    var receiver = wallets.FirstOrDefault(w => w.Name == receiverName);
                    if (receiver == null && !string.IsNullOrWhiteSpace(receiverName))
                    {
                        receiver = walletService.CreateWallet(receiverName);
                        wallets.Add(receiver);
                    }

                    Console.WriteLine("Enter amount:");
                    decimal.TryParse(Console.ReadLine(), out var amount);

                    if (sender == null || receiver == null || amount <= 0)
                    {
                        Console.WriteLine("Wrong data. Transaction was not created.");
                        break;
                    }

                    var transaction = transactionService.CreateTransaction(sender, receiver.Address, amount);
                    listOfTransactions.Add(transaction);
                    break;
                case "2":
                    Console.WriteLine("Enter miner name:");
                    var minerName = Console.ReadLine();
                    var miner = wallets.FirstOrDefault(w => w.Name == minerName);

                    if (miner == null)
                    {
                        Console.WriteLine("Miner not found.");
                        break;
                    }

                    try
                    {
                        blockChain.AddBlock(listOfTransactions, miner.Address);
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Block not added: {ex.Message}");
                    }
                    listOfTransactions.Clear();
                    break;
                case "3":
                    displayService.DisplayBlockChain(blockChain);
                    break;
                case "4":
                    Console.WriteLine($"Is valid: {blockChain.IsChainValid()}");
                    break;
                case "5":
                    Console.WriteLine("Enter wallet name to check balance:");
                    var walletName = Console.ReadLine();
                    var wallet = wallets.FirstOrDefault(w => w.Name == walletName);
                    if (wallet != null)
                    {
                        var balanceService = new BalanceService();
                        var balance = balanceService.GetBalance(wallet.Address, blockChain.Chain);
                        Console.WriteLine($"Balance of {wallet.Name}: {balance}");
                    }
                    else
                    {
                        Console.WriteLine("Wallet not found.");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
            Console.WriteLine("------------MENU-------------");
            Console.WriteLine("1. Create a new transaction");
            Console.WriteLine("2. Add new block");
            Console.WriteLine("3. Display the blockchain");
            Console.WriteLine("4. Is the blockchain valid?");
            Console.WriteLine("5. Check balance");
            Console.WriteLine("6. Exit");
            enter = Console.ReadLine();
        }
    }
}
