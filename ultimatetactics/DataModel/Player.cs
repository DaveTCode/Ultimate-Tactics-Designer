using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateTacticsDesigner;
using System.ComponentModel;

namespace UltimateTacticsDesigner.DataModel
{
  /// <summary>
  /// Used to contain any details unique to each player on the field.
  /// </summary>
  [Serializable()]
  class Player
  {
    private static long NextUniqueId = 0;

    public long UniqueId;
    public float MaxSpeed { get; set; }
    public int VisibleID { get; set; }
    public String Name { get; set; }
    public Team PlayerTeam { get; set; }

    public Player(Team team, String name, float maxSpeed, int visibleId)
    {
      PlayerTeam = team;
      Name = name;
      MaxSpeed = maxSpeed;
      VisibleID = visibleId;
      UniqueId = NextUniqueId++;
    }
  }
}
