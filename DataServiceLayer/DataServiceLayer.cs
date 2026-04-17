using Microsoft.VisualBasic;
using Models;
using System.Transactions;

namespace DataServiceLayer
{
    // Data service layer
    // where data talks to storage (db, json, etc)
    public class DataServiceLayer
    {
        private List<PaymentAccountsModel> _accounts = new();
        private List<UserAccount> _user = new();
        private List<UserBills> _bills = new();
        private List<BillTransaction> _transactions = new();
        public DataServiceLayer()
        {
            LoadAccounts();


            _user = new List<UserAccount>
            {
                new UserAccount
                {
                    Username = "tom",
                    Password = "tom123"
                }
            };

            _bills = new List<UserBills>
            {
                new UserBills
                {
                    Username = "TOM",
                    BillAmount = 370,
                    BillType = "Product"
                }
            };

        }

        public void LoadAccounts()
        {
            _accounts = new List<PaymentAccountsModel>
            {
                new PaymentAccountsModel
                {
                    Username = "TOM",
                    AccountName = "Gcash Acc1",
                    AccountNumber = "G001",
                    Balance = 500,
                    PaymentType = "GCash"
                },
                new PaymentAccountsModel
                {
                    Username = "TOM",
                    AccountName = "Maya Acc1",
                    AccountNumber = "M001",
                    Balance = 5000,
                    PaymentType = "Maya"
                }
            };
        }

        // get list of all accounts
        public List<PaymentAccountsModel> AllAccounts()
        {
            return _accounts;
        }

        // create account (user account)
        public bool CreateUserAccount(string fullName, string cellNum, string username, string password)
        {
            try
            {
                _user = new List<UserAccount>
                {
                    new UserAccount
                    {
                        Username = username,
                        Password = password,
                        CellphoneNumber = cellNum,
                        FullName = fullName
                    }
                };

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckLoginCredentials(string username, string password)
        {
            try
            {
                for (int i = 0; i < _user.Count; i++)
                {
                    if (_user[i].Username == username)
                    {
                        return true;
                    }

                    if (_user[i].Password == password)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public UserBills CheckUserHasBills(string username, string billType)
        {
            for (int i = 0; i < _bills.Count; i++)
            {
                if (_bills[i].Username == username && _bills[i].BillType == billType)
                {
                    return _bills[i];
                }
            }
            return null;

        }

        public PaymentAccountsModel GetPaymentAccountType(string username, string paymentType)
        {
            for (int i = 0; i < _accounts.Count; i++)
            {
                if (_accounts[i].Username == username && _accounts[i].PaymentType == paymentType)
                {
                    return _accounts[i];
                }
            }
            return null;
        }

        public bool RemoveUserBill(string username, string billType)
        {
            for (int i = 0; i < _bills.Count; i++)
            {
                if (_bills[i].Username == username &&
                _bills[i].BillType == billType)
                {
                    _bills.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public bool AddToTransaction(string username, string billType, decimal billAmount, string paymentType)
        {
            try
            {
                _transactions.Add(new BillTransaction
                {
                    Username = username,
                    BillType = billType,
                    Amount = billAmount,
                    PaymentType = paymentType,
                    DatePaid = DateTime.Now
                });
                return true;
            }
            catch
            {
                return false;
            }

        }

        public void AddBillTransaction(string username, string billType, decimal amount, string paymentType)
        {
            _transactions.Add(new BillTransaction
            {
                Username = username,
                BillType = billType,
                Amount = amount,
                PaymentType = paymentType,
                DatePaid = DateTime.Now
            });
        }

        public List<BillTransaction> GetUserTransactions(string username)
        {
            List<BillTransaction> result = new();

            for (int i = 0; i < _transactions.Count; i++)
            {
                if (_transactions[i].Username.ToUpper() == username.ToUpper())
                {
                    result.Add(_transactions[i]);
                }
            }

            return result;
        }

        // 3. 
        public List<UserAccount> GetUserAccDetails(string username)
        {
            List<UserAccount> loggedUser = new();
            for (int i = 0; i < _user.Count; i++)
            {
                if (_user[i].Username.ToUpper() == username.ToUpper())
                {
                    loggedUser.Add(_user[i]);
                }
            }

            return loggedUser;
        }

        public List<PaymentAccountsModel> RetrieveUserPaymentAcc(string username, string paymentType)
        {
            List<PaymentAccountsModel> matchAcc = new();
            for (int i = 0; i < _accounts.Count; i++)
            {
                if (_accounts[i].Username == username && _accounts[i].PaymentType == paymentType)
                {
                    matchAcc.Add(_accounts[i]);
                }
            }

            return matchAcc;
        }

        public bool CreatePaymentAccount(string username, string accountName, string accountNumber, decimal balance, string paymentType)
        {
            try
            {
                _accounts.Add(new PaymentAccountsModel
                {
                    Username = username,
                    AccountName = accountName,
                    AccountNumber = accountNumber,
                    Balance = balance,
                    PaymentType = paymentType
                });

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateUserAccount(string username, string fullName, string cellphoneNumber, string password)
        {
            for (int i = 0; i < _user.Count; i++)
            {
                if (_user[i].Username.ToUpper() == username.ToUpper())
                {
                    _user[i].FullName = fullName;
                    _user[i].CellphoneNumber = cellphoneNumber;
                    _user[i].Password = password;

                    return true;
                }
            }

            return false;
        }

        public bool UpdatePaymentAccount(string username, string paymentType, string accountName, string accountNumber, decimal balance)
        {
            for (int i = 0; i < _accounts.Count; i++)
            {
                if (_accounts[i].Username.ToUpper() == username.ToUpper() &&
                    _accounts[i].PaymentType == paymentType)
                {
                    _accounts[i].AccountName = accountName;
                    _accounts[i].AccountNumber = accountNumber;
                    _accounts[i].Balance = balance;

                    return true;
                }
            }

            return false;
        }

    }
}
