using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Library.Models;

namespace Library.Tests
{
  [TestClass]
  public class BookTest : IDisposable
  {
    public BookTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }
    public void Dispose()
    {
      Author.DeleteAll();
      Book.DeleteAll();
    }
    [TestMethod]
    public void Equals_TrueForSameBookTitle_True()
    {
      //Arrange, Act
      Book firstBook = new Book("Bootcamp Experience");
      Book secondBook = new Book("Bootcamp Experience");

      //Assert
      Assert.AreEqual(firstBook, secondBook);
    }
    [TestMethod]
    public void Save_SavesBookToDatabase_BookList()
    {
      Book testBook = new Book("Bootcamp Experience");
      testBook.Save();

      List<Book> expected = new List<Book> {testBook};
      List<Book> result = Book.GetAll();

      CollectionAssert.AreEqual(expected, result);
    }
    [TestMethod]
    public void Save_AssignsIdToObject_id()
    {
      //Arrange
      Book testBook = new Book("C# tutorials");
      testBook.Save();

      //Act
      Book savedBook = Book.GetAll()[0];

      int result = savedBook.GetId();
      int testId = testBook.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void Find_FindsBookInDatabase_Book()
    {
      //Arrange
      Book testBook = new Book("C# tutorials");
      testBook.Save();

      //Act
      Book result = Book.Find(testBook.GetId());

      //Assert
      Assert.AreEqual(testBook, result);
    }

    [TestMethod]
    public void Delete_DeletesBookFromDatabase_BookList()
    {
      //Arrange
      Book testBook1 = new Book("MVC");
      testBook1.Save();

      Book testBook2 = new Book(".NET");
      testBook2.Save();

      //Act
      testBook1.Delete();
      List<Book> resultBookList = Book.GetAll();
      List<Book> testBookList = new List<Book> {testBook2};

      //Assert
      CollectionAssert.AreEqual(testBookList, resultBookList);
    }
    [TestMethod]
    public void Test_AddAuthor_AddsAuthorToBook()
    {
      //Arrange
      Book testBook = new Book("Epicodus");
      testBook.Save();

      Author testAuthor = new Author("Parul");
      testAuthor.Save();

      Author testAuthor2 = new Author("Robert");
      testAuthor2.Save();

      //Act
      testBook.AddAuthor(testAuthor);
      testBook.AddAuthor(testAuthor2);

      List<Author> result = testBook.GetAuthors();
      List<Author> testList = new List<Author>{testAuthor, testAuthor2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void GetAuthors_ReturnsAllAuthorsforBook_AuthorList()
    {
      //Arrange
      Book testBook = new Book("Michael Jackson");
      testBook.Save();

      Author testAuthor1 = new Author("Michael");
      testAuthor1.Save();

      Author testAuthor2 = new Author("Parul");
      testAuthor2.Save();

      //Act
      testBook.AddAuthor(testAuthor1);
      List<Author> savedAuthors = testBook.GetAuthors();
      List<Author> testList = new List<Author> {testAuthor1};
      //Assert
      CollectionAssert.AreEqual(testList, savedAuthors);
    }
    [TestMethod]
    public void Search_ReturnsAllBooksMatchingSearchTerm_BookList()
    {
      //Arrange
      Book testBook = new Book(".NET");
      testBook.Save();

      Book testBook2 = new Book("JavaScript");
      testBook2.Save();

      //Act

      List<Book> savedBooks = Book.Search(".n");
      List<Book> testList = new List<Book> {testBook};

      //Assert
      CollectionAssert.AreEqual(testList, savedBooks);
    }
    [TestMethod]
    public void Update_ReturnsAllBooksMatchingUpdateTerm_BookList()
    {
      //Arrange
      Book testBook = new Book("db");
      testBook.Save();

      testBook.Update("rdbms");
      Book expected = new Book("rdbms", testBook.GetId());
      //Act
      Book actual = Book.GetAll()[0];
      //Assert
      Assert.AreEqual(expected, actual);
    }
  }
}
