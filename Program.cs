using System;
using Malshinon.DataBase;

namespace Malshinon.Models
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MySqlData sqlData = new MySqlData();
            // People people = new People(2, "firstName", "lastName", "people1", "reporter", 0, 0);
            // PeopleDAL peopleDAL = new PeopleDAL(sqlData);
            // peopleDAL.AddPeople(people);

            IntelReports reports = new IntelReports(1, 1, 1, "Report", $"{DateTime.Now}");
            IntelReportsDAL reportsDAL = new IntelReportsDAL(sqlData);
            reportsDAL.AddReports(reports);
        }
    }
} 
