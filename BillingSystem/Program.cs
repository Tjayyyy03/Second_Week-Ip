using System;
using System.Diagnostics.Metrics;
using System.Xml.Serialization;
using static System.Collections.Specialized.BitVector32;

namespace BillingSystem
{
    // FRONTEND - UI LAYER
    internal class Program
    {
        static private BusinessLayer.BusinessLayer businessLayer = new();

        static void Main(string[] args)
        {
            var action = 0;

            while (true)
            {
                Console.WriteLine("WELCOME!!!!");
                LineFormatter();

                Console.WriteLine();
                Console.WriteLine("1. Sign In");
                Console.WriteLine("2. Log In");
                Console.WriteLine("3. Exit");

                LineFormatter();
                Console.Write("Action: ");

                // tryparse convert the input (string) to int, ! means if string input cannot be converted to int, means input is string therefore throws an error
                if (!int.TryParse(Console.ReadLine(), out action))
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: Input is not a number");
                    Console.WriteLine();
                    Console.Write("Press Any Key to try again...");
                    Console.ReadKey(); // will read any key press by user;

                    Console.Clear();
                    continue;
                }

                switch (action)
                {
                    case 1:
                        CreateAccount();
                        break;
                    case 2:
                        LoginAccount();
                        break;
                    case 3:
                        Console.WriteLine();
                        Console.Write("Exiting the program....");
                        Thread.Sleep(1000);
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Error: Enter a number from 1-3");
                        Console.WriteLine();
                        Console.Write("Press Any Key to try again...");
                        Console.ReadKey(); // will read any key press by user;

                        Console.Clear();
                        continue;
                }
            }
        }
        static void CreateAccount()
        {
            Console.Clear();
            Console.WriteLine("Create Account");
            LineFormatter();
            Console.Write("Full Name (Format: FN SN): ");
            var fullName = Console.ReadLine();

            // no validation as of now (TODO: check if input really starts with 09)
            Console.Write("Cellphone Number (09): ");
            var cellNum = Console.ReadLine();
            Console.Write("Username: ");
            var username = Console.ReadLine();

            Console.Write("Password: ");
            var password = Console.ReadLine();

            bool isTrue = businessLayer.CreateAccount(fullName, cellNum, username, password);

            if (isTrue)
            {
                Console.Write("Your Account is Created Successfully. Please wait...");
                Thread.Sleep(1500);
                Console.WriteLine();
                Console.Write("Redirecting you to dashboard. Please wait...");
                Thread.Sleep(1500);
                Console.Clear();
                Dashboard(username);
            }
            else
            {
                Console.WriteLine("Error: Failed to create your account");
                Environment.Exit(0);
            }
        }
        static void LoginAccount()
        {
            Console.Clear();
            var attempt = 3;

            do
            {
                Console.WriteLine("Log In to your Account");
                LineFormatter();

                Console.Write("Username: ");
                var username = Console.ReadLine();

                Console.Write("Password: ");
                var password = Console.ReadLine();

                var correctCredentials = businessLayer.CheckLoginCredentials(username, password);
                if (correctCredentials)
                {
                    Console.Write("Login Successfully. Please wait...");
                    Thread.Sleep(1500);
                    Console.WriteLine();
                    Console.Write("Redirecting you to dashboard. Please wait...");
                    Thread.Sleep(1500);
                    Console.Clear();
                    Dashboard(username.ToUpper());
                }
                else
                {
                    Console.WriteLine();
                    LineFormatter();
                    Console.WriteLine($"Error: Inccorect Credentials. \nTry Again. Attempt Remaining {attempt}");
                    LineFormatter();
                    Console.WriteLine();
                    Thread.Sleep(1000);
                    Console.Clear();
                    attempt--;
                }

            } while (attempt >= 0);
            Console.Write("To Many attempts. Please try again later.");
            Thread.Sleep(1500);
            Console.Clear();
            return; // return to main() method 
        }
        static void Dashboard(string username)
        {
            var action = 0;

            while (true)
            {
                Console.WriteLine($"WELCOME {username.ToUpper()}");
                LineFormatter();

                Console.WriteLine();
                Console.WriteLine("1. Bills");
                Console.WriteLine("2. Transaction History");
                Console.WriteLine("3. Payment Accounts and Profile");
                Console.WriteLine("4. Exit");

                LineFormatter();
                Console.Write("Action: ");

                // tryparse convert the input (string) to int, ! means if string input cannot be converted to int, means input is string therefore throws an error
                if (!int.TryParse(Console.ReadLine(), out action))
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: Input is not a number");
                    Console.WriteLine();
                    Console.Write("Press Any Key to try again...");
                    Console.ReadKey(); // will read any key press by user;

                    Console.Clear();
                    continue;
                }

                switch (action)
                {
                    case 1:
                        Console.Clear();
                        Bills(username);
                        break;
                    case 2:
                        Console.Clear();
                        ViewTransactionHistory(username);
                        // payment history method
                        break;
                    case 3:
                        Console.Clear();
                        ViewUserAccounts(username);
                        // account details method
                        break;
                    case 4:
                        Console.WriteLine();
                        Console.Write("Exiting the program....");
                        Thread.Sleep(1000);
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Error: Enter a number from 1-3");
                        Console.WriteLine();
                        Console.Write("Press Any Key to try again...");
                        Console.ReadKey(); // will read any key press by user;

                        Console.Clear();
                        continue;
                }
            }
        }

        // all methods connected to bill feature
        static void Bills(string username)
        {
            var billChoice = 0;

            while (true)
            {
                Console.WriteLine("Bill Type:");
                LineFormatter();

                Console.WriteLine("1. Product\r\n2. Electricity Bill\r\n3. Water Bill\r\n4. Back");

                LineFormatter();
                Console.Write("Select Bill Type: ");

                if (!int.TryParse(Console.ReadLine(), out billChoice))
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: Input is not a number");
                    Console.WriteLine();
                    Console.Write("Press Any Key to try again...");
                    Console.ReadKey(); // will read any key press by user;

                    Console.Clear();
                    continue;
                }

                if (billChoice < 1 || billChoice > 4)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: Enter 1-4 only");
                    Console.Write("Press any key to try again...");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }

                if (billChoice == 4)
                {
                    Console.WriteLine();
                    Console.Write("Please wait...");
                    Thread.Sleep(1000);
                    Console.Clear();
                    Dashboard(username.ToUpper());
                }

                var billType = GetBillType(billChoice);

                Console.WriteLine();
                Console.Clear();
                Console.WriteLine("Your Bills");

                var bills = businessLayer.CheckUserHasBill(username, billType);

                if (bills == null)
                {
                    LineFormatter();
                    Console.WriteLine("You have no Bills yet");
                    Console.WriteLine();
                    Console.Write("Press Any Key to try again...");
                    Console.ReadKey(); // will read any key press by user;
                    Console.Clear();
                    continue;
                }
                var billPrice = bills.BillAmount;

                if (billChoice == 1)
                {
                    BillPrice(username, 1, billPrice, billType);
                }
                else if (billChoice == 2)
                {
                    BillPrice(username, 2, billPrice, billType);
                }
                else if (billChoice == 3)
                {
                    BillPrice(username, 3, billPrice, billType);
                }

            }

        }
        static string GetBillType(int billChoice)
        {
            if (billChoice == 1)
            {
                return "Product";
            }
            else if (billChoice == 2)
            {
                return "Electricity Bill";
            }
            else if (billChoice == 3)
            {
                return "Water Bill";
            }
            else
            {
                return "";
            }
        }
        static void BillPrice(string username, int billChoice, decimal billPrice, string billType)
        {
            Console.Clear();

            Console.WriteLine("Your Bill");
            LineFormatter();
            Console.WriteLine($"{billType}: {billPrice}");
            LineFormatter();

            Console.WriteLine();

            var action = 0;
            while (true)
            {
                Console.WriteLine("Payment Method");
                LineFormatter();

                Console.WriteLine();
                Console.WriteLine("1. Gcash");
                Console.WriteLine("2. PayMaya");

                LineFormatter();
                Console.Write("Action: ");

                // tryparse convert the input (string) to int, ! means if string input cannot be converted to int, means input is string therefore throws an error
                if (!int.TryParse(Console.ReadLine(), out action))
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: Input is not a number");
                    Console.WriteLine();
                    Console.Write("Press Any Key to try again...");
                    Console.ReadKey(); // will read any key press by user;

                    Console.Clear();
                    continue;
                }

                switch (action)
                {
                    case 1:
                        PayWithGcash(username, "GCash", billType, billPrice);
                        return;
                    case 2:
                        PayWithMaya(username, "Maya", billType, billPrice);
                        return;
                    case 3:
                        Console.WriteLine();
                        Console.Write("Exiting the program....");
                        Thread.Sleep(1000);
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Error: Enter a number from 1-3");
                        Console.WriteLine();
                        Console.Write("Press Any Key to try again...");
                        Console.ReadKey(); // will read any key press by user;

                        Console.Clear();
                        continue;
                }
            }

        }
        static void PayWithGcash(string username, string paymentType, string billType, decimal billPrice)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Your Bill");
                LineFormatter();
                Console.WriteLine($"{billType}: {billPrice}");
                LineFormatter();

                Console.WriteLine();
                Console.WriteLine("Account Information: ");
                LineFormatter();
                Console.Write("Enter Account Name: ");
                var accountName = Console.ReadLine();

                Console.Write("Enter Account Number: ");
                var accountNum = Console.ReadLine();

                var accounts = businessLayer.GetPaymentAccounts(username, paymentType);

                if (!businessLayer.ValidateAccountName(username, accountName, paymentType)
                    || !businessLayer.ValidateAccountNumber(username, accountNum, paymentType))
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: Input do not match with your account credentials");
                    Console.WriteLine();
                    Console.Write("Press Any Key to try again...");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }

                Console.WriteLine();
                Console.WriteLine("Do you want to pay this bill? Yes or No?");
                Console.WriteLine($"Your Gcash Balance: {accounts.Balance}");
                LineFormatter();
                Console.Write("Answer: ");
                var answer = Console.ReadLine().ToLower().Trim();

                if (answer == "yes")
                {
                    if (!businessLayer.CheckSufficientBalance(username, billPrice, paymentType))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Error: Your balance is not enough. Returning..");
                        LineFormatter();
                        Console.WriteLine();
                        Thread.Sleep(1500);
                        Console.Clear();
                        return;
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Payment Cancelled. Returning to Dashboard...");

                    LineFormatter();
                    Thread.Sleep(1500);
                    Console.Clear();
                    Dashboard(username);
                }

                var newBalance = businessLayer.ProcessPayment(username, paymentType, billPrice, billType);

                if (newBalance != -1)
                {
                    Console.WriteLine();
                    LineFormatter();
                    Console.WriteLine("Payment Successful!");
                    Console.WriteLine($"New Balance: {newBalance}");
                    LineFormatter();
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
                else
                {
                    LineFormatter();
                    Console.WriteLine("Payment Failed (Insufficient balance or account not found)");
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
            }

        }
        static void PayWithMaya(string username, string paymentType, string billType, decimal billPrice)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Your Bill");
                LineFormatter();
                Console.WriteLine($"{billType}: {billPrice}");
                LineFormatter();

                Console.WriteLine();
                Console.WriteLine("Account Information: ");
                LineFormatter();
                Console.Write("Enter Account Name: ");
                var accountName = Console.ReadLine();

                Console.Write("Enter Account Number: ");
                var accountNum = Console.ReadLine();

                var accounts = businessLayer.GetPaymentAccounts(username, paymentType);

                if (!businessLayer.ValidateAccountName(username, accountName, paymentType)
                    || !businessLayer.ValidateAccountNumber(username, accountNum, paymentType))
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: Input do not match with your account credentials");
                    Console.WriteLine();
                    Console.Write("Press Any Key to try again...");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }

                Console.WriteLine();
                Console.WriteLine("Do you want to pay this bill? Yes or No?");
                Console.WriteLine($"Your Gcash Balance: {accounts.Balance}");
                LineFormatter();
                Console.Write("Answer: ");
                var answer = Console.ReadLine().ToLower().Trim();

                if (answer == "yes")
                {
                    if (!businessLayer.CheckSufficientBalance(username, billPrice, paymentType))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Error: Your balance is not enough. Returning..");
                        LineFormatter();
                        Console.WriteLine();
                        Thread.Sleep(1500);
                        Console.Clear();
                        return;
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Payment Cancelled. Returning to Dashboard...");
                    LineFormatter();
                    Thread.Sleep(1500);
                    Dashboard(username);
                    return;
                }

                var newBalance = businessLayer.ProcessPayment(username, paymentType, billPrice, billType);

                if (newBalance != -1)
                {
                    Console.WriteLine();
                    LineFormatter();
                    Console.WriteLine("Payment Successful!");
                    Console.WriteLine($"New Balance: {newBalance}");
                    LineFormatter();
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
                else
                {
                    LineFormatter();
                    Console.WriteLine("Payment Failed (Insufficient balance or account not found)");
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
            }
        }

        // all methods connected to payment acc and profile
        static void ViewUserAccounts(string username)
        {

            Console.WriteLine("User Profile");
            LineFormatter();

            foreach (var detail in businessLayer.GetUserAccDetails(username))
            {
                Console.WriteLine($"Name: {detail.FullName}");
                Console.WriteLine($"Cellphone Number: {detail.CellphoneNumber}");
                Console.WriteLine($"Username: {detail.Username}");
                Console.WriteLine($"Password: {detail.Password}");
            }

            Console.WriteLine();
            Console.WriteLine("GCash Account");
            LineFormatter();


            string paymentType = "GCash";
            var paymentAccounts = businessLayer.RetrieveUserPaymentAcc(username, paymentType);
            bool noGcashAcc = false;
            bool noMayaAcc = false;

            if (paymentAccounts.Count > 0)
            {
                foreach (var detail in paymentAccounts)
                {
                    Console.WriteLine($"Account Name: {detail.AccountName}");
                    Console.WriteLine($"Account Number: {detail.AccountNumber}");
                    Console.WriteLine($"Balance: {detail.Balance}");
                }
            }
            else
            {
                Console.WriteLine(" ---- No Gcash Account ----");
                noGcashAcc = true;
            }

            Console.WriteLine();
            Console.WriteLine("PayMaya Account");
            LineFormatter();

            paymentType = "Maya";
            paymentAccounts = businessLayer.RetrieveUserPaymentAcc(username, paymentType);

            if (paymentAccounts.Count > 0)
            {
                foreach (var detail in paymentAccounts)
                {
                    Console.WriteLine($"Account Name: {detail.AccountName}");
                    Console.WriteLine($"Account Number: {detail.AccountNumber}");
                    Console.WriteLine($"Balance: {detail.Balance}");
                }
            }
            else
            {
                Console.WriteLine(" ---- No PayMaya Account ----");
                noMayaAcc = true;
            }


            Console.WriteLine();
            Console.WriteLine();
            LineFormatter();

            if (noGcashAcc || noMayaAcc)
            {
                Console.WriteLine("No Payment Account? Create Now? ");
                Console.WriteLine("1. Yes\n2. No");
                LineFormatter();
                Console.Write("Action: ");
                var choice = Convert.ToInt16(Console.ReadLine());

                if (choice == 1)
                {
                    if (noGcashAcc)
                    {
                        Console.Clear();
                        CreateGCashAccount(username);
                    }
                    if (noMayaAcc)
                    {
                        Console.Clear();
                        CreateMayaAccount(username);
                    }
                }
                else
                {
                    Console.Clear();
                    return;
                }
            }
            else
            {
                while (true)
                {
                    Console.WriteLine("Update your Account?");
                    LineFormatter();
                    Console.WriteLine("1. Update User profile ");
                    Console.WriteLine("2. Update Gcash Account");
                    Console.WriteLine("3. Update PayMaya Account");
                    Console.WriteLine("4. Back");
                    LineFormatter();
                    Console.Write("Action: ");
                    var choice = Convert.ToInt16(Console.ReadLine());

                    if (choice == 1)
                    {
                        Console.Clear();
                        UpdateUserProfile(username);
                    }
                    else if (choice == 2)
                    {
                        Console.Clear();
                        UpdateGcashAccount(username, paymentType);
                    }
                    else if (choice == 3)
                    {
                        Console.Clear();
                        UpdateMayaAccount(username, paymentType);
                    }
                    else if (choice == 4)
                    {
                        Console.Clear();
                        return;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Error: Enter number from 1-3 only");
                        Console.WriteLine("Please any key to continue");
                        Console.ReadKey();
                        continue;
                    }
                }
            }
        }

        static void CreateGCashAccount(string username)
        {
            Console.WriteLine("CREATE GCASH ACCOUNT");
            LineFormatter();

            Console.Write("Account Name: ");
            var accountName = Console.ReadLine();

            Console.Write("Account Number: ");
            var accountNumber = Console.ReadLine();

            Console.Write("Deposit Inital Cash: ");
            var balance = Convert.ToDecimal(Console.ReadLine());

            var paymentType = "GCash";

            bool accCreated = businessLayer.CreatePaymentAccount(username, accountName, accountNumber, balance, paymentType);

            if (accCreated)
            {
                LineFormatter();
                Console.WriteLine("Account Creation Successfull");
                Console.Write("Press Any Key to Continue");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            else
            {
                LineFormatter();
                Console.WriteLine("Error: Failed to Create your Account. Try again");
                Console.Write("Press Any Key to Continue");
                Console.ReadKey();
                Console.Clear();
                return;
            }

        }

        static void CreateMayaAccount(string username)
        {
            Console.WriteLine("CREATE PAYMAYA ACCOUNT");
            LineFormatter();

            Console.Write("Account Name: ");
            var accountName = Console.ReadLine();

            Console.Write("Account Number: ");
            var accountNumber = Console.ReadLine();

            Console.Write("Deposit Inital Cash: ");
            var balance = Convert.ToDecimal(Console.ReadLine());

            var paymentType = "Maya";


            bool accCreated = businessLayer.CreatePaymentAccount(username, accountName, accountNumber, balance, paymentType);

            if (accCreated)
            {
                LineFormatter();
                Console.WriteLine("Account Creation Successfull");
                Console.Write("Press Any Key to Continue");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            else
            {
                LineFormatter();
                Console.WriteLine("Error: Failed to Create your Account. Try again");
                Console.Write("Press Any Key to Continue");
                Console.ReadKey();
                Console.Clear();
                return;
            }
        }

        static void UpdateUserProfile(string username)
        {
            Console.Clear();
            Console.WriteLine("UPDATE USER PROFILE");
            LineFormatter();

            Console.Write("New Full Name: ");
            var fullName = Console.ReadLine();

            Console.Write("New Cellphone Number: ");
            var cellNumber = Console.ReadLine();

            Console.Write("New Password: ");
            var password = Console.ReadLine();

            bool isUpdated = businessLayer.UpdateUserProfile(username, fullName, cellNumber, password);

            Console.WriteLine();
            if (isUpdated)
            {
                Console.WriteLine("Profile updated successfully!");
            }
            else
            {
                Console.WriteLine("Update failed. User not found.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static void UpdateGcashAccount(string username, string paymentType)
        {
            Console.Clear();
            Console.WriteLine("UPDATE GCASH ACCOUNT");
            LineFormatter();

            paymentType = "GCash";
            var accounts = businessLayer.RetrieveUserPaymentAcc(username, paymentType);

            if (accounts.Count == 0)
            {
                Console.WriteLine("No GCash account found.");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return;
            }

            foreach (var acc in accounts)
            {
                Console.WriteLine($"Current Account Name: {acc.AccountName}");
                Console.WriteLine($"Current Account Number: {acc.AccountNumber}");
                Console.WriteLine($"Current Balance: {acc.Balance}");
                Console.WriteLine();
            }

            Console.Write("New Account Name: ");
            var accountName = Console.ReadLine();

            Console.Write("New Account Number: ");
            var accountNumber = Console.ReadLine();

            Console.Write("New Balance: ");
            decimal balance;
            while (!decimal.TryParse(Console.ReadLine(), out balance))
            {
                Console.Write("Invalid input. Enter balance again: ");
            }

            bool isUpdated = businessLayer.UpdatePaymentAccount(
                username,
                paymentType,
                accountName,
                accountNumber,
                balance
            );

            Console.WriteLine();

            if (isUpdated)
            {
                Console.WriteLine("GCash account updated successfully!");
            }
            else
            {
                Console.WriteLine("Update failed.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static void UpdateMayaAccount(string username, string paymentType)
        {
            Console.Clear();
            Console.WriteLine("UPDATE PAYMAYA ACCOUNT");
            LineFormatter();

            paymentType = "Maya";
            var accounts = businessLayer.RetrieveUserPaymentAcc(username, paymentType);

            if (accounts.Count == 0)
            {
                Console.WriteLine("No PayMaya account found.");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return;
            }

            foreach (var acc in accounts)
            {
                Console.WriteLine($"Current Account Name: {acc.AccountName}");
                Console.WriteLine($"Current Account Number: {acc.AccountNumber}");
                Console.WriteLine($"Current Balance: {acc.Balance}");
                Console.WriteLine();
            }

            Console.Write("New Account Name: ");
            var accountName = Console.ReadLine();

            Console.Write("New Account Number: ");
            var accountNumber = Console.ReadLine();

            Console.Write("New Balance: ");
            decimal balance;
            while (!decimal.TryParse(Console.ReadLine(), out balance))
            {
                Console.Write("Invalid input. Enter balance again: ");
            }

            bool isUpdated = businessLayer.UpdatePaymentAccount(
                username,
                paymentType,
                accountName,
                accountNumber,
                balance
            );

            Console.WriteLine();

            if (isUpdated)
            {
                Console.WriteLine("PayMaya account updated successfully!");
            }
            else
            {
                Console.WriteLine("Update failed.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static void ViewTransactionHistory(string username)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("TRANSACTION HISTORY");
                LineFormatter();

                var transactions = businessLayer.GetUserBillTransactions(username);

                if (transactions.Count == 0)
                {
                    Console.WriteLine("No bill payments found.");
                }
                else
                {
                    for (int i = 0; i < transactions.Count; i++)
                    {
                        Console.WriteLine("Bill Type: " + transactions[i].BillType);
                        Console.WriteLine("Amount Paid: " + transactions[i].Amount);
                        Console.WriteLine("Payment Method: " + transactions[i].PaymentType);
                        Console.WriteLine("Date Paid: " + transactions[i].DatePaid);
                        Console.WriteLine("----------------------------");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("1. Refresh");
                Console.WriteLine("2. Back to Dashboard");
                LineFormatter();

                Console.Write("Action: ");
                var input = Console.ReadLine();

                if (input == "2")
                {
                    Console.Clear();
                    return;
                }
            }
        }




        static void LineFormatter()
        {
            Console.WriteLine("____________________________");
        }

    }


}
