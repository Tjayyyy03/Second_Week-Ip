using Models;
using System.Text.Json;

namespace DataServiceLayer
{
    public class JsonDataService
    {

        //bugsssss
        //TODO:
        // 1. kahit bayad na di parin nawawasan sa json yung balance ng mga accounts
        // 2. sa payment ng bills, isa lang or yung una lang sa json yung nakukuha nya. kapag 
        //    madaming electricity bill, yung pinaka una lang nakukuha


        private string accountsFile = "accounts.json";
        private string usersFile = "users.json";
        private string billsFile = "bills.json";
        private string transactionsFile = "transactions.json";

        private List<PaymentAccountsModel> _accounts = new();
        private List<UserAccount> _user = new();
        private List<UserBills> _bills = new();
        private List<BillTransaction> _transactions = new();

        public JsonDataService()
        {
            LoadAccounts();
            LoadUsers();
            LoadBills();
            LoadTransactions();
        }


        private void LoadAccounts()
        {
            if (!File.Exists(accountsFile))
            {
                _accounts = new();
                return;
            }

            string json = File.ReadAllText(accountsFile);
            _accounts = JsonSerializer.Deserialize<List<PaymentAccountsModel>>(json)
                        ?? new List<PaymentAccountsModel>();
        }

        private void LoadUsers()
        {
            if (!File.Exists(usersFile))
            {
                _user = new();
                return;
            }

            string json = File.ReadAllText(usersFile);
            _user = JsonSerializer.Deserialize<List<UserAccount>>(json)
                    ?? new List<UserAccount>();
        }

        private void LoadBills()
        {
            if (!File.Exists(billsFile))
            {
                _bills = new();
                return;
            }

            string json = File.ReadAllText(billsFile);
            _bills = JsonSerializer.Deserialize<List<UserBills>>(json)
                    ?? new List<UserBills>();
        }

        private void LoadTransactions()
        {
            if (!File.Exists(transactionsFile))
            {
                _transactions = new();
                return;
            }

            string json = File.ReadAllText(transactionsFile);
            _transactions = JsonSerializer.Deserialize<List<BillTransaction>>(json)
                    ?? new List<BillTransaction>();
        }

        private void SaveAccounts()
        {
            File.WriteAllText(accountsFile,
                JsonSerializer.Serialize(_accounts, new JsonSerializerOptions { WriteIndented = true }));
        }

        private void SaveUsers()
        {
            File.WriteAllText(usersFile,
                JsonSerializer.Serialize(_user, new JsonSerializerOptions { WriteIndented = true }));
        }

        private void SaveBills()
        {
            File.WriteAllText(billsFile,
                JsonSerializer.Serialize(_bills, new JsonSerializerOptions { WriteIndented = true }));
        }

        private void SaveTransactions()
        {
            File.WriteAllText(transactionsFile,
                JsonSerializer.Serialize(_transactions, new JsonSerializerOptions { WriteIndented = true }));
        }


        public List<PaymentAccountsModel> AllAccounts()
        {
            LoadAccounts();
            return _accounts;
        }

