using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Dice
    {
        private int _value;
        public int Value { get => _value; set => _value = value; }

        public void GetRandom()
        {
            var rnd = new Random();
            Value = rnd.Next(1, 6);
            //return Value;
        }
    }
}
