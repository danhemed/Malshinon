using System;
using Malshinon.DataBase;

namespace Malshinon.Models
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MySqlData sqlData = new MySqlData();
            Menu menu = new Menu(sqlData);
            menu.TheMenu();
        }
    }
} 
