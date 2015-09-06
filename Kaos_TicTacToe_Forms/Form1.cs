using Kaos_TicTacToe_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kaos_TicTacToe_Forms
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Button[] _buttonArray;
        private bool _player1ToDo = true;
        private ITicTacToe _ittt;
        private Play[] _board;
        private int _playcount;

        public Form1()
        {
            InitializeComponent();
            
            _buttonArray = new System.Windows.Forms.Button[] { button1, button2, button3, button4, button5, button6, button7, button8, button9 };

            // Initialize the game
            NewGame();
        }

        private void NewGame()
        {
            _ittt = new TicTacToe("Player 1", "Player 2");
            _board = _ittt.GetGameBoard();
            label1.Text = "To start the game select your first move Player 1";
			_player1ToDo = true;
            _playcount = 0;

            foreach (var btn in _buttonArray)
            {
                btn.Enabled = true;
                btn.Text = "";
            }
        }

        /// <summary>
        /// Asks the players if they want to play another game
        /// </summary>
        private void PlayButton(int squareId)
        {
            if (_player1ToDo)
            {
                _board[squareId] = Play.Player1;
                _buttonArray[squareId].Text = "X";
                _buttonArray[squareId].Enabled = false;

                _player1ToDo = false;
                _playcount += 1;
            }
            else
            {
                _board[squareId] = Play.Player2;
                _buttonArray[squareId].Text = "O";
                _buttonArray[squareId].Enabled = false;

                _player1ToDo = true;
                _playcount += 1;
            }

            // Check for a winner or if nobody has yet won at the end of the game
            var check = _ittt.CheckForWinner();

            if (_playcount >= 4 && check != null)
            {
                if (!_player1ToDo)
                    MessageBox.Show("Player 1 won the game! woohoo");
                else
                    MessageBox.Show("Player 2 won the game! woohoo");
				NewGame();

            }
            else if (_playcount == 9)
            {
                MessageBox.Show("Nobody won the game, booo");
                NewGame();
            }
            else if (_player1ToDo)
            {
                label1.Text = "It's your turn Player 1";
            }
            else
            {
                label1.Text = "It's your turn Player 2";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlayButton(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PlayButton(1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PlayButton(2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PlayButton(3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PlayButton(4);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            PlayButton(5);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            PlayButton(6);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PlayButton(7);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            PlayButton(8);
        }

        private void buttonRandom_Click(object sender, EventArgs e)
        {
            PlayButton(_ittt.GetRandomPlay());
            
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
