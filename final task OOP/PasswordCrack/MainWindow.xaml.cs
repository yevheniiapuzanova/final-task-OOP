using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordCrack
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            string password = PasswordTextBox.Text;
            if (!string.IsNullOrEmpty(password))
            {
                string encryptedPassword = EncryptPassword(password);
                EncryptedPasswordLabel.Content = "Encrypted Password: " + encryptedPassword;
                MessageBox.Show("Password encrypted successfully.");
            }
            else
            {
                MessageBox.Show("Please enter a password.");
            }
        }

        private string EncryptPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private async void Crack_Click(object sender, RoutedEventArgs e)
        {
            int maxThreads = 1;
            if (int.TryParse(ThreadsTextBox.Text, out int result))
            {
                maxThreads = result;
            }

            ResultsTextBox.Text = "Cracking Password...";

            string encryptedPassword = EncryptedPasswordLabel.Content.ToString().Replace("Encrypted Password: ", "");
            if (string.IsNullOrEmpty(encryptedPassword))
            {
                ResultsTextBox.Text = "Please encrypt a password first.";
                return;
            }

            BruteForceCracker bruteForceCracker = new BruteForceCracker();

            await Task.Run(() => bruteForceCracker.StartBruteForce(maxThreads, encryptedPassword));

            ResultsTextBox.Text = bruteForceCracker.Results;
        }
    }
}
