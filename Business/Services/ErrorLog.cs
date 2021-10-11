using Business.Interfaces.Log;
using System;

namespace Business.Services
{
    public class ErrorLog : IErrorLog
    {
        private readonly ILog _logger;

        public ErrorLog(ILog logger)
        {
            _logger = logger;
        }

        public void Log(string path, string message) {
            _logger.Log(path, $"Message: {message}");
        }

        public void Log(string path, Exception ex) {
            _logger.Log(path, $"Type: {ex.GetType()} Message: {ex.Message}");
        }
    }
}
