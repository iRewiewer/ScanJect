//using MySql.Data.MySqlClient;
using MySqlConnector;
using System;

namespace ScanJect.SQL
{
	public class Database
	{
		static public MySqlConnection db_con()
		{
			string ip = "78.96.207.87";
			string uid = "root";
			string pwd = "ciceronaznaz1422";
			string database = "central";

			MySqlConnection con = new MySqlConnection();
			//string str_con = "Server=" + ip + "; Database=" + database + "; Uid=" + uid + "; Pwd=" + pwd;
			string str_con = String.Format("Server={0}; Database={1}; Uid={2}; Pwd={3}", ip, database, uid, pwd);

			con.ConnectionString = str_con;

			return con;
		}

		static public void db_get(ref string obj_name, ref int obj_count)
		{
			MySqlConnection con = db_con();
			con.Open();
		}

		static public void db_add(string obj_name)
		{
			MySqlConnection con = db_con();
			try
			{
				con.Open();
				Console.WriteLine("Open");
				con.Close();
				Console.WriteLine("Close");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			string query = "INSERT INTO central.objects (Chair) values ('3')";

			MySqlCommand command = new MySqlCommand(query, con);
			
			//MySqlDataReader data_reader = command.ExecuteReader();

			command.ExecuteNonQuery();

			con.Close();
		}
	}
}