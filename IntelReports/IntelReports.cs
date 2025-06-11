using System;
using Malshinon.Models;
using MySql.Data.MySqlClient;
using Malshinon.DataBase;

namespace Malshinon.Models
{
    public class IntelReports
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public int TargetId { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }

        public IntelReports()
        {

        }

        // # for print the row #
        public override string ToString()
        {
            return $"ID: {Id}, ReporterId: {ReporterId}, TargetId: {TargetId}, \nText: {Text},\n TimeStamp: {TimeStamp}";
        }

        public static IntelReports createFromReader(MySqlDataReader reader)
        {
            reader.Read();
            IntelReports report = new IntelReports
            {
                Id = reader.GetInt32("Id"),
                ReporterId = reader.GetInt32("ReporterId"),
                TargetId = reader.GetInt32("TargetId"),
                Text = reader.GetString("Text"),
                TimeStamp = reader.GetDateTime("TimeStamp")
            };
            return report;
        }
    }
}