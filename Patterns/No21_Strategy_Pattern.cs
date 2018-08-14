namespace Patterns
{
    #region using
    using System;
    using System.Collections.Generic;
    #endregion

    /// <summary>
    /// 
    /// </summary>
    class No21_Strategy_Pattern
    {
        static void Main()
        {
            SortedList studentRecords = new SortedList();
            studentRecords.Add("Samual");
            studentRecords.Add("Jimmy");
            studentRecords.Add("Sandra");
            studentRecords.Add("Vivek");
            studentRecords.Add("Anna");

            studentRecords.SetSortStrategy(new QuickSort());
            studentRecords.Sort();
            studentRecords.SetSortStrategy(new ShellSort());
            studentRecords.Sort();
            studentRecords.SetSortStrategy(new MergeSort());
            studentRecords.Sort();
        }

        abstract class SortStrategy
        {
            public abstract void Sort(List<string> list);
        }

        class QuickSort : SortStrategy
        {
            public override void Sort(List<string> list)
            {
                list.Sort(); // Default is Quicksort
                Console.WriteLine("QuickSorted list ");
            }
        }

        class ShellSort : SortStrategy
        {
            public override void Sort(List<string> list)
            {
                //list.ShellSort(); not-implemented

                Console.WriteLine("ShellSorted list ");
            }
        }

        class MergeSort : SortStrategy
        {

            public override void Sort(List<string> list)
            {
                //list.MergeSort(); not-implemented
                Console.WriteLine("MergeSorted list ");
            }
        }

        class SortedList
        {
            private List<string> _list = new List<string>();
            private SortStrategy _sortstrategy = new QuickSort();
            public void SetSortStrategy(SortStrategy sortstrategy)
            {
                //TODO 「♠主処理カートリッジを入れ替えていろいろな処理を実行することができる」
                this._sortstrategy = sortstrategy;
            }

            public void Add(string name)
            {
                _list.Add(name);
            }

            public void Sort()
            {
                
                _sortstrategy.Sort(_list);
                foreach (string name in _list)
                {
                    Console.WriteLine(" " + name);
                }
            }
        }
    }
}