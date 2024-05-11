using System.ComponentModel.DataAnnotations;

namespace SimpleTransactionSystem.Models
{
    public abstract class Account
    {
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
        public string AccountType { get; set; } // Discriminator member

        public virtual void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be greater than zero.");
            }

            if (Balance - amount < 0)
            {
                throw new InvalidOperationException("Insufficient funds for withdrawal.");
            }

            Balance -= amount;
        }

        public virtual void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be greater than zero.");
            }

            Balance += amount;
        }
    }

}
