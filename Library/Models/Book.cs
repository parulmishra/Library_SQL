using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
  public class Book
  {
    private int _id;
    private string _title;

    public Book(string title, int id=0)
    {
      _id= id;
      _title = title;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetTitle()
    {
      return _title;
    }

    public override bool Equals(Object otherBook)
    {
      if (!(otherBook is Book))
      {
        return false;
      }
      else
      {
        Book newBook = (Book) otherBook;
        bool idEquality = newBook.GetId() == this._id;
        bool titleEquality = newBook.GetTitle() == this._title;
        return (idEquality && titleEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetTitle().GetHashCode();
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT  INTO books (title) VALUES (@title);";

      MySqlParameter titleParameter = new MySqlParameter();
      titleParameter.ParameterName = "@title";
      titleParameter.Value = _title;
      cmd.Parameters.Add(titleParameter);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Book> GetAll()
    {
      List<Book> allBooks = new List<Book>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM books;";

      var rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string title = rdr.GetString(1);
        Book newBook = new Book(title, id);
        allBooks.Add(newBook);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allBooks;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM books;";

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Book Find(int id)
    {
      MySqlConnection conn =DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM books WHERE id=@thisId";

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = id;
      cmd.Parameters.Add(idParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int idBook = 0;
      string title = "";

      while(rdr.Read())
      {
        idBook = rdr.GetInt32(0);
        title = rdr.GetString(1);
      }
      var book = new Book(title, idBook);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return book;
    }

    public void Delete()
    {
      MySqlConnection conn =DB.Connection();
      conn.Open();

      MySqlCommand cmd = new MySqlCommand(@"DELETE FROM books WHERE id=@thisId; DELETE FROM authors_books WHERE book_id =@thisId;",conn);

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

    public static List<Book> Search(string title)
    {
      List<Book> foundBooks = new List<Book>{};
      string searchTerm = title.ToLower()[0].ToString();
      string wildCard = searchTerm + "%";
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM books WHERE title LIKE @wildCard;";

      MySqlParameter searchTermParameter = new MySqlParameter();
      searchTermParameter.ParameterName = "@wildCard";
      searchTermParameter.Value = wildCard;
      cmd.Parameters.Add(searchTermParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int idBook = rdr.GetInt32(0);
        string bookTitle = rdr.GetString(1);
        Book book = new Book(bookTitle, idBook);
        foundBooks.Add(book);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundBooks;
    }

    public void AddAuthor(Author newAuthor)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO authors_books(book_id, author_id) VALUES (@bookId, @authorId);";

      MySqlParameter bookIdParameter = new MySqlParameter();
      bookIdParameter.ParameterName = "@bookId";
      bookIdParameter.Value = this._id;
      cmd.Parameters.Add(bookIdParameter);

      MySqlParameter authorIdParameter = new MySqlParameter();
      authorIdParameter.ParameterName = "@authorId";
      authorIdParameter.Value = newAuthor.GetId();
      cmd.Parameters.Add(authorIdParameter);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public List<Author> GetAuthors()
    {
      List<Author> authors = new List<Author>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT authors.* FROM books JOIN authors_books ON(books.id = authors_books.book_id) JOIN authors ON(authors_books.author_id = authors.id) WHERE books.id=@bookId;";

      MySqlParameter bookIdParameter= new MySqlParameter();
      bookIdParameter.ParameterName = "@bookId";
      bookIdParameter.Value = this._id;
      cmd.Parameters.Add(bookIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string authorName = rdr.GetString(1);
        Author author = new Author(authorName, authorId);
        authors.Add(author);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return authors;
    }
    public List<Copy> GetCopies()
    {
      List<Copy> copies = new List<Copy>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM copies WHERE book_id=@bookId;";

      MySqlParameter bookIdParameter= new MySqlParameter();
      bookIdParameter.ParameterName = "@bookId";
      bookIdParameter.Value = this._id;
      cmd.Parameters.Add(bookIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int copyId = rdr.GetInt32(0);
        int bookId = rdr.GetInt32(1);
        bool available = rdr.GetBoolean(2);

        Copy copy = new Copy(bookId, available, copyId);
        copies.Add(copy);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return copies;
    }
    public void Update(string newTitle)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE books SET title=@newTitle WHERE id=@thisId;";

      MySqlParameter titleParameter = new MySqlParameter();
      titleParameter.ParameterName = "@newTitle";
      titleParameter.Value = newTitle;
      cmd.Parameters.Add(titleParameter);

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
