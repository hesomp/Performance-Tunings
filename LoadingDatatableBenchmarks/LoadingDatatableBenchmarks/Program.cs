using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LoadingDatatableBenchmarks
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var data = GetData();
            var numOfColumns = data[0].Split().Length;
            var numOfRows = data.Length;
          
            var sw = new Stopwatch();
            
            sw.Start();
            LoadDatatableSelectMethod(data, GetDataTable(numOfColumns, numOfRows));
            sw.Stop();
            Console.WriteLine(String.Format("Loading datatable using select method: {0}ms" ,sw.ElapsedMilliseconds));
            sw.Restart();
            LoadDatatableForMethod(data, GetDataTable(numOfColumns, numOfRows));
            Console.WriteLine(String.Format("Loading datatable using for loop method: {0}ms", sw.ElapsedMilliseconds));
            sw.Restart();
            LoadDatatableWithArrays(data, GetDataTable(numOfColumns, numOfRows));
            sw.Stop();
            Console.WriteLine(String.Format("Loading datatable using arrays: {0}ms", sw.ElapsedMilliseconds));

            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        }


        static private DataTable GetDataTable(int numOfColumns, int numOfRows)
        {
            DataTable dt = new DataTable();
            DataColumn[] key = new DataColumn[1];

            dt.Columns.Add(new DataColumn("key"));
            key[0] = dt.Columns[0];

            for (int i = 0; i <= numOfColumns - 1; i++)
            {
                var col = new DataColumn("Col" + i);
                dt.Columns.Add(col);
            }

            for (int i = 0; i <= numOfRows - 1; i++)
            {
                var row = dt.NewRow();
                row[0] = i;
                dt.Rows.Add(row);
            }

            dt.PrimaryKey = key;

            return dt;
        }


        static private string[] GetData()
        {
            string[] retVal = null;

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LoadingDatatableBenchmarks.marketing.data"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                retVal = result.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            }

            return retVal;
        }


        static private void LoadDatatableSelectMethod(string[] data, DataTable dt)
        {

            for (int i = 0; i <= data.Length - 1; i++)
            {
                var row = dt.Select(string.Format("key='{0}'", i));

                if (row.Length > 0)
                {
                    var tmp = data[i].Split();
                    for (int j = 0; j <= tmp.Length - 1; j++)
                    {
                        row[0][j + 1] = tmp[j];
                    }
                }

            }

        }


        static private void LoadDatatableForMethod(string[] data, DataTable dt)
        {

            for (int i = 0; i <= data.Length - 1; i++)
            {
                DataRow row = null;
                foreach (DataRow dr in dt.Rows)
                {
                    if ((string)dr[0] == i.ToString())
                    {
                        row = dr;
                        break;
                    }
                }

                if (row != null)
                {
                    var tmp = data[i].Split();
                    for (int j = 0; j <= tmp.Length - 1; j++)
                    {
                        row[j + 1] = tmp[j];
                    }
                }


            }

        }

        
        static private void LoadDatatableWithArrays(string[] data, DataTable dt)
        {

            var rows = new List<object[]>();
            foreach (DataRow dr in dt.Rows)
            {
                var itemRow = dr.ItemArray;
                int key = -1;

                if (int.TryParse(itemRow[0].ToString(), out key))
                {
                    var tmp = data[key].Split();

                    for (int j = 0; j <= tmp.Length - 1; j++)
                    {
                        itemRow[j + 1] = tmp[j];
                    }
                }

                rows.Add(itemRow);
            }

            //remove rows, re-add via list
            dt.Rows.Clear();
            foreach (var row in rows)
            {
                dt.Rows.Add(row);
            }

        }

    }
}
