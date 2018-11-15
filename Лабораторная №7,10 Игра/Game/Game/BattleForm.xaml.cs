using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace Game
{
    /// <summary>
    /// Логика взаимодействия для BattleForm.xaml
    /// </summary>
    public partial class BattleForm : Window
    {

        public Players Players { get; set; }
        Players finalists = new Players();
       
        Rounds ListRound = new Rounds();
        private int _bank;
        private bool _dopRound;

        private int _roundNumber;

        private int _playerNumber;

        private int[] _score;

        private string _bestPlayer;

        private int _bestPlayerId;

        private int _bestScore;
        //string connection = @"Data Source=DESKTOP-FPQ69BF\SQLSERVER;Initial Catalog=GameDice;Integrated Security=True";
        public BattleForm()
        {
            InitializeComponent();
        }
        private void BattleForm1_Loaded(object sender, RoutedEventArgs e)
        {
            _roundNumber = 0;
            _playerNumber = 0;
            _score=new int[Players.Length()];
            ListRound.AddRound(0, _score, Players);
            //_score = new int[ListRound[_roundNumber].Players.Length()];
            //MessageBox.Show(ListRound[_roundNumber].Score.Length.ToString());

            ListPlayers.Text += "<<<<<<<Правила>>>>>>>\n";
            ListPlayers.Text += "1.Набрать наибольшее кол-во очков\n";
            ListPlayers.Text += "2.Не превысить 21 очко\n";
            ListPlayers.Text += "<<<<<<<Начало игры>>>>>>>\n";
            ListPlayers.Text += $"Player's turn {ListRound[_roundNumber].Players[_playerNumber].Name}\n";
            Status.Text = $"Status: Player's turn {ListRound[_roundNumber].Players[_playerNumber].Name}, money: {ListRound[_roundNumber].Players[_playerNumber].SumMoney-1}";
            Next.IsEnabled = false;
            End.IsEnabled = false;
            ListRound[_roundNumber].Bank = ListRound[_roundNumber].Players.Length();
            //MessageBox.Show(ListRound[roundNumber].Bank.ToString());
            //Players.Bank = Players.Length();
            for (var i = 0; i < Players.Length(); i++)
            {
                Players[i].SumMoney--;
            }
        }
        private void Throw_Click(object sender, RoutedEventArgs e)
        {
            var throwResult=Players.ThrowDice();
            ListRound[_roundNumber].Score[_playerNumber] += throwResult;
            if (ListRound[_roundNumber].Score[_playerNumber]>21)
            {
                ListPlayers.Text += $"Игрок {ListRound[_roundNumber].Players[_playerNumber].Name} набирает {throwResult} очко." +
                                    $" Сумма очков: {_score[_playerNumber]}\n";
                ListPlayers.Text += "Score more than 21. Actual score = 0. Pass!\n";
                ListRound[_roundNumber].Score[_playerNumber] = 0;
                Throw.IsEnabled=false;
            }
            else
            {


                if (throwResult == 1)
                {
                    ListPlayers.Text +=
                        $"Игрок {ListRound[_roundNumber].Players[_playerNumber].Name} набирает {throwResult} очко." +
                        $" Сумма очков: {_score[_playerNumber]}\n";
                }
                else if (throwResult > 4)
                {
                    ListPlayers.Text +=
                        $"Игрок {ListRound[_roundNumber].Players[_playerNumber].Name} набирает {throwResult} очков." +
                        $" Сумма очков: {_score[_playerNumber]}\n";
                }
                else
                {
                    ListPlayers.Text +=
                        $"Игрок {ListRound[_roundNumber].Players[_playerNumber].Name} набирает {throwResult} очка." +
                        $" Сумма очков: {_score[_playerNumber]}\n";
                }
            }

        }

        private void Pass_Click(object sender, RoutedEventArgs e)
        {
            if (_bestScore < ListRound[_roundNumber].Score[_playerNumber])
            {
                _bestScore = ListRound[_roundNumber].Score[_playerNumber];
                _bestPlayer = ListRound[_roundNumber].Players[_playerNumber].Name;
                _bestPlayerId = ListRound[_roundNumber].Players[_playerNumber].Id;
                _dopRound = false;
            }
            else if (_bestScore == ListRound[_roundNumber].Score[_playerNumber])
            {
                _dopRound = true;
            }
           

            if (_playerNumber!=ListRound[_roundNumber].Players.Length()-1)
            {
                _playerNumber++;
                ListPlayers.Text += $"Player's turn {ListRound[_roundNumber].Players[_playerNumber].Name}\n";
                Status.Text = $"Status: Player's turn {ListRound[_roundNumber].Players[_playerNumber].Name}, money: {ListRound[_roundNumber].Players[_playerNumber].SumMoney}";
                Throw.IsEnabled = true;
            }
            else
            {
                
                if (!_dopRound)
                {
                    ListPlayers.Text += $"Round is over, winner {_bestPlayer}, bank: {ListRound[_roundNumber].Bank}";
                    for (var i = 0; i < Players.Length(); i++)
                    {
                        if (_bestPlayerId==Players[i].Id)
                        {
                             Players[i].SumMoney += ListRound[_roundNumber].Bank;
                        }
                    }
                   
                    Throw.IsEnabled = false;
                    Pass.IsEnabled = false;
                    Next.IsEnabled = true;
                    End.IsEnabled = true;
                }
                else
                {
                    _bank = ListRound[_roundNumber].Bank;
                    ListPlayers.Text += $"Extra Round, players: ";
                    finalists=new Players();
                    for (var i = 0; i < ListRound[_roundNumber].Players.Length(); i++)
                    {
                        if (ListRound[_roundNumber].Score[i]==_bestScore)
                        {
                            
                            finalists.AddPlayer(ListRound[_roundNumber].Players[i].Id, ListRound[_roundNumber].Players[i].Name,
                                ListRound[_roundNumber].Players[i].SumMoney, false);
                            
                        }
                    }
                    for (var i = 0; i < finalists.Length(); i++)
                    {
                        ListPlayers.Text += $"{finalists[i].Name}, ";
                    }
                    Throw.IsEnabled = false;
                    Pass.IsEnabled = false;
                    Next.IsEnabled = true;

                }
            }

           

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            End.IsEnabled = false;
            if (_dopRound)
            {
                _roundNumber++;
                _playerNumber = 0;
                _bestScore = 0;
                _bestPlayer = "";
                _bestPlayerId = -1;
                _score = new int[finalists.Length()];
                ListRound.AddRound(_bank, _score, finalists);
               
                Throw.IsEnabled = true;
                Pass.IsEnabled = true;
                Next.IsEnabled = false;
                ListPlayers.Text = "";
                ListPlayers.Text += "<<<<<<<Правила>>>>>>>\n";
                ListPlayers.Text += "1.Набрать наибольшее кол-во очков\n";
                ListPlayers.Text += "2.Не превысить 21 очко\n";
                ListPlayers.Text += "<<<<<<<Начало игры>>>>>>>\n";
                ListPlayers.Text += $"Player's turn {ListRound[_roundNumber].Players[_playerNumber].Name}\n";
                Status.Text = $"Status: Player's turn {ListRound[_roundNumber].Players[_playerNumber].Name}, money: {ListRound[_roundNumber].Players[_playerNumber].SumMoney}";
                _dopRound = false;
            }
            else
            {
                _roundNumber++;
                _playerNumber = 0;
                _bestScore = 0;
                _bestPlayer = "";
                _bestPlayerId = -1;
                for (var i = 0; i < Players.Length(); i++)
                {
                    if (Players[i].SumMoney < 1)
                    {
                        Players.DeletePlayer(i);
                    }
                }
                for (var i = 0; i < Players.Length(); i++)
                {
                    Players[i].SumMoney--;
                }
                _score =new int[Players.Length()];
                _bank = Players.Length();
                ListRound.AddRound(_bank,_score,Players);

                if (Players.Length() > 1)
                {
                    Throw.IsEnabled = true;
                    Pass.IsEnabled = true;
                    Next.IsEnabled = false;
                    ListPlayers.Text = "";
                    ListPlayers.Text += "<<<<<<<Rules>>>>>>>\n";
                    ListPlayers.Text += "1.Набрать наибольшее кол-во очков\n";
                    ListPlayers.Text += "2.Не превысить 21 очко\n";
                    ListPlayers.Text += "<<<<<<<Start game>>>>>>>\n";
                    ListPlayers.Text += $"Player's turn {ListRound[_roundNumber].Players[_playerNumber].Name}\n";
                    Status.Text = $"Status: Player's turn {ListRound[_roundNumber].Players[_playerNumber].Name}, money: {ListRound[_roundNumber].Players[_playerNumber].SumMoney}";
                }
                else
                {
                    MessageBox.Show("Not enough players for game");
                    var main = new MainWindow();
                    main.Show();
                    Close();
                }
            }
            
        }

        private void End_Click(object sender, RoutedEventArgs e)
        {
            //for (var i = 0; i < Players.Length(); i++)
            //{
            //    var checkQuery = $"SELECT Name From Player WHERE Name='{Players[i].Name}'";
            //    var insertQuery = $"INSERT INTO Player(Name,Score) VALUES('{Players[i].Name}','{Players[i].SumMoney}')";
            //    using (var con = new SqlConnection(connection))
            //    {
            //        con.Open();
            //        var cmd = new SqlCommand(checkQuery, con);
            //        var res = cmd.ExecuteScalar();
            //        if (res != null)
            //        {
            //            var updateQuery = $"UPDATE Player SET Score='{Players[i].SumMoney}'WHERE Name='{Players[i].Name}'";
            //            var cmdUpd = new SqlCommand(updateQuery, con);
            //            cmdUpd.ExecuteNonQuery();
            //        }
            //        else
            //        {
            //            var cmdIns = new SqlCommand(insertQuery, con);
            //            cmdIns.ExecuteNonQuery();
            //        }
            //        con.Close();
            //    }
            //}
            ListRound.AddInDB(Players);
            var score = new ScoreTable();
            score.Show();
            Close();
        }

       
    }
}
