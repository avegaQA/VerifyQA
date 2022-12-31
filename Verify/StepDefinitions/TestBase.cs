using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verify.Context;
using Verify.Hooks;

namespace Verify.StepDefinitions
{
    public class TestBase
    {

        public JObject payload = new JObject();
        public void LogAndReport(String log)
        {
            if (AWSContext.consoleLog) Console.WriteLine(log);
            if (AWSContext.reportLog) ReportHooks.defineTestText(log);
        }

        public void addLinkToReport(String link, String text)
        {
            ReportHooks.addLinkToReport(link, text);
        }

        public JObject readJSONfile(String fileName)
        {
            String workingDirectory = Environment.CurrentDirectory;
            String path = Directory.GetParent(workingDirectory).Parent.Parent.FullName + @"/TestData/" + fileName;

            StreamReader file = File.OpenText(path);
            JsonTextReader reader = new JsonTextReader(file);
                     
            return (JObject)JToken.ReadFrom(reader);

        }

        public Dictionary<string, string> ToDictionary(Table table)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var row in table.Rows)
            {
                dictionary.Add(row[0], row[1]);
            }
            return dictionary;
        }
    }
}
