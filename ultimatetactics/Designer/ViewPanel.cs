using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using UltimateTacticsDesigner.DataModel;
using UltimateTacticsDesigner.Renderer;
using UltimateTacticsDesigner.Properties;

namespace UltimateTacticsDesigner.Designer
{
  /// <summary>
  /// The ViewPanel is where the design is displayed and also where the design 
  /// is played when we are in play mode.
  /// 
  /// It has a boolean indicating which mode it is currently in (IsDesignMode)
  /// and contains all the information required to display the current frame 
  /// in either mode.
  /// </summary>
  public partial class ViewPanel : Panel
  {
    internal PlayFrame CurrentFrame { get; set; }
    internal PlayThread PlayingThread { get; set; }
    internal FrameRenderer Renderer { get; set; }
    internal Tool CurrentTool { get; set; }
    internal Boolean IsDesignMode { get; set; }
    internal MainDesignerForm DesignerForm { get; set; }
    internal VisualOverlay VisualOverlay { get; set; }
    internal PitchScreenCoordConverter Converter { get; set; }
    internal KeyPressHandler KeyPressHandler { get; set; }

    public ViewPanel()
    {
      this.SetStyle(ControlStyles.UserPaint | 
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.AllPaintingInWmPaint | 
                    ControlStyles.SupportsTransparentBackColor,
                    true);

      InitializeComponent();
    }

    public ViewPanel(IContainer container)
    {
      container.Add(this);

      InitializeComponent();
    }

    /// <summary>
    /// Called whenever we need to clear the current tool being used on the
    /// frame.
    /// 
    /// Done in a single function so we can guarantee that the visual overlay
    /// is cleared and the tool is cancelled properly.
    /// </summary>
    internal void CancelTool()
    {
      if (CurrentTool != null)
      {
        CurrentTool.Cancel(VisualOverlay, CurrentFrame);
        CurrentTool = null;
        VisualOverlay.Clear();
      }
    }

    protected override void OnResize(EventArgs e)
    {
      using (Graphics display = this.CreateGraphics())
      {
        Converter = new PitchScreenCoordConverter(display);
      }

      base.OnResize(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      // There are two drawing modes for the view panel. We are either drawing
      // in design mode or in view mode. View mode refreshes are caused by the 
      // play thread.
      if (IsDesignMode)
      {
        if (Renderer != null)
        {
          Renderer.DrawFrame(CurrentFrame,
                             e.Graphics,
                             Converter,
                             VisualOverlay,
                             this.DisplayRectangle);
        }
      }
      else
      {
        if (PlayingThread != null)
        {
          PlayingThread.RenderCurrentCycle(e.Graphics);
        }
      }
    }

    /// <summary>
    /// We need to ignore the paint background method as the background is 
    /// painted in the main paint method. Removing this will cause the 
    /// parent class method to be called and that in turn causes flickering.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPaintBackground(PaintEventArgs e)
    {
    }

    /// <summary>
    /// Mouse movement handler. Selects the appropriate cursor and allows tools
    /// which are based off the mouse movement to act on it.
    /// 
    /// Also updates the overlay with the new position.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseMove(MouseEventArgs e)
    {
      if (IsDesignMode)
      {
        Point mouseLocation = ClosestPointInPitch(e.Location);

        if (CurrentTool == null)
        {
          ChooseCursor(mouseLocation);
        }
        else
        {
          VisualOverlay.MouseLocation = mouseLocation;
          CurrentTool.handleMouseMove(mouseLocation,
                                      CurrentFrame,
                                      Converter,
                                      VisualOverlay);
        }

        this.Refresh();
      }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      if (IsDesignMode)
      {
        // Choose a default tool if there isn't one selected.
        if (CurrentTool == null)
        {
          if (e.Button == System.Windows.Forms.MouseButtons.Left)
          {
            PointF pitchCoords = Converter.screenToPitchCoords(ClosestPointInPitch(e.Location));

            // 
            // Action taken is dependent on which modifier key is held down.
            //
            // Ctrl + Click -> Draw cut
            // Shift + Click -> Draw flight path
            // Click -> Move item
            if (KeyPressHandler.IsKeyPressed(Keys.ControlKey))
            {
              if (CurrentFrame.CanDrawCut(pitchCoords, Settings.Default.PlayerDiameter, 0.0f))
              {
                CurrentTool = new PlaceCutTool(null);
              }
            }
            else if (KeyPressHandler.IsKeyPressed(Keys.ShiftKey))
            {
              Player closestPlayer = CurrentFrame.GetClosestPlayer(pitchCoords, 
                                                                   Settings.Default.PlayerDiameter);

              if (CurrentFrame.DiscFrameMovement.Thrower == closestPlayer && closestPlayer != null)
              {
                CurrentTool = new DiscFlightTool(null);
                VisualOverlay.DrawingDiscMovement = true;
              }
            }
            else
            {
              if (CurrentFrame.CanMoveItem(pitchCoords, Settings.Default.PlayerDiameter, 0.0f))
              {
                CurrentTool = new MoveItemTool(null);
              }
            }
          }
        }
        
        if (CurrentFrame != null && CurrentTool != null)
        {
          if (e.Button == System.Windows.Forms.MouseButtons.Left)
          {
            UpdateTool(e.Location);
          }
          else if (e.Button == System.Windows.Forms.MouseButtons.Right)
          {
            CancelTool();
          }

          this.Refresh();
        }
      }
      else
      {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
          // Handle mouse down in play mode.
          if (PlayingThread.ThreadState == PlayThreadState.Paused)
          {
            PlayingThread.Continue();
          }
          else
          {
            PlayingThread.Pause();
          }
        }
      }
    }

