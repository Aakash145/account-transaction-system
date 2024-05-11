using SimpleTransactionSystem.Models;

namespace SimpleTransactionSystem.Managers
{
    public interface IAccountManager
    {
        public void TransferFunds(int fromAccountId, int toAccountId, decimal amount);
        public IEnumerable<Transaction> GetTransactionHistory(int accountId);
    }
}
