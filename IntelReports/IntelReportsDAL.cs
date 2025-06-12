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

        public List<IntelReports> GetAllIntelReportsByReporter(string secretCode)
        {
            try
            {
                var reports = new List<IntelReports>();
                PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
                int personId = peopleDAL.GetIdOfPerson(secretCode);
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    var cmd = new MySqlCommand("SELECT * FROM intel_reports WHERE reporter_id = @ReporterId", conn);
                    cmd.Parameters.AddWithValue("@ReporterId", personId);
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

        public void CreateReport(int reporterId)
        {
            PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
            IntelReportsDAL intelReportsDAL = new IntelReportsDAL(_sqlData);
            Console.WriteLine("Enter the First Name of target:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Enter the Last Name of target:");
            string lastName = Console.ReadLine();
            Console.WriteLine("Enter Why do you think he's a target:");
            string text = Console.ReadLine();
            string secretCode = peopleDAL.GenerateRandomCode();
            People person = new People { FirstName = firstName, LastName = lastName, SecretCode = secretCode, Type = "target" };
            peopleDAL.AddPeople(person);
            Console.WriteLine("Create New Person!!");
            IntelReports intelReports = new IntelReports { ReporterId = reporterId, TargetId = peopleDAL.GetIdOfPerson(secretCode), Text = text , TimeStamp = DateTime.Now};
            intelReportsDAL.AddReports(intelReports);
            Console.WriteLine("Create New Intel Reports!!");
            peopleDAL.UpdateReportsNum(intelReportsDAL.GetSecretCodeByReporterId(reporterId));
            Console.WriteLine("Update the num Reports!!");
            peopleDAL.UpdateMentionsNum(secretCode);
            Console.WriteLine($"Update the num Mentions of: {secretCode}!!");
            peopleDAL.UpdateType(intelReportsDAL.GetSecretCodeByReporterId(reporterId));
            Console.WriteLine($"Update the Type of: {intelReportsDAL.GetSecretCodeByReporterId(reporterId)}!!");
            if (peopleDAL.GetPerson(secretCode).Type == "reporter")
            {             
                peopleDAL.UpdateType(secretCode);
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

        public void UpdateReport( string text, string secretCode)
        {
            try
            {
                PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
                int personId = peopleDAL.GetIdOfPerson(secretCode);

                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    string query = @"UPDATE intel_reports SET text = @Text" +
                                " WHERE reporter_id = @ReporterId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ReporterId", personId);
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
                    string queryReports = @"DELETE FROM intel_reports" +
                    " WHERE reporter_id = @PersonId OR target_id = @PersonId";

                    MySqlCommand cmdReports = new MySqlCommand(queryReports, conn);
                    cmdReports.Parameters.AddWithValue("@PersonId", personId);
                    int rowsAffectedReports = cmdReports.ExecuteNonQuery();

                    Console.WriteLine($"Deleted {rowsAffectedReports} related reports.");
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
                List<IntelReports> reports = intelReportsDAL.GetAllIntelReportsByReporter(secretCode);
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

        public string GetSecretCodeByReporterId(int reporterId)
        {
            try
            {
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    string query = "SELECT people.secret_code FROM people " +
                                "INNER JOIN intel_reports ON intel_reports.reporter_id = people.id " +
                                "WHERE reporter_id = @ReporterId;";
                    var cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ReporterId", reporterId);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return reader["secret_code"].ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
                return null;
            }
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

        public bool SearchBySecretCode(string secretCode)
        {
            try
            {
                PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
                var reports = new List<IntelReports>();
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    var cmd = new MySqlCommand("SELECT * FROM intel_reports", conn);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        IntelReports intelReports = IntelReports.createFromReader(reader);
                        reports.Add(intelReports);
                    }
                    foreach (var report in reports)
                    {
                        if (report.ReporterId == peopleDAL.GetIdOfPerson(secretCode))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
                return false;
            }
        }
    }
}