namespace Patterns
{
    #region using
    using System;
    using System.Collections.Generic;
    #endregion

    /// <summary>
    /// 「データ構造」と「それに対する処理」を分離することを目的とするパターンです。
    /// </summary>
    class No23_Visitor_Pattern
    {
        static void Main()
        {
            Employees e = new Employees();
            e.Attach(new Clerk());
            e.Attach(new Director());
            e.Attach(new President());

            e.Accept(new IncomeVisitor());
            e.Accept(new VacationVisitor());
        }

        interface IVisitor
        {
            void Visit(Element element);
        }

        #region Visitor

        class IncomeVisitor : IVisitor
        {
            public void Visit(Element element)
            {
                BaseEmployee employee = element as BaseEmployee;
                employee.Income *= 1.10;
                Console.WriteLine("{0} {1}'s new income: {2:C}", employee.GetType().Name, employee.Name, employee.Income);
            }
        }

        class VacationVisitor : IVisitor
        {
            public void Visit(Element element)
            {
                BaseEmployee employee = element as BaseEmployee;
                employee.VacationDays += 3;
                Console.WriteLine("{0} {1}'s new vacation days: {2}", employee.GetType().Name, employee.Name, employee.VacationDays);
            }
        }

        #endregion

        #region Employee

        abstract class Element

        {

            public abstract void Accept(IVisitor visitor);

        }

        class BaseEmployee : Element
        {
            private string _name;
            private double _income;
            private int _vacationDays;

            public BaseEmployee(string name, double income, int vacationDays)
            {
                this._name = name;
                this._income = income;
                this._vacationDays = vacationDays;
            }

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            public double Income
            {
                get { return _income; }
                set { _income = value; }
            }

            public int VacationDays
            {
                get { return _vacationDays; }
                set { _vacationDays = value; }
            }

            public override void Accept(IVisitor visitor)
            {
                //TODO 「☸POCOとビジネスプロセスの分離の表現」
                //「データ構造」と「それに対する処理」が分離する瞬間。
                //「データ構造」が自分自身を「処理」に引き渡す。
                visitor.Visit(this);
            }
        }

        #endregion

        #region 個々のEmployee情報クラス
        class Clerk : BaseEmployee
        {
            public Clerk() : base("Hank", 25000.0, 14) { }
        }

        class Director : BaseEmployee
        {
            public Director() : base("Elly", 35000.0, 16) { }
        }

        class President : BaseEmployee
        {
            public President() : base("Dick", 45000.0, 21) { }
        }
        #endregion

        class Employees
        {
            private List<BaseEmployee> _employees = new List<BaseEmployee>();

            public void Attach(BaseEmployee employee)
            {
                _employees.Add(employee);
            }

            public void Detach(BaseEmployee employee)
            {
                _employees.Remove(employee);
            }

            public void Accept(IVisitor visitor)
            {
                foreach (BaseEmployee e in _employees)
                {
                    e.Accept(visitor);
                }
                Console.WriteLine();
            }
        }
    }
}