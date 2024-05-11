namespace SimpleTransactionSystem.Models
{
    public class SavingsAccount : Account
    {
        public SavingsAccount()
        {
            AccountType = "Savings";
        }
        public override void Withdraw(decimal amount)
        {
            if (amount > Balance)
            {
                throw new InvalidOperationException("Insufficient funds for withdrawal.");
            }

            base.Withdraw(amount);
        }

        public override void Deposit(decimal amount)
        {
            base.Deposit(amount);
        }
    }

}
