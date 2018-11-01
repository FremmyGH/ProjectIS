namespace Game
{
    public class Player
    {
        private int _id;
        private string _name;
        private double _sumMoney;
        private int _sumValue;
        private bool _isComp;
        public Player(int id, string name, double sumMoney, int sumValue, bool isComp)
        {
            Id = id;
            Name = name;
            SumMoney = sumMoney;
            SumValue = sumValue;
            IsComp = isComp;
        }
        public double SumMoney { get => _sumMoney; set => _sumMoney = value; }
        public int SumValue { get => _sumValue; set => _sumValue = value; }
        public bool IsComp { get => _isComp; set => _isComp = value; }
        public string Name { get => _name; set => _name = value; }
        public int Id { get => _id; set => _id = value; }
    }
}
