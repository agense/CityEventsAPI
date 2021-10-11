using System;

namespace Business.Interfaces.Log
{
    public interface IErrorLog
    {
        public void Log(string path, string message);

        public void Log(string path, Exception ex);
        
    }
}
