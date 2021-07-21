using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RockPaperScissors
{
    class RockPaperScissors
    {
        public string Key { get; set; }
        public string[] Signs { get; set; }

        public RockPaperScissors(string[] signs) => Signs = signs;

        public void Start()
        {
            var playerStep = "";
            var computerStep = "";
            do
            {
                computerStep = ComputerStep();
                Console.WriteLine("HMAC: " + GenerateHMAC(computerStep));
                DrawMenu();
                playerStep = Console.ReadLine();
                if (Int32.TryParse(playerStep, out int j) && j > 0 && j <= Signs.Length)
                {
                    Console.WriteLine("Your move: " + Signs[j - 1]);
                    Console.WriteLine("Computer move: " + computerStep);
                    if (Signs[j - 1] == computerStep)
                        Console.WriteLine("Draw!");
                    else
                        Console.WriteLine(CheckVictory(Signs[j - 1], computerStep) ? "You win!" : "You lose:(");
                    Console.WriteLine("HMAC Key: " + Key);
                }
                Console.WriteLine();
            } while (playerStep != "0");
        }

        public void GenerateKey()
        {
            var bytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }
            Key = BitConverter.ToString(bytes).Replace(@"-", "");
        }

        public bool CheckValues()
        {
            try
            {
                CheckOdd();
                CheckUnique();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " Try again:(");
                return false;
            }
            return true;
        }

        private void CheckOdd()
        {
            if (Signs.Length % 2 == 0 || Signs.Length < 3)
                throw new Exception("There must be an odd number of parameters (and > 2).");
        }

        private void CheckUnique()
        {
            if (Signs.Distinct().Count() != Signs.Length)
                throw new Exception("The parameters must have a unique value.");
        }

        private string ComputerStep()
        {
            var random = new Random();
            return Signs[random.Next(0, Signs.Length)];
        }

        private string GenerateHMAC(string data)
        {
            using (var hasher = new HMACSHA256(Encoding.Default.GetBytes(Key)))
            {
                var hash = hasher.ComputeHash(Encoding.Default.GetBytes(data));
                return BitConverter.ToString(hash).Replace(@"-", "");
            }
        }

        private void DrawMenu()
        {
            Console.WriteLine("Available moves:");
            for (var i = 0; i < Signs.Length; i++)
                Console.WriteLine((i + 1) + " - " + Signs[i]);
            Console.WriteLine("0 - exit");
            Console.Write("Enter your move: ");
        }

        private bool CheckVictory(string playerStep, string computerStep)
        {
            var enemyStep = Array.IndexOf(Signs, computerStep) + 1;
            var userStep = Array.IndexOf(Signs, playerStep) + 1;
            var average = Signs.Length / 2;
            if (enemyStep > userStep)
                return enemyStep - userStep - average > 0;
            else
                return userStep - enemyStep - average <= 0;
        }
    }
}