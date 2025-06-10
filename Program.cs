using System;
using Malshinon.DataBase;

namespace Malshinon.Models
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MySqlData sqlData = new MySqlData();
            People people = new People(2, "firstName", "lastName", "people2", "reporter", 0, 0);
            PeopleDAL peopleDAL = new PeopleDAL(sqlData);
            // peopleDAL.AddPeople(people);

            // # for print the table people #
            foreach (var person in peopleDAL.GetAllPeople())
            {
                Console.WriteLine(person);
            }

            IntelReports reports = new IntelReports(id: 1, reporterId: 1, targetId: 2, text: "Report", timeStamp: DateTime.Now);
            IntelReportsDAL reportsDAL = new IntelReportsDAL(sqlData);
            // reportsDAL.AddReports(reports);
        }
    }
} 
