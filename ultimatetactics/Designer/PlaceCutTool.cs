using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Playbook.DataModel;
using Playbook.Renderer;
using System.Windows.Forms;
using Playbook.Properties;

namespace Playbook.Designer
{
  class PlaceCutTool : Tool
  {
    Player mSelectedPlayer = null;

    public PlaceCutTool(PictureBox toolPicture)
      : base(toolPicture)
    {
    }

    public PlaceCutTool(PictureBox toolPicture, Player player)
      : this(toolPicture)
    {
      mSelectedPlayer = player;
    }

    public override void handleMouseDown(Point mouseLocation, 
                                         PlayFrame frame,
                                         PitchScreenCoordConverter converter,
                                         VisualOverlay overlay)
    {
      if (mSelectedPlayer == null)
      {
        PointF pitchCoords = converter.screenToPitchCoords(mouseLocation);

        // No player is yet selected so attempt to find the one closest
        // to the selected spot.
        mSelectedPlayer = frame.GetClosestPlayer(pitchCoords,
                                                 Settings.Default.PlayerDiameter * 2.0f);

        if (mSelectedPlayer == null)
        {
          LinearMovement prevCut = frame.GetClosestCutEnd(pitchCoords,
                                                          Settings.Default.PlayerDiameter * 2.0f);

          if (prevCut != null && 
              prevCut != frame.DiscFrameMovement.ReceivingCut)
          {
            mSelectedPlayer = prevCut.Player;
            overlay.DrawingNewCut = true;
            overlay.CutStart = prevCut;
          }
        }
        else
        {
          overlay.DrawingNewCut = true;
          overlay.CutStart = frame.PlayerMovement[mSelectedPlayer][0];
        }
      }
    }

    public override void handleMouseUp(Point mouseLocation, 
                                       PlayFrame frame,
                                       PitchScreenCoordConverter converter,
                                       VisualOverlay overlay)
    {
      if (mSelectedPlayer != null)
      {
        frame.PlayerMovement[mSelectedPlayer].Add(
              new LinearMovement(converter.screenToPitchCoords(mouseLocation),
                                 100,
                                 mSelectedPlayer));
        IsComplete = true;
        ModelChanged = true;
      }
    }

    public override void Cancel(VisualOverlay overlay, PlayFrame frame)
    {
    }
  }
}
