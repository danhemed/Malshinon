using System;
using Malshinon.Models;
using Malshinon.DataBase;
using MySql.Data.MySqlClient;

namespace Malshinon.Models
{
    public class PeopleDAL
    {
        private MySqlData _sqlData;

        public PeopleDAL(MySqlData sqlData)
        {
            _sqlData = sqlData;
        }

        public void AddPeople(People people)
        {
            try
            {
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    string query = "INSERT INTO people (Id, FirstName, LastName, Secret_Code, Type, Num_Reports, Num_Mentions)" +
                        "VALUE (@Id, @FirstName, @LastName, @Secret_Code, @Type, @Num_Reports, @Num_Mentions)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", people.Id);
                    cmd.Parameters.AddWithValue("@FirstName", people.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", people.LastName);
                    cmd.Parameters.AddWithValue("@Secret_Code", people.SecretCode);
                    cmd.Parameters.AddWithValue("@Type", people.Type);
                    cmd.Parameters.AddWithValue("@Num_Reports", people.NumReports);
                    cmd.Parameters.AddWithValue("@Num_Mentions", people.NumMentions);
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