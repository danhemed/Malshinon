using System;
using Malshinon.DataBase;

namespace Malshinon.Models
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MySqlData sqlData = new MySqlData();
            sqlData.GetConnect();
            sqlData.DisConnection();
        }
    }
} 
