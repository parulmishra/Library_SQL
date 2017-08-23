using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Library.Models;

namespace Library.Tests
{
  [TestClass]
  public class CopyTest : IDisposable
  {
    public CopyTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }
    public void Dispose()
    {
      Copy.DeleteAll();
      Author.DeleteAll();
      Book.DeleteAll();
    }
    [TestMethod]
    public void Equals_TrueForSameCopy_True()
    {
      //Arrange, Act
      Copy firstCopy = new Copy(1,false);
      Copy secondCopy = new Copy(1,false);

      //Assert
      Assert.AreEqual(firstCopy, secondCopy);
    }
    [TestMethod]
    public void Save_SavesCopyToDatabase_CopyList()
    {
      Copy testCopy = new Copy(1,false);
      testCopy.Save();

      List<Copy> expected = new List<Copy> {testCopy};
      List<Copy> result = Copy.GetAll();

      CollectionAssert.AreEqual(expected, result);
    }
    [TestMethod]
    public void Save_AssignsIdToObject_id()
    {
      //Arrange
      Copy testCopy = new Copy(1,true);
      testCopy.Save();

      //Act
      Copy savedCopy = Copy.GetAll()[0];

      int result = savedCopy.GetId();
      int testId = testCopy.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void Find_FindsCopyInDatabase_Copy()
    {
      //Arrange
      Copy testCopy = new Copy(1,true);
      testCopy.Save();

      //Act
      Copy result = Copy.Find(testCopy.GetId());

      //Assert
      Assert.AreEqual(testCopy, result);
    }
    [TestMethod]
    public void Delete_DeletesCopyFromDatabase_CopyList()
    {
      //Arrange
      Copy testCopy1 = new Copy(1,true);
      testCopy1.Save();

      Copy testCopy2 = new Copy(2,true);
      testCopy2.Save();

      //Act
      testCopy1.Delete();
      List<Copy> resultCopyList = Copy.GetAll();
      List<Copy> testCopyList = new List<Copy> {testCopy2};

      //Assert
      CollectionAssert.AreEqual(testCopyList, resultCopyList);
    }

    [TestMethod]
    public void Update_ReturnsAllCopyMatchingUpdateTerm_CopyList()
    {
      //Arrange
      Copy testCopy = new Copy(1, true);
      testCopy.Save();

      testCopy.Update(false);
      Copy expected = new Copy(1, false, testCopy.GetId());
      //Act
      Copy actual = Copy.GetAll()[0];
      //Assert
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetBook_ReturnsBook_Book()
    {
      //Arrange
      Book testBook = new Book("The join complexity resolved");
      testBook.Save();
      Copy copy = new Copy(testBook.GetId(), false);
      Book expected = new Book("The join complexity resolved", testBook.GetId());
      //Act
      Book actual = copy.GetBook();
      //Assert
      Assert.AreEqual(expected, actual);
    }
  }
}
