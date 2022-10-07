using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verify.Hooks;

namespace Verify.StepDefinitions
{
    public class TestBase
    {
        public void LogAndReport(String log)
        {
            Console.WriteLine(log);
            ReportHooks.defineTestText(log);
        }

        public JObject readJSONfile(String fileName)
        {
            String workingDirectory = Environment.CurrentDirectory;
            String path = Directory.GetParent(workingDirectory).Parent.Parent.FullName + @"\TestData\" + fileName;

            StreamReader file = File.OpenText(path);
            JsonTextReader reader = new JsonTextReader(file);
                     
            return (JObject)JToken.ReadFrom(reader);

        }
    }
}
