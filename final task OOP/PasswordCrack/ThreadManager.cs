using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace PasswordCrack
{
    public class ThreadManager
    {
        private readonly int maxThreads;
        private readonly string encryptedPassword;
        private readonly BruteForceCracker bruteForceCracker;

        public ThreadManager(int maxThreads, string encryptedPassword, BruteForceCracker bruteForceCracker)
        {
            this.maxThreads = maxThreads;
            this.encryptedPassword = encryptedPassword;
            this.bruteForceCracker = bruteForceCracker;
        }

        public void Start()
        {
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < maxThreads; i++)
            {
                Thread thread = new Thread(BruteForce);
                threads.Add(thread);
                thread.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }

        private void BruteForce()
        {
            char[] charset = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            StringBuilder currentAttempt = new StringBuilder();

            while (!bruteForceCracker.IsPasswordFound())
            {
                string passwordAttempt = GenerateNextPassword(currentAttempt, charset);
                if (CheckPassword(passwordAttempt))
                {
                    bruteForceCracker.ReportPasswordFound(passwordAttempt);
                    break;
                }
            }
        }

        private string GenerateNextPassword(StringBuilder currentAttempt, char[] charset)
        {
            if (currentAttempt.Length == 0)
            {
                currentAttempt.Append(charset[0]);
            }
            else
            {
                int index = currentAttempt.Length - 1;
                while (index >= 0)
                {
                    if (currentAttempt[index] < charset[charset.Length - 1])
                    {
                        currentAttempt[index]++;
                        break;
                    }
                    else
                    {
                        currentAttempt[index] = charset[0];
                        if (index == 0)
                        {
                            currentAttempt.Insert(0, charset[0]);
                        }
                        index--;
                    }
                }
            }
            return currentAttempt.ToString();
        }

        private bool CheckPassword(string passwordAttempt)
        {
            return CalculateHash(passwordAttempt) == encryptedPassword;
        }

        private string CalculateHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
