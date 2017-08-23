using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
  public class Copy
  {
    private int _id;
    private int _bookId;
    private bool _available;

    public Copy(int bookId, bool available = true,int id = 0)
    {
      _id = id;
      _bookId = bookId;
      _available = available;
    }
    public int GetId()
    {
      return _id;
    }
    public int GetBookId()
    {
      return _bookId;
    }
    public bool GetAvailable()
    {
      return _available;
    }
    public override bool Equals(System.Object otherCopy)
		{
			if(!(otherCopy is Copy))
			{
				return false;
			}
			else
			{
				Copy newCopy = (Copy) otherCopy;
				bool idEquality = (this.GetId() == newCopy.GetId());
				bool bookIdEquality = (this.GetBookId() == newCopy.GetBookId());
				bool availableEquality = (this.GetAvailable() == newCopy.GetAvailable());
				return (idEquality && bookIdEquality && availableEquality);
			}
		}
    public override int GetHashCode()
		{
			return this.GetId().GetHashCode();
		}
    public static List<Copy> GetAll()
		{
			List<Copy> allCopies = new List<Copy>();
			MySqlConnection conn = DB.Connection();
			conn.Open();

			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"SELECT * FROM copies;";
			var rdr = cmd.ExecuteReader() as MySqlDataReader;
			while(rdr.Read())
			{
				int id = rdr.GetInt32(0);
				int bookId = rdr.GetInt32(1);
				bool available = rdr.GetBoolean(2);
				Copy newCopy = new Copy(bookId,available,id);
				allCopies.Add(newCopy);
			}
			conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
			return allCopies;
		}
    public void Save()
		{
			MySqlConnection conn = DB.Connection();
			conn.Open();

			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"INSERT INTO copies(book_id,available) VALUES(@book_id, @available);";

			MySqlParameter availableParameter = new MySqlParameter();
			availableParameter.ParameterName = "@available";
			availableParameter.Value = this._available;
			cmd.Parameters.Add(availableParameter);

			MySqlParameter bookIdParameter = new MySqlParameter();
			bookIdParameter.ParameterName = "@book_id";
			bookIdParameter.Value = this._bookId;
			cmd.Parameters.Add(bookIdParameter);

			cmd.ExecuteNonQuery();
			_id = (int) cmd.LastInsertedId;
			conn.Close();
		}
    public static Copy Find(int id)
    {
      MySqlConnection conn =DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM copies WHERE id=@thisId";

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = id;
      cmd.Parameters.Add(idParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int copyId = 0;
      int bookId = 0;
      bool available  = false;

      while(rdr.Read())
      {
        copyId = rdr.GetInt32(0);
        bookId = rdr.GetInt32(1);
        available = rdr.GetBoolean(2);
      }
      var copy = new Copy(bookId,available,copyId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return copy;
    }
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM copies;";

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public void Delete()
    {
      MySqlConnection conn =DB.Connection();
      conn.Open();

      MySqlCommand cmd = new MySqlCommand(@"DELETE FROM copies WHERE id=@thisId; DELETE FROM checkouts WHERE copy_id =@thisId;",conn);

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = _id;
      cmd.Parameters.Add(idParameter);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public void Update(bool available)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE copies SET available=@available WHERE id=@thisId;";

      MySqlParameter availableParameter = new MySqlParameter();
      availableParameter.ParameterName = "@available";
      availableParameter.Value = available;
      cmd.Parameters.Add(availableParameter);

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = _id;
      cmd.Parameters.Add(idParameter);

      cmd.ExecuteNonQuery();
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
    public Book GetBook()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM books WHERE id=@bookId";

      MySqlParameter bookIdParameter = new MySqlParameter();
      bookIdParameter.ParameterName = "@bookId";
      bookIdParameter.Value = this._bookId;
      cmd.Parameters.Add(bookIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int bookId = 0;
      string title = "";

      while(rdr.Read())
      {
        bookId = rdr.GetInt32(0);
        title = rdr.GetString(1);
      }
      var book = new Book(title, bookId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return book;
    }
  }
}
