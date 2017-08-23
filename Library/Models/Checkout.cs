using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
  public class Checkout
  {
    private int _id;
    private int _patronId;
    private int _copyId;
    private DateTime _dueDate;
    private DateTime _checkoutDate;
    private bool _overdue;

    public Checkout(int patronId,int copyId,bool overdue, DateTime dueDate,DateTime checkoutDate,int id=0)
    {
      _id = id;
      _patronId = patronId;
      _copyId = copyId;
      _overdue = overdue;
      _dueDate = dueDate;
      _checkoutDate = checkoutDate;
    }
    public int GetId()
    {
      return _id;
    }
    public int GetPatronId()
    {
      return _patronId;
    }
    public int GetCopyId()
    {
      return _copyId;
    }
    public DateTime GetDueDate()
    {
      return _dueDate;
    }
    public DateTime GetCheckoutDate()
    {
      return _checkoutDate;
    }
    public bool GetOverDue()
    {
      return _overdue;
    }
    public override bool Equals(Object otherCheckout)
    {
      if (!(otherCheckout is Checkout))
      {
        return false;
      }
      else
      {
        Checkout newCheckout = (Checkout) otherCheckout;
        bool idEquality = newCheckout.GetId() == this._id;
        bool patronIdEquality = newCheckout.GetPatronId() == this._patronId;
        bool copyIdEquality = newCheckout.GetCopyId() == this._copyId;
        bool dueDateEquality = newCheckout.GetDueDate() == this._dueDate;
        bool checkoutDateEquality = newCheckout.GetCheckoutDate() == this._checkoutDate;
        bool overDueEquality = newCheckout.GetOverDue() == this._overdue;
        return (idEquality && patronIdEquality && copyIdEquality && dueDateEquality && checkoutDateEquality && overDueEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO checkouts(patron_id,copy_id,overdue,due_date,checkout_date) VALUES(@patron_id,@copy_id,@overdue,@due_date,@checkout_date);";

      MySqlParameter patronIdParameter = new MySqlParameter();
      patronIdParameter.ParameterName = "@patron_id";
      patronIdParameter.Value = _patronId;
      cmd.Parameters.Add(patronIdParameter);

      MySqlParameter copyId = new MySqlParameter();
      copyId.ParameterName = "@copy_id";
      copyId.Value = _copyId;
      cmd.Parameters.Add(copyId);

      MySqlParameter overdueParameter = new MySqlParameter();
      overdueParameter.ParameterName = "@overdue";
      overdueParameter.Value = _overdue;
      cmd.Parameters.Add(overdueParameter);

      MySqlParameter dueDateParameter = new MySqlParameter();
      dueDateParameter.ParameterName = "@due_date";
      dueDateParameter.Value = _dueDate;
      cmd.Parameters.Add(dueDateParameter);

      MySqlParameter checkoutDateParameter = new MySqlParameter();
      checkoutDateParameter.ParameterName = "@checkout_date";
      checkoutDateParameter.Value = _checkoutDate;
      cmd.Parameters.Add(checkoutDateParameter);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

  }
}
