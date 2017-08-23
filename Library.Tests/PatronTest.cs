using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Library.Models;

namespace Library.Tests
{
  [TestClass]
  public class PatronTest : IDisposable
  {
    public PatronTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }
    public void Dispose()
    {
      Patron.DeleteAll();
      Author.DeleteAll();
      Book.DeleteAll();
    }
    [TestMethod]
    public void Equals_TrueForSamePatronName_true()
    {
      //Arrange, Act
      Patron firstPatron = new Patron("Michael");
      Patron secondPatron = new Patron("Michael");

      //Assert
      Assert.AreEqual(firstPatron, secondPatron);
    }
    [TestMethod]
    public void Save_SavesPatronToDatabase_PatronList()
    {
      Patron testPatron = new Patron("Michael");
      testPatron.Save();

      List<Patron> expected = new List<Patron> {testPatron};
      List<Patron> result = Patron.GetAll();

      CollectionAssert.AreEqual(expected, result);
    }
    [TestMethod]
    public void Save_AssignsIdToObject_id()
    {
      //Arrange
      Patron testPatron = new Patron("Parul");
      testPatron.Save();
      //Act
      Patron savedPatron = Patron.GetAll()[0];
      int result = savedPatron.GetId();
      int testId = testPatron.GetId();
      //Assert
      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void Find_FindsPatronInDatabase_Patron()
    {
      //Arrange
      Patron testPatron = new Patron("Mishra");
      testPatron.Save();
      //Act
      Patron result = Patron.Find(testPatron.GetId());
      //Assert
      Assert.AreEqual(testPatron, result);
    }
    [TestMethod]
    public void Delete_DeletesPatronFromDatabase_PatronList()
    {
      //Arrange
      Patron testPatron1 = new Patron("Michael");
      testPatron1.Save();
      Patron testPatron2 = new Patron("Robert");
      testPatron2.Save();
      //Act
      testPatron1.Delete();
      List<Patron> resultPatronList = Patron.GetAll();
      List<Patron> testPatronList = new List<Patron> {testPatron2};
      //Assert
      CollectionAssert.AreEqual(testPatronList, resultPatronList);
    }
    [TestMethod]
    public void GetCopies_ReturnsAllCopiesForPatron_CopyList()
    {
      //Arrange
      Copy testCopy1 = new Copy(1,false);
      testCopy1.Save();
      Copy testCopy2 = new Copy(2,false);
      testCopy2.Save();
      Patron newPatron = new Patron("Parul");
      newPatron.Save();
      Checkout newCheckout = (testCopy1.GetId(), newPatron.GetId(), false, new DateTime(2017, 09, 23), new DateTime(2017, 09, 23));
      newCheckout.Save();
      //Act
      List<Copy> savedCopies = newPatron.GetCopies();
      List<Copy> testList = new List<Copy>{testCopy1};
      //Assert
      CollectionAssert.AreEqual(testList, savedCopies);
    }
  }
}
