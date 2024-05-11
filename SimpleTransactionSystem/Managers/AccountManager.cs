using SimpleTransactionSystem.Data;
using SimpleTransactionSystem.Models;

namespace SimpleTransactionSystem.Managers
{
    public class AccountManager:IAccountManager
    {
        private readonly ApiContext dbContext;

        public AccountManager(ApiContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void TransferFunds(int fromAccountId, int toAccountId, decimal amount)
        {
            var transaction = new Transaction
            {
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Amount = amount,
                Timestamp = DateTime.UtcNow
            };

            transaction.Validate();

            var fromAccount = dbContext.Accounts.Find(fromAccountId);
            var toAccount = dbContext.Accounts.Find(toAccountId);

            if (fromAccount == null || toAccount == null)
            {
                throw new InvalidOperationException("Invalid account IDs.");
            }

            fromAccount.Withdraw(amount);
            toAccount.Deposit(amount);

            dbContext.Transactions.Add(transaction);
            dbContext.SaveChanges();
        }

        public IEnumerable<Transaction> GetTransactionHistory(int accountId)
        {
            return dbContext.Transactions
                .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
                .ToList();
        }
    }

}
