using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kaos_TicTacToe_Library;

namespace Kaos_TicTacToe_Console
{
    public class TicTacToeConsole
    {
        private String user1;
        private String user2;
        private bool player1ToDo = true;
        private int selectedSquare;
        private char playAgain;

        /// <summary>
        /// Constructor who runs the game
        /// </summary>
        public TicTacToeConsole()
        {
            Console.WriteLine("Welcome to TicTacToe!\n");
            Console.WriteLine("Insert your players names");
            SetPlayerNames();
            do
            {
				selectedSquare = 0;
				player1ToDo = true;
                playGame();  

            } while (playAgain.ToString().ToLower() != "n");
        }

        /// <summary>
        /// Allows input of the players names
        /// </summary>
        public void SetPlayerNames()
        {
           
            Console.WriteLine("Player 1: ");
            user1 = Console.ReadLine();
            Console.WriteLine("Player 2: ");
            user2 = Console.ReadLine();

			// Ganga úr skugga um að báðir notendur hafi nafn
			if(user1 == "" || user2 == "")
			{
				Console.Clear();
				Console.WriteLine("Both players have to have a name, please try again");
				user1 = "";
				user2 = "";
				SetPlayerNames();
			}
			// Ganga úr skugga um að báðir notendur hafi ekki sama nafn
			if (user1 == user2)
			{
				Console.Clear();
				Console.WriteLine("Players can't use the same name, please try again");
				user1 = "";
				user2 = "";
				SetPlayerNames();
			}
        }

        /// <summary>
        /// Draws the Game board for the user
        /// </summary>
        /// <param name="board">The list containing the TicTacToe board and the status of each square on the board</param>
        public void DrawGameBoard(Play[] board)
        {
			Console.Clear();
            //Draws the squares of the TicTacToe board
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i].Equals(Play.NotPlayed))
                {
                    Console.Write("_{0}_|", i + 1);
                }
                else if (board[i].Equals(Play.Player1))
                {
                    Console.Write("_X_|");
                    board[i] = Play.Player1;
                }
                else
                {
                    Console.Write("_O_|");
                    board[i] = Play.Player2;
                }
                if(i%3 == 2)
                Console.WriteLine("\n");
            }

            
        }

        /// <summary>
        /// Gets user input on what play he wants to make
        /// </summary>
        /// <param name="user">User making the play</param>
        public void GetPlayerInput(String user, ITicTacToe t)
        {
            //throw new NotImplementedException();
			try
			{
				Console.WriteLine(user + "'s game: ");
                var input = Console.ReadLine();
                if (input.ToString().ToLower() == "r")
                {
                    selectedSquare = t.GetRandomPlay()+1; // +1 til að velja rétta reitinn
														  // og fara ekki niður fyrir 0
														  // og ná upp í 9
                }
                else
                {
                    selectedSquare = Convert.ToInt32(input);
                    if (selectedSquare < 1 || selectedSquare > 9)
                        throw new Exception("Please select numbers between 1-9");
                }
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);				
				Console.WriteLine("Try again!");
				GetPlayerInput(user, t);
			}
            

        }


        /// <summary>
        /// Plays the game, create new board, asks the user for input
        /// </summary>
        private void playGame()
        {
            ITicTacToe Ittt = new TicTacToe(user1, user2);
            Play[] board = Ittt.GetGameBoard();			

            for (int i = 0; i < board.Length; i++)
            {
                DrawGameBoard(board);
                if (player1ToDo)
                {
                    GetPlayerInput(user1, Ittt);
                    player1ToDo = false;
					
					// koma í veg fyrir að hægt sé að velja sama reit oftar en 1 sinni	(player1)				
					player1Move:	// goto playerMove			
					if(board[selectedSquare - 1].Equals(Play.NotPlayed))
					{
						board[selectedSquare - 1] = Play.Player1;
					}
					else
					{
						Console.WriteLine("This square is already selected, please choose another one");
						GetPlayerInput(user1, Ittt);
						goto player1Move;
					}
                }
                else
                {
                    GetPlayerInput(user2, Ittt);
                    player1ToDo = true;
					// koma í veg fyrir að hægt sé að velja sama reit oftar en 1 sinni	(player2)				
					player2Move:	// goto playerMove			
					if (board[selectedSquare - 1].Equals(Play.NotPlayed))
					{
						board[selectedSquare - 1] = Play.Player2;
					}
					else
					{
                        Console.WriteLine("This square is already selected, please choose another one");
						GetPlayerInput(user2, Ittt);
						goto player2Move;
					}
                }

                // Check for a winner or if nobody has yet won at the end of the game
                var check = Ittt.CheckForWinner();
                if (i >= 4 && check != null)
                {
					DrawGameBoard(board);
                    Console.WriteLine("Player: {0} won the game! woohoo", check.Name);
                    break;
                }
                else if (i == 8)
                {
					DrawGameBoard(board);
                    Console.WriteLine("Nobody won the game, booo");
                    break;
                }
            }
            Console.WriteLine("Do you want to play again? Press N to quit or any other key to continue");
            playAgain = Console.ReadKey().KeyChar;

        }
    }
}
