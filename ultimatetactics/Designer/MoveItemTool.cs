using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using UltimateTacticsDesigner.DataModel;
using UltimateTacticsDesigner.Renderer;
using System.Windows.Forms;

namespace UltimateTacticsDesigner.Designer
{
  class MoveItemTool : Tool
  {
    private const float SELECTION_MAX_DISTANCE = 1.0F;

    private Player mSelectedPlayer = null;
    private LinearMovement mSelectedCut = null;
    private Trigger mSelectedTrigger = null;
    private Boolean mIsMovingDiscControlPoint = false;

    public MoveItemTool(PictureBox toolPicture)
      : base(toolPicture)
    {

    }

    /// <summary>
    /// In order to move an item we catch the mouse down event to decide which
    /// thing has been selected. This function chooses either the selected
    /// cut, trigger or player.
    /// </summary>
    /// <param name="mouseLocation">Screen coordinates.</param>
    /// <param name="frame">Frame currently being designed.</param>
    /// <param name="converter">Used to convert from screen to pitch 
    /// coordinates.</param>
    public override void handleMouseDown(Point mouseLocation, 
                                         PlayFrame frame,
                                         PitchScreenCoordConverter converter,
                                         VisualOverlay overlay)
    {
      PointF pitchCoords = converter.screenToPitchCoords(mouseLocation);

      // Choose closest selected item.
      mSelectedPlayer = frame.GetClosestPlayer(pitchCoords, 
                                               SELECTION_MAX_DISTANCE);
      mSelectedCut = frame.GetClosestCutEnd(pitchCoords,
                                              SELECTION_MAX_DISTANCE);
      mSelectedTrigger = frame.GetClosestTrigger(pitchCoords,
                                                 SELECTION_MAX_DISTANCE);
      mIsMovingDiscControlPoint = frame.IsNearControlPoint(pitchCoords,
                                                           SELECTION_MAX_DISTANCE);

      // Note that the ordering of priorities here MUST be the same as the
      // ordering on mouse up.
      if (mSelectedTrigger != null)
      {
        overlay.SelectedTrigger = mSelectedTrigger;
        overlay.TriggerPlayer = mSelectedTrigger.AffectedPlayer;
      }
      else if (mSelectedCut != null)
      {
        overlay.DrawingNewCut = false;
        overlay.CutStart = mSelectedCut;
      }
      else if (mSelectedPlayer != null)
      {
        overlay.SelectedPlayer = mSelectedPlayer;
      }
      else if (mIsMovingDiscControlPoint)
      {
        overlay.MovingDiscControlPoint = true;
      }
    }

    /// <summary>
    /// We capture the mouse up event and use it to place whatever it was that
    /// was selected on the mouse down event. If nothing was selected this does
    /// nothing. 
    /// </summary>
    /// <param name="mouseLocation"></param>
    /// <param name="frame"></param>
    /// <param name="converter"></param>
    public override void handleMouseUp(Point mouseLocation,
                                       PlayFrame frame,
                                       PitchScreenCoordConverter converter,
                                       VisualOverlay overlay)
    {
      PointF pitchCoords = converter.screenToPitchCoords(mouseLocation);

      if (mSelectedTrigger != null)
      {
        // If we are moving a trigger then only move it if the position it is 
        // being placed is valid.
        if (frame.MaybeCreateTrigger(pitchCoords,
                                     SELECTION_MAX_DISTANCE,
                                     mSelectedTrigger.AffectedPlayer))
        {
          frame.RemoveTrigger(mSelectedTrigger);
          ModelChanged = true;
        }
      }
      else if (mSelectedPlayer != null)
      {
        frame.PlayerMovement[mSelectedPlayer][0].FinalPosition = pitchCoords;
        ModelChanged = true;
      }
      else if (mSelectedCut != null)
      {
        mSelectedCut.FinalPosition = pitchCoords;
        ModelChanged = true;
      }
      else if (mIsMovingDiscControlPoint)
      {
        frame.DiscFrameMovement.ControlPoint = pitchCoords;
        ModelChanged = true;
      }

      // Regardless of whether anything moved the move tool is complete on 
      // mouse up.
      mSelectedCut = null;
      mSelectedPlayer = null;
      mSelectedTrigger = null;
      IsComplete = true;
    }

    public override void Cancel(VisualOverlay overlay, PlayFrame frame)
    {
    }
  }
}
