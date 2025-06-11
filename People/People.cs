using System;
using Malshinon.Models;
using MySql.Data.MySqlClient;
using Malshinon.DataBase;

namespace Malshinon.Models
{
    public class People
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecretCode { get; set; }
        public string Type { get; set; }
        public int NumReports { get; set; }
        public int NumMentions { get; set; }

        public People()
        {

        }

        // # for print the row #
        public override string ToString()
        {
            return $"ID: {Id}, FirstName: {FirstName}, LastName: {LastName}, SecretCode: {SecretCode}, Type: {Type}, NumReports: {NumReports}, NumMentions: {NumMentions}";
        }

        public static People createFromReader(MySqlDataReader reader)
        {
            reader.Read();
            People person = new People
            {
                Id = reader.GetInt32("Id"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                SecretCode = reader.GetString("Secret_Code"),
                Type = reader.GetString("Type"),
                NumReports = reader.GetInt32("Num_Reports"),
                NumMentions = reader.GetInt32("Num_Mentions")
            };
            return person;
        }
    }
}