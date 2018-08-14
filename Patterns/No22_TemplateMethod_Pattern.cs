namespace Patterns
{
    #region using
    using System;
    using System.Data;
    using System.Data.OleDb;
    #endregion

    /// <summary>
    /// 
    /// </summary>
    class No22_TemplateMethod_Pattern
    {
        static void Main()
        {
            DataAccessObject daoCategories = new Categories();
            daoCategories.Run();

            DataAccessObject daoProducts = new Products();
            daoProducts.Run();
        }

        //TODO 「★Baseクラスを使って見ましょう的な感じ」
        abstract class DataAccessObject
        {
            protected string connectionString;
            protected DataSet dataSet;

            public virtual void Connect()
            {
                connectionString =string.Empty;
            }

            public virtual void Disconnect()
            {
                connectionString = string.Empty;
            }

            public abstract void Select();
            public abstract void Process();

            public void Run()
            {
                Connect();
                Select();
                Process();
                Disconnect();
            }
        }

        class Categories : DataAccessObject
        {
            public override void Select()
            {
                string sql = "select CategoryName from Categories";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connectionString);
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "Categories");
            }

            public override void Process()
            {
                Console.WriteLine("Categories ---- ");
                DataTable dataTable = dataSet.Tables["Categories"];
                foreach (DataRow row in dataTable.Rows)
                {
                    Console.WriteLine(row["CategoryName"]);
                }
                Console.WriteLine();
            }
        }

        class Products : DataAccessObject
        {
            public override void Select()
            {
                string sql = "select ProductName from Products";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connectionString);
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "Products");
            }

            public override void Process()
            {
                Console.WriteLine("Products ---- ");
                DataTable dataTable = dataSet.Tables["Products"];
                foreach (DataRow row in dataTable.Rows)
                {
                    Console.WriteLine(row["ProductName"]);
                }
                Console.WriteLine();
            }
        }
    }
}