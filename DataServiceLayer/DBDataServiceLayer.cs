using Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace DataServiceLayer
{
    public class DBDataService
    {
        static string connectionString =
            "Data Source=DESKTOP-1PU2QVR\\SQLEXPRESS;Initial Catalog=BillingSystem;Integrated Security=True;TrustServerCertificate=True;";

        static SqlConnection sqlConnection;

        public DBDataService()
        {
            sqlConnection = new SqlConnection(connectionString);
        }

        public bool CreateUserAccount(string fullName, string cellNum, string username, string password)
        {
            string query = @"
                INSERT INTO tbl_UserAccounts (FullName, CellphoneNumber, Username, Password)
                VALUES (@FullName, @CellNum, @Username, @Password)";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            cmd.Parameters.AddWithValue("@FullName", fullName);
            cmd.Parameters.AddWithValue("@CellNum", cellNum);
            cmd.Parameters.AddWithValue("@Username", username.ToUpper());
            cmd.Parameters.AddWithValue("@Password", password);

            sqlConnection.Open();
            int result = cmd.ExecuteNonQuery();
            sqlConnection.Close();

            return result > 0;
        }

        public bool CheckLoginCredentials(string username, string password)
        {
            string query = @"
                SELECT 1 FROM tbl_UserAccounts
                WHERE UPPER(Username)=UPPER(@Username)
                AND Password=@Password";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);

            sqlConnection.Open();
            var reader = cmd.ExecuteReader();

            bool exists = reader.Read();

            sqlConnection.Close();

            return exists;
        }

        public List<UserAccount> GetUserAccDetails(string username)
        {
            List<UserAccount> list = new();

            string query = @"
                SELECT * FROM tbl_UserAccounts
                WHERE UPPER(Username)=UPPER(@Username)";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            cmd.Parameters.AddWithValue("@Username", username);

            sqlConnection.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new UserAccount
                {
                    FullName = reader["FullName"].ToString(),
                    CellphoneNumber = reader["CellphoneNumber"].ToString(),
                    Username = reader["Username"].ToString(),
                    Password = reader["Password"].ToString()
                });
            }

            sqlConnection.Close();

            return list;
        }

        public bool UpdateUserAccount(string username, string fullName, string cellphoneNumber, string password)
        {
            string query = @"
                UPDATE tbl_UserAccounts
                SET FullName=@FullName,
                    CellphoneNumber=@Cell,
                    Password=@Password
                WHERE UPPER(Username)=UPPER(@Username)";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@FullName", fullName);
            cmd.Parameters.AddWithValue("@Cell", cellphoneNumber);
            cmd.Parameters.AddWithValue("@Password", password);

            sqlConnection.Open();
            int result = cmd.ExecuteNonQuery();
            sqlConnection.Close();

            return result > 0;
        }

        public bool CreatePaymentAccount(string username, string accountName, string accountNumber, decimal balance, string paymentType)
        {
            string query = @"
                INSERT INTO tbl_PaymentAccounts (Username, AccountName, AccountNumber, Balance, PaymentType)
                VALUES (@Username,@Name,@Number,@Balance,@Type)";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            cmd.Parameters.AddWithValue("@Username", username.ToUpper());
            cmd.Parameters.AddWithValue("@Name", accountName);
            cmd.Parameters.AddWithValue("@Number", accountNumber);
            cmd.Parameters.AddWithValue("@Balance", balance);
            cmd.Parameters.AddWithValue("@Type", paymentType);

            sqlConnection.Open();
            int result = cmd.ExecuteNonQuery();
            sqlConnection.Close();

            return result > 0;
        }

        public List<PaymentAccountsModel> AllAccounts()
        {
            List<PaymentAccountsModel> list = new();

            string query = "SELECT * FROM tbl_PaymentAccounts";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            sqlConnection.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new PaymentAccountsModel
                {
                    Username = reader["Username"].ToString(),
                    AccountName = reader["AccountName"].ToString(),
                    AccountNumber = reader["AccountNumber"].ToString(),
                    Balance = Convert.ToDecimal(reader["Balance"]),
                    PaymentType = reader["PaymentType"].ToString()
                });
            }

            sqlConnection.Close();

            return list;
        }

        public PaymentAccountsModel GetPaymentAccountType(string username, string paymentType)
        {
            string query = @"
                SELECT TOP 1 * FROM tbl_PaymentAccounts
                WHERE UPPER(Username)=UPPER(@Username)
                AND PaymentType=@Type";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Type", paymentType);

            sqlConnection.Open();
            var reader = cmd.ExecuteReader();

            PaymentAccountsModel acc = null;

            if (reader.Read())
            {
                acc = new PaymentAccountsModel
                {
                    Username = reader["Username"].ToString(),
                    AccountName = reader["AccountName"].ToString(),
                    AccountNumber = reader["AccountNumber"].ToString(),
                    Balance = Convert.ToDecimal(reader["Balance"]),
                    PaymentType = reader["PaymentType"].ToString()
                };
            }

            sqlConnection.Close();

            return acc;
        }

        public bool UpdatePaymentAccount(string username, string paymentType, string accountName, string accountNumber, decimal balance)
        {
            string query = @"
                UPDATE tbl_PaymentAccounts
                SET AccountName=@Name,
                    AccountNumber=@Number,
                    Balance=@Balance
                WHERE UPPER(Username)=UPPER(@Username)
                AND PaymentType=@Type";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Type", paymentType);
            cmd.Parameters.AddWithValue("@Name", accountName);
            cmd.Parameters.AddWithValue("@Number", accountNumber);
            cmd.Parameters.AddWithValue("@Balance", balance);

            sqlConnection.Open();
            int result = cmd.ExecuteNonQuery();
            sqlConnection.Close();

            return result > 0;
        }

        public UserBills CheckUserHasBills(string username, string billType)
        {
            string query = @"
                SELECT TOP 1 * FROM tbl_Bills
                WHERE UPPER(Username)=UPPER(@Username)
                AND BillType=@Type";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Type", billType);

            sqlConnection.Open();
            var reader = cmd.ExecuteReader();

            UserBills bill = null;

            if (reader.Read())
            {
                bill = new UserBills
                {
                    Username = reader["Username"].ToString(),
                    BillType = reader["BillType"].ToString(),
                    BillAmount = Convert.ToDecimal(reader["BillAmount"])
                };
            }

            sqlConnection.Close();

            return bill;
        }

        public bool RemoveUserBill(string username, string billType)
        {
            string query = @"
                DELETE FROM tbl_Bills
                WHERE UPPER(Username)=UPPER(@Username)
                AND BillType=@Type";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Type", billType);

            sqlConnection.Open();
            int result = cmd.ExecuteNonQuery();
            sqlConnection.Close();

            return result > 0;
        }

        public bool AddBillTransaction(string username, string billType, decimal amount, string paymentType)
        {
            string query = @"
                INSERT INTO tbl_Transactions (Username, BillType, Amount, PaymentType, DatePaid)
                VALUES (@Username,@Bill,@Amount,@Type,GETDATE())";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            cmd.Parameters.AddWithValue("@Username", username.ToUpper());
            cmd.Parameters.AddWithValue("@Bill", billType);
            cmd.Parameters.AddWithValue("@Amount", amount);
            cmd.Parameters.AddWithValue("@Type", paymentType);

            sqlConnection.Open();
            int result = cmd.ExecuteNonQuery();
            sqlConnection.Close();

            return result > 0;
        }

        public List<BillTransaction> GetUserTransactions(string username)
        {
            List<BillTransaction> list = new();

            string query = @"
                SELECT * FROM tbl_Transactions
                WHERE UPPER(Username)=UPPER(@Username)";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            cmd.Parameters.AddWithValue("@Username", username);

            sqlConnection.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new BillTransaction
                {
                    Username = reader["Username"].ToString(),
                    BillType = reader["BillType"].ToString(),
                    Amount = Convert.ToDecimal(reader["Amount"]),
                    PaymentType = reader["PaymentType"].ToString(),
                    DatePaid = Convert.ToDateTime(reader["DatePaid"])
                });
            }

            sqlConnection.Close();

            return list;
        }

        public List<PaymentAccountsModel> RetrieveUserPaymentAcc(string username, string paymentType)
        {
            List<PaymentAccountsModel> list = new();

            string query = @"
                SELECT * FROM tbl_PaymentAccounts
                WHERE UPPER(Username)=UPPER(@Username)
                AND PaymentType=@Type";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Type", paymentType);

            sqlConnection.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new PaymentAccountsModel
                {
                    Username = reader["Username"].ToString(),
                    AccountName = reader["AccountName"].ToString(),
                    AccountNumber = reader["AccountNumber"].ToString(),
                    Balance = Convert.ToDecimal(reader["Balance"]),
                    PaymentType = reader["PaymentType"].ToString()
                });
            }

            sqlConnection.Close();

            return list;
        }
    }
}