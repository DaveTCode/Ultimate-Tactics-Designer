using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using UltimateTacticsDesigner.DataModel;
using UltimateTacticsDesigner.Designer;
using UltimateTacticsDesigner.Properties;
using System.Globalization;

namespace UltimateTacticsDesigner.Renderer
{
  /// <summary>
  /// Draws the design view of a frame.
  /// </summary>
  class FrameRenderer
    : IDisposable
  {
    private static float sPitchWidth = Settings.Default.PitchWidth;
    private static float sPitchLength = Settings.Default.PitchLength;
    private static float sBrickRadius = Settings.Default.BrickRadius;
    private static float sBrickDistance = Settings.Default.BrickDistance;
    private static float sEndzoneDepth = Settings.Default.EndzoneDepth;

    private Color mBackColor;
    private Color mPitchColor;
    private Color mLineColor;
    private float mLineWidth;
    private Pen mCutPen;
    private Pen mDiscFlightPen;
    private Brush mPitchBrush;

    public FrameRenderer(Color backColor,
                         Color pitchColor,
                         Color lineColor,
                         float lineWidth = 2.0f)
    {
      mBackColor = backColor;
      mPitchColor = pitchColor;
      mLineColor = lineColor;
      mLineWidth = lineWidth;

      mPitchBrush = new SolidBrush(pitchColor);
      mCutPen = new Pen(Color.DimGray, 1.0f);
      mCutPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
      mDiscFlightPen = new Pen(Color.White, 1.0f);
      mDiscFlightPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
    }

    /// <summary>
    /// Render the frame into a small visible area with squares for players 
    /// and without drawing the cuts or disc flight.
    /// </summary>
    /// <param name="frame"></param>
    /// <param name="display"></param>
    /// <param name="converter"></param>
    /// <param name="displayRectangle"></param>
    public void DrawSmallFrame(PlayFrame frame,
                               Graphics display,
                               PitchScreenCoordConverter converter)
    {
      display.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
      display.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

      // Draw the pitch itself.
      DrawPitch(display, converter);

      foreach (Player player in frame.PlayerMovement.Keys)
      {
        DrawSmallPlayer(display, 
                        player, 
                        frame.PlayerMovement[player][0].FinalPosition,
                        converter);
      }
    }

    /// <summary>
    /// Called whenever the underlying frame changes to redraw the frame.
    /// </summary>
    public void DrawFrame(PlayFrame frame, 
                          Graphics display, 
                          PitchScreenCoordConverter converter,
                          VisualOverlay overlay,
                          Rectangle displayRectangle)
    {
      BufferedGraphicsContext currentContext;
      BufferedGraphics myBuffer;
      currentContext = BufferedGraphicsManager.Current;
      myBuffer = currentContext.Allocate(display,
                                         displayRectangle);
      myBuffer.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
      myBuffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
      myBuffer.Graphics.Clear(mBackColor);

      // Draw the pitch itself.
      DrawPitch(myBuffer.Graphics, converter);

      // This moves items around based on what the user is doing.
      // e.g. If a user is dragging a cut at the moment it adds a new cut for
      // the given player.
      PlayFrame adjustableFrame = FrameRenderer.AdjustFrameForOverlay(frame, 
                                                                      overlay, 
                                                                      converter);

      foreach (Player player in adjustableFrame.PlayerMovement.Keys)
      {
        List<LinearMovement> playerMoves = adjustableFrame.PlayerMovement[player];

        if (playerMoves.Count > 0)
        {
          
          DrawPlayer(player,
                     playerMoves[0].FinalPosition,
                     myBuffer.Graphics,
                     converter,
                     false);

          if (playerMoves.Count > 1)
          {
            List<Point> cutLocations = new List<Point>();

            foreach (LinearMovement playerMove in playerMoves)
            {
              cutLocations.Add(converter.pitchToScreenCoords(playerMove.FinalPosition));
            }

            myBuffer.Graphics.DrawLines(mCutPen, cutLocations.ToArray<Point>());
          }
        }
      }

      foreach (Trigger trigger in adjustableFrame.Triggers)
      {
        DrawTrigger(myBuffer.Graphics, trigger, converter);
      }

      // Draw the disc after the players because the sprite sits on top of the
      // player sprite who is holding it.
      if (adjustableFrame.DiscFrameMovement.Thrower != null)
      {
        DrawDisc(myBuffer.Graphics,
                 adjustableFrame.DiscFrameMovement.StartPosition(), 
                 converter);

        if (adjustableFrame.DiscFrameMovement.HasMoved)
        {
          DrawDiscFlight(myBuffer.Graphics,
                         adjustableFrame.DiscFrameMovement.StartPosition(),
                         adjustableFrame.DiscFrameMovement.ControlPoint,
                         adjustableFrame.DiscFrameMovement.EndPosition(),
                         converter);
        }
      } 
      else if (overlay.PlacingDisc)
      {
        // The mouse location is stored in the overlay but we need to pass the
        // pitch coordinates to the drawing function.
        DrawDisc(myBuffer.Graphics, 
                 converter.screenToPitchCoords(overlay.MouseLocation), 
                 converter);
      }

      myBuffer.Render();
      myBuffer.Dispose();
    }

    /// <summary>
    /// There are two components to the drawing. The first is the data model
    /// which is represented by a single PlayFrame.
    /// 
    /// The second is a visual overlay indicating what the user is current 
    /// doing.
    /// 
    /// This function takes the frame and 'applies' the overlay to it by
    /// creating a deep copy and then moving/adding items as required by
    /// the overlay. For this to work we need to be able to uniquely
    /// reference items in a playmodel by an id (as their refs change on a
    /// deep copy).
    /// </summary>
    /// <param name="frame"></param>
    /// <param name="overlay"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    private static PlayFrame AdjustFrameForOverlay(PlayFrame frame, 
                                                   VisualOverlay overlay, 
                                                   PitchScreenCoordConverter converter)
    {
      // Take a deep copy of the frame so that we can adjust it based on what
      // visual overlay is required.
      PlayFrame adjustableFrame = frame.Clone();
      
      PointF pitchCoords = converter.screenToPitchCoords(overlay.MouseLocation);

      // First move any players who need to be.
      if (overlay.SelectedPlayer != null)
      {
        Player movedPlayer = adjustableFrame.GetPlayerById(overlay.SelectedPlayer.UniqueId);
        if (movedPlayer != null)
        {
          adjustableFrame.PlayerMovement[movedPlayer][0].FinalPosition = pitchCoords;
        }
        else
        {
          adjustableFrame.AddPlayer(overlay.SelectedPlayer, pitchCoords);
        }
      }

      // Either a new cut is being drawn or a cut is being moved.
      if (overlay.CutStart != null)
      {
        if (overlay.DrawingNewCut)
        {
          Player cuttingPlayer = adjustableFrame.GetPlayerById(overlay.CutStart.Player.UniqueId);

          adjustableFrame.PlayerMovement[cuttingPlayer].Add(
                                           new LinearMovement(pitchCoords,
                                                              100,
                                                              cuttingPlayer));
        }
        else
        {
          LinearMovement movedCut = adjustableFrame.GetCutById(overlay.CutStart.UniqueId);

          adjustableFrame.ReplaceCut(movedCut, pitchCoords);
        }
      }

      // Check if the user is drawing the disc movement at the moment.
      if (overlay.DrawingDiscMovement)
      {
        adjustableFrame.DiscFrameMovement.AbsoluteFlightPath = pitchCoords;
        adjustableFrame.DiscFrameMovement.HasMoved = true;
      }

      if (overlay.MovingDiscControlPoint)
      {
        adjustableFrame.DiscFrameMovement.ControlPoint = pitchCoords;
      }

      // If there is a trigger being moved then the selected trigger must have
      // it's position updated.
      if (overlay.SelectedTrigger != null)
      {
        CutRatio closestCutPoint = adjustableFrame.GetClosestCutPoint(pitchCoords, 
          sPitchLength, 
          overlay.SelectedTrigger.AffectedPlayer);

        if (closestCutPoint != null)
        {
          Trigger adjustedTrigger = adjustableFrame.GetTriggerById(overlay.SelectedTrigger.UniqueId);

          if (adjustedTrigger != null)
          {
            adjustedTrigger.CausingCutRatio = closestCutPoint;
          }
        }
      }
      else if (overlay.PlacingTrigger)
      {
        adjustableFrame.MaybeCreateTrigger(pitchCoords,
                                           sPitchLength,
                                           overlay.TriggerPlayer);
      }

      return adjustableFrame;
    }

