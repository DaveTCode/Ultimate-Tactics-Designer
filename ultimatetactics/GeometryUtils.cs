using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace UltimateTacticsDesigner
{
  /// <summary>
  /// Provides useful geometric functions as public static functions which
  /// can be used anywhere. These may all exist in some geometry library which 
  /// I don't know about...
  /// </summary>
  class GeometryUtils
  {
    private GeometryUtils()
    {

    }

    /// <summary>
    /// Equations from http://local.wasp.uwa.edu.au/~pbourke/geometry/pointline/
    /// where u is written here as the coefficient.
    /// </summary>
    /// <param name="lineStart"></param>
    /// <param name="lineEnd"></param>
    /// <param name="point"></param>
    public static PointF ClosestPointLineSegment(PointF lineStart,
                                                 PointF lineEnd,
                                                 PointF point)
    {
      PointF closestPoint = new PointF();
      float segmentLengthSqrd = (float)(Math.Pow(lineEnd.X - lineStart.X, 2) + 
                                        Math.Pow(lineEnd.Y - lineStart.Y, 2));
      float coefficient = ((point.X - lineStart.X) * (lineEnd.X - lineStart.X)) +
                          ((point.Y - lineStart.Y) * (lineEnd.Y - lineStart.Y));
      coefficient /= segmentLengthSqrd;

      // Clamp the coefficient to be between 0 and 1 as we are only comparing 
      // with line segments.
      coefficient = Math.Max(0.0F, coefficient);
      coefficient = Math.Min(1.0F, coefficient);

      closestPoint.X = lineStart.X + coefficient * (lineEnd.X - lineStart.X);
      closestPoint.Y = lineStart.Y + coefficient * (lineEnd.Y - lineStart.Y);

      return closestPoint;
    }

    /// <summary>
    /// Simple norm of 2 points in R2.
    /// </summary>
    /// <param name="pointA"></param>
    /// <param name="pointB"></param>
    /// <returns></returns>
    public static float DistBetweenPoints(PointF pointA, PointF pointB)
    {
      return (float)Math.Sqrt(Math.Pow(pointA.X - pointB.X, 2) + 
                              Math.Pow(pointA.Y - pointB.Y, 2));
    }

    /// <summary>
    /// Given a start point, end point, the time taken between intervals 
    /// and the speed of movement it is possible to calculate the list of 
    /// points which are passed along this linear line.
    /// 
    /// We do that using a 
    /// 
    /// It is basically a Bresenham style line drawing algorithm.
    /// </summary>
    /// <param name="start">The start of the line.</param>
    /// <param name="end">The last point on the line.</param>
    /// <param name="timeBetweenPointsMs">ms between points</param>
    /// <param name="speedMovementMps">m/s speed.</param>
    /// <returns>A list of the points travelled.</returns>
    public static List<PointF> ListPointsAlongLine(PointF start, 
                                                   PointF end, 
                                                   float timeBetweenPointsMs,
                                                   float speedMovementMps)
    {
      float distTravelledPerCycle = (float)(timeBetweenPointsMs / 1000.0F * speedMovementMps);
      float currX = start.X;
      float currY = start.Y;
      float dx;
      float dy;
      int numCycles;

      // Precalculation done to make sure that all we have to do 
      // each cycle is a pair of additions. 
      float xDiff = end.X - currX;
      float yDiff = end.Y - currY;
      double totalDistTravelled = Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2));
      numCycles = (int)(totalDistTravelled / distTravelledPerCycle);
      dx = xDiff / numCycles;
      dy = yDiff / numCycles;

      IEnumerable<PointF> points = from num in Enumerable.Range(0, numCycles) 
                                   select new PointF(start.X + dx * num, start.Y + dy * num);
      return points.ToList<PointF>();
    }

    /// <summary>
    /// Returns the point equidistant between two given points.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static PointF MiddleOfLine(PointF start, PointF end)
    {
      return new PointF((start.X + end.X) / 2, (start.Y + end.Y) / 2);
    }

    /// <summary>
    /// Returns a point which is between the start and end points and is a 
    /// ratio between them.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="ratio">Between 0 and 1 to place the point in between the
    /// start and end points.</param>
    /// <returns></returns>
    public static PointF PointOnLine(PointF start, PointF end, float ratio)
    {
      return new PointF(start.X + (end.X - start.X) * ratio,
                        start.Y + (end.Y - start.Y) * ratio);
    }
  }
}
