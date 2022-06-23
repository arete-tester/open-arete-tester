using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;

namespace AreteTester.UI
{
    internal class Report
    {
        public string ProjectName { get; set; }
        public DateTime ExecutionStart { get; set; }
        public DateTime ExecutionEnd { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }

        public List<AssertionReportItem> Assertions { get; set; }

        public List<LogReportItem> Logs { get; set; }

        public Report()
        {
            this.Assertions = new List<AssertionReportItem>();
            this.Logs = new List<LogReportItem>();
        }

        public void WriteHtmlReport(string path)
        {
            string templateFile = Globals.LocalBinDir + "report_template.html";

            string html = File.ReadAllText(templateFile);
            html = html.Replace("@@@ProjectName", this.ProjectName);
            html = html.Replace("@@@ExecutionStartTime", this.ExecutionStart.ToString("dd-MMM-yyyy HH:mm:ss"));
            html = html.Replace("@@@ExecutionEndTime", this.ExecutionEnd.ToString("dd-MMM-yyyy HH:mm:ss"));
            html = html.Replace("@@@AssertionsCount", this.Assertions.Count.ToString());
            html = html.Replace("@@@SuccessCount", this.SuccessCount.ToString());
            html = html.Replace("@@@FailureCount", this.FailureCount.ToString());
            html = html.Replace("@@@LogsCount", this.Logs.Count.ToString());

            List<string> assertionRows = new List<string>();
            for (int i = 0; i < this.Assertions.Count; i++)
            {
                AssertionReportItem item = this.Assertions[i];
                string row = item.ToString();
                row = row.Replace("@@@Index", (i + 1).ToString());
                assertionRows.Add(row);
            }
            html = html.Replace("@@@AssertionRows", String.Join(Environment.NewLine, assertionRows.ToArray()));

            List<string> logRows = new List<string>();
            foreach (LogReportItem item in this.Logs)
            {
                logRows.Add(item.ToString());
            }
            html = html.Replace("@@@LogRows", String.Join(Environment.NewLine, logRows.ToArray()));

            File.WriteAllText(path, html);
        }

        public void WriteXmlResult(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<AssertionReportItem>));
            using (TextWriter txtWriter = new StreamWriter(path))
            {
                serializer.Serialize(txtWriter, this.Assertions);
            }
        }
    }

    [XmlType(TypeName="Assertion")]
    public class AssertionReportItem
    {
        #region Templates
        private const string ASSERTION_TEMPLATE = @"				<tr>
					<td>@@@Index</td>
					<td>@@@Description</td>
					<td>@@@AssertionType</td>
					<td>@@@Expected</td>
					<td>@@@Actual</td>
					<td>@@@Status</td>
				</tr>";
        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        [XmlIgnore]
        public string AssertionType { get; set; }
        public string Expected { get; set; }
        public string Actual { get; set; }
        public string Success { get; set; }

        public override string ToString()
        {
            string tr = ASSERTION_TEMPLATE;
            tr = tr.Replace("@@@Description", this.Description);
            tr = tr.Replace("@@@AssertionType", this.AssertionType);
            tr = tr.Replace("@@@Expected", this.Expected);
            tr = tr.Replace("@@@Actual", this.Actual);
            string status = (this.Success == "1") ? "<b><span style='color:blue'>SUCCESS</span></b>" : "<b><span style='color:red'>FAIL</span></b>";
            tr = tr.Replace("@@@Status", status);

            return tr;
        }
    }

    internal class LogReportItem
    {
        #region Templates
        private const string LOG_TEMPLATE = @"				<tr>
					<td>@@@Description</td>
				</tr>";
        #endregion

        public string Description { get; set; }

        public override string ToString()
        {
            string tr = LOG_TEMPLATE;

            tr = tr.Replace("@@@Description", this.Description);

            return tr;
        }
    }
}
