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

        public void TheMenu()
        {
            PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
            IntelReportsDAL intelReportsDAL = new IntelReportsDAL(_sqlData);
            Menu menu = new Menu(_sqlData);

            bool flag = true;
            while (flag)
            {
                Console.WriteLine("=== MENU MALSHINON ===");
                Console.WriteLine("--- Enter your number choice ---");
                Console.WriteLine("1. SING IN/Up >>");
                Console.WriteLine("2. People >>");
                Console.WriteLine("3. Intel Reports >>");
                Console.WriteLine("4. Alert >>");
                Console.WriteLine("0. EXIT >>");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.WriteLine("Enter your Secret Code:");
                        string secretCode = Console.ReadLine();
                        if (!peopleDAL.SearchBySecretCode(secretCode))
                        {
                            menu.MenuParmeters(secretCode, peopleDAL);
                            Console.WriteLine("Create A New Person!!");
                        }
                        if (peopleDAL.SearchBySecretCode(secretCode))
                        {
                            intelReportsDAL.CreateReport(peopleDAL.GetIdOfPerson(secretCode));
                        }
                        else
                        {
                            Console.WriteLine("You are not included in the table!");
                        }
                        break;

                    case "2":
                        menu.MenuPeople();
                        break;

                    case "3":
                        menu.MenuIntelReports();
                        break;

                    case "4":
                        menu.MenuAlert();
                        break;

                    case "0":
                        flag = false;
                        break;
                }

            }
        }

        public void MenuParmeters(string secretCode, PeopleDAL peopleDAL)
        {
            Console.WriteLine("Enter your First Name:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Enter your Last Name:");
            string lastName = Console.ReadLine();
            string type = "reporter";
            People people = new People
            {
                FirstName = firstName,
                LastName = lastName,
                SecretCode = secretCode,
                Type = type
            };
            peopleDAL.AddPeople(people);
        }

        public void MenuPeople()
        {
            PeopleDAL peopleDAL = new PeopleDAL(_sqlData);

            bool flag = true;
            while (flag)
            {
                Console.WriteLine("1. View All People >>");
                Console.WriteLine("2. View All Reporters >>");
                Console.WriteLine("3. View Person By Secret Code >>");
                Console.WriteLine("4. UPDATE Person >>");
                Console.WriteLine("5. DELETE Person >>");
                Console.WriteLine("0. EXIT >>");
                string option = Console.ReadLine();
                switch (option)
                {

                    case "1":
                        foreach (var person in peopleDAL.GetAllPeople())
                        {
                            Console.WriteLine(person);
                        }
                        break;

                    case "2":
                        foreach (var person in peopleDAL.GetAllReporter())
                        {
                            Console.WriteLine(person);
                        }
                        break;

                    case "3":
                        Console.WriteLine("Enter your Secret Code you want to View:");
                        string secretCode3 = Console.ReadLine();
                        if (peopleDAL.SearchBySecretCode(secretCode3))
                        {
                            Console.WriteLine(peopleDAL.GetPerson(secretCode3));
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
                                        
                    case "0":
                        flag = false;
                        break;

                }
            }

        }

        public void MenuIntelReports()
        {
            IntelReportsDAL intelReportsDAL = new IntelReportsDAL(_sqlData);

            bool flag = true;
            while (flag)
            {
                Console.WriteLine("1. View All Intel Reports >>");
                Console.WriteLine("2. View ALL Intel Report By Secret Code >>");
                Console.WriteLine("3. UPDATE Intel Report >>");
                Console.WriteLine("4. DELETE Intel Report >>");
                Console.WriteLine("0. EXIT >>");
                string option = Console.ReadLine();
                switch (option)
                {
                    
                    case "1":
                        foreach (var report in intelReportsDAL.GetAllIntelReports())
                        {
                            Console.WriteLine(report);
                        }
                        break;

                    case "2":
                        Console.WriteLine("Enter your Secret Code you want to View:");
                        string secretCode7 = Console.ReadLine();
                        if (intelReportsDAL.SearchBySecretCode(secretCode7))
                        {
                            foreach (var report in intelReportsDAL.GetAllIntelReportsByReporter(secretCode7))
                            {
                                Console.WriteLine(report);
                            }
                        }
                        else
                        {
                            Console.WriteLine("The Secret Code you entered is not found!");
                        }
                        break;

                    case "3":
                        Console.WriteLine("Enter the Secret Code you want to Update:");
                        string secretCode8 = Console.ReadLine();
                        Console.WriteLine("Enter the new Text:");
                        string text = Console.ReadLine();

                        if (intelReportsDAL.SearchBySecretCode(secretCode8))
                        {
                            intelReportsDAL.UpdateReport(text, secretCode8);
                        }
                        else
                        {
                            Console.WriteLine("The Secret Code you entered is not found!");
                        }
                        break;

                    case "4":
                        Console.WriteLine("Enter your Secret Code you want to delete:");
                        string secretCode9 = Console.ReadLine();
                        if (intelReportsDAL.SearchBySecretCode(secretCode9))
                        {
                            intelReportsDAL.DeleteReport(secretCode9);
                        }
                        else
                        {
                            Console.WriteLine("The Secret Code you entered is not found!");
                        }
                        break;

                    case "0":
                        flag = false;
                        break;

                }
            }
        }

        public void MenuAlert()
        {
            PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
            IntelReportsDAL intelReportsDAL = new IntelReportsDAL(_sqlData);
            Menu menu = new Menu(_sqlData);

            bool flag = true;
            while (flag)
            {
                Console.WriteLine("1. View All Alert >>");
                Console.WriteLine("2. View All the Dangerous Targets >>");
                Console.WriteLine("3. View Alert By Secret Code >>");
                Console.WriteLine("4. DELETE Alert >>");
                Console.WriteLine("0. EXIT >>");
                string option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        break;


                    case "0":
                        flag = false;
                        break;

                }
            }

        }
    }
}