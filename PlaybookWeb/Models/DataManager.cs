using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PlaybookWeb.Models
{
  public class DataManager
  {
#if DEBUG
    private static String DB_CONN_STRING = "data source=.\\SQLEXPRESS;Integrated Security=SSPI;AttachDBFilename=C:\\Users\\David\\UltimateTactics\\PlaybookWeb\\App_Data\\PlayDatabase.mdf;User Instance=true";
#else
    private static String DB_CONN_STRING = "Server=mssql.aspnethosting.co.uk, 14330;Database=ultimate_playbook;Uid=ultimate_playbook_user;Password=nam11161V21!;";
#endif

    private static SqlConnection mConn = new SqlConnection(DB_CONN_STRING);
    private static Object mConnLock = new Object();

    public static String GetPlayData(int playId, string teamName)
    {
      String playData = "";

      lock (mConnLock)
      {
        SqlConnection conn = mConn;

        try
        {
          conn.Open();

          SqlCommand command = conn.CreateCommand();
          command.CommandText = "GetPlayById";
          command.CommandType = System.Data.CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@Name", teamName);
          command.Parameters.AddWithValue("@PlayId", playId);

          SqlDataReader reader = command.ExecuteReader();

          while (reader.Read())
          {
            playData = reader.GetString(0);
          }
        }
        finally
        {
          if (conn.State != System.Data.ConnectionState.Closed)
          {
            conn.Close();
          }
        }
      }

      return playData;
    }

    public static List<PlayGroup> GetPlaysForTeam(String teamName)
    {
      Dictionary<int, PlayGroup> playGroups = new Dictionary<int, PlayGroup>();

      lock (mConnLock)
      {
        SqlConnection conn = mConn;

        try
        {
          conn.Open();

          SqlCommand command = conn.CreateCommand();
          command.CommandText = "GetPlaysForTeam";
          command.CommandType = System.Data.CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@Name", teamName);

          SqlDataReader reader = command.ExecuteReader();

          while (reader.Read())
          {
            int PlayId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
            String PlayName = reader.IsDBNull(0) ? "" : reader.GetString(1);
            String PlayText = reader.IsDBNull(0) ? "" : reader.GetString(2);
            String PlayGroupName = reader.GetString(3);
            int PlayGroupId = reader.GetInt32(4);

            PlayGroup group;
            if (playGroups.Keys.Contains(PlayGroupId))
            {
              group = playGroups[PlayGroupId];
            }
            else
            {
              group = new PlayGroup(PlayGroupId, PlayGroupName, new List<Play>());

              playGroups.Add(PlayGroupId, group);
            }

            if (PlayId != -1)
            {
              group.Plays.Add(new Play(PlayId, PlayName, null, PlayText));
            }
          }
        }
        finally
        {
          if (conn.State != System.Data.ConnectionState.Closed)
          {
            conn.Close();
          }
        }
      }

      return playGroups.Values.ToList();
    }

    #region "Create items"
    public static void CreateTeam(String name)
    {
      lock (mConnLock)
      {
        SqlConnection conn = mConn;

        try
        {
          conn.Open();

          SqlCommand command = conn.CreateCommand();
          command.CommandText = "AddTeam";
          command.CommandType = System.Data.CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@Name", name);

          command.ExecuteNonQuery();
        }
        finally
        {
          if (conn.State != System.Data.ConnectionState.Closed)
          {
            conn.Close();
          }
        }
      }
    }

    public static void CreatePlayGroup(String name, String teamName)
    {
      lock (mConnLock)
      {
        SqlConnection conn = mConn;

        try
        {
          conn.Open();

          SqlCommand command = conn.CreateCommand();
          command.CommandText = "AddPlayGroup";
          command.CommandType = System.Data.CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@Name", name);
          command.Parameters.AddWithValue("@TeamName", teamName);

          command.ExecuteNonQuery();
        }
        finally
        {
          if (conn.State != System.Data.ConnectionState.Closed)
          {
            conn.Close();
          }
        }
      }
    }

    public static void CreatePlay(String name, 
                                  String playXml, 
                                  int groupId, 
                                  String playText,
                                  String teamName)
    {
      lock (mConnLock)
      {
        SqlConnection conn = mConn;

        try
        {
          conn.Open();

          SqlCommand command = conn.CreateCommand();
          command.CommandText = "AddPlay";
          command.CommandType = System.Data.CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@Name", name);
          command.Parameters.AddWithValue("@PlayData", playXml);
          command.Parameters.AddWithValue("@PlayGroupId", groupId);
          command.Parameters.AddWithValue("@PlayText", playText);
          command.Parameters.AddWithValue("@TeamName", teamName);

          command.ExecuteNonQuery();
        }
        finally
        {
          if (conn.State != System.Data.ConnectionState.Closed)
          {
            conn.Close();
          }
        }
      }
    }
    #endregion

    #region "Edit items"
    public static bool EditTeam(int id, String name)
    {
      throw new NotImplementedException();
    }

    public static void EditPlayGroup(int id, 
                                     String name, 
                                     String teamName)
    {
      lock (mConnLock)
      {
        SqlConnection conn = mConn;

        try
        {
          conn.Open();

          SqlCommand command = conn.CreateCommand();
          command.CommandText = "EditPlayGroup";
          command.CommandType = System.Data.CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@Id", id);
          command.Parameters.AddWithValue("@Name", name);
          command.Parameters.AddWithValue("@TeamName", teamName);

          command.ExecuteNonQuery();
        }
        finally
        {
          if (conn.State != System.Data.ConnectionState.Closed)
          {
            conn.Close();
          }
        }
      }
    }

    public static void EditPlay(int id,
                                String name, 
                                String playXml, 
                                int groupId, 
                                String playText,
                                String teamName)
    {
      lock (mConnLock)
      {
        SqlConnection conn = mConn;

        try
        {
          conn.Open();

          SqlCommand command = conn.CreateCommand();
          command.CommandText = "EditPlay";
          command.CommandType = System.Data.CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@Id", id);
          command.Parameters.AddWithValue("@Name", name);
          command.Parameters.AddWithValue("@PlayData", playXml);
          command.Parameters.AddWithValue("@PlayGroupId", groupId);
          command.Parameters.AddWithValue("@PlayText", playText);
          command.Parameters.AddWithValue("@TeamName", teamName);

          command.ExecuteNonQuery();
        }
        finally
        {
          if (conn.State != System.Data.ConnectionState.Closed)
          {
            conn.Close();
          }
        }
      }
    }
    #endregion

    #region "Removing items"
    public static bool RemovePlay(int playId, String teamName)
    {
      bool isDeleted = false;

      lock (mConnLock)
      {
        SqlConnection conn = mConn;

        try
        {
          conn.Open();

          SqlCommand command = conn.CreateCommand();
          command.CommandText = "DeletePlay";
          command.CommandType = System.Data.CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@Id", playId);
          command.Parameters.AddWithValue("@TeamName", teamName);

          int rowsAdded = command.ExecuteNonQuery();
          isDeleted = rowsAdded == 1;
        }
        finally
        {
          if (conn.State != System.Data.ConnectionState.Closed)
          {
            conn.Close();
          }
        }
      }

      return isDeleted;
    }

    public static bool RemoveGroup(int groupId, String teamName)
    {
      bool isDeleted = false;

      lock (mConnLock)
      {
        SqlConnection conn = mConn;

        try
        {
          conn.Open();

          SqlCommand command = conn.CreateCommand();
          command.CommandText = "DeletePlayGroup";
          command.CommandType = System.Data.CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@Id", groupId);
          command.Parameters.AddWithValue("@TeamName", teamName);

          int rowsDeleted = command.ExecuteNonQuery();
          isDeleted = rowsDeleted == 1;
        }
        finally
        {
          if (conn.State != System.Data.ConnectionState.Closed)
          {
            conn.Close();
          }
        }
      }

      return isDeleted;
    }

    public static bool RemoveTeam(int teamId)
    {
      return false;
    }
    #endregion
  }
}