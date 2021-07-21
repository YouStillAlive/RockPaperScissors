namespace RockPaperScissors
{
    class Program
    {
        static void Main(string[] args)
        {
            RockPaperScissors TheGame = new RockPaperScissors(args);
            if (TheGame.CheckValues())
            {
                TheGame.GenerateKey();
                TheGame.Start();
            }
        }
    }
}

//RockPaperScissors TheGame = new RockPaperScissors(new[] { "rock", "paper", "scissors" });
//If you don't like cmd