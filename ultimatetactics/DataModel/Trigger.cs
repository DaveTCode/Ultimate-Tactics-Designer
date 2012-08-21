using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Playbook.DataModel
{
  /// <summary>
  /// Triggers are placed on cuts to indicate that another player should start
  /// moving once the first player gets to that point.
  /// 
  /// We store a lot of information about the trigger to make it more obvious
  /// what exactly it refers to.
  /// </summary>
  [Serializable()]
  class Trigger
  {
    private static long NextUniqueId = 0;

    public long UniqueId;
    public Player AffectedPlayer { get; set; }
    public CutRatio CausingCutRatio { get; set; }

    public Trigger(CutRatio cutRatio, Player affectedPlayer)
    {
      CausingCutRatio = cutRatio;
      AffectedPlayer = affectedPlayer;
      UniqueId = NextUniqueId++;
    }
  }
}
