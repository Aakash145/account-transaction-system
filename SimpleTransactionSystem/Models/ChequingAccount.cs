namespace SimpleTransactionSystem.Models
{
    public class ChequingAccount : Account
    {
        private const decimal OverdraftLimit = 1000;
        private const decimal OverdraftFee = 20;
        private decimal OverdraftAmount = 0;
       
        public ChequingAccount()
        {
            AccountType = "Chequing";
            OverdraftAmount = 0;
        }

        public override void Withdraw(decimal amount)
        {
            if(Balance < amount)
            {
                if (amount - (Balance + OverdraftAmount )> OverdraftLimit)
                {
                    throw new InvalidOperationException("Exceeded overdraft limit.");
                }
                else
                {
                    OverdraftAmount += (amount - Balance);
                    Balance -= OverdraftFee; // Apply overdraft fee
                }
            }

            Balance -= amount;

        }

        public override void Deposit(decimal amount)
        {
            if(OverdraftAmount > 0)
            {
                if(amount > OverdraftAmount)
                {
                    OverdraftAmount = 0;
                }
                else
                {
                    OverdraftAmount -= amount;
                }
            }
            base.Deposit(amount);
        }
    }
}
