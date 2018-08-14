namespace Patterns
{
    #region using
    using System;
    #endregion

    /// <summary>
    /// 
    /// </summary>
    class No18_Memento_Pattern
    {
        static void Main()
        {
            SalesProspect s = new SalesProspect();
            s.Name = "Noel van Halen";
            s.Phone = "(412) 256-0990";
            s.Budget = 25000.0;
            ProspectMemory m = new ProspectMemory();
            m.Memento = s.SaveMemento();

            s.Name = "Leo Welch";
            s.Phone = "(310) 209-7111";
            s.Budget = 1000000.0;
            s.RestoreMemento(m.Memento);
        }


        class SalesProspect
        {
            private string _name;
            private string _phone;
            private double _budget;

            public string Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                    Console.WriteLine("Name:  " + _name);
                }
            }

            public string Phone
            {
                get { return _phone; }
                set
                {
                    _phone = value;
                    Console.WriteLine("Phone: " + _phone);
                }
            }

            public double Budget
            {
                get { return _budget; }
                set
                {
                    _budget = value;
                    Console.WriteLine("Budget: " + _budget);
                }
            }

            public Memento SaveMemento()
            {
                Console.WriteLine("\nSaving state --\n");
                return new Memento(_name, _phone, _budget);
            }

            public void RestoreMemento(Memento memento)
            {
                Console.WriteLine("\nRestoring state --\n");
                this.Name = memento.Name;
                this.Phone = memento.Phone;
                this.Budget = memento.Budget;
            }
        }

        class Memento
        {
            private string _name;
            private string _phone;
            private double _budget;

            public Memento(string name, string phone, double budget)
            {
                this._name = name;
                this._phone = phone;
                this._budget = budget;
            }

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            public string Phone
            {
                get { return _phone; }
                set { _phone = value; }
            }

            public double Budget
            {
                get { return _budget; }
                set { _budget = value; }
            }
        }

        class ProspectMemory
        {
            private Memento _memento;

            public Memento Memento
            {
                set { _memento = value; }
                get { return _memento; }
            }
        }
    }
}