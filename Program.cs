using System;
using Malshinon.DataBase;

namespace Malshinon.Models
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MySqlData sqlData = new MySqlData();
            People people = new People { FirstName = "f2", LastName = "l2", SecretCode = "45t", Type = "reporter" };
            PeopleDAL peopleDAL = new PeopleDAL(sqlData);
            // peopleDAL.AddPeople(people);
            peopleDAL.UpdatePerson("name", "name", "45t", "reporter");

            // PeopleDAL peopleDAL = new PeopleDAL(sqlData);
            // People NewPeople = new People(2, "newFirstName", "newLastName", "people2", "potential_agent", 10, 5);
            // peopleDAL.UpdatePerson(NewPeople, "people2");
            // peopleDAL.DeletePerson(2);


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
