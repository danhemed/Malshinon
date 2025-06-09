using System;
using Malshinon.Models;
using Malshinon.DataBase;
using MySql.Data.MySqlClient;

namespace Malshinon.Models
{
    public class IntelReportsDAL
    {
        private MySqlData _sqlData;

        public IntelReportsDAL(MySqlData sqlData)
        {
            _sqlData = sqlData;
        }

        public void AddReports(IntelReports reports)
        {
            try
            {
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    string query = "INSERT INTO people (Id, Reporter_Id, Target_Id, Text, Time_Stamp)" +
                        "VALUE (@Id, @Reporter_Id, @Target_Id, @Text, @Time_Stamp)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", reports.Id);
                    cmd.Parameters.AddWithValue("@Reports_Id", reports.ReporterId);
                    cmd.Parameters.AddWithValue("@Target_Id", reports.TargetId);
                    cmd.Parameters.AddWithValue("@Text", reports.Text);
                    cmd.Parameters.AddWithValue("@Time_Stamp", reports.TimeStamp);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
            }
        }
    }
}