namespace Patterns
{
    #region using
    using System;
    #endregion

    /// <summary>
    /// このパターンは、ある要求の受取り対象となる複数のオブジェクトに鎖状の関係を構築し、
    /// 要求を処理する事が可能なオブジェクトに渡るまで、
    /// 順次、構築した鎖状の関係に沿って要求を受流していくパターンです。
    /// </summary>
    class No13_ChainOfResponsibility_Pattern
    {
        static void Main()
        {
            Approver larry = new Director();
            Approver sam = new VicePresident();
            Approver tammy = new President();
            larry.SetSuccessor(sam);
            sam.SetSuccessor(tammy);

            Purchase p = new Purchase(2034, 350.00, "Assets");
            larry.ProcessRequest(p);
            p = new Purchase(2035, 32590.10, "Project X");
            larry.ProcessRequest(p);
            p = new Purchase(2036, 122100.00, "Project Y");
            larry.ProcessRequest(p);
        }

        abstract class Approver
        {
            protected Approver successor;
            public void SetSuccessor(Approver successor)
            {
                this.successor = successor;
            }
            //このメソッドを実装したものについて
            //TODO 「再帰呼び出しによる繰り返し処理に適したクラス構造」
            public abstract void ProcessRequest(Purchase purchase);
        }

        class Director : Approver
        {
            public override void ProcessRequest(Purchase purchase)
            {
                if (purchase.Amount < 10000.0)
                {
                    Console.WriteLine("{0} approved request# {1}",
                      this.GetType().Name, purchase.Number);
                }
                else if (successor != null)
                {
                    successor.ProcessRequest(purchase);
                }
            }
        }

        class VicePresident : Approver
        {
            public override void ProcessRequest(Purchase purchase)
            {
                if (purchase.Amount < 25000.0)
                {
                    Console.WriteLine("{0} approved request# {1}", this.GetType().Name, purchase.Number);
                }
                else if (successor != null)
                {
                    successor.ProcessRequest(purchase);
                }
            }
        }

        class President : Approver
        {
            public override void ProcessRequest(Purchase purchase)
            {
                if (purchase.Amount < 100000.0)
                {
                    Console.WriteLine("{0} approved request# {1}", this.GetType().Name, purchase.Number);
                }
                else
                {
                    Console.WriteLine(
                      "Request# {0} requires an executive meeting!",
                      purchase.Number);
                }
            }
        }

        class Purchase
        {
            private int _number;
            private double _amount;
            private string _purpose;

            // Constructor
            public Purchase(int number, double amount, string purpose)
            {
                this._number = number;
                this._amount = amount;
                this._purpose = purpose;
            }

            // Gets or sets purchase number
            public int Number
            {
                get { return _number; }
                set { _number = value; }
            }

            // Gets or sets purchase amount
            public double Amount
            {
                get { return _amount; }
                set { _amount = value; }
            }

            // Gets or sets purchase purpose
            public string Purpose
            {
                get { return _purpose; }
                set { _purpose = value; }
            }
        }
    }
}