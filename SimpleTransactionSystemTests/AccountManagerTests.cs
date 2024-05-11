using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Moq;
using SimpleTransactionSystem.Data;
using SimpleTransactionSystem.Managers;
using SimpleTransactionSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SimpleTransactionSystemTests
{
    public class AccountManagerTests
    {
        [Fact]
        public void TransferFunds_ValidTransfer_SuccessfulTransaction()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase5")
                .Options;

            using (var dbContext = new ApiContext(options))
            {
                var fromAccount = dbContext.Accounts.SingleOrDefault(x=> x.AccountId == 1);
                var toAccount = dbContext.Accounts.SingleOrDefault(x=> x.AccountId == 2); 

                if (fromAccount == null)
                {
                    fromAccount = new SavingsAccount { AccountId = 1, Balance = 1000 };
                    dbContext.Accounts.Add(fromAccount);
                }
                else
                {
                    fromAccount.Balance = 1000; // Update the balance
                }

                if (toAccount == null)
                {
                    toAccount = new ChequingAccount { AccountId = 2, Balance = 500 };
                    dbContext.Accounts.Add(toAccount);
                }
                else
                {
                    toAccount.Balance = 500; // Update the balance
                }
                try
                {
                    dbContext.SaveChanges();
                }
                catch (Exception ex) { }
                var accountManager = new AccountManager(dbContext);

                // Act
                accountManager.TransferFunds(1, 2, 500);

                // Assert
                Assert.Equal(500, fromAccount.Balance);
                Assert.Equal(1000, toAccount.Balance);
              
            }
        }

        [Fact]
        public void GetTransactionHistory_ValidAccountId_ReturnsTransactions()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase5")
                .Options;

            using (var dbContext = new ApiContext(options))
            {
                var transactions = new List<SimpleTransactionSystem.Models.Transaction>
            {
                new SimpleTransactionSystem.Models.Transaction {  FromAccountId = 1, ToAccountId = 2, Amount = 100, Timestamp = DateTime.Now },
                new SimpleTransactionSystem.Models.Transaction { FromAccountId = 2, ToAccountId = 1, Amount = 200, Timestamp = DateTime.Now }
            };

                dbContext.Transactions.AddRange(transactions);
                dbContext.SaveChanges();

                var accountManager = new AccountManager(dbContext);

                // Act
                var result = accountManager.GetTransactionHistory(1);

                // Assert
                Assert.True( result.Count()>0);
            }

        }

        [Fact]
        public void TransferFunds_InvalidWithdraw_ThrowsException()
        {

            // Arrange
            var options = new DbContextOptionsBuilder<ApiContext>()
               .UseInMemoryDatabase(databaseName: "TestDatabase5")
               .Options;

            using (var dbContext = new ApiContext(options))
            {

                var fromAccount = dbContext.Accounts.SingleOrDefault(x => x.AccountId == 3);
                var toAccount = dbContext.Accounts.SingleOrDefault(x => x.AccountId == 4);

                if (fromAccount == null)
                {
                    fromAccount = new SavingsAccount { AccountId = 3, Balance = 500 };
                    dbContext.Accounts.Add(fromAccount);
                }
                else
                {
                    fromAccount.Balance = 500; // Update the balance
                }

                if (toAccount == null)
                {
                    toAccount = new SavingsAccount { AccountId = 4, Balance = 500 };
                    dbContext.Accounts.Add(toAccount);
                }
                else
                {
                    toAccount.Balance = 500; // Update the balance
                }

                dbContext.SaveChanges();


                AccountManager accountManager = new AccountManager(dbContext); 

                // Act & Assert
                var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                accountManager.TransferFunds(3, 4, 600); // Withdraw amount (600) exceeds balance (500)
            });

                Assert.Equal("Insufficient funds for withdrawal.", exception.Message);
            }
        }

        [Fact]
        public void TransferFunds_ChequingAccountOverdraftLimit_IncurFee()
        {
            var options = new DbContextOptionsBuilder<ApiContext>()
               .UseInMemoryDatabase(databaseName: "TestDatabase5")
               .Options;

            using (var dbContext = new ApiContext(options))
            {
                var fromAccount = dbContext.Accounts.SingleOrDefault(x => x.AccountId == 2);
                var toAccount = dbContext.Accounts.SingleOrDefault(x => x.AccountId == 1);

                if (fromAccount == null)
                {
                    fromAccount = new ChequingAccount { AccountId = 2, Balance = 500 };
                    dbContext.Accounts.Add(fromAccount);
                }
                else
                {
                    fromAccount.Balance = 500; // Update the balance
                }

                if (toAccount == null)
                {
                    toAccount = new SavingsAccount { AccountId = 1, Balance = 500 };
                    dbContext.Accounts.Add(toAccount);
                }
                else
                {
                    toAccount.Balance = 500; // Update the balance
                }

                dbContext.SaveChanges();

                AccountManager accountManager = new AccountManager (dbContext);
                // Act
                accountManager.TransferFunds(2, 1, 1500); // Withdraw amount (1500) exceeds balance (500) and overdraft limit (1000) of the chequing account

                // Assert
                Assert.Equal(2000, toAccount.Balance); // Balance is reduced by the full withdrawal amount
                Assert.Equal(-1020, fromAccount.Balance); // Balance is reduced by the withdrawal amount,and withdrawal fee is applied
         
            }
        }

        
    
   
    }
}
