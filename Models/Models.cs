namespace Models
{
    // MODELS
    // data models - representation of data
    public class Models
    {

    }

    public class PaymentAccountsModel
    {
        public string Username { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string PaymentType { get; set; }
    }

    public class UserAccount
    {
        public string FullName { get; set; }
        public string CellphoneNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserBills
    {
        public string Username { get; set; }
        public string BillType { get; set; }
        public decimal BillAmount { get; set; }
    }

    public class BillTransaction
    {
        public string Username { get; set; }
        public string BillType { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; }
        public DateTime DatePaid { get; set; }
    }




}

