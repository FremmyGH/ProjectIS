using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Game
{
    class Rounds
    {
        private List<Round> _rounds = new List<Round>();
        public Record Record { get; set; }

        public void AddRound(int bank, int[] score, Players players)
        {
            _rounds.Add(new Round(bank,score,players));
        }
        public Round this[int i]
        {
            get => _rounds[i];
            set => _rounds[i] = value;
        }
        public int Length()
        {
            return _rounds.Count;
        }
        public void AddInDB(Players Players)
        {
            Record = Record.GetInstance(Players);
        }
    }
}
