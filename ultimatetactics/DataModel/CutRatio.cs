using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace UltimateTacticsDesigner.DataModel
{
  [Serializable()]
  class CutRatio
  {
    private static long NextUniqueId = 0;

    public long UniqueId;
    public LinearMovement PreviousCut { get; set; }
    public LinearMovement CausingCut { get; set; }
    public Player Player { get; set; }
    public float RatioAlongCut;

    public CutRatio()
    {
      UniqueId = NextUniqueId++;
    }

    /// <summary>
    /// This returns the absolute pitch coordinates of the ratio along the given
    /// cut. Used to work out where to place things that use this for
    /// positioning (like triggers).
    /// </summary>
    /// <param name="startOfCut">Pitch coordinates for the start of this part
    /// of the cut</param>
    /// <returns>Pitch coordinates for the location.</returns>
    public PointF GetAbsolutePosition()
    {
      PointF absolutePosition = new PointF();
      float xDiff = CausingCut.FinalPosition.X - PreviousCut.FinalPosition.X;
      float yDiff = CausingCut.FinalPosition.Y - PreviousCut.FinalPosition.Y;

      absolutePosition.X = PreviousCut.FinalPosition.X + (xDiff * RatioAlongCut);
      absolutePosition.Y = PreviousCut.FinalPosition.Y + (yDiff * RatioAlongCut);

      return absolutePosition;
    }
  }
}