    /// <summary>
    /// Place the disc onto the display. This must happen after the players
    /// are drawn so that they don't end up on top of the disc.
    /// </summary>
    /// <param name="display"></param>
    /// <param name="location"></param>
    /// <param name="converter"></param>
    private void DrawDisc(Graphics display, 
                          PointF location, 
                          PitchScreenCoordConverter converter)
    {
      PointF offsetCentre = new PointF(location.X, 
                                       location.Y);
      offsetCentre.X -= ((float)Settings.Default.PlayerDiameter) / 4.0F;
      offsetCentre.Y -= ((float)Settings.Default.PlayerDiameter) / 4.0F;

      DrawSprite(display, 
                 converter, 
                 UltimateTacticsDesigner.Properties.Resources.disc,
                 offsetCentre,
                 Settings.Default.PlayerDiameter / 2.0f,
                 Settings.Default.PlayerDiameter / 2.0f);
    }

    /// <summary>
    /// Draw the disc flight.
    /// 
    /// Currently simply a straight line from A to B. Could enhance this so
    /// that it draws a cardinal spline instead at some point. The UI to draw
    /// that is the more difficult area.
    /// </summary>
    /// <param name="display"></param>
    /// <param name="startLocation"></param>
    /// <param name="controlPoint"></param>
    /// <param name="endLocation"></param>
    /// <param name="converter"></param>
    private void DrawDiscFlight(Graphics display,
                                PointF startLocation,
                                PointF controlPoint,
                                PointF endLocation,
                                PitchScreenCoordConverter converter)
    { 
      Point screenCoordsStart = converter.pitchToScreenCoords(startLocation);
      Point screenCoordsEnd = converter.pitchToScreenCoords(endLocation);
      Point screenControlPoint = converter.pitchToScreenCoords(controlPoint);

      QuadraticBezierCurve curve = new QuadraticBezierCurve(screenCoordsStart,
                                                            screenControlPoint,
                                                            screenCoordsEnd);

      display.DrawBeziers(mDiscFlightPen, curve.ToCubic());

      display.DrawRectangle(mDiscFlightPen,
                            screenControlPoint.X - 1,
                            screenControlPoint.Y - 1,
                            2, 2);
    }

