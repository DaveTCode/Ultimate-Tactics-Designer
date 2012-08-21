using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Playbook.Properties;

namespace Playbook.Renderer
{
  class PitchScreenCoordConverter
  {
    private float mPitchRatio;
    private int mLeftOffset = 0;

    public PitchScreenCoordConverter(Graphics display)
    {
      mPitchRatio = calculatePitchRatio(display);

      if (Settings.Default.PitchWidth < display.VisibleClipBounds.Width)
      {
        mLeftOffset = (int) (display.VisibleClipBounds.Width - 
                             Settings.Default.PitchLength * mPitchRatio) / 4;
      }
    }

    /// <summary>
    /// This function is called every time that the underlying display either
    /// changes completely or is resized. 
    /// 
    /// The idea is that we keep track of the current pitch ratio by updating
    /// it each time that the display is changed.
    /// </summary>
    /// <param name="display"></param>
    public void DisplayChanged(Graphics display)
    {
      mPitchRatio = calculatePitchRatio(display);
    }

    /// <summary>
    /// The ratio we are calculating here is one which will allow us to fit the
    /// maximum amount of pitch inside the viewing area.
    /// </summary>
    /// <param name="display"></param>
    /// <returns></returns>
    private float calculatePitchRatio(Graphics display)
    {
      float viewHeight = display.VisibleClipBounds.Width;
      float viewWidth = display.VisibleClipBounds.Height;
      float pitchWidthRatio = viewWidth / Settings.Default.PitchWidth;
      float pitchLengthRatio = viewHeight / Settings.Default.PitchLength;
      float pitchRatio = Math.Min(pitchLengthRatio, pitchWidthRatio);

      return pitchRatio;
    }

    /// <summary>
    /// Used to convert between pich and screen coordinates.
    /// </summary>
    /// <param name="pitchCoords">The on pitch coordinates as floats</param>
    /// <returns>The point as a pair of integers in screen coordinates
    /// </returns>
    public Point pitchToScreenCoords(PointF pitchCoords)
    {
      Point screenCoords = new Point();

      screenCoords.X = (int)(pitchCoords.Y * mPitchRatio) + mLeftOffset;
      screenCoords.Y = (int)(pitchCoords.X * mPitchRatio);

      return screenCoords;
    }

    /// <summary>
    /// Used to convert between screen and pitch coordinates. Mostly
    /// used for interpreting user clicks.
    /// </summary>
    /// <param name="screenCoords">Screen coordinates as a pair of ints.</param>
    /// <returns>Pitch coordinates as a pair of floats.</returns>
    public PointF screenToPitchCoords(Point screenCoords)
    {
      PointF pitchCoords = new PointF();

      pitchCoords.X = (screenCoords.Y) /
                      mPitchRatio;
      pitchCoords.Y = (screenCoords.X - mLeftOffset) /
                      mPitchRatio;

      return pitchCoords;
    }

    /// <summary>
    /// Convert a single number from pitch to screen coordinates.
    /// Used to move lengths between the two views.
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public int pitchToScreenLength(float length)
    {
      return (int) (length * mPitchRatio);
    }

    /// <summary>
    /// If the converter is using offsets then this function must be used to 
    /// convert sizes so that it doesn't apply the offset to the size 
    /// calculation.
    /// </summary>
    /// <param name="screenCoords"></param>
    /// <returns>The size as a point in screen coordinates.</returns>
    internal Point pitchToScreenSize(PointF screenCoords)
    {
      Point pitchCoords = new Point();

      pitchCoords.X = (int) (screenCoords.Y * mPitchRatio);
      pitchCoords.Y = (int) (screenCoords.X * mPitchRatio);

      return pitchCoords;
    }
  }
}
