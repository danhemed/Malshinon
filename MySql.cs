using System;
using Malshinon.Models;
using MySql.Data.MySqlClient;

namespace Malshinon.DataBase
{
    public class MySqlData
    {
        static string connectionString = "Server=localhost;Database=malshinon;User=root;Password='';Port=3307";
        public MySqlConnection connection;

        public MySqlConnection GetConnect()
        {
            try
            {
                var connection = new MySqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("Connected to MySql database successfuly.");
                return connection;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
                return null;
            }
        }

        public MySqlConnection DisConnection()
        {
            try
            {
                var connection = new MySqlConnection(connectionString);
                connection.Close();
                Console.WriteLine("The connection Close!");
                return connection;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"ERROR!! {ex.Message}");
                return null;
            }
        }
    }
}