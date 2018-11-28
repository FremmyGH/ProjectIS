using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace Game
{
    class Record
    {
        private static Record instance;
        //public bool check { get; private set; }
        public Players players { get; private set; }
        protected Record(Players players)
        {
            this.players = players;
            //this.check = check;
        }

        public void AddInDB()
        {
            //if (check)
            //{
                
            //}
            var connection = @"Data Source=DESKTOP-FPQ69BF\SQLSERVER;Initial Catalog=GameDice;Integrated Security=True";
            const string sqlExpression = "sp_checkQuery";
            const string sqlExpression1 = "sp_scoreQuery";
            const string sqlExpression2 = "sp_insertQuery";
            const string sqlExpression3 = "sp_updateQuery";


            for (var i = 0; i < players.Length(); i++)
            {
                //var checkQuery = $"SELECT Name, Score From Player WHERE Name='{players[i].Name}'";
                //var scoreQuery = $"SELECT Score From Player WHERE Name='{players[i].Name}'";

                //var insertQuery = $"INSERT INTO Player(Name,Score) VALUES('{players[i].Name}','{players[i].SumMoney}')";

                var nameP = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = players[i].Name
                };
                var nameP2 = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = players[i].Name
                };
                var nameP3 = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = players[i].Name
                };
                var scoreP = new SqlParameter
                {
                    ParameterName = "@score",
                    Value = players[i].SumMoney
                };
                var nameP4 = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = players[i].Name
                };
               
                using (var con = new SqlConnection(connection))
                {
                    con.Open();
                    var cmd = new SqlCommand(sqlExpression, con)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add(nameP);
                    var cmdScore = new SqlCommand(sqlExpression1, con)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    cmdScore.Parameters.Add(nameP2);


                    var res = cmd.ExecuteScalar();
                    var resScore = cmdScore.ExecuteScalar();
                    if (res != null)
                    {
                        var resultMoney = Convert.ToInt32(resScore);
                        resultMoney += Convert.ToInt32(players[i].SumMoney);
                        var scoreP2 = new SqlParameter
                        {
                            ParameterName = "@score",
                            Value = resultMoney
                        };
                        var cmdUpd = new SqlCommand(sqlExpression3, con)
                        {
                            CommandType = System.Data.CommandType.StoredProcedure
                        };
                        cmdUpd.Parameters.Add(nameP4);
                        cmdUpd.Parameters.Add(scoreP2);
                        cmdUpd.ExecuteNonQuery();
                        //var updateQuery = $"UPDATE Player SET Score='{resultMoney}'WHERE Name='{players[i].Name}'";
                        //var cmdUpd = new SqlCommand(updateQuery, con);
                        //cmdUpd.ExecuteNonQuery();
                    }
                    else
                    {
                        //var cmdIns = new SqlCommand(sqlExpression2, con);
                        var cmdIns = new SqlCommand(sqlExpression2, con)
                        {
                            CommandType = System.Data.CommandType.StoredProcedure
                        };
                        cmdIns.Parameters.Add(nameP3);
                        cmdIns.Parameters.Add(scoreP);
                        cmdIns.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
        }
        public static Record GetInstance(Players players)
        {
            if (instance == null)
            {
                instance=new Record(players);
                instance.AddInDB();
            }
            return instance;

            //return instance ?? (instance = new Record());
        }

    }
}
