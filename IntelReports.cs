using System;
using Malshinon.Models;

namespace Malshinon.Models
{
    public class IntelReports
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public int TargetId { get; set; }
        public string Text { get; set; }
        public string TimeStamp { get; set; }

        public IntelReports(int id, int reporterId, int targetId, string text, string timeStamp)
        {
            Id = id;
            ReporterId = reporterId;
            TargetId = targetId;
            Text = text;
            TimeStamp = timeStamp;
        }
    }
}