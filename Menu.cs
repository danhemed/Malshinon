using System;
using Google.Protobuf;
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
            Menu menu = new Menu(_sqlData);

            bool flag = true;
            while (flag)
            {
                Console.WriteLine("=== MENU MALSHINON ===");
                Console.WriteLine("--- Enter your number choice ---");
                Console.WriteLine("1. SING IN/Up >>");
                Console.WriteLine("2. View All People >>");
                Console.WriteLine("3. View Person By Secret Code >>");
                Console.WriteLine("4. UPDATE Person >>");
                Console.WriteLine("5. DELETE Person >>");
                Console.WriteLine("6. View All Intel Reports >>");
                Console.WriteLine("7. View Intel Report By Secret Code >>");
                Console.WriteLine("8. UPDATE Intel Report >>");
                Console.WriteLine("9. DELETE Intel Report >>");
                Console.WriteLine("10. EXIT >>");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.WriteLine("Enter your Secret Code:");
                        string secretCode = Console.ReadLine();
                        if (peopleDAL.SearchBySecretCode(secretCode))
                        {
                            menu.MenuOption();
                        }
                        else
                        {
                            menu.MenuParmeters(secretCode);
                            if (peopleDAL.SearchBySecretCode(secretCode))
                            {
                                menu.MenuOption();
                            }
                            else
                            {
                                Console.WriteLine("You are not included in the table!");
                            }
                        }
                        break;

                    case "2":
                        peopleDAL.GetAllPeople();
                        break;

                    case "3":
                        Console.WriteLine("Enter your Secret Code you want to View:");
                        string secretCode3 = Console.ReadLine();
                        if (peopleDAL.SearchBySecretCode(secretCode3))
                        {
                            peopleDAL.GetPerson(secretCode3);
                        }
                        else
                        {
                            Console.WriteLine("The Secret Code you entered is not found!");
                        }

                        break;

                    case "4":
                        Console.WriteLine("Enter the Secret Code you want to Update:");
                        string secretCode4 = Console.ReadLine();
                        Console.WriteLine("Enter the new first name:");
                        string firstName = Console.ReadLine();
                        Console.WriteLine("Enter the new last name:");
                        string lastName = Console.ReadLine();
                        Console.WriteLine("Enter the new type:");
                        Console.WriteLine("Only: ('reporter','target','both','potential_agent')");
                        string type = Console.ReadLine();
                        Console.WriteLine("Enter the new num reports:");
                        int numReports = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the new num mentions:");
                        int numMentions = int.Parse(Console.ReadLine());

                        if (peopleDAL.SearchBySecretCode(secretCode4))
                        {
                            peopleDAL.UpdatePerson(firstName, lastName, secretCode4, type, numReports, numMentions);
                        }
                        else
                        {
                            Console.WriteLine("The Secret Code you entered is not found!");
                        }
                        break;

                    case "5":
                        Console.WriteLine("Enter your Secret Code you want to delete:");
                        string secretCode5 = Console.ReadLine();
                        if (peopleDAL.SearchBySecretCode(secretCode5))
                        {
                            peopleDAL.DeletePerson(secretCode5);
                        }
                        else
                        {
                            Console.WriteLine("The Secret Code you entered is not found!");
                        }
                        break;

                    case "6":
                        break;

                    case "7":
                        break;

                    case "8":
                        break;

                    case "9":
                        break;

                    case "10":
                        flag = false;
                        break;
                }

            }       
        }

        public void MenuParmeters(string secretCode)
        {
            PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
            Console.WriteLine("Enter your First Name:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Enter your Last Name:");
            string lastName = Console.ReadLine();
            Console.WriteLine("Enter your Type Name:\n('reporter','target','both','potential_agent')");
            string type = Console.ReadLine();
            peopleDAL.AddPeople(firstName, lastName, secretCode, type);
        }
        
        public void MenuOption()
        {
            bool flag = true;
            while (flag)
            {
                Console.WriteLine("~~~ MENU OPTIONS ~~~");
                Console.WriteLine("--- Enter your number choice ---");
                Console.WriteLine("1. Create Intel Report ");
                Console.WriteLine("2. ");
            }
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