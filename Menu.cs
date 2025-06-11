// using System;
// using Google.Protobuf.WellKnownTypes;
// using Malshinon.DataBase;
// using MySql.Data.MySqlClient;


// namespace Malshinon.Models
// {
//     public class Menu
//     {
//         private MySqlData _sqlData;

//         public Menu(MySqlData sqlData)
//         {
//             _sqlData = sqlData;
//         }

//         public void MenuSecretCode()
//         {
//             PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
//             bool flag = true;
//             while (flag)
//             {
//                 Console.WriteLine("=== MENU MALSHINON ===");
//                 Console.WriteLine("--- Enter your number choice ---");
//                 Console.WriteLine("1. SING IN ");
//                 Console.WriteLine("2. SING UP ");
//                 Console.WriteLine("3. UPDATE Person ");
//                 Console.WriteLine("4. DELETE Person ");
//                 Console.WriteLine("5. EXIT ");
//                 string option = Console.ReadLine();

//                 switch (option)
//                 {
//                     case "1":
//                         Menu menu = new Menu(_sqlData);
//                         Console.WriteLine("Enter your Secret Code:");
//                         string secretCode = Console.ReadLine();
//                         if (peopleDAL.SearchBySecretCode(secretCode))
//                         {
//                             menu.MenuOption();
//                         }
//                         else
//                         {
//                             menu.MenuParmeters(secretCode);
//                             if (peopleDAL.SearchBySecretCode(secretCode))
//                             {
//                                 menu.MenuOption();
//                             }
//                             else
//                             {
//                                 Console.WriteLine("You are not included in the table!");
//                             }
//                         }
//                         break;

//                     case "2":
//                         Console.WriteLine("Enter your Secret Code:");
//                         string secretCode2 = Console.ReadLine();
//                         Menu menu2 = new Menu(_sqlData);
//                         menu2.MenuParmeters(secretCode2);
//                         if (peopleDAL.SearchBySecretCode(secretCode2))
//                         {
//                             menu2.MenuOption();
//                         }
//                         else
//                         {
//                             Console.WriteLine("You are not included in the table!");
//                         }
//                         break;

//                     case "3":
//                         PeopleDAL peopleDAL1 = new PeopleDAL(_sqlData);
//                         Console.WriteLine("Enter the Secret Code you want to Update:");
//                         string secretCode3 = Console.ReadLine();
//                         if (peopleDAL.SearchBySecretCode(secretCode3))
//                         {
//                             peopleDAL.UpdatePerson( , secretCode3);
//                         }
//                         else
//                         {
//                             Console.WriteLine("The Secret Code you entered is not found!");
//                         }
//                         break;

//                     case "4":
//                         PeopleDAL peopleDAL2 = new PeopleDAL(_sqlData);
//                         Console.WriteLine("Enter your Secret Code you want to delete:");
//                         string secretCode4 = Console.ReadLine();
//                         if (peopleDAL2.SearchBySecretCode(secretCode4))
//                         {
//                             peopleDAL2.DeletePerson(secretCode4);
//                         }
//                         else
//                         {
//                             Console.WriteLine("The Secret Code you entered is not found!");
//                         }
//                         break;

//                     case "5":
//                         flag = false;
//                         break;
//                 }

//             }       
//         }

//         public void MenuParmeters(string secretCode)
//         {
//             PeopleDAL peopleDAL = new PeopleDAL(_sqlData);
//             Console.WriteLine("Enter your First Name:");
//             string firstName = Console.ReadLine();
//             Console.WriteLine("Enter your Last Name:");
//             string lastName = Console.ReadLine();
//             Console.WriteLine("Enter your Type Name:\n('reporter','target','both','potential_agent')");
//             string type = Console.ReadLine();
//             peopleDAL.AddPeople(firstName, lastName, secretCode, type);
//         }
        
//         public void MenuOption()
//         {
//             bool flag = true;
//             while (flag)
//             {
//                 Console.WriteLine("~~~ MENU OPTIONS ~~~");
//                 Console.WriteLine("--- Enter your number choice ---");
//                 Console.WriteLine("1. Create Intel Report ");
//                 Console.WriteLine("2. ");
//             }
//         }

//         public void AddNewReporter(People people)
//         {
//             try
//             {
//                 using (MySqlConnection conn = _sqlData.GetConnect())
//                 {
//                     string query = "INSERT INTO people (Id, FirstName, LastName, Secret_Code, Type, Num_Reports, Num_Mentions)" +
//                         "VALUE (@Id, @FirstName, @LastName, @Secret_Code, @Type, @Num_Reports, @Num_Mentions)";
//                     MySqlCommand cmd = new MySqlCommand(query, conn);
//                     cmd.Parameters.AddWithValue("@Id", people.Id);
//                     cmd.Parameters.AddWithValue("@FirstName", people.FirstName);
//                     cmd.Parameters.AddWithValue("@LastName", people.LastName);
//                     cmd.Parameters.AddWithValue("@Secret_Code", people.SecretCode);
//                     cmd.Parameters.AddWithValue("@Type", people.Type);
//                     cmd.Parameters.AddWithValue("@Num_Reports", people.NumReports);
//                     cmd.Parameters.AddWithValue("@Num_Mentions", people.NumMentions);
//                     cmd.ExecuteNonQuery();
//                 }
//             }
//             catch (System.Exception ex)
//             {
//                 Console.WriteLine($"ERROR!! {ex.Message}");
//             }
//         }

//     }
// }