    internal void UpdateTool(Point mouseLocation)
    {
      VisualOverlay.MouseLocation = ClosestPointInPitch(mouseLocation);
      CurrentTool.handleMouseDown(VisualOverlay.MouseLocation,
                                  CurrentFrame,
                                  Converter,
                                  VisualOverlay);

      // A tool determines whether it is complete itself. 
      if (CurrentTool.IsComplete)
      {
        if (CurrentTool.ModelChanged)
        {
          DesignerForm.ModelChanged();
        }

        VisualOverlay.Clear();
        CurrentTool = null;
      }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
      if (IsDesignMode)
      {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
          if (CurrentTool != null && CurrentFrame != null)
          {
            CurrentTool.handleMouseUp(ClosestPointInPitch(e.Location),
                                      CurrentFrame,
                                      Converter,
                                      VisualOverlay);

            // A tool determines whether it is complete itself. 
            if (CurrentTool.IsComplete)
            {
              if (CurrentTool.ModelChanged)
              {
                DesignerForm.ModelChanged();
              }

              VisualOverlay.Clear();
              CurrentTool = null;
            }

            this.Refresh();
          }
        }
      }
    }

    /// <summary>
    /// Common code to choose what the cursor should be based on where it is
    /// located on the view panel.
    /// 
    /// Note that this is called very frequently (OnMouseMove) so it should 
    /// be optimised where possible.
    /// </summary>
    /// <param name="coords"></param>
    internal void ChooseCursor(Point coords)
    {
      PointF pitchCoords = Converter.screenToPitchCoords(coords);

      if ((KeyPressHandler.IsKeyPressed(Keys.ControlKey) ||
           KeyPressHandler.IsKeyPressed(Keys.ShiftKey)) &&
          CurrentFrame.CanDrawCut(pitchCoords, Settings.Default.PlayerDiameter, 0.0f))
      {
        Cursor.Current = Cursors.Cross;
      }
      else if (CurrentFrame.CanMoveItem(pitchCoords,
                                   Settings.Default.PlayerDiameter / 2.0f,
                                   0.0f))
      {
        Cursor.Current = Cursors.Hand;
      }
      else
      {
        Cursor.Current = Cursors.Default;
      }
    }

