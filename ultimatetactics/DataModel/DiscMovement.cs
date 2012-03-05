using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using UltimateTacticsDesigner.Properties;
using System.Drawing.Drawing2D;

namespace UltimateTacticsDesigner.DataModel
{
  /// <summary>
  /// This class describes the disc movement in a single frame. Each frame has
  /// exactly ONE such class.
  /// 
  /// When the disc is placed on a player the thrower is set to !null.
  /// 
  /// When the disc final position is set it either goes to the end of a cut OR
  /// to ground somewhere. Therefore, to access the final position of the disc
  /// the easiest way is to use the method on this class EndPosition.
  /// 
  /// To test if the disc final position has been set yet it is sufficient to 
  /// check that the HasMoved field is true.
  /// </summary>
  [Serializable()]
  class DiscMovement
  {
    private static long NextUniqueId = 0;

    internal long UniqueId;
    internal Boolean HasMoved;
    internal Player Thrower { get; set; }
    internal float DiscSpeed { get; set; }
    internal PlayFrame Frame { get; set; }
    internal LinearMovement ReceivingCut { get; set; }
    internal PointF AbsoluteFlightPath { get; set; }
    
    private Boolean mIsControlPointModified = false;
    private PointF mControlPoint;
    internal PointF ControlPoint
    {
      get
      {
        if (mIsControlPointModified)
        {
          return mControlPoint;
        }
        else
        {
          return GetDefaultControlPoint();
        }
      }
      set
      {
        mIsControlPointModified = true;
        mControlPoint = value;
      }
    }

    public DiscMovement(PlayFrame frame)
    {
      Clear();
      UniqueId = NextUniqueId++;
      Frame = frame;
    }

    /// <summary>
    /// When the disc path is first drawn the control point must be set to the 
    /// middle of the line.
    /// 
    /// Note that we set mControlPoint and not the property itself so that we
    /// don't set the "control point has been moved flag" to true.
    /// </summary>
    /// <returns></returns>
    private PointF GetDefaultControlPoint()
    {
      PointF point = GeometryUtils.MiddleOfLine(StartPosition(), EndPosition());

      return point;
    }

    /// <summary>
    /// Use this to access the final position which the disc lands.
    /// </summary>
    /// <returns></returns>
    internal PointF EndPosition()
    {
      if (ReceivingCut != null)
      {
        return ReceivingCut.FinalPosition;
      }
      else
      {
        return AbsoluteFlightPath;
      }
    }

    /// <summary>
    /// This function creates the flight path of the disc regardless
    /// of whether the disc is travelling to a person or into space.
    /// </summary>
    /// <param name="msBetweenPoints">The number of ms between each pair of 
    /// adjacent points.</param>
    /// <returns>The full list of points that the disc traverses</returns>
    internal List<PointF> FlightPath(float msBetweenPoints)
    {
      QuadraticBezierCurve curve = new QuadraticBezierCurve(StartPosition(),
                                                            ControlPoint,
                                                            EndPosition());

      float distPerDivision = msBetweenPoints / 1000.0f * DiscSpeed;

      return curve.Points(distPerDivision).ToList();
    }

    /// <summary>
    /// The distance from the control point is used to determine whether the 
    /// control point can be moved.
    /// </summary>
    /// <param name="mouseLocation"></param>
    /// <returns></returns>
    internal float DistFromControlPoint(PointF mouseLocation)
    {
      return GeometryUtils.DistBetweenPoints(ControlPoint, mouseLocation);
    }

    /// <summary>
    /// Calculates the nearest distance from the given coordinates to 
    /// the discs flight path.
    /// 
    /// This assumes that the disc travels a long a quadratic bezier curve to
    /// get an analytic solution.
    /// </summary>
    /// <param name="mouseLocation"></param>
    /// <returns></returns>
    internal float DistFromFlightPath(PointF mouseLocation)
    {
      QuadraticBezierCurve curve = new QuadraticBezierCurve(StartPosition(),
                                                            ControlPoint,
                                                            EndPosition());

      return curve.DistanceFromPoint(mouseLocation);
    }

    /// <summary>
    /// Returns the starting position of the disc. For a disc movement to have
    /// been created there must have been a thrower so we can guarantee this 
    /// will not hit null pointer exceptions.
    /// </summary>
    /// <returns>Initial location of the disc in pitch coordinates.</returns>
    internal PointF StartPosition()
    {
      return Frame.PlayerMovement[Thrower][0].FinalPosition;
    }

    /// <summary>
    /// Clears the disc of all movement and resets the speed to default.
    /// </summary>
    internal void Clear()
    {
      ClearFlightPath();
      Thrower = null;
    }

    /// <summary>
    /// Removes the flight path from the disc but doesn't reset the thrower.
    /// </summary>
    internal void ClearFlightPath()
    {
      HasMoved = false;
      mIsControlPointModified = false;
      ReceivingCut = null;
      AbsoluteFlightPath = new PointF();
      DiscSpeed = Settings.Default.DefaultDiscSpeed;
    }
  }
}
