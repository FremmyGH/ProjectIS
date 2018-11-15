using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Players
    {
        private List<Player> _players = new List<Player>();
        //private double _bank;

        //public double Bank { get => _bank; set => _bank = value; }

        public void AddPlayer(int id, string name,double sumMoney, bool isComp)
        {
            _players.Add(new Player(id, name, sumMoney, isComp));
        }
        public Player this[int i]
        {
            get => _players[i];
            set => _players[i] = value;
        }
        public int Length()
        {
            return _players.Count;
        }

        public void DeletePlayer(int i)
        {
            _players.RemoveAt(i);
        }

       

        //public void Clear()
        //{
        //    _players.Clear();
        //}
        public int ThrowDice()
        {
            var rnd = new Random();
            var result = rnd.Next(1, 6);
            return result;
            //return Value;
        }
    }
}
