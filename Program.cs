using System;
using Malshinon.DataBase;

namespace Malshinon.Models
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MySqlData sqlData = new MySqlData();
            People people = new People { FirstName = "F3", LastName = "L3", SecretCode = "19B", Type = "reporter" };
            PeopleDAL peopleDAL = new PeopleDAL(sqlData);
            peopleDAL.AddPeople(people);

            // # for print the table people #
            foreach (var person in peopleDAL.GetAllPeople())
            {
                Console.WriteLine(person);
            }

            Console.WriteLine("PERSON!!");
            Console.WriteLine(peopleDAL.GetPerson("19B"));
            
            peopleDAL.UpdatePerson("name", "name", "45t", "potential_agent");
            peopleDAL.UpdateReportsNum("45t");
            peopleDAL.UpdateMentionsNum("45t");
            peopleDAL.DeletePerson("19B");


            // # for print the table people #
            foreach (var person in peopleDAL.GetAllPeople())
            {
                Console.WriteLine(person);
            }


            // IntelReports reports = new IntelReports(id: 1, reporterId: 1, targetId: 2, text: "Report", timeStamp: DateTime.Now);
            // IntelReportsDAL reportsDAL = new IntelReportsDAL(sqlData);
            // reportsDAL.AddReports(reports);
        }
    }
} 
