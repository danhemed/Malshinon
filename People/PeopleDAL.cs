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

        public List<People> GetAllPeople()
        {
            try
            {
                var people = new List<People>();
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    var cmd = new MySqlCommand("SELECT * FROM people", conn);
                    var reader = cmd.ExecuteReader();
                    people.Add(People.createFromReader(reader));
                    while (reader.Read())
                    {
                        People person = People.createFromReader(reader);
                        people.Add(person);
                    }
                }
                return people;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
                return null;
            }
        }

        public People GetPerson(string secretCode)
        {
            try
            {
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    var cmd = new MySqlCommand("SELECT * FROM people WHERE secret_code = @SecretCode", conn);
                    cmd.Parameters.AddWithValue("@SecretCode", secretCode);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return People.createFromReader(reader);
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

        public int GetIdOfPerson(string secretCode)
        {
            try
            {
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    var cmd = new MySqlCommand("SELECT * FROM people WHERE secret_code = @SecretCode", conn);
                    cmd.Parameters.AddWithValue("@SecretCode", secretCode);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return reader.GetInt32("Id");
                    }
                }
                return -1;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
                return -1;
            }
        }

        public void UpdatePerson(string firstName, string lastName, string secretCode, string type, int numReports, int numMentions)
        {
            try
            {
                People person = new People();

                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    string query = @"UPDATE people SET firstName = @FirstName, lastName = @LastName, secret_code = @Secret_Code, type = @Type, num_reports = @Num_Reports, num_mentions = @Num_Mentions" +
                                " WHERE secret_code = @SecretCode";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SecretCode", secretCode);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@Secret_Code", secretCode);
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@Num_Reports", numReports);
                    cmd.Parameters.AddWithValue("@Num_Mentions", numMentions);
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

        public void DeletePerson(string secretCode)
        {
            try
            {
                PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
                int personId = peopleDAL.GetIdOfPerson(secretCode);
                Console.Write($"Are you sure you want to delete person with code {secretCode} and all their reports? (y/n): ");
                string confirm = Console.ReadLine();
                if (confirm?.ToLower() != "n")
                {
                    Console.WriteLine("Delete cancelled.");
                    IntelReportsDAL intelReportsDAL = new IntelReportsDAL(_sqlData);
                    intelReportsDAL.DeleteReport(secretCode);

                    using (MySqlConnection conn = _sqlData.GetConnect())
                    {
                        string queryPeople = "DELETE FROM people WHERE secret_code = @SecretCode";
                        MySqlCommand cmdPeople = new MySqlCommand(queryPeople, conn);
                        cmdPeople.Parameters.AddWithValue("@SecretCode", secretCode);
                        int rowsAffectedPeople = cmdPeople.ExecuteNonQuery();

                        if (rowsAffectedPeople > 0)
                        {
                            Console.WriteLine("Deleted person successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Delete failed. Person not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
            }
        }

        public bool SearchBySecretCode(string secretCode)
        {
            try
            {
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    string query = "SELECT COUNT(*) FROM people WHERE Secret_Code = @SecretCode";
                    var cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SecretCode", secretCode);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
                return false;
            }
        }

        public void UpdateReportsNum(string secretCode)
        {
            try
            {
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    string query = @"UPDATE people SET num_reports = @Num_Reports" +
                        " WHERE secret_code = @Secret_Code";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Secret_Code", secretCode);
                    cmd.Parameters.AddWithValue("@Num_Reports", +1);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Updated Reports successfully!");
                    }
                    else
                    {
                        Console.WriteLine("The update Reports was not successful!");
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
            }
        }

        public void UpdateMentionsNum(string secretCode)
        {
            try
            {
                var ListPeople = new List<People>();
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    string query = @"UPDATE people SET num_mentions = @Num_Mentions" +
                        " WHERE secret_code = @Secret_Code";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Secret_Code", secretCode);
                    cmd.Parameters.AddWithValue("@Num_Mentions", +1);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Updated Mentions successfully!");
                    }
                    else
                    {
                        Console.WriteLine("The update Mentions was not successful!");
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
            }
        }

        public void UpdateType(string secretCode)
        {
            try
            {
                PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
                People person = peopleDAL.GetPerson(secretCode);

                IntelReportsDAL intelReportsDAL = new IntelReportsDAL(_sqlData);


                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    string query = @"UPDATE people SET type = @Type" +
                        " WHERE secret_code = @SecretCode";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SecretCode", secretCode);
                    if (person.Type == "reporter" || person.Type == "both")
                    {
                        if (person.NumReports >= 10 && intelReportsDAL.GetReportStats(secretCode) > 100)
                        {
                            cmd.Parameters.AddWithValue("@Type", "potential_agent");
                        }
                    }
                    else if (person.Type == "target")
                    {
                        cmd.Parameters.AddWithValue("@Type", "both");
                    }
                    else if (person.Type == "potential_agent")
                    {
                        Console.WriteLine("The Type is already potential_agent!!");
                    }
                    if (!cmd.Parameters.Contains("@Type"))
                    {
                        Console.WriteLine("No type change needed, skipping update.");
                        return;
                    }

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Updated Mentions successfully!");
                    }
                    else
                    {
                        Console.WriteLine("The update Mentions was not successful!");
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
            }
        }

        public List<People> GetAllReporter()
        {
            try
            {
                var people = new List<People>();
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    var cmd = new MySqlCommand("SELECT * FROM people WHERE type = @Type OR type = @Type2", conn);
                    cmd.Parameters.AddWithValue("@Type", "reporter");
                    cmd.Parameters.AddWithValue("@Type2", "both");
                    var reader = cmd.ExecuteReader();
                    people.Add(People.createFromReader(reader));
                    while (reader.Read())
                    {
                        People person = People.createFromReader(reader);
                        people.Add(person);
                    }
                }
                return people;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
                return null;
            }
        }

        public string GenerateRandomCode()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string result = "";

            for (int i = 0; i < 4; i++)
            {
                int index = random.Next(chars.Length);
                result += chars[index];
            }

            return result;
        }
    }
}