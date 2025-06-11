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
                        People person = new People
                        {
                            Id = reader.GetInt32("Id"),
                            FirstName = reader.GetString("FirstName"),
                            LastName = reader.GetString("LastName"),
                            SecretCode = reader.GetString("Secret_Code"),
                            Type = reader.GetString("Type"),
                            NumReports = reader.GetInt32("Num_Reports"),
                            NumMentions = reader.GetInt32("Num_Mentions")
                        };
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

        public void UpdatePerson(string firstName, string lastName, string secretCode, string type)
        {
            try
            {
                People person = new People();

                using (MySqlConnection conn = _sqlData.GetConnect())
                {
                    string query = @"UPDATE people SET firstName = @FirstName, lastName = @LastName, secret_code = @Secret_Code, type = @Type" +
                                " WHERE secret_code = @SecretCode";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SecretCode", secretCode);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@Secret_Code", secretCode);
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@Num_Reports", person.NumReports);
                    cmd.Parameters.AddWithValue("@Num_Mentions", person.NumMentions);
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

        
    }
}