using System;
using Google.Protobuf.WellKnownTypes;
using Malshinon.DataBase;
using MySql.Data.MySqlClient;


namespace Malshinon.Models
{
    public class Menu
    {
        private MySqlData _sqlData;

        public Menu(MySqlData sqlData)
        {
            _sqlData = sqlData;
        }

        public void MenuSecretCode()
        {
            PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
            bool flag = true;
            while (flag)
            {
                Console.WriteLine("=== MENU MALSHINON ===");
                Console.WriteLine("--- Enter your choice ---");
                Console.WriteLine("1. SING IN ");
                Console.WriteLine("2. SING UP ");
                Console.WriteLine("3. UPDATE Person ");
                Console.WriteLine("4. DELETE Person ");
                Console.WriteLine("5. EXIT ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.WriteLine("Enter your Secret Code:");
                        string secreCode = Console.ReadLine();
                        if (peopleDAL.SearchByID(secreCode))
                        {
                            Menu menu = new Menu(_sqlData);
                            menu.MenuOption();
                        }
                        else
                        {
                            Console.WriteLine("Enter your First Name:");
                            string firstName = Console.ReadLine();
                            Console.WriteLine("Enter your Last Name:");
                            string lastName = Console.ReadLine();
                            Console.WriteLine("Enter your Type Name:\n('reporter','target','both','potential_agent')");
                            string type = Console.ReadLine();
                            peopleDAL.AddPeople(firstName, lastName, type);
                        }
                        break;
                        
                }

            }       
            

            while (!peopleDAL.SearchByID(secreCode)) ;
        }

        public void MenuParmeters()
        {

        }
        
        public void MenuOption()
        {

        }

        public void AddNewReporter(People people)
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