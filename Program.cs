using System;
using Malshinon.DataBase;

namespace Malshinon.Models
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MySqlData sqlData = new MySqlData();
            PeopleDAL peopleDAL = new PeopleDAL(sqlData);
            peopleDAL.UpdatePerson("name", "name", "45t", "reporter", 10, 3);
            peopleDAL.UpdateType("45t");

            Console.WriteLine(peopleDAL.GetPerson("45t"));
        }
    }
} 
