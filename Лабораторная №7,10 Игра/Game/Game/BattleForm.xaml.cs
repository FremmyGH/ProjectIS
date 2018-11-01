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
        string connection = @"Data Source=DESKTOP-FPQ69BF\SQLSERVER;Initial Catalog=GameDice;Integrated Security=True";
        DataSet ds = new DataSet();
        private int _number ;
        int _bestPlayerSum ;
        private string _bestPlayer = "";
        public BattleForm()
        {
            InitializeComponent();
        }

        private void Throw_Click(object sender, RoutedEventArgs e)
        {
            
            var dice = new Dice();
            dice.GetRandom();
            Players[_number].SumValue += dice.Value;
            if (dice.Value == 1)
            {
                ListPlayers.Text += $"Игрок {Players[_number].Name} набирает {dice.Value} очко." +
                                    $" Сумма очков: {Players[_number].SumValue}\n";
            }
            else if (dice.Value>4)
            {
                ListPlayers.Text += $"Игрок {Players[_number].Name} набирает {dice.Value} очков." +
                                    $" Сумма очков: {Players[_number].SumValue}\n";
            }
            else
            {
                ListPlayers.Text += $"Игрок {Players[_number].Name} набирает {dice.Value} очка." +
                                    $" Сумма очков: {Players[_number].SumValue}\n";
            }


        }

        private void BattleForm1_Activated(object sender, EventArgs e)
        {
            Next.IsEnabled = false;
            Players.Bank = Players.Length();
            for (var i = 0; i < Players.Length(); i++)
            {
                Players[i].SumMoney--;
            }
            ListPlayers.Text += "<<<<<<<Правила>>>>>>>\n";
            ListPlayers.Text += "1.Набрать наибольшее кол-во очков\n";
            ListPlayers.Text += "2.Не превысить 21 очко\n";
            ListPlayers.Text += "<<<<<<<Начало игры>>>>>>>\n";
            ListPlayers.Text += $"Ход игрока {Players[_number].Name}\n";
            Status.Text= $"Status: Turn from Player {Players[_number].Name}";

        }

        private void Pass_Click(object sender, RoutedEventArgs e)
        {
            if (_number==Players.Length()-1)
            {
                var ch = 0;
                for (var i = 0; i < Players.Length(); i++)
                {
                    if (_bestPlayerSum < Players[i].SumValue)
                    {
                    _bestPlayerSum = Players[i].SumValue;
                    _bestPlayer = Players[i].Name;
                    }
                    //else if (_bestPlayerSum[ch] == Players[i].SumValue)
                    //{
                    //    ch++;
                    //    _bestPlayerSum[ch] = Players[i].SumValue;
                    //    _bestPlayer[ch] = Players[i].Name;
                    //}

                }


                ListPlayers.Text += $"Игра закончена, победитель {_bestPlayer}, выигрыш: {Players.Length()}";
                Throw.IsEnabled = false;
                Pass.IsEnabled = false;
                Next.IsEnabled = true;
                _number = 0;
            }
            else
            {
                _number++;
                Status.Text = $"Status: Turn from Player {Players[_number].Name}";
                ListPlayers.Text += $"Ход игрока {Players[_number].Name}\n";
            }

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {

            for (var i = 0; i < Players.Length(); i++)
            {
                if (Players[i].SumMoney<1)
                {
                    Players.DeletePlayer(i);
                }
            }
            for (var i = 0; i < Players.Length(); i++)
            {
                Players[i].SumMoney--;
            }

            if (Players.Length()>1)
            {
                Throw.IsEnabled = true;
                Pass.IsEnabled = true;
                Next.IsEnabled = false;
                ListPlayers.Text = "";
                ListPlayers.Text += "<<<<<<<Правила>>>>>>>\n";
                ListPlayers.Text += "1.Набрать наибольшее кол-во очков\n";
                ListPlayers.Text += "2.Не превысить 21 очко\n";
                ListPlayers.Text += "<<<<<<<Начало игры>>>>>>>\n";
                ListPlayers.Text += $"Ход игрока {Players[_number].Name}\n";
                Status.Text = $"Status: Turn from Player {Players[_number].Name}";
            }
            else
            {
                MessageBox.Show("Not enough players for game");
                var main = new MainWindow();
                main.Show();
                Close();
            }
        }

        private void End_Click(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i < Players.Length(); i++)
            {
                var checkQuery = $"SELECT Name From Player WHERE Name='{Players[i].Name}'";
                var insertQuery = $"INSERT INTO Player(Name,Score) VALUES('{Players[i].Name}','{Players[i].SumMoney}')";
                using (var con = new SqlConnection(connection))
                {
                    con.Open();
                    var cmd = new SqlCommand(checkQuery,con);
                    var res = cmd.ExecuteScalar();
                    if (res!=null)
                    {
                        var updateQuery = $"UPDATE Player SET Score='{Players[i].SumMoney}'WHERE Name='{Players[i].Name}'";
                        var cmdUpd = new SqlCommand(updateQuery,con);
                        cmdUpd.ExecuteNonQuery();
                    }
                    else
                    {
                        var cmdIns= new SqlCommand(insertQuery,con);
                        cmdIns.ExecuteNonQuery();
                    }
                    con.Close();
                }

            }

            var score = new ScoreTable();
            score.Show();
            Close();
        }
    }
}
