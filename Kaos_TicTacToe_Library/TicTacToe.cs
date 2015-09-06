using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kaos_TicTacToe_Library
{
	 
    public class TicTacToe : ITicTacToe
    {
        /// <summary>
        /// Class variables
        /// </summary>
        public User Player1;
        public User Player2;
        public Play[] GameBoard { get; set; }
        public int[,] _winningCombinations { get; set; }
        public int playedSquares = 0;

        /// <summary>
        /// Constructs a new Tic Tac Toe game using the two player names
        /// </summary>
        /// <param name="player1name">Name of Player 1</param>
        /// <param name="player2name">Name of Player 2</param>
        public TicTacToe(string player1, string player2)
        {
            Player1 = new User(player1);
            Player2 = new User(player2);
            GameBoard = new Play[9];
            _winningCombinations = new int[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 } };
			
        }

        /// <summary>
        /// Randomly selects a square to play
        /// </summary>
        public int GetRandomPlay()
        {
            var options = new List<int>();
            for(int i = 0; i < GameBoard.Length; i++)
            {
                if(GameBoard[i] == Play.NotPlayed)
                {
                    options.Add(i);
                }
            }
            
            Random rand = new Random();
            var randVal = rand.Next(options.ToArray().Length);
            var result = options[randVal];

            return result;
        }

        /// <summary>
        /// Checks to see if either player has a winning combination on the TicTacToe board
        /// </summary>
        public User CheckForWinner()
        {
            //throw new NotImplementedException();

            //Farið yfir hver vinnur leikinn
            for (int i = 0; i < _winningCombinations.GetLength(0); i++)
            {
                if ((GameBoard[_winningCombinations[i, 0]].Equals(Play.Player1)
                && GameBoard[_winningCombinations[i, 1]].Equals(Play.Player1)
                && GameBoard[_winningCombinations[i, 2]].Equals(Play.Player1)))
                    return Player1;
                else if ((GameBoard[_winningCombinations[i, 0]].Equals(Play.Player2)
                    && GameBoard[_winningCombinations[i, 1]].Equals(Play.Player2)
                    && GameBoard[_winningCombinations[i, 2]].Equals(Play.Player2)))
                    return Player2;

            }
           

            return null;
        }

        /// <summary>
        /// The chosen play by a user
        /// </summary>
        /// <param name="user">The User who's turn it is to play</param>
        /// <param name="play">The player's symbol which marks the selected square</param>
        public void PlayAction(User user, int play)
        {
            //throw new NotImplementedException();

            if (CheckIfAvailable(play))
            {
                //ef Player1 kemur inn er sett á réttan stað í GameBoard tákn um að hann hafi valið ákveðinn reit
                if (user.Equals(Player1))
                    GameBoard.SetValue(Play.Player1, play);
                else
                    GameBoard.SetValue(Play.Player2, play);
                playedSquares++;
                
                //ef búið er að velja í það minnsta 5 reiti er byrjað að athuga hvort annar spilaranum tekst að vinna
                if (playedSquares >= 5)
                    CheckForWinner();
            }
        }

        /// <summary>
        /// Checks if selected square is available
        /// </summary>
        /// <param name="i">Players chosen square</param>
        private bool CheckIfAvailable(int i)
        {
            //throw new NotImplementedException();

            if (GameBoard[i] == Play.NotPlayed)
                return true;
            return false;
            
        }


        /// <summary>
        /// Returns the TicTacToe currently being played
        /// </summary>
        public Play[] GetGameBoard()
        {
            return GameBoard;
        }


        /// <summary>
        /// Sets the player's square of choice
        /// </summary>
        public void SetPlayedSquare(int selectedSqare, String name)
        {
            //throw new NotImplementedException();
            if (name.Equals(Player1.Name))
                GameBoard[selectedSqare] = Play.Player1;
            else
                GameBoard[selectedSqare] = Play.Player2;
        }
    }
}
