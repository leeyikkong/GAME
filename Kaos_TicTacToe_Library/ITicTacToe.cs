using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kaos_TicTacToe_Library
{
    public interface ITicTacToe
    {
        /// <summary>
        /// Gets a random available play for the current user
        /// </summary>
        /// <returns>A number of a play to make, not the Array Id</returns>
        int GetRandomPlay();

        /// <summary>
        /// Checks if there is a winner
        /// </summary>
        /// <returns>The user who won, null if nobody has won yet</returns>
        User CheckForWinner();

        /// <summary>
        /// Makes a play for a user
        /// </summary>
        /// <param name="user">The user making the play</param>
        /// <param name="play">The play the user intends to make</param>
        void PlayAction(User user, int play);

        /// <summary>
        /// Allows access to the TicTacToe board of the game in progress
        /// </summary>
        Play[] GetGameBoard();
        
        // <summary>
        /// Inserts the played square into the TicTacToe board of the game in progress
        /// </summary>
        /// <param name="selectedSqare">The square being played</param>
        /// <param name="name">The name of the player who is selecting a square</param>
        void SetPlayedSquare(int selectedSqare, String name);
    }
}
