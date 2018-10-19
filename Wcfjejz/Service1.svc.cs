using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Configuration;
using Wcfjejz.公共类;

namespace Wcfjejz
{
    public class Service1 : IService1
    {
        private static StreamWriter streamWriter;
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        string connString = ConfigurationManager.ConnectionStrings["connString"].ToString();
        /*实现异常日志方法*/
        public void WriteError(string message)
        {
            try
            {
                string directPath = ConfigurationManager.AppSettings["LogFilePath"].ToString().Trim();    //获得文件夹路径
                if (!Directory.Exists(directPath))   //判断文件夹是否存在，如果不存在则创建
                {
                    Directory.CreateDirectory(directPath);
                }
                directPath += string.Format(@"\{0}.log", DateTime.Now.ToString("yyyy-MM-dd"));
                if (streamWriter == null)
                {
                    streamWriter = !File.Exists(directPath) ? File.CreateText(directPath) : File.AppendText(directPath);    //判断文件是否存在如果不存在则创建，如果存在则添加。
                }
                streamWriter.WriteLine("***********************************************************************");
                streamWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
                streamWriter.WriteLine("输出信息：错误信息");
                if (message != null)
                {
                    streamWriter.WriteLine("异常信息：\r\n" + message);
                }
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Flush();
                    streamWriter.Dispose();
                    streamWriter = null;
                }
            }
        }
        /*实现结转方法*/
        public DataSet Exchange(DateTime date1, DateTime date2)
        {
            using(OleDbConnection conn = new OleDbConnection(connString))
            {
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CFJE_TEST100";
                cmd.Parameters.Add("date1", OleDbType.Date).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("date2", OleDbType.Date).Direction = ParameterDirection.Input;
                cmd.Parameters["date1"].Value = date1;
                cmd.Parameters["date2"].Value = date2;
                cmd.ExecuteNonQuery();

                string select = "SELECT E.RQ 日期,C.XM 医生姓名,E.ID 医生工号,E.YZJE 西药总金额,E.ZZJE 中成药总金额,E.CZJE 草药总金额 FROM YSCFJE_TEST E,ZGXX C WHERE E.ID=C.ID AND (E.RQ BETWEEN :date1 AND :date2) ORDER BY E.RQ";
                OleDbCommand cmd2 = new OleDbCommand(select, conn);
                cmd2.Parameters.AddWithValue("date1", date1);
                cmd2.Parameters.AddWithValue("date2", date2);
                DataSet ds = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter();
                da.SelectCommand = cmd2;
                da.Fill(ds);
                return ds;
            }
        }
        /*实现登陆方法*/
        public bool CheckInformation(string Code, string Password)
        {
            string sql = @"SELECT COUNT(*) FROM PUB_EMP WHERE ID = " + Code + " AND PASSWORD = " + Password + "";
            int? count = sqlDBHelper.GetCount(sql);
            if (count != null && count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /*用户权限管理*/
        public string GetPower(string Code)
        {
            string sql = @"SELECT POWER FROM PUB_EMP WHERE CODE = "+Code+"";
            string power = sqlDBHelper.GetScalar(sql).ToString();
            return power;
        }
    }
}
