using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace EXETester.Models
{
    public class Tester
    {
        private static string TempFolder { get; set; } = @"C:\Temp";
        private string ExecutablePath { get; set; }
        private List<string> ArgumentFiles { get; set; }
        private string OutPutPath { get; set; }
        public Tester(string exePath, string argFilePath)
        {
            if (string.IsNullOrWhiteSpace(exePath) || string.IsNullOrWhiteSpace(argFilePath))
            {
                WriteToFile("Executable path or arguments path cannot be null");
                Process.Start("notepad.exe", OutPutPath);
            }

            ArgumentFiles = new List<string>();
            ExecutablePath = exePath;
            var args = File.ReadAllLines(argFilePath).ToList();
            foreach(var arg in args)
            {
                string argFile = $"{TempFolder}\\{Guid.NewGuid()}.txt";
                File.WriteAllText(argFile, arg);
                ArgumentFiles.Add(argFile);
            }
        }
        public bool Execute()
        {
            bool status = false;
            try
            {
                var results = new List<string>();
                foreach(var argFile in ArgumentFiles)
                {
                    var proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = $"/c {ExecutablePath} < {argFile}",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                    };

                    proc.Start();
                    string resultPerRun = string.Empty;
                    while (!proc.StandardOutput.EndOfStream)
                    {
                        resultPerRun += proc.StandardOutput.ReadLine();

                    }
                    results.Add(resultPerRun);
                }

                WriteToFile(string.Join("\n\n---------------------------------\n\n", results));
                status = true;
            }
            catch(Exception e)
            {
                WriteToFile($"Ex={e?.Message} \n InnerEx={e?.InnerException?.Message}");
            }
            ArgumentFiles.ForEach(a => File.Delete(a));

            Process.Start("notepad.exe", OutPutPath);

            return status;
        }

        public void WriteToFile(string content)
        {
            OutPutPath = $"C:/Temp/{Guid.NewGuid()}.txt";
            File.WriteAllText(OutPutPath, content);
        }
    }
}
