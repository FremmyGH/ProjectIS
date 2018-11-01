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
using System.Data.SqlClient;
using System.Data;
using System.Windows.Documents.DocumentStructures;

namespace Game
{
    /// <summary>
    /// Логика взаимодействия для ScoreTable.xaml
    /// </summary>
    public partial class ScoreTable : Window
    {
        public ScoreTable()
        {
            InitializeComponent();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            const string connection = @"Data Source=DESKTOP-FPQ69BF\SQLSERVER;Initial Catalog=GameDice;Integrated Security=True";
            var ds = new DataSet();
            using (var con = new SqlConnection(connection))
            {
                var query = "SELECT Name,Score FROM Player";
                //con.Open();
                //var cmd = new SqlCommand(query,con);
                var adapter = new SqlDataAdapter(query,con);
                adapter.Fill(ds, "Player");
                ScoreGridView.ItemsSource = ds.Tables[0].DefaultView;

            }
        }
    }
}
