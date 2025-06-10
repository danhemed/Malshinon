using System;
using Malshinon.Models;

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

        public People(int id, string firstName, string lastName, string secretCode, string type, int numReports, int numMentions)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            SecretCode = secretCode;
            Type = type;
            NumReports = numReports;
            NumMentions = numMentions;
        }

        // # for print the row #
        public override string ToString()
        {
            return $"ID: {Id}, FirstName: {FirstName}, LastName: {LastName}, SecretCode: {SecretCode}, Type: {Type}, NumReports: {NumReports}, NumMentions: {NumMentions}";
        }
    }
}