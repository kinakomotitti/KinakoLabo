namespace Patterns
{
    #region using
    using System;
    using System.Collections.Generic;
    #endregion

    /// <summary>
    /// なんらかの文法規則をもった文書(プログラム言語)を解析し、
    /// その結果得られた手順(命令)に基づき処理を実行していくというパターンです。
    /// </summary>
    class No15_Interpreter_Pattern
    {
        static void Main()
        {
            List<Expression> tree = new List<Expression>();
            tree.Add(new ThousandExpression());
            tree.Add(new HundredExpression());
            tree.Add(new TenExpression());
            tree.Add(new OneExpression());

            Context context = new Context("MCMXXVIII");
            tree.ForEach(exp => exp.Interpret(context));
            Console.WriteLine("{0} = {1}", "MCMXXVIII", context.Output);
        }

        class Context
        {
            private string _input;
            private int _output;
            public Context(string input)
            {
                this._input = input;
            }

            public string Input
            {
                get { return _input; }
                set { _input = value; }
            }

            public int Output
            {
                get { return _output; }
                set { _output = value; }
            }
        }

        abstract class Expression
        {
            public void Interpret(Context context)
            {
                if (context.Input.Length == 0)return;
                if (context.Input.StartsWith(Nine()))
                {
                    context.Output += (9 * Multiplier());
                    context.Input = context.Input.Substring(2);
                }
                else if (context.Input.StartsWith(Four()))
                {
                    context.Output += (4 * Multiplier());
                    context.Input = context.Input.Substring(2);
                }
                else if (context.Input.StartsWith(Five()))
                {
                    context.Output += (5 * Multiplier());
                    context.Input = context.Input.Substring(1);
                }

                while (context.Input.StartsWith(One()))
                {
                    context.Output += (1 * Multiplier());
                    context.Input = context.Input.Substring(1);
                }
            }

            public abstract string One();
            public abstract string Four();
            public abstract string Five();
            public abstract string Nine();
            public abstract int Multiplier();
        }

        class ThousandExpression : Expression
        {
            public override string One() { return "M"; }
            public override string Four() { return " "; }
            public override string Five() { return " "; }
            public override string Nine() { return " "; }
            public override int Multiplier() { return 1000; }
        }

        class HundredExpression : Expression
        {
            public override string One() { return "C"; }
            public override string Four() { return "CD"; }
            public override string Five() { return "D"; }
            public override string Nine() { return "CM"; }
            public override int Multiplier() { return 100; }
        }

        class TenExpression : Expression
        {
            public override string One() { return "X"; }
            public override string Four() { return "XL"; }
            public override string Five() { return "L"; }
            public override string Nine() { return "XC"; }
            public override int Multiplier() { return 10; }
        }

        class OneExpression : Expression
        {
            public override string One() { return "I"; }
            public override string Four() { return "IV"; }
            public override string Five() { return "V"; }
            public override string Nine() { return "IX"; }
            public override int Multiplier() { return 1; }
        }
    }
}