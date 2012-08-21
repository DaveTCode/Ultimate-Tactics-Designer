using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Playbook
{
  class QuadraticBezierCurve
  {
    private PointF P0;
    private PointF P1;
    private PointF P2;

    public QuadraticBezierCurve(PointF P0, PointF P1, PointF P2)
    {
      this.P0 = P0;
      this.P1 = P1;
      this.P2 = P2;
    }

    /// <summary>
    /// Converts this quadratic bezier curve to a cubic by adding an extra
    /// control point.
    /// 
    /// This is used because the C# drawing routines require all bezier curves
    /// be cubic.
    /// </summary>
    /// <returns></returns>
    public PointF[] ToCubic()
    {
      PointF[] points = new PointF[4];

      points[0] = P0;
      points[3] = P2;
      points[1] = new PointF(P1.X + 2.0f / 3.0f * (P1.X - P1.X),
                             P1.Y + 2.0f / 3.0f * (P1.Y - P1.Y));
      points[2] = new PointF(P2.X + 2.0f / 3.0f * (P1.X - P2.X),
                             P2.Y + 2.0f / 3.0f * (P1.Y - P2.Y));

      return points;
    }

    /// <summary>
    /// Calculates the length of the curve piecewise.
    /// </summary>
    /// <returns></returns>
    public float Length()
    {
      PointF[] points = PointsUneven(1000);

      float length = 0.0f;
      PointF prevPoint = points[0];
      for (int ii = 1; ii < points.Length; ii++)
      {
        PointF currPoint = points[ii];

        length += GeometryUtils.DistBetweenPoints(prevPoint, currPoint);

        prevPoint = currPoint;
      }

      return length;
    }

    /// <summary>
    /// Calculates an array of points along the curve spaces so that each pair
    /// of adjacent points are the same distance apart.
    /// </summary>
    /// <param name="distPerDivision">The required distance per pair of points
    /// </param>
    /// <returns></returns>
    public PointF[] Points(float distPerDivision)
    {
      int numDivisions = Math.Max((int) (Length() / distPerDivision), 2);
      PointF[] oldPoints = PointsUneven(numDivisions / 2);
      PointF[] points = new PointF[numDivisions];

      points[0] = oldPoints[0];

      int lastPassedPointIndex = 0;
      float distFromLastPassedPoint = 0.0f;
      float distToLastPassedPoint = 0.0f;
      for (int ii = 1; ii < numDivisions; ii++)
      {
        PointF prevPoint = oldPoints[lastPassedPointIndex];

        for (int jj = lastPassedPointIndex + 1; jj < oldPoints.Length; jj++)
        {
          PointF nextPoint = oldPoints[jj];
          float distBetweenOldPoints = GeometryUtils.DistBetweenPoints(prevPoint, nextPoint);

          if (distBetweenOldPoints - distFromLastPassedPoint > distPerDivision)
          {
            float ratio = (distPerDivision + distFromLastPassedPoint) / 
                          distBetweenOldPoints;

            distFromLastPassedPoint += distPerDivision;
            points[ii] = GeometryUtils.PointOnLine(prevPoint, 
                                                   nextPoint, 
                                                   ratio);
            break;
          }
          else
          {
            distToLastPassedPoint = distBetweenOldPoints - distFromLastPassedPoint;
            distFromLastPassedPoint = -1 * distToLastPassedPoint;
            lastPassedPointIndex = jj;
            prevPoint = oldPoints[lastPassedPointIndex];
          }
        }
      }

      return points.ToArray();
    }

    /// <summary>
    /// Returns a certain number of points along the curve without taking
    /// into account the distance between each pair of points.
    /// </summary>
    /// <param name="numDivisions">The number of points to return - 1</param>
    /// <returns></returns>
    public PointF[] PointsUneven(int numDivisions)
    {
      List<PointF> points = new List<PointF>();

      float divisionSize = 1.0f / (float) numDivisions;
      float t = 0;

      while (t <= 1)
      {
        double x = Math.Pow((1 - t), 2) * P0.X +
                   2.0 * (1 - t) * t * P1.X +
                   Math.Pow(t, 2) * P2.X;
        double y = Math.Pow((1 - t), 2) * P0.Y +
                   2.0 * (1 - t) * t * P1.Y +
                   Math.Pow(t, 2) * P2.Y;

        points.Add(new PointF((float)x, (float)y));

        t += divisionSize;
      }

      return points.ToArray();
    }

    /// <summary>
    /// Calculates the shortest distance from the given point to this
    /// curve. 
    /// 
    /// Splits the curve into linear segments and gives the shortest
    /// distance to any of these rather than solving the problem analytically.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public float DistanceFromPoint(PointF point)
    {
      float shortestDistance = 0.0f;
      PointF[] points = Points(1.0f);

      for (int ii = 0; ii < points.Length - 1; ii++)
      {
        PointF closestPoint =
            GeometryUtils.ClosestPointLineSegment(points[ii],
                                                  points[ii + 1],
                                                  point);
        float dist = GeometryUtils.DistBetweenPoints(point, closestPoint);
        
        if (ii == 0)
        {
          shortestDistance = dist;
        }
        else
        {
          if (shortestDistance > dist)
          {
            shortestDistance = dist;
          }
        }
      }

      return shortestDistance;
    }
  }
}
