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

        public void UpdatePerson(string firstName, string lastName, string secretCode, string type, int numReports , int numMentions)
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

                    string queryPeople = "DELETE FROM people WHERE secret_code = @SecretCode";
                    MySqlCommand cmdPeople = new MySqlCommand(queryPeople, conn);
                    cmdPeople.Parameters.AddWithValue("@SecretCode", secretCode);
                    int rowsAffectedPeople = cmdPeople.ExecuteNonQuery();

                    if (rowsAffectedPeople > 0)
                    {
                        Console.WriteLine("Delete person successfully!");
                    }
                    else
                    {
                        Console.WriteLine("The delete was not successful!");
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
                var people = new List<People>();
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    var cmd = new MySqlCommand("SELECT * FROM people", conn);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        People person = People.createFromReader(reader);
                        people.Add(person);
                    }
                    foreach (var person in people)
                    {
                        if (person.SecretCode == secretCode)
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
                
                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    string query = @"UPDATE people SET type = @Type" +
                        " WHERE secret_code = @Secret_Code";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Secret_Code", secretCode);
                    if (person.Type == "reporter" || person.Type == "both")
                    {
                        if (person.NumReports >= 10)
                        {
                            cmd.Parameters.AddWithValue("@Type", "potential_agent");
                        }
                    }
                    else if (person.Type == "potential_agent")
                    {
                        Console.WriteLine("The Type is already potential_agent!!");
                    }
                    else
                    {
                        Console.WriteLine("The Type isn't reporter or both");
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
    }
}