    /// <summary>
    /// Returns the closest point within the pitch to a given location. This
    /// is used to prevent things getting placed outside of the pitch.
    /// </summary>
    /// <param name="coords"></param>
    /// <returns></returns>
    private Point ClosestPointInPitch(Point coords)
    {
      // We want the mouse up event to be inside of the pitch so the
      // new point here forces that.
      Point adjMouseCoords = new Point(coords.X, coords.Y);
      Point pitchStart = Converter.pitchToScreenCoords(new PointF(0.0f, 0.0f));
      Point pitchEnd = Converter.pitchToScreenCoords(new PointF(Settings.Default.PitchWidth,
                                                                Settings.Default.PitchLength));
      pitchEnd.X += pitchStart.X;
      pitchEnd.Y += pitchStart.Y;

      adjMouseCoords.X = Math.Max(pitchStart.X, adjMouseCoords.X);
      adjMouseCoords.X = Math.Min(pitchEnd.X, adjMouseCoords.X);
      adjMouseCoords.Y = Math.Max(pitchStart.Y, adjMouseCoords.Y);
      adjMouseCoords.Y = Math.Min(pitchEnd.Y, adjMouseCoords.Y);

      return adjMouseCoords;
    }
  }

  /// <summary>
  /// Context menu for the view panel. Contains context specific information
  /// depending on what was clicked on.
  /// </summary>
  class ViewPanelContextMenu : ContextMenu
  {
    /// <summary>
    /// The context menu needs access to the view panel to determine what 
    /// elements to show in the menu.
    /// </summary>
    private ViewPanel mViewPanel;

    private Player mClickedPlayer;
    private Trigger mClickedTrigger;
    private CutRatio mClickedCutRatio;
    private Point mMouseLocation;

    /// <summary>
    /// Passing in the view panel gives us access to the current frame to find
    /// out what was at a given point on the screen.
    /// </summary>
    /// <param name="viewPanel"></param>
    public ViewPanelContextMenu(ViewPanel viewPanel)
    {
      mViewPanel = viewPanel;
    }

    /// <summary>
    /// Overriding the onpopup handler (gets called before the menu is 
    /// displayed) to set up the menu based on what was clicked on.
    /// 
    /// This is to get around the fact that each player and cut is not
    /// actually a control.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPopup(EventArgs e)
    {
      this.MenuItems.Clear();

      // The mouse location comes in here as the location on the entire application
      // We need to use PointToClient to convert it to mouse location on the view
      // panel.
      Point mouseLocation = Cursor.Position;
      mMouseLocation = mViewPanel.PointToClient(mouseLocation);
      PointF pitchCoordinates = mViewPanel.Converter.screenToPitchCoords(mMouseLocation);
      PlayFrame currentFrame = mViewPanel.CurrentFrame;

      // Find out what was near the right click.
      mClickedPlayer = currentFrame.GetClosestPlayer(pitchCoordinates,
                                                     Settings.Default.PlayerDiameter);
      mClickedTrigger = currentFrame.GetClosestTrigger(pitchCoordinates,
                                                       Settings.Default.PlayerDiameter);
      mClickedCutRatio = mViewPanel.CurrentFrame.GetClosestCutPoint(pitchCoordinates,
                                                                    Settings.Default.PlayerDiameter,
                                                                    null);
      bool discPathClicked = currentFrame.IsOnFlightPath(pitchCoordinates,
                                                         Settings.Default.PlayerDiameter);
      if (mClickedPlayer != null)
      {
        SetUpPlayerMenu();
      }
      if (mClickedTrigger != null)
      {
        SetUpTriggerMenu();
      }
      if (mClickedCutRatio != null)
      {
        SetUpCutMenu();
      }
      if (discPathClicked)
      {
        SetUpDiscFlightMenu();
      }
      if (currentFrame.CanDrawCut(pitchCoordinates, Settings.Default.PlayerDiameter, 0.0f))
      {
        SetUpNewCutMenu();
      }

      base.OnPopup(e);
    }

    private void SetUpDiscFlightMenu()
    {
      if (this.MenuItems.Count > 0) MenuItems.Add("-");
      this.MenuItems.Add(new MenuItem("Clear Disc Flight", (sender, e) => { mViewPanel.DesignerForm.ClearDiscFlight(); }));
    }

    private void SetUpNewCutMenu()
    {
      if (this.MenuItems.Count > 0) MenuItems.Add("-");
      this.MenuItems.Add(new MenuItem("Draw new cut", (sender, e) => { mViewPanel.DesignerForm.UseDrawCutTool(mMouseLocation); }));
    }

    /// <summary>
    /// Set up the context menu when the user right clicks on a cut.
    /// </summary>
    private void SetUpCutMenu()
    {
      if (this.MenuItems.Count > 0) MenuItems.Add("-");
      this.MenuItems.Add(new MenuItem("Clear cuts", (sender, e) => { mViewPanel.DesignerForm.ClearCuts(mClickedCutRatio.CausingCut.Player); }));
      this.MenuItems.Add(new MenuItem("Clear from here", (sender, e) => { 
        mViewPanel.DesignerForm.ClearCuts(mClickedCutRatio.CausingCut.Player, mClickedCutRatio.CausingCut); 
      }));
    }

    /// <summary>
    /// Sets up the context menu when the user right clicks on a trigger.
    /// </summary>
    private void SetUpTriggerMenu()
    {
      if (this.MenuItems.Count > 0) MenuItems.Add("-");
      this.MenuItems.Add(new MenuItem("Clear Trigger", (sender, e) => { mViewPanel.DesignerForm.DeleteTrigger(mClickedTrigger); }));
    }

    /// <summary>
    /// Sets up the context menu for a player.
    /// 
    /// The contents of the menu depend on what the player has (cuts, disc, triggers).
    /// </summary>
    private void SetUpPlayerMenu()
    {
      if (this.MenuItems.Count > 0) MenuItems.Add("-");
      this.MenuItems.Add(new MenuItem("Set player speed", (sender, e) => { mViewPanel.DesignerForm.SetPlayerSpeed(mClickedPlayer); }));
      this.MenuItems.Add(new MenuItem("Remove Player", (sender, e) => { mViewPanel.DesignerForm.DeletePlayer(mClickedPlayer); }));

      if (mViewPanel.CurrentFrame.Triggers.Count(trigger => trigger.AffectedPlayer == mClickedPlayer) > 0)
      {
        this.MenuItems.Add(new MenuItem("Clear trigger",(sender, e) => { mViewPanel.DesignerForm.ClearTrigger(mClickedPlayer); }));
      }
      else
      {
        this.MenuItems.Add(new MenuItem("Add trigger", AddTrigger_Click));
      }

      if (mViewPanel.CurrentFrame.DiscFrameMovement.Thrower == mClickedPlayer)
      {
        if (this.MenuItems.Count > 0) MenuItems.Add("-");
        this.MenuItems.Add(new MenuItem("Remove Disc", (sender, e) => { mViewPanel.DesignerForm.DeleteDisc(); }));

        if (mViewPanel.CurrentFrame.DiscFrameMovement.HasMoved)
        {
          this.MenuItems.Add(new MenuItem("Set disc speed", (sender, e) => { mViewPanel.DesignerForm.SetDiscSpeed(); }));
        }
        else
        {
          this.MenuItems.Add(new MenuItem("Draw flight path", (sender, e) => { mViewPanel.DesignerForm.StartDrawingDiscFlight(); }));
        }
      }
    }

    #region "Menu Click Handlers"

    internal void AddTrigger_Click(object sender, System.EventArgs e)
    {
      mViewPanel.VisualOverlay.TriggerPlayer = mClickedPlayer;
      mViewPanel.VisualOverlay.PlacingTrigger = true;
      mViewPanel.CurrentTool = new PlaceTriggerTool(null, mClickedPlayer);
    }

    #endregion
  }
}