        public bool CreatePaymentAccount(string username, string accountName, string accountNumber, decimal balance, string paymentType)
        {
            try
            {
                LoadAccounts();

                username = username.ToUpper();

                _accounts.Add(new PaymentAccountsModel
                {
                    Username = username,
                    AccountName = accountName,
                    AccountNumber = accountNumber,
                    Balance = balance,
                    PaymentType = paymentType
                });

                SaveAccounts();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public PaymentAccountsModel GetPaymentAccountType(string username, string paymentType)
        {
            LoadAccounts();

            username = username.ToUpper();

            for (int i = 0; i < _accounts.Count; i++)
            {
                if (_accounts[i].Username.ToUpper() == username &&
                    _accounts[i].PaymentType == paymentType)
                {
                    return _accounts[i];
                }
            }

            return null;
        }

        public bool UpdatePaymentAccount(string username, string paymentType, string accountName, string accountNumber, decimal balance)
        {
            LoadAccounts();

            username = username.ToUpper();

            for (int i = 0; i < _accounts.Count; i++)
            {
                if (_accounts[i].Username.ToUpper() == username &&
                    _accounts[i].PaymentType == paymentType)
                {
                    _accounts[i].AccountName = accountName;
                    _accounts[i].AccountNumber = accountNumber;
                    _accounts[i].Balance = balance;

                    SaveAccounts();
                    return true;
                }
            }

            return false;
        }

        public bool CreateUserAccount(string fullName, string cellNum, string username, string password)
        {
            try
            {
                LoadUsers();

                username = username.ToUpper();

                _user.Add(new UserAccount
                {
                    Username = username,
                    Password = password,
                    CellphoneNumber = cellNum,
                    FullName = fullName
                });

                SaveUsers();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckLoginCredentials(string username, string password)
        {
            LoadUsers();

            username = username.ToUpper();

            for (int i = 0; i < _user.Count; i++)
            {
                if (_user[i].Username.ToUpper() == username &&
                    _user[i].Password == password)
                {
                    return true;
                }
            }

            return false;
        }

        public List<UserAccount> GetUserAccDetails(string username)
        {
            LoadUsers();

            username = username.ToUpper();

            List<UserAccount> result = new();

            for (int i = 0; i < _user.Count; i++)
            {
                if (_user[i].Username.ToUpper() == username)
                {
                    result.Add(_user[i]);
                }
            }

            return result;
        }

        public bool UpdateUserAccount(string username, string fullName, string cellphoneNumber, string password)
        {
            LoadUsers();

            username = username.ToUpper();

            for (int i = 0; i < _user.Count; i++)
            {
                if (_user[i].Username.ToUpper() == username)
                {
                    _user[i].FullName = fullName;
                    _user[i].CellphoneNumber = cellphoneNumber;
                    _user[i].Password = password;

                    SaveUsers();
                    return true;
                }
            }

            return false;
        }

        public UserBills CheckUserHasBills(string username, string billType)
        {
            LoadBills();

            username = username.ToUpper();

            for (int i = 0; i < _bills.Count; i++)
            {
                if (_bills[i].Username.ToUpper() == username &&
                    _bills[i].BillType == billType)
                {
                    return _bills[i];
                }
            }

            return null;
        }

        public bool RemoveUserBill(string username, string billType)
        {
            LoadBills();

            username = username.ToUpper();

            for (int i = 0; i < _bills.Count; i++)
            {
                if (_bills[i].Username.ToUpper() == username &&
                    _bills[i].BillType == billType)
                {
                    _bills.RemoveAt(i);
                    SaveBills();
                    return true;
                }
            }

            return false;
        }


        public bool AddBillTransaction(string username, string billType, decimal amount, string paymentType)
        {
            try
            {
                LoadTransactions();

                username = username.ToUpper();

                _transactions.Add(new BillTransaction
                {
                    Username = username,
                    BillType = billType,
                    Amount = amount,
                    PaymentType = paymentType,
                    DatePaid = DateTime.Now
                });

                SaveTransactions();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<BillTransaction> GetUserTransactions(string username)
        {
            LoadTransactions();

            username = username.ToUpper();

            List<BillTransaction> result = new();

            for (int i = 0; i < _transactions.Count; i++)
            {
                if (_transactions[i].Username.ToUpper() == username)
                {
                    result.Add(_transactions[i]);
                }
            }

            return result;
        }

        public List<PaymentAccountsModel> RetrieveUserPaymentAcc(string username, string paymentType)
        {
            LoadAccounts();

            username = username.ToUpper();

            List<PaymentAccountsModel> matchAcc = new();

            for (int i = 0; i < _accounts.Count; i++)
            {
                if (_accounts[i].Username.ToUpper() == username &&
                    _accounts[i].PaymentType == paymentType)
                {
                    matchAcc.Add(_accounts[i]);
                }
            }

            return matchAcc;
        }
    }
}