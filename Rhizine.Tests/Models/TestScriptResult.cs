using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhizine.Tests.Models
{
    public class TestScriptResult
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public long Duration { get; set; } // Duration in milliseconds
        public string Remarks { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<string> Logs { get; set; }
        public Exception Exception { get; set; }
        public Dictionary<string, string> AdditionalData { get; set; }

        public TestScriptResult()
        {
            Logs = new List<string>();
            AdditionalData = new Dictionary<string, string>();
        }

        // Method to add a log entry
        public void AddLog(string log)
        {
            Logs.Add(log);
        }

        // Method to add additional data
        public void AddAdditionalData(string key, string value)
        {
            AdditionalData[key] = value;
        }

        public void CalculateDuration()
        {
            Duration = (EndTime - StartTime).Milliseconds;
        }

        public void SetPassed()
        {
            Status = "Passed";
        }

        public void SetFailed()
        {
            Status = "Failed";
        }
    }
}
