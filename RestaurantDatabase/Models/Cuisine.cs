using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RestaurantDatabase.Models
{
  public class Cuisine
  {
    public int Id {get; private set;}
    public string Name {get; private set;}

    public Cuisine(string name, int id = 0)
    {
      Name = name;
      Id = id;
    }

    public static List<Cuisine> GetAll()
    {
      List<Cuisine> output = new List<Cuisine> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cuisines;";

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Cuisine newCuisine = new Cuisine(name, id);
        output.Add(newCuisine);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return output;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM cuisines;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Cuisine FindById(int searchId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cuisines WHERE id = @CuisineId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@CuisineId";
      thisId.Value = searchId;
      cmd.Parameters.Add(thisId);

      int cuisineId = 0;
      string cuisineName = "";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        cuisineId = rdr.GetInt32(0);
        cuisineName = rdr.GetString(1);
      }
      Cuisine output = new Cuisine(cuisineName, cuisineId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return output;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO cuisines (name) VALUES (@Name);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@Name";
      name.Value = this.Name;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      this.Id = (int)cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public bool HasSamePropertiesAs(Cuisine other)
    {
      return (
        this.Id == other.Id &&
        this.Name == other.Name);
    }
  }
}