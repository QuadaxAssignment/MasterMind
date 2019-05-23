using System;
using System.Linq;

namespace MasterMind
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            while (game.GetUserResponse(prompt: "Do you want to play MasterMind? ", validResponses: "YN") == 'Y')
            {
                game.Play();
            }
        }
    }
}
