namespace Patterns.Labo
{
    #region using
    using System;
    using System.Collections.Generic;
    #endregion

    class No_21_14_Sample
    {
        /// <summary>
        /// StrategyPatternを参考に「入力チェック処理」の実装を簡単にしてみた例
        /// </summary>
        public void Main()
        {
            //実際には、画面からの入力値や、argsの中身が入ってくる
            string input = "aabcde";

            //どのチェックを行うかをListにまとめる
			//Strategyパターンはここの考え方に利用
            var checkList = new List<IChecker>()
            {
				//必要なチェッククラスをリストに登録
				new NumberCheck(),
                new HalfAlphanumericCheck(),
                new FullWidthAlphanumericCheck()
            };

            //チェック処理をまとめたメソッドに必要な情報を渡して結果を取得する(複雑 or よく利用する処理の集約)
            //個々の考え方はFacadeパターンやMediatorパターンに近い
            var result = ValidateManager.Validate(input, checkList, out var errorMessage);

            //処理結果による条件分岐
            if (result == false) errorMessage.ForEach((message) => Console.WriteLine(message));
        }
    }

    #region 入力チェックパターンの実装
    interface IChecker
    {
        bool ExecCheck(string targetString);
    }
    public class NumberCheck : IChecker
    {
        bool IChecker.ExecCheck(string targetString)
        {
            return true;
        }
    }
    public class HalfAlphanumericCheck : IChecker
    {
        bool IChecker.ExecCheck(string targetString)
        {
            return true;
        }
    }
    public class FullWidthAlphanumericCheck : IChecker
    {
        bool IChecker.ExecCheck(string targetString)
        {
            return true;
        }
    }
    #endregion

    #region ValidateManager

    static class ValidateManager
    {
        public static bool Validate(string targetStr, List<IChecker> checkList, out List<string> ErrorMessage)
        {
            ErrorMessage = new List<string>();
            var localMessage = new List<string>();
            bool allResult = false;
            checkList.ForEach(checker =>
            {
                bool result = checker.ExecCheck(targetStr);
                if (result == false)
                {
                    allResult = result;
                    localMessage.Add($"入力値 【{targetStr} 】は 【{checker.GetType().Name}】チェックで失敗しました");
                }
            });

            ErrorMessage = localMessage;
            return allResult;
        }

        #endregion

    }
}