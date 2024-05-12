# Account Transaction System

This is a simple account transaction system implemented using .NET/C#. It includes two types of accounts: Savings and Chequing. The system allows transactions between any two accounts and offers the ability to query transaction history through API endpoints.

## Links

- GitHub Repository: [account-transaction-system](https://github.com/Aakash145/account-transaction-system)

## Tech Stack

- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.InMemory (For InMemory Database)
- Moq (used for mocking the app)
- XUnit (for Test Driven Development)

## Project Structure

The project is divided into two sections:

1. **SimpleTransactionSystem**: Contains Data Model, Managers, and Controller for the Web API Application.
2. **SimpleTransactionSystemTests**: Contains unit tests for the application.

## Functionality

- Two types of accounts:
  1. Savings (balance must not be negative)
  2. Chequing (balance can go down to -1000, but overdrawn transactions incur a $20 fee)
- Transactions between any two accounts
- Query transaction history through API endpoints

## How to Run

To clone and run the application, follow these steps:

1. Run the following command in CMD: git clone https://github.com/Aakash145/account-transaction-system.git
   
2. Double-Click the SimpleTransactionSystem.sln to open the solution in Visual Studio.

## Concurrency Issues, Errors, and Validation

- **Concurrency Issues**: Implement optimistic concurrency control using entity versioning or timestamps.
- **Errors Handling**: Use try-catch blocks to handle exceptions gracefully and return appropriate error messages to the client.
- **Validation**: Implement data validation at the API level using Data Annotations or FluentValidation. Validate input data before processing transactions.

## Unit Tests

The project includes unit tests for the core functionality. These tests are located in the **SimpleTransactionSystemTests** project. They are written using XUnit framework to ensure the reliability and correctness of the code.

## Extensibility

The design of classes, interfaces, and data models is done with extensibility in mind for future database integration. You can easily switch from an in-memory data store to a persistent database by implementing the required interfaces.


