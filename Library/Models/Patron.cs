using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
  public class Patron
  {
    private int _id;
    private string _name;

    public Patron(string name, int id=0)
    {
      _id= id;
      _name = name;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public override bool Equals(Object otherPatron)
    {
      if (!(otherPatron is Patron))
      {
        return false;
      }
      else
      {
        Patron newPatron = (Patron) otherPatron;
        bool idEquality = newPatron.GetId() == this._id;
        bool nameEquality = newPatron.GetName() == this._name;
        return (idEquality && nameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO patrons(name) VALUES(@name);";

      MySqlParameter nameParameter = new MySqlParameter();
      nameParameter.ParameterName = "@name";
      nameParameter.Value = _name;
      cmd.Parameters.Add(nameParameter);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Patron> GetAll()
    {
      List<Patron> allPatrons = new List<Patron>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons;";

      var rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Patron newPatron = new Patron(name, id);
        allPatrons.Add(newPatron);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allPatrons;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM patrons;";

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Patron Find(int id)
    {
      MySqlConnection conn =DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons WHERE id=@thisId";

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = id;
      cmd.Parameters.Add(idParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int idPatron = 0;
      string name = "";

      while(rdr.Read())
      {
        idPatron = rdr.GetInt32(0);
        name = rdr.GetString(1);
      }
      var patron = new Patron(name, idPatron);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return patron;
    }

    public void Delete()
    {
      MySqlConnection conn =DB.Connection();
      conn.Open();

      MySqlCommand cmd = new MySqlCommand(@"DELETE FROM patrons WHERE id=@thisId; DELETE FROM checkouts WHERE patron_id =@thisId;",conn);

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

    public void AddToCheckout(Copy newCopy)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO checkouts(patron_id, copy_id, overdue, due_date, checkout_date) VALUES (@patronId, @copyId, @overdue, @dueDate, @checkoutDate);";

      MySqlParameter patronIdParameter = new MySqlParameter();
      patronIdParameter.ParameterName = "@patronId";
      patronIdParameter.Value = this._id;
      cmd.Parameters.Add(patronIdParameter);

      MySqlParameter copyIdParameter = new MySqlParameter();
      copyIdParameter.ParameterName = "@copyId";
      copyIdParameter.Value = newCopy.GetId();
      cmd.Parameters.Add(copyIdParameter);

      MySqlParameter overdueParameter = new MySqlParameter();
      overdueParameter.ParameterName = "@overdue";
      overdueParameter.Value = false;
      cmd.Parameters.Add(overdueParameter);

      MySqlParameter dueDateParameter = new MySqlParameter();
      dueDateParameter.ParameterName = "@dueDate";
      dueDateParameter.Value = DateTime.Now.AddDays(15);
      cmd.Parameters.Add(dueDateParameter);

      MySqlParameter checkoutDateParameter = new MySqlParameter();
      checkoutDateParameter.ParameterName = "@checkoutDate";
      checkoutDateParameter.Value = DateTime.Now;
      cmd.Parameters.Add(checkoutDateParameter);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Copy> GetCopies()
    {
      List<Copy> copies = new List<Copy>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT copies.* FROM patrons
      JOIN checkouts ON(checkouts.patron_id = patrons.id)
      JOIN copies ON(copies.id = checkouts.copy_id)
      WHERE patrons.id=@patronId;";

      MySqlParameter patronIdParameter= new MySqlParameter();
      patronIdParameter.ParameterName = "@patronId";
      patronIdParameter.Value = this._id;
      cmd.Parameters.Add(patronIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        Console.WriteLine("GetCopies method while loop==============" + copies.Count.ToString());

        int copyId = rdr.GetInt32(0);
        int bookId = rdr.GetInt32(1);
        bool available = rdr.GetBoolean(2);
        Copy copy = new Copy(bookId, available, copyId);
        copies.Add(copy);
      }
      Console.WriteLine("GetCopies method ==============" + copies.Count.ToString());

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return copies;
    }

    public List<Book> GetBooks()
    {
      List<Book> patronBooks = new List<Book>{};
      List<Copy> patronCopies = this.GetCopies();

      foreach(var copy in patronCopies)
      {
        patronBooks.Add(copy.GetBook());
      }
      return patronBooks;
    }

    public static List<Patron> Search(string name)
    {
      List<Patron> foundPatrons = new List<Patron>{};
      string searchTerm = name.ToLower()[0].ToString();
      string wildCard = searchTerm + "%";
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons WHERE name LIKE @wildCard;";

      MySqlParameter searchTermParameter = new MySqlParameter();
      searchTermParameter.ParameterName = "@wildCard";
      searchTermParameter.Value = wildCard;
      cmd.Parameters.Add(searchTermParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int idPatron = rdr.GetInt32(0);
        string patronName = rdr.GetString(1);
        Patron patron = new Patron(patronName, idPatron);
        foundPatrons.Add(patron);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundPatrons;
    }
    public void Update(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE patrons SET name=@newName WHERE id=@thisId;";

      MySqlParameter nameParameter = new MySqlParameter();
      nameParameter.ParameterName = "@newName";
      nameParameter.Value = newName;
      cmd.Parameters.Add(nameParameter);

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

  }
}
