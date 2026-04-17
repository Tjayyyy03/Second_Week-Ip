using Models;
using DataServiceLayer;
using System.Reflection.Metadata.Ecma335;
using System.Transactions;

namespace BusinessLayer
{
    // Business Layer 
    // where every validation goes

    public class BusinessLayer
    {
        //private DataServiceLayer.DataServiceLayer dataService = new();
        //
        //private JsonDataService dataService = new();

        private DBDataService dataService = new();

        // create account (user account)
        public bool CreateAccount(string fullName, string cellNum, string username, string password)
        {
            return dataService.CreateUserAccount(fullName, cellNum, username, password);
        }

        public bool CheckLoginCredentials(string username, string password)
        {
            return dataService.CheckLoginCredentials(username, password);

        }

        public UserBills CheckUserHasBill(string username, string billType)
        {
            return dataService.CheckUserHasBills(username, billType);
        }

        public PaymentAccountsModel GetPaymentAccounts(string username, string paymentType)
        {
            return dataService.GetPaymentAccountType(username, paymentType);
        }

        public bool ValidateAccountName(string username, string accountName, string paymentType)
        {
            var account = GetPaymentAccounts(username, paymentType);

            if (account == null)
            {
                return false;
            }

            if (account.AccountName == accountName)
            {
                return true;
            }

            return false;
        }

        public bool ValidateAccountNumber(string username, string accountNumber, string paymentType)
        {
            var account = GetPaymentAccounts(username, paymentType);

            if (account == null)
            {
                return false;
            }

            if (account.AccountNumber == accountNumber)
            {
                return true;
            }

            return false;
        }

        public bool CheckSufficientBalance(string username, decimal amount, string paymentType)
        {
            var account = GetPaymentAccounts(username, paymentType);

            if (account == null)
            {
                return false;
            }

            if (account.Balance >= amount)
            {
                return true;
            }

            return false;
        }

        public decimal ProcessPayment(string username, string paymentType, decimal billAmount, string billType)
        {
            var account = GetPaymentAccounts(username, paymentType);

            if (account == null)
            {
                return -1;
            }

            if (account.Balance < billAmount)
            {
                return -1;
            }

            account.Balance -= billAmount;

            dataService.RemoveUserBill(username, billType);
            SaveTransaction(username, billType, billAmount, paymentType);
            return account.Balance;
        }

        public void SaveTransaction(string username, string billType, decimal amount, string paymentType)
        {
            dataService.AddBillTransaction(username, billType, amount, paymentType);
        }

        public List<BillTransaction> GetUserBillTransactions(string username)
        {
            return dataService.GetUserTransactions(username);
        }
        public List<UserAccount> GetUserAccDetails(string username)
        {
            return dataService.GetUserAccDetails(username);
        }

        public List<PaymentAccountsModel> RetrieveUserPaymentAcc(string username, string paymentType)
        {
            return dataService.RetrieveUserPaymentAcc(username, paymentType);
        }

        public bool CreatePaymentAccount(string username, string accountName, string accountNumber, decimal balance, string paymentType)
        {
            return dataService.CreatePaymentAccount(username, accountName, accountNumber, balance, paymentType);
        }

        public bool UpdateUserProfile(string username, string fullName, string cellphoneNumber, string password)
        {
            return dataService.UpdateUserAccount(username, fullName, cellphoneNumber, password);
        }

        public bool UpdatePaymentAccount(string username, string paymentType, string accountName, string accountNumber, decimal balance)
        {
            return dataService.UpdatePaymentAccount(username, paymentType, accountName, accountNumber, balance);
        }
    }



}
