using System;
using System.Linq;

namespace MasterMind
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            while (game.GetUserResponse("Do you want to play MasterMind? ", "YN") == 'Y')
            {
                game.Play();
            }
        }
    }
}
