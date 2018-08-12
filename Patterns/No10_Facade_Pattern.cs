namespace Patterns
{
    #region using
    using System;
    #endregion

    /// <summary>
    /// 「Facade(ファサード)」という英単語は、「建物の正面」を意味します。
    /// このパターンは、複雑な内部処理(データベース処理・業務処理etc)を隠蔽し、
    /// 利用者にシンプルなインタフェースを提供するパターンです。
    /// </summary>
    class No10_Facade_Pattern
    {
        static void Main()
        {
            // Facade
            Mortgage mortgage = new Mortgage();

            Customer customer = new Customer("Ann McKinsey");
            bool eligible = mortgage.IsEligible(customer, 125000);
            Console.WriteLine("\n" + customer.Name + " has been " + (eligible ? "Approved" : "Rejected"));
        }
    }

    #region 複雑な処理が定義されたクラスたち

    /// <summary>
    /// The 'Subsystem ClassA' class
    /// </summary>
    class Bank
    {
        public bool HasSufficientSavings(Customer c, int amount)
        {
            Console.WriteLine("Check bank for " + c.Name);
            return true;
        }
    }

    /// <summary>
    /// The 'Subsystem ClassB' class
    /// </summary>
    class Credit
    {
        public bool HasGoodCredit(Customer c)
        {
            Console.WriteLine("Check credit for " + c.Name);
            return true;
        }
    }

    /// <summary>
    /// The 'Subsystem ClassC' class
    /// </summary>
    class Loan
    {
        public bool HasNoBadLoans(Customer c)
        {
            Console.WriteLine("Check loans for " + c.Name);
            return true;
        }
    }

    #endregion

    /// <summary>
    /// Customer class
    /// </summary>
    class Customer
    {
        private string _name;

        // Constructor
        public Customer(string name)
        {
            this._name = name;
        }

        // Gets the name
        public string Name
        {
            get { return _name; }
        }
    }

    /// <summary>
    /// The 'Facade' class
    /// </summary>
    class Mortgage
    {
        private Bank _bank = new Bank();
        private Loan _loan = new Loan();
        private Credit _credit = new Credit();

        public bool IsEligible(Customer cust, int amount)
        {
            Console.WriteLine("{0} applies for {1:C} loan\n", cust.Name, amount);
            bool eligible = true;
            //TODO 「複雑な処理（ここでは条件分離）の手順を隠蔽する。」
            // Check creditworthyness of applicant
            if (!_bank.HasSufficientSavings(cust, amount)) eligible = false;
            else if (!_loan.HasNoBadLoans(cust)) eligible = false;
            else if (!_credit.HasGoodCredit(cust)) eligible = false;
            return eligible;
        }
    }
}