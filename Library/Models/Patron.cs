// using System.Collections.Generic;
// using MySql.Data.MySqlClient;
// using System;
//
// namespace Library.Models
// {
//   public class Patron
//   {
//     private int _id;
//     private string _name;
//
//     public Patron(string name, int id=0)
//     {
//       _id= id;
//       _name = name;
//     }
//     public int GetId()
//     {
//       return _id;
//     }
//
//     public string GetName()
//     {
//       return _name;
//     }
//
//     public override bool Equals(Object otherPatron)
//     {
//       if (!(otherPatron is Patron))
//       {
//         return false;
//       }
//       else
//       {
//         Patron newPatron = (Patron) otherPatron;
//         bool idEquality = newPatron.GetId() == this._id;
//         bool nameEquality = newPatron.GetName() == this._name;
//         return (idEquality && nameEquality);
//       }
//     }
//     public override int GethashCode()
//     {
//       return this.GetName().GetHashCode();
//     }
//     public void Save()
//     {
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//       var cmd = conn.CreateCommand() as MySqlCommand;
//       cmd.CommandText = @"INSERT INTO patrons(name) VALUES(@name);";
//
//       MySqlParameter nameParameter = new MySqlParameter();
//       nameParameter.ParameterName = "@name";
//       nameParameter.Value = _name;
//       cmd.Parameters.Add(nameParameter);
//
//       cmd.ExecuteNonQuery();
//       _id = (int) cmd.LastInsertedId;
//       conn.Close();
//       if (conn != null)
//       {
//         conn.Dispose();
//       }
//     }
//     public static List<Patron> GetAll()
//     {
//       List<Patron> allPatrons = new List<Patron>{};
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//
//       var cmd = conn.CreateCommand() as MySqlCommand;
//       cmd.CommandText = @"SELECT * FROM patrons;";
//
//       var rdr = cmd.ExecuteReader();
//       while(rdr.Read())
//       {
//         int id = rdr.GetInt32(0);
//         string name = rdr.GetString(1);
//         Patron newPatron = new Patron(name, id);
//         allPatrons.Add(newPatron);
//       }
//       conn.Close();
//       if (conn != null)
//       {
//         conn.Dispose();
//       }
//       return allPatrons;
//     }
//     public static void DeleteAll()
//     {
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//
//       var cmd = conn.CreateCommand() as MySqlCommand;
//       cmd.CommandText = @"DELETE FROM patrons;";
//
//       cmd.ExecuteNonQuery();
//       conn.Close();
//       if (conn != null)
//       {
//         conn.Dispose();
//       }
//     }
//     public static Patron Find(int id)
//     {
//       MySqlConnection conn =DB.Connection();
//       conn.Open();
//
//       var cmd = conn.CreateCommand() as MySqlCommand;
//       cmd.CommandText = @"SELECT * FROM patrons WHERE id=@thisId";
//
//       MySqlParameter idParameter = new MySqlParameter;
//       idParameter.ParameterName = "@thisId";
//       idParameter.Value = _id;
//       cmd.Parameters.Add(idParameter);
//
//       var rdr = cmd.ExecuteReader() as MySqlDataReader;
//
//       int idPatron = 0;
//       string name = "";
//
//       while(rdr.Read())
//       {
//         idPatron = rdr.GetInt32(0);
//         name = rdr.GetString(1);
//       }
//       var author = new Patron(name, idPatron);
//
//       conn.Close();
//       if (conn != null)
//       {
//         conn.Dispose();
//       }
//       return author;
//     }
//     public void Delete()
//     {
//       MySqlConnection conn =DB.Connection();
//       conn.Open();
//
//       MySqlCommand cmd = new MySqlCommnd(@"DELETE FROM patrons WHERE id=@thisId; DELETE FROM checkouts WHERE patron_id =@thisId;",conn);
//
//       MySqlParameter idParameter = new MySqlParameter;
//       idParameter.ParameterName = "@thisId";
//       idParameter.Value = _id;
//       cmd.Parameters.Add(idParameter);
//
//       cmd.ExecuteNonQuery();
//       conn.Close();
//       if (conn != null)
//       {
//         conn.Dispose();
//       }
//     }
//     public void AddBook(Book newBook)
//     {
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//
//       var cmd = conn.CreateCommand() as MySqlCommand;
//       cmd.CommandText = @"INSERT INTO patrons_books(book_id, author_id) VALUES (@bookId, @authorId);";
//
//       MySqlParameter bookIdParameter = new MySqlParameter;
//       bookIdParameter.ParameterName = "@bookId";
//       bookIdParameter.Value = newBook.GetId();
//       cmd.Parameters.Add(bookIdParameter);
//
//       MySqlParameter authorIdParameter = new MySqlParameter;
//       authorIdParameter.ParameterName = "@authorId";
//       authorIdParameter.Value = this._id;
//       cmd.Parameters.Add(authorIdParameter);
//
//       cmd.ExecuteNonQuery();
//       conn.Close();
//       if (conn != null)
//       {
//         conn.Dispose();
//       }
//     }
//     public List<Book> GetBooks()
//     {
//       List<Book> books = new List<Book>{};
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//
//       var cmd = conn.CreateCommand() as MySqlCommand;
//       cmd.CommandText = @"SELECT books.* FROM patrons JOIN patrons_books ON(patrons.id = patrons_books.author_id) JOIN books ON(patrons_books.book_id = books.id) WHERE patrons.id=@authorId;";
//
//       MySqlParameter authorIdParameter= new MySqlParameter;
//       authorIdParameter.ParameterName = "@authorId";
//       authorIdParameter.Value = this._id;
//       cmd.Parameters.Add(authorIdParameter);
//
//       var rdr = cmd.ExecuteReader() as MySqlDataReader;
//       while(rdr.Read())
//       {
//         int bookId = rdr.GetInt32(0);
//         int bookTitle = rdr.GetString(1);
//         Book book = new Book(bookTitle, bookId)
//         books.Add(book);
//       }
//       conn.Close();
//       if (conn != null)
//       {
//         conn.Dispose();
//       }
//       return books;
//     }
//     public static List<Patron> Search(string name)
//     {
//       List<Patron> foundPatrons = new List<Patron>{};
//       string searchTerm = name.ToLower()[0].ToString();
//       string wildCard = searchTerm + "%";
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//       var cmd = conn.CreateCommand() as MySqlCommand;
//       cmd.CommandText = @"SELECT * FROM patrons WHERE name LIKE @wildCard;";
//
//       MySqlParameter searchTermParameter = new MySqlParameter;
//       searchTermParameter.ParameterName = "@wildCard";
//       searchTermParameter.Value = wildCard;
//       cmd.Parameters.Add(searchTermParameter);
//
//       var rdr = cmd.ExecuteReader() as MySqlDataReader;
//       while(rdr.Read())
//       {
//         int idPatron = rdr.GetInt32(0);
//         string name = rdr.GetString(1);
//         Patron author = new Patron(name, idPatron);
//         foundPatrons.Add(author);
//       }
//       conn.Close();
//       if (conn != null)
//       {
//         conn.Dispose();
//       }
//       return foundPatrons;
//     }
//     public void Update(string newName)
//     {
//       MySqlConnection conn = DB.Connection();
//       conn.Open();
//
//       var cmd = conn.CreateCommand() as MySqlCommand;
//       cmd.CommandText = @"UPDATE patrons SET name=@newName WHERE id=@thisId;";
//
//       MySqlParameter nameParameter = new MySqlParameter();
//       nameParameter.ParameterName = "@newName";
//       nameParameter.Value = newName;
//       cmd.Parameters.Add(nameParameter);
//
//       MySqlParameter idParameter = new MySqlParameter();
//       idParameter.ParameterName = "@thisId";
//       idParameter.Value = _id;
//       cmd.Parameters.Add(idParameter);
//
//       cmd.ExecuteNonQuery();
//       conn.close();
//       if(conn != null)
//       {
//         conn.Dispose();
//       }
//     }
//
//   }
// }
