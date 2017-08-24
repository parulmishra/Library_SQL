using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Library.Models;

namespace Library.Tests
{
  [TestClass]
  public class CheckoutTest : IDisposable
  {
    public CheckoutTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }
    public void Dispose()
    {
      Checkout.DeleteAll();
      Copy.DeleteAll();
      Author.DeleteAll();
      Book.DeleteAll();
    }
    [TestMethod]
    public void Equals_TrueForSameCheckout_True()
    {
      DateTime newDueDate = default(DateTime);
      DateTime newCheckoutDate = default(DateTime);
      //Arrange, Act
      Checkout firstCheckout = new Checkout(1,1,false,newDueDate,newCheckoutDate,1);
      Checkout secondCheckout = new Checkout(1,1,false,newDueDate,newCheckoutDate,1);
      //Assert
      Assert.AreEqual(firstCheckout, secondCheckout);
    }
    [TestMethod]
    public void Save_SavesCheckoutToDatabase_CheckoutList()
    {
      DateTime newDueDate = default(DateTime);
      DateTime newCheckoutDate = default(DateTime);
      Checkout testCheckout = new Checkout(1,1,false,newDueDate,newCheckoutDate);
      testCheckout.Save();

      List<Checkout> expected = new List<Checkout> {testCheckout};
      List<Checkout> result = Checkout.GetAll();

      CollectionAssert.AreEqual(expected, result);
    }
  }
}
