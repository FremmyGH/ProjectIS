using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Round
    {
        private int _bank;
        private int[] _score;
        private Players _players;

        public Round(int bank, int[] score, Players players)
        {
            Bank = bank;
            Score = score;
            Players = players;
        }

        public Players Players { get => _players; set => _players = value; }
        public int[] Score { get => _score; set => _score = value; }
        public int Bank { get => _bank; set => _bank = value; }
    }
}
