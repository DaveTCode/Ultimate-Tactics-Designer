using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Playbook.DataModel
{
  /// <summary>
  /// A linear movement used to describe the movement of a player or the disc
  /// during a frame. The start position is not stored locally as this is 
  /// independent of where the player starts from.
  /// </summary>
  [Serializable()]
  class LinearMovement
  {
    private static long NextUniqueId = 0;

    public long UniqueId = NextUniqueId++;
    public Player Player { get; set; }
    public PointF FinalPosition { get; set; }
    public int SpeedPercentage { get; set; }

    public LinearMovement(float x, float y, int speedPercentage, Player player)
    {
      FinalPosition = new PointF(x, y);
      SpeedPercentage = speedPercentage;
      Player = player;
    }

    public LinearMovement(PointF finalPosition, int speedPercentage, Player player)
    {
      FinalPosition = finalPosition;
      SpeedPercentage = speedPercentage;
      Player = player;
    }

    /// <summary>
    /// Generate the list of points on which this linear movement travels.
    /// 
    /// This is used to generate the cycle data to play a frame. It acts in
    /// isolation and the combined data is padded by the calling code.
    /// </summary>
    /// <param name="startPoint">The starting point for the cut
    /// is stored in another cut.</param>
    /// <param name="cycleTimeMs">Measured in milliseconds.</param>
    /// <param name="maxSpeedMpS">Measured in m/s</param>
    /// <returns></returns>
    public List<PointF> GeneratePoints(PointF startPoint, 
                                       double cycleTimeMs, 
                                       float maxSpeedMpS)
    {
      // If the speed percentage is 0 then the cut does not go anywhere.
      // In that case just return the final position as a single element
      // list.
      if (SpeedPercentage == 0.0f)
      {
        List<PointF> singlePointList = new List<PointF>();
        singlePointList.Add(FinalPosition);
        return singlePointList;
      }
      else
      {
        return GeometryUtils.ListPointsAlongLine(startPoint,
                                                 FinalPosition,
                                                 (float)cycleTimeMs,
                                                 maxSpeedMpS);
      }
    }
  }
}
