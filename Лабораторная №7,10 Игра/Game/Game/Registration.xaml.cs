using System;
using System.Collections.Generic;
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

namespace Game
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        Players players = new Players();
        public static int Id { get; set; } = 0;
        private bool _close = true;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text!=""&&Sum.Text!="")
            {
                var repeat = true;
                var name = Name.Text;
                if (int.TryParse(Sum.Text, out var sum))
                {
                    for (var i = 0; i < players.Length(); i++)
                    {
                        if (players[i].Name == name)
                        {
                            MessageBox.Show("Name exists!");
                            repeat = false;
                        }
                        else if (sum == 0)
                        {
                            MessageBox.Show("Capital can be 0!");
                        }
                       
                    }
                    if (!repeat) return;
                    players.AddPlayer(Id, name, sum, false);
                    Id++;
                    Description.Text = $"New player {name}! Welcome!";
                }
                else
                {
                    MessageBox.Show("Enter correct sum!");
                }
            }
            else
            {
                MessageBox.Show("Empty Fields!Correct this!");
            }

            Name.Text = "";
            Sum.Text = "";
            //MessageBox.Show(players.Length().ToString());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Id = 1;
        }
        private void Start_Click_1(object sender, RoutedEventArgs e)
        {
            if (players.Length() >= 2)
            {
                var battleForm = new BattleForm {Players = players};
                battleForm.Show();
                _close = false;
                Close();
            }
            else
            {
                MessageBox.Show("Need more players");
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_close) return;
            var main = new MainWindow();
            main.Show();

        }
    }
}
