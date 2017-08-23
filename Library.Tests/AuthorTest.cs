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

    public void Dispose()
    {
      // Student.DeleteAll();
      Author.DeleteAll();
      Book.DeleteAll();
    }
  }
}
