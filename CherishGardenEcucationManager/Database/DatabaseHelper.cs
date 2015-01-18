using CherishGardenEducationManager.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace CherishGardenEducationManager.Database
{
    class DatabaseHelper
    {
        private static string CONNECTIONSTR = ConfigurationManager.ConnectionStrings["cgemConnectionString"].ConnectionString;

        public static OperatorUser findOperatorUser(string loginuser)
        {
            OperatorUser user = null;
            MySqlConnection conn;
            MySqlCommand cmd;
            conn = new MySqlConnection();
            conn.ConnectionString = CONNECTIONSTR;
            string query = "select password, mbid from operatorinfo where mbid in (select _id from memberbasic where engname=@username)";

            try
            {
                conn.Open();
                cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@username", loginuser);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    Console.WriteLine("no data!");
                }
                else {
                    while (reader.Read())
                    {
                        string password = reader[0].ToString();
                        int mbid = (int)reader[1];
                        user = new OperatorUser(password, mbid);
                    }
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.StackTrace);
            }
            finally {
                conn.Close();
            }

            return user;
        }
    }
}
