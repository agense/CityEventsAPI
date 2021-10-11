using Business.Interfaces.Log;
using System;
using System.IO;

namespace Business.Services
{
    public class FileLog : ILog
    {
        public void Log(string path, string message)
        {
            try
            {
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    { }
                }

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine($"{DateTime.Now} {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File log operation failed Error: {ex.Message}");
            }
        }
    }
}
