using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleTransactionSystem.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        
        //validate if the accounts are different and amount is non negative
        
        public void Validate()
        {

            if (FromAccountId <= 0 || ToAccountId <= 0)
            {
                throw new ArgumentException("Invalid account IDs.");
            }

            if (Amount <= 0)
            {
                throw new ArgumentException("Transaction amount must be greater than zero.");
            }
        }
    }

}