    /// <summary>
    /// Draw a trigger onto the screen. The location is tied to the trigger 
    /// object.
    /// 
    /// The description of what the trigger will look like is contained wholly 
    /// within this function.
    /// </summary>
    /// <param name="display"></param>
    /// <param name="trigger"></param>
    /// <param name="converter"></param>
    private void DrawTrigger(Graphics display, 
                             Trigger trigger, 
                             PitchScreenCoordConverter converter)
    {
      PointF locationToDraw = trigger.CausingCutRatio.GetAbsolutePosition();

      DrawPlayer(trigger.AffectedPlayer, 
                 trigger.CausingCutRatio.GetAbsolutePosition(), 
                 display, 
                 converter, 
                 true);
    }

    /// <summary>
    /// The pitch is drawn as a pair of rectangles rather than as a sprite.
    /// 
    /// All conversion between pitch coordinates and screen coordinates is done
    /// internally.
    /// </summary>
    /// <param name="display"></param>
    /// <param name="converter"></param>
    public void DrawPitch(Graphics display, 
                          PitchScreenCoordConverter converter)
    {
      Rectangle outerPitch = new Rectangle();
      Rectangle innerPitch = new Rectangle();
      using (Pen linePen = new Pen(mLineColor, mLineWidth))
      {
        Point bottomRight = converter.pitchToScreenCoords(
          new PointF(sPitchWidth, sPitchLength));
        Point innerBottomRight = converter.pitchToScreenCoords(
          new PointF(sPitchWidth, (sPitchLength - 2.0f * sEndzoneDepth)));
        Point leftBrick = converter.pitchToScreenCoords(
          new PointF(sPitchWidth / 2.0f,
                     sEndzoneDepth + sBrickDistance));
        Point rightBrick = converter.pitchToScreenCoords(
          new PointF(sPitchWidth / 2.0f,
                     sPitchLength - sEndzoneDepth - sBrickDistance));
        int brickDistanceScreen = converter.pitchToScreenLength(sBrickRadius);

        outerPitch.Location = converter.pitchToScreenCoords(new PointF(0.0f, 0.0f));
        outerPitch.Width = bottomRight.X;
        outerPitch.Height = bottomRight.Y;

        innerPitch.Location = converter.pitchToScreenCoords(new PointF(0.0f, sEndzoneDepth));
        innerPitch.Width = innerBottomRight.X;
        innerPitch.Height = innerBottomRight.Y;

        display.FillRectangle(mPitchBrush, outerPitch);
        display.DrawRectangle(linePen, outerPitch);
        display.DrawRectangle(linePen, innerPitch);
        DrawCross(display, linePen, leftBrick, brickDistanceScreen);
        DrawCross(display, linePen, rightBrick, brickDistanceScreen);
      }
    }

    /// <summary>
    /// Draws a single cross with each arm having length equal to the radius.
    /// </summary>
    /// <param name="graphics"></param>
    /// <param name="pen"></param>
    /// <param name="centre"></param>
    /// <param name="radius"></param>
    private void DrawCross(Graphics graphics, 
                           Pen pen, 
                           Point centre, 
                           int radius)
    {
      graphics.DrawLine(pen, centre.X, centre.Y + radius, centre.X, centre.Y - radius);
      graphics.DrawLine(pen, centre.X + radius, centre.Y, centre.X - radius, centre.Y);
    }

