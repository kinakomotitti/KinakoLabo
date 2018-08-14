namespace Patterns
{
    #region using
    using System;
    #endregion

    /// <summary>
    /// このパターンは、本人の代理人となるオブジェクトが本人でなくともできる処理を受持ち、
    /// 「どうしても本人でないと・・・」という場合のみ、本人に処理を任せるというパターンです。
    /// </summary>
    class No12_Proxy_Pattern
    {
        static void Main()
        {
            MathProxy proxy = new MathProxy();
            //Mathクラスの利用を意識せずに処理を実行できる
            Console.WriteLine("4 + 2 = " + proxy.Add(4, 2));
            Console.WriteLine("4 - 2 = " + proxy.Sub(4, 2));
            Console.WriteLine("4 * 2 = " + proxy.Mul(4, 2));
            Console.WriteLine("4 / 2 = " + proxy.Div(4, 2));
        }

        public interface IMath
        {
            double Add(double x, double y);
            double Sub(double x, double y);
            double Mul(double x, double y);
            double Div(double x, double y);
        }

        class Math : IMath
        {
            public double Add(double x, double y) { return x + y; }
            public double Sub(double x, double y) { return x - y; }
            public double Mul(double x, double y) { return x * y; }
            public double Div(double x, double y) { return x / y; }
        }

        class MathProxy : IMath
        {
            //TODO 「★Proxy対象のクラスの利用を代行する」
            //対象のクラス（ここではMath）の内部を容易に変更できない場合に、挙動を追加したり、
            //対象のクラスにバグがあったときに対応することができる（Adapter的な使い方？）
            private Math _math = new Math();

            public double Add(double x, double y)
            {
                return _math.Add(x, y);
            }
            public double Sub(double x, double y)
            {
                return _math.Sub(x, y);
            }
            public double Mul(double x, double y)
            {
                return _math.Mul(x, y);
            }
            public double Div(double x, double y)
            {
                return _math.Div(x, y);
            }
        }
    }
}