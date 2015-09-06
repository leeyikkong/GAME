using System;
using Kaos_TicTacToe_Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kaos_TicTacToe_Test
{
    [TestClass]
    public class UnitTestTicTacToe
    {
		//þetta er óþarfi - það á bara að Unit Testa lógík en ekki frumstillingar og slíkt
        [TestMethod]
        public void TestTicTacToeConstructor()
        {
            TicTacToe game = new TicTacToe("Testguy", "Testgal");
			Assert.IsNotNull(game);
			
			//TicTacToe g2;
			//Assert.IsNull(g2 = new TicTacToe("ble",null),"villa");
			
			// Bera saman upphafsborð, ath hvort smiður búi til rétt borð í upphafi leiks	
			int i = 0;
			Play[] pArr = {0,0,0,0,0,0,0,0,0};
			foreach (var item in pArr)
			{
				Assert.AreEqual(game.GameBoard[i], pArr[i]);
				i++;
			} 
			// Ath fjölda reita í borði i verður 9 = 10 reitir, einum of mikið. Lykkjan bætir einum við en hættir keyrslu á réttum stað
            Assert.AreEqual(i, 9);

			// Ath hvort að vinningsstöður séu réttar í _winningCombinations
			CollectionAssert.AreEqual(new int[,] {	{ 0, 1, 2 }, { 3, 4, 5 }, 
													{ 6, 7, 8 }, { 0, 3, 6 }, 
													{ 1, 4, 7 }, { 2, 5, 8 }, 
													{ 0, 4, 8 }, { 2, 4, 6 } 
												 },
												 game._winningCombinations);
        }

		[TestMethod]
		public void TestCheckForWinner()
		{
			TicTacToe game = new TicTacToe("Player1", "Player2");
						
			// Ath vinningsstöðuna [0, 1, 2]
			game.PlayAction(game.Player1, 0);
			Assert.AreEqual(game.CheckForWinner(), null);

			game.PlayAction(game.Player1, 1);
			game.PlayAction(game.Player1, 2);
			Assert.AreEqual(game.Player1.Name, game.CheckForWinner().Name);			
																		
		}

		/// <summary>
		/// TestPlayAction, athugar hvort rétt sé merkt í reiti sem leikmaður hefur gert í
		/// </summary>
		[TestMethod]
		public void TestPlayAction()
		{
			TicTacToe game = new TicTacToe("Player1", "Player2");	

            //Player1 setur sitt merki á nokkra reiti
			game.PlayAction(game.Player1, 0);
			game.PlayAction(game.Player1, 5);
			game.PlayAction(game.Player1, 6);
			
            //Kannað hvort reitirnir sem player1 valdi hafi skilað sér á borðið
			Assert.AreEqual(game.GameBoard[0], Play.Player1);
            Assert.AreEqual(game.GameBoard[5], Play.Player1);
            Assert.AreEqual(game.GameBoard[6], Play.Player1);
		}

        [TestMethod]
        public void TestCheckIfAvailable()
        {
            TicTacToe game = new TicTacToe("Player1", "Player2");

            //Kannað hvort gildi sem sett var á ákveðinn stað í myllu sé rétt og á réttum stað
            game.GameBoard[3] = Play.Player1;
            game.GameBoard[3] = Play.Player2;

            //Ath hvort nokkuð hafi verið breytt á reitnum
            Assert.AreEqual(game.GameBoard[3], Play.Player2);
        }
  
        [TestMethod]
        public void TestGetGameBoard()
        {
            TicTacToe game = new TicTacToe("Player1", "Player2");

            //GameBoard á ekki að vera tómt
            CollectionAssert.AreNotEqual(game.GameBoard, null);
        }

        [TestMethod]
        public void TestSetPlayedSquare()
        {
            TicTacToe game = new TicTacToe("Player1", "Player2");

            game.SetPlayedSquare(0, "Player1");
            game.SetPlayedSquare(8, "Player2");

            Play[] temp = new Play[9] { Play.Player1, 0, 0, 0, 0, 0, 0, 0, Play.Player2 };

            CollectionAssert.AreEqual(game.GameBoard, temp);
        }
    }
}