    /// <summary>
    /// Render a small square for a player. This is used when we're rendering
    /// into a small space.
    /// </summary>
    /// <param name="graphics"></param>
    /// <param name="player"></param>
    /// <param name="location"></param>
    /// <param name="converter"></param>
    private void DrawSmallPlayer(Graphics graphics, 
                                 Player player, 
                                 PointF location, 
                                 PitchScreenCoordConverter converter)
    {
      Point screenCoords = converter.pitchToScreenCoords(location);
      Brush playerBrush = player.PlayerTeam.UniqueId == 
                          Team.RED_TEAM.UniqueId ? Brushes.Red : Brushes.Blue;

      graphics.FillRectangle(playerBrush,
                             screenCoords.X,
                             screenCoords.Y,
                             4, 4);
    }

    /// <summary>
    /// Draw a single player onto the given display object. The conversion 
    /// from pitch to screen coordinates is done internally.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="position"></param>
    /// <param name="display"></param>
    /// <param name="converter"></param>
    /// <param name="isSelected">A selected player shows a slightly different 
    /// sprite to indicate the selection.</param>
    /// <param name="isOutline">Set to true to draw only the outline of the 
    /// player.</param>
    internal void DrawPlayer(Player player, 
                             PointF position, 
                             Graphics display, 
                             PitchScreenCoordConverter converter,
                             Boolean isOutline)
    {
      Image playerImage = isOutline ? player.PlayerTeam.OutlineSprite : 
                                      player.PlayerTeam.Sprite;

      PointF offsetCentre = new PointF(position.X, position.Y);
      offsetCentre.X -= Settings.Default.PlayerDiameter / 2.0f;
      offsetCentre.Y -= Settings.Default.PlayerDiameter / 2.0f;
      DrawSprite(display,
                 converter,
                 playerImage,
                 offsetCentre,
                 Settings.Default.PlayerDiameter,
                 Settings.Default.PlayerDiameter);

      using (Font font = CreatePlayerIdFont(converter))
      {
        // Draw the player id as an overlay onto the sprite.
        DrawString(display,
                   converter,
                   player.VisibleID.ToString(CultureInfo.InvariantCulture),
                   offsetCentre,
                   font,
                   Brushes.White);
      }
    }

    /// <summary>
    /// Creates a font object with the correct size to be placed on a player.
    /// 
    /// Must be disposed of by the calling code.
    /// </summary>
    /// <param name="converter"></param>
    /// <returns></returns>
    static internal Font CreatePlayerIdFont(PitchScreenCoordConverter converter)
    {
      // The player width and length in screen coordinates cannot be 0
      // or the drawing will crash.
      int playerDiameterScreen = converter.pitchToScreenLength(Settings.Default.PlayerDiameter);
      if (playerDiameterScreen == 0) playerDiameterScreen = 1;

      int fontSize = playerDiameterScreen > 2 ? playerDiameterScreen - 2 : playerDiameterScreen;
      Font font = new Font("consolas",
                           fontSize,
                           FontStyle.Regular,
                           GraphicsUnit.Pixel);

      return font;
    }

    /// <summary>
    /// Draw a single sprite on the pitch at the given coordinates. Does the 
    /// conversion of pitch -> screen internally.
    /// </summary>
    /// <param name="display">The display on which to draw.</param>
    /// <param name="converter">The converter utility used to turn pitch
    /// coordinates into screen coordinates</param>
    /// <param name="image">The image to draw</param>
    /// <param name="position">The pitch coordinates</param>
    /// <param name="width">Pitch width</param>
    /// <param name="length">Pitch height/length</param>
    public void DrawSprite(Graphics display,
                           PitchScreenCoordConverter converter, 
                           Image image, 
                           PointF position, 
                           float width, 
                           float length)
    {
      Rectangle imagePlacement = new Rectangle();
      Point screenCoords = converter.pitchToScreenCoords(position);
      PointF imageSize = new PointF(width, length);
      Point convertedImageSize = converter.pitchToScreenSize(imageSize);

      imagePlacement.Location = screenCoords;
      imagePlacement.Width = convertedImageSize.X;
      imagePlacement.Height = convertedImageSize.Y;

      display.DrawImage(image,
                        imagePlacement);
    }

    /// <summary>
    /// Draw a string onto the pitch at the given pitch coordinates. Does
    /// the conversion from pitch to screen internally.
    /// </summary>
    /// <param name="display"></param>
    /// <param name="converter"></param>
    /// <param name="text"></param>
    /// <param name="position"></param>
    /// <param name="font"></param>
    /// <param name="brush"></param>
    public void DrawString(Graphics display,
                           PitchScreenCoordConverter converter,
                           String text,
                           PointF position,
                           Font font,
                           Brush brush)
    {
      Point screenCoords = converter.pitchToScreenCoords(position);

      display.DrawString(text, font, brush, screenCoords);
    }

    public void Dispose()
    {
      mCutPen.Dispose();
      mDiscFlightPen.Dispose();
      mPitchBrush.Dispose();
    }
  }
}
