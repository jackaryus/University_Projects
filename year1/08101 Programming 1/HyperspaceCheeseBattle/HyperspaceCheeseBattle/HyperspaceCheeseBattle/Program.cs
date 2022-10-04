using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperspaceCheeseBattle
{
    class Program
    {
        static int[,] board = new int[,]
        {
           //0 1 2 3 4 5 6 7
            {2,1,1,1,1,1,1,3}, 
            {2,1,1,1,1,1,1,1}, 
            {2,2,2,2,1,1,2,1}, 
            {2,2,0,1,1,1,3,1}, 
            {2,2,0,2,2,2,2,1}, 
            {2,2,0,2,2,2,0,1}, 
            {2,0,0,0,0,0,0,3}, 
            {2,0,0,0,0,0,0,4} 

        };

        public enum Direction
        {
            left,
            right,
            up,
            down, 
            win
        };

        struct Player
        {
            public string Name;
            public int Xpos;
            public int Ypos;
        }

        static Player[] players;
        static Direction[,] BoardDirection;

        static ConsoleColor[] playercolours = { ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Magenta }; 

        static int NumberOfPlayers;

        static void Main(string[] args)
        {
            while (true)
            {
                ResetGame();
                BoardDirection = new Direction[board.GetLength(0), board.GetLength(1)];
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        BoardDirection[x, y] = (Direction)board[x, y];
                    }
                }
                MakeMoves();
                // this section is where the player decides if they want to play again
                string playagain;
                while (true)
                {
                    Console.WriteLine("do you want to play again? (Y/N)");
                    playagain = Console.ReadLine();
                    if ((playagain != "Y" ) && (playagain != "y") && (playagain != "N") && (playagain != "n"))
                        Console.WriteLine("Invalid input");
                    else 
                        break;
                }
                if (playagain == "N" || playagain == "n")
                    break;
            }
        }

        static void ResetGame()
        {
            // this method is for setting the players at the beginning when a new game is started
            while (true)
            {
                Console.WriteLine("please enter the number of players (2-4):");
                try
                {               
                        NumberOfPlayers = int.Parse(Console.ReadLine());
                        if (NumberOfPlayers < 2 || NumberOfPlayers > 4)
                            Console.WriteLine("number of players must be between 2 and 4");
                        else
                            break;                        
                }
                catch
                {
                    Console.WriteLine("invalid input");
                }
            }
            players = new Player[NumberOfPlayers];
            for (int i = 0; i < NumberOfPlayers; i++)
            {
                while (true)
                {
                    Console.WriteLine("please enter player " + (i + 1) + " name: ");
                    players[i].Name = Console.ReadLine();
                    if (players[i].Name.Length == 0)
                        Console.WriteLine("invalid input");
                    else
                        break;
                }
                players[i].Xpos = 0;
                players[i].Ypos = 0;
            }
        }

        static int DiceThrow()
        {
            //return 1;
            //Random random = new Random();
            //return random.Next(1, 7);
            Console.WriteLine("enter a dice roll");
            return int.Parse(Console.ReadLine());
        }

        private static void PlayerTurn(int playerNo)
        {
            // this method gets dice roll and puts it into the movement method
            Console.WriteLine("press enter to roll dice");
            Console.ReadLine();
            int dice = DiceThrow();
            Console.WriteLine(players[playerNo].Name + " rolls a " + dice);
            playermovement(playerNo, dice);
            //this section calls the collision method and moves the player by 1 if needed
            while (collision(playerNo) == true)
            {
                playermovement(playerNo, 1);
                Console.WriteLine("movement adjusted due to collision");
            }
            if (cheeseinsquare(playerNo) == true)
            {
                Console.WriteLine("you have landed on cheese do you want to roll again or shoot a player? (R/S)");
                while (true)
                {
                    string cheesechoice = Console.ReadLine();
                    if ((cheesechoice == "R") || (cheesechoice == "r"))
                    {
                        PlayerTurn(playerNo);
                        break;
                    }
                    else if ((cheesechoice == "S") || (cheesechoice == "s"))
                    {
                        Console.WriteLine("Choose a player to shoot");
                        for (int i = 0; i < NumberOfPlayers; i++)
                        {
                            Console.WriteLine((i+1)+". " + players[i].Name);
                        }
                        while(true)
                        {
                            int shootchoice = int.Parse(Console.ReadLine());
                            if (shootchoice > 0 && shootchoice <= NumberOfPlayers && (shootchoice-1) != playerNo)
                            {
                                Console.WriteLine(players[playerNo].Name + " shot " + players[shootchoice-1].Name );
                                players[shootchoice - 1].Ypos = 0;
                                break;
                            }
                            else if ((shootchoice - 1) == playerNo)
                            {
                                Console.WriteLine("you cant shoot yourself choose again");
                            }
                            else
                            {
                                Console.WriteLine("enter a valid input");
                            }
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("invalid input");
                    }
                }
            }
        }

        static void playermovement(int playerNo, int dice)
        {
            // this method moves the players X and Y position depending on the dice roll and also prevents the
            // player from moving if the roll puts them off the board
            int x = players[playerNo].Xpos;
            int y = players[playerNo].Ypos;
            switch (BoardDirection[x, y])
            {
                case Direction.left:
                    players[playerNo].Xpos -= dice;
                    if (players[playerNo].Xpos < 0)
                    {
                        players[playerNo].Xpos += dice;
                        
                    }
                    break;
                case Direction.right:
                    players[playerNo].Xpos += dice;
                    if (players[playerNo].Xpos > 7)
                    {
                        players[playerNo].Xpos -= dice;
                        
                    }
                    break;
                case Direction.up:
                    players[playerNo].Ypos += dice;
                    if (players[playerNo].Ypos > 7)
                    {
                        players[playerNo].Ypos -= dice;
                        
                    }
                    break;
                case Direction.down:
                    players[playerNo].Ypos -= dice;
                    if (players[playerNo].Ypos < 0)
                    {
                        players[playerNo].Ypos += dice;
                        
                    }
                    break;
            }
        }

        static bool collision(int currentplayer)
        {
            //TODO: write a method that checs through the rocket postitions 
            //and returns true if there is a rocket on the given square
            for (int i = 0; i < NumberOfPlayers; i++)
            {
                if (i == currentplayer)
                    continue;
                if (players[i].Xpos == players[currentplayer].Xpos && players[i].Ypos == players[currentplayer].Ypos)
                    return true;

            }
            return false;
        }



        static void drawboard()
        {
            // this method draws the board and changes the colour of the square a player is on to correspond with
            // the player colour
            Console.Clear();
            Console.WriteLine(" ");
            Console.WriteLine("     HyperSpace Cheese Battle");
            Console.WriteLine("     ~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine(" ");
            Console.WriteLine(" ╔═══╦═══╦═══╦═══╦═══╦═══╦═══╦═══╗");
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    Console.Write(" ║");
                    for (int i = 0; i < NumberOfPlayers; i++)
                    {
                        if (players[i].Xpos == x && players[i].Ypos == y)
                        {
                            Console.ForegroundColor = playercolours[i];
                            break;
                        }
                    }
                        switch (BoardDirection[x, y])
                        {
                            case Direction.left:
                                Console.Write(" ←");
                                break;
                            case Direction.right:
                                Console.Write(" →");
                                break;
                            case Direction.up:
                                Console.Write(" ↑");
                                break;
                            case Direction.down:
                                Console.Write(" ↓");
                                break;
                            case Direction.win:
                                Console.Write(" X");
                                break;
                        }
                        Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.WriteLine(" ║");
                if (y != 0)
                    Console.WriteLine(" ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣");
            }
            Console.WriteLine(" ╚═══╩═══╩═══╩═══╩═══╩═══╩═══╩═══╝");
            Console.WriteLine("   1   2   3   4   5   6   7   8  ");
        }

        static void MakeMoves()
        {
            // this method redraws the board after a player has moved and also initiates the playerturn method
            int i;
            bool NoWin = true;
            while(NoWin)
            {
                for (i = 0; i < NumberOfPlayers; i++)
                {
                    drawboard();
                    Console.WriteLine("it is " + players[i].Name + "'s turn to roll");
                    PlayerTurn(i);
                    drawboard();
                    if (players[i].Xpos == 7 && players[i].Ypos == 7)
                    {
                        Console.WriteLine(players[i].Name + " has won :D");
                        NoWin = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("press enter to continue");
                        Console.ReadLine();
                    }
                 
                }
                
            }

        }

        static bool cheeseinsquare(int currentplayer)
        {
            if ((players[currentplayer].Xpos == 0 && players[currentplayer].Ypos == 3) || (players[currentplayer].Xpos == 1 && players[currentplayer].Ypos == 4)
                || (players[currentplayer].Xpos == 6 && players[currentplayer].Ypos == 4) || (players[currentplayer].Xpos == 3 && players[currentplayer].Ypos == 5))
            {
                return true;
            }
            return false;
        }
    }
}
