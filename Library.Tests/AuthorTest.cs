using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Library.Models;

namespace Library.Tests
{
  [TestClass]
  public class AuthorTest : IDisposable
  {
    public AuthorTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }
    [TestMethod]
    public void Equals_TrueForSameAuthorName_Author()
    {
      //Arrange, Act
      Author firstAuthor = new Author("Michael");
      Author secondAuthor = new Author("Michael");

      //Assert
      Assert.AreEqual(firstAuthor, secondAuthor);
    }
    [TestMethod]
    public void Save_SavesAuthorToDatabase_AuthorList()
    {
      Author testAuthor = new Author("Michael");
      testAuthor.Save();

      List<Author> expected = new List<Author> {testAuthor};
      List<Author> result = Author.GetAll();

      CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_id()
    {
      //Arrange
      Author testAuthor = new Author("Mow the lawn");
      testAuthor.Save();

      //Act
      Author savedAuthor = Author.GetAll()[0];

      int result = savedAuthor.GetId();
      int testId = testAuthor.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void Find_FindsAuthorInDatabase_Author()
    {
      //Arrange
      Author testAuthor = new Author("Michael");
      testAuthor.Save();

      //Act
      Author result = Author.Find(testAuthor.GetId());

      //Assert
      Assert.AreEqual(testAuthor, result);
    }

    [TestMethod]
    public void Delete_DeletesAuthorFromDatabase_AuthorList()
    {
      //Arrange
      Author testAuthor1 = new Author("Michael");
      testAuthor1.Save();

      Author testAuthor2 = new Author("Robert");
      testAuthor2.Save();

      //Act
      testAuthor1.Delete();
      List<Author> resultAuthorList = Author.GetAll();
      List<Author> testAuthorList = new List<Author> {testAuthor2};

      //Assert
      CollectionAssert.AreEqual(testAuthorList, resultAuthorList);
    }

    [TestMethod]
    public void Test_AddBook_AddsBookToAuthor()
    {
      //Arrange
      Author testAuthor = new Author("Parul");
      testAuthor.Save();

      Book testBook = new Book("The tea");
      testBook.Save();

      Book testBook2 = new Book("The coffee");
      testBook2.Save();

      //Act
      testAuthor.AddBook(testBook);
      testAuthor.AddBook(testBook2);

      List<Book> result = testAuthor.GetBooks();
      List<Book> testList = new List<Book>{testBook, testBook2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetBooks_ReturnsAllAuthorBooks_BookList()
    {
      //Arrange
      Author testAuthor = new Author("Michael");
      testAuthor.Save();

      Book testBook1 = new Book("The author");
      testBook1.Save();

      Book testBook2 = new Book("Common mistakes");
      testBook2.Save();

      //Act
      testAuthor.AddBook(testBook1);
      List<Book> savedBooks = testAuthor.GetBooks();
      List<Book> testList = new List<Book> {testBook1};

      //Assert
      CollectionAssert.AreEqual(testList, savedBooks);
    }

    [TestMethod]
    public void Search_ReturnsAllAuthorMatchingSearchTerm_AuthorList()
    {
      //Arrange
      Author testAuthor = new Author("Michael");
      testAuthor.Save();

      Author testAuthor2 = new Author("Parul");
      testAuthor2.Save();

      //Act

      List<Author> savedAuthors = Author.Search("Mi");
      List<Author> testList = new List<Author> {testAuthor};

      //Assert
      CollectionAssert.AreEqual(testList, savedAuthors);
    }

    [TestMethod]
    public void Update_ReturnsAllAuthorMatchingUpdateTerm_AuthorList()
    {
      //Arrange
      Author testAuthor = new Author("Michael");
      testAuthor.Save();

      testAuthor.Update("Robert");
      Author expected = new Author("Robert", testAuthor.GetId());
      //Act
      Author actual = Author.GetAll()[0];
      //Assert
      Assert.AreEqual(expected, actual);
    }

    public void Dispose()
    {
      // Student.DeleteAll();
      Author.DeleteAll();
      Book.DeleteAll();
    }
  }
}
