using System;
using Malshinon.DataBase;

namespace Malshinon.Models
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MySqlData sqlData = new MySqlData();
            People people = new People(1, "firstName", "lastName", "people1", "reporter", 0 , 0 );
            PeopleDAL peopleDAL = new PeopleDAL(sqlData);
            peopleDAL.AddPeople(people);
        }
    }
} 
