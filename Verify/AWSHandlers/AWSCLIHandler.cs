using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verify.AWSHandlers
{
    
    public class AWSCLIHandler : HandlerBase
    {
        public Process extScript;
        public String fileName = "";
        public String arguments = "";
        public StringBuilder stdOutput;

        public AWSCLIHandler() 
        {
            this.initProcess();
        }

        public void initProcess()
        {
            extScript = new Process();
            extScript.StartInfo.CreateNoWindow = true;
            extScript.StartInfo.RedirectStandardOutput = true;
            extScript.StartInfo.RedirectStandardInput = false;

            stdOutput = new StringBuilder();
            extScript.OutputDataReceived += (sender, args) => stdOutput.AppendLine(args.Data);
        }

        public void SetFileName(String fileName)
        {
            String workingDirectory = Environment.CurrentDirectory;
            this.fileName = Directory.GetParent(workingDirectory).Parent.Parent.FullName + @"/AWSCLI/" + fileName;
        }

        public void SetQueryParams(String tableName, String columnName, String value)
        {
            this.arguments = tableName + " " + columnName + " " + value;
        }

        public String RunQuery()
        {
            extScript.StartInfo.FileName = this.fileName;
            extScript.StartInfo.Arguments = this.arguments;
            extScript.Start();
            extScript.BeginOutputReadLine();
            extScript.WaitForExit(30000);
            if (extScript.HasExited)
            {
                Console.WriteLine("Query result");
                Console.WriteLine(stdOutput.ToString());
                extScript.Dispose();
                return stdOutput.ToString();
            }
            else
            {
                extScript.Dispose();
                return null;
            }

        }
    }
}
