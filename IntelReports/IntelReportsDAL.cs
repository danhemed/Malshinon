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
                    string query = "INSERT INTO intel_reports (Id, Reporter_Id, Target_Id, Text, Time_Stamp)" +
                        "VALUE (@Id, @Reporter_Id, @Target_Id, @Text, @Time_Stamp)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", reports.Id);
                    cmd.Parameters.AddWithValue("@Reporter_Id", reports.ReporterId);
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

        public List<IntelReports> GetAllIntelReports()
        {
            try
            {
                var reports = new List<IntelReports>();
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    var cmd = new MySqlCommand("SELECT * FROM intel_reports", conn);
                    var reader = cmd.ExecuteReader();
                    reports.Add(IntelReports.createFromReader(reader));
                    while (reader.Read())
                    {
                        IntelReports report = IntelReports.createFromReader(reader);
                        reports.Add(report);
                    }
                }
                return reports;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
                return null;
            }
        }

        public List<IntelReports> GetAllIntelReportsOfReporter(string secretCode)
        {
            try
            {
                var reports = new List<IntelReports>();
                PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
                int personId = peopleDAL.GetIdOfPerson(secretCode);
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    var cmd = new MySqlCommand("SELECT * FROM intel_reports WHERE reporter_id = @ReporterId", conn);
                    var reader = cmd.ExecuteReader();
                    cmd.Parameters.AddWithValue("@ReporterId", personId);
                    reports.Add(IntelReports.createFromReader(reader));
                    while (reader.Read())
                    {
                        IntelReports report = IntelReports.createFromReader(reader);
                        reports.Add(report);
                    }
                }
                return reports;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
                return null;
            }
        }

        public IntelReports GetReport(string secretCode)
        {
            try
            {
                PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
                int personId = peopleDAL.GetIdOfPerson(secretCode);
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    var cmd = new MySqlCommand("SELECT * FROM intel_Reports WHERE reporter_id = @ReporterId", conn);
                    cmd.Parameters.AddWithValue("@ReporterId", personId);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return IntelReports.createFromReader(reader);
                    }
                }
                return null;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
                return null;
            }
        }

        public void UpdateReport(int targetId, string text, string secretCode)
        {
            try
            {
                PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
                int personId = peopleDAL.GetIdOfPerson(secretCode);

                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    string query = @"UPDATE intel_reports SET target_id = @TargetId, text = @Text" +
                                " WHERE reporter_id = @ReporterId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ReporterId", personId);
                    cmd.Parameters.AddWithValue("@TargetId", targetId);
                    cmd.Parameters.AddWithValue("@Text", text);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Updated Person successfully!");
                    }
                    else
                    {
                        Console.WriteLine("The update person was not successful!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
            }
        }

        public void DeleteReport(string secretCode)
        {
            try
            {
                PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
                int personId = peopleDAL.GetIdOfPerson(secretCode);
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    string queryReports = @"DELETE FROM intel_reports WHERE reporter_id = @Reporter_Id";
                    MySqlCommand cmdReports = new MySqlCommand(queryReports, conn);
                    cmdReports.Parameters.AddWithValue("@Reporter_Id", personId);
                    int rowsAffectedReports = cmdReports.ExecuteNonQuery();

                    if (rowsAffectedReports > 0)
                    {
                        Console.WriteLine("Delete reports of person successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
            }
        }

        public int GetReportStats(string secretCode)
        {
            try
            {
                IntelReportsDAL intelReportsDAL = new IntelReportsDAL(_sqlData);
                List<IntelReports> reports = intelReportsDAL.GetAllIntelReportsOfReporter(secretCode);
                int counter = 0;
                foreach (var report in reports)
                {
                    counter += report.Text.Length;
                }
                return counter;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
                return 0;
            }
        }

        public void CreateTarget(string text)
        {
            string firstName = "";
            string lastName = "";
            string[] textList = text.Split(' ');
            int count = 0;
            foreach (string word in textList)
            {
                if (word == word.ToUpper())
                {
                    if (count == 0)
                    {
                        firstName = word;
                        count++;
                    }
                    else if (count == 1)
                    {
                        lastName = word;
                        break;
                    }
                }
            }
            People newPerson = new People{ FirstName = firstName, LastName = lastName, Type = "target" };
            PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
            peopleDAL.AddPeople(newPerson);
        }

        public int GetTargetStats(string secretCode)
        {
            try
            {
                var people = new List<People>();
                PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
                int personId = peopleDAL.GetIdOfPerson(secretCode);
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    var cmd = new MySqlCommand("SELECT * FROM people WHERE id = @TargetId", conn);
                    cmd.Parameters.AddWithValue("@TargetId", personId);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        People person = People.createFromReader(reader);
                        people.Add(person);
                    }
                }
                return people.Count;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
                return 0;
            }
        }

    }
}