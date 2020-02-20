using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ExportCsv
{
    class Program
    {


        #region Declarations
        #endregion

        #region Property
        #endregion

        #region Memberfunction
        static void Main(string[] args)
        {
            //用Windows身分驗證
            //String strCon = "Data Source=MY-PC;Initial Catalog=test;Integrated Security=SSPI;";
            //用SQL Server身分驗證
            String strCon = @"Data Source=xxxx;Initial Catalog=xxxx;User Id=xxxx;Password=xxxx;";

            //連接資料庫設定
            SqlConnection conn = new SqlConnection(strCon);
            conn.Open();
            string sqlstr = "select * from AccBal";

            //執行SQL指令
            SqlCommand cmd = new SqlCommand(sqlstr, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            List<AccBalData> dataCollection = new List<AccBalData>();
            while (dr.Read())
            {
                AccBalData dt = new AccBalData();
                dt.Cur = dr["Cur"].ToString();
                dt.Amt = (decimal)dr["Amt"];
                 var temp = dr["LogDate"].ToString();
                dt.Date = temp.Substring(0, temp.IndexOf(" "));

                dataCollection.Add(dt);
            }

            cmd.Cancel();
            dr.Close();
            conn.Close();
            conn.Dispose();


            using (var file = new StreamWriter(@"D:\Data.csv"))
            {
                file.WriteLine("Cur,Amt,Date");
                foreach (var item in dataCollection)
                    file.WriteLine($"{item.Cur},{item.Amt},{item.Date}");
            }
        }

        #endregion

        #region DataModel
        public class AccBalData
        {
            public string Cur { get; set; }
            public decimal Amt{get;set;}
            public string Date { get; set; }

        }
        #endregion


    }
}
