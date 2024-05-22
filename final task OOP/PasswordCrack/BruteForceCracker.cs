using System;
using System.Text;
using System.Threading;

namespace PasswordCrack
{
    public class BruteForceCracker
    {
        private volatile bool isPasswordFound;
        private string foundPassword = string.Empty;
        private readonly object lockObject = new object();

        public string Results { get; private set; } = string.Empty;

        public void StartBruteForce(int maxThreads, string encryptedPassword)
        {
            isPasswordFound = false;
            foundPassword = string.Empty;
            Results = string.Empty;
            DateTime startTime = DateTime.Now;

            ThreadManager threadManager = new ThreadManager(maxThreads, encryptedPassword, this);
            threadManager.Start();

            DateTime endTime = DateTime.Now;
            TimeSpan duration = endTime - startTime;
            Results += $"Brute force attack completed in {duration.TotalSeconds} seconds.\n";
        }

        public void ReportPasswordFound(string password)
        {
            lock (lockObject)
            {
                if (!isPasswordFound)
                {
                    isPasswordFound = true;
                    foundPassword = password;
                    Results += $"Password cracked: {password}\n";
                }
            }
        }

        public bool IsPasswordFound()
        {
            lock (lockObject)
            {
                return isPasswordFound;
            }
        }

        public string GetFoundPassword()
        {
            lock (lockObject)
            {
                return foundPassword;
            }
        }
    }
}
