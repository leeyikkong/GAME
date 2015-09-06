using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kaos_TicTacToe_Library
{
    /// <summary>
    /// Mjög simple User klasi sem geymir ekkert enn sem komið er nema nafn spilarans
    /// </summary>
    public class User
    {
        public String Name { get; set; }

        public User(string name)
        {
            Name = name;
        }
    }
}
