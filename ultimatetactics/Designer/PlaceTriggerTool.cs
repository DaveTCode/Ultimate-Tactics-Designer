using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using UltimateTacticsDesigner.DataModel;
using UltimateTacticsDesigner.Renderer;
using System.Windows.Forms;
using UltimateTacticsDesigner.Properties;

namespace UltimateTacticsDesigner.Designer
{
  class PlaceTriggerTool : Tool
  {
    private Player mSelectedPlayer;

    public PlaceTriggerTool(PictureBox toolPicture, Player player)
      : base(toolPicture)
    {
      mSelectedPlayer = player;
    }

    public override void handleMouseDown(Point mouseLocation, 
                                         PlayFrame frame, 
                                         PitchScreenCoordConverter converter,
                                         VisualOverlay overlay)
    {
      //Noop for now
    }

    public override void handleMouseUp(Point mouseLocation, 
                                       PlayFrame frame,
                                       PitchScreenCoordConverter converter,
                                       VisualOverlay overlay)
    {
      PointF pitchCoords = converter.screenToPitchCoords(mouseLocation);

      if (mSelectedPlayer != null)
      {
        // This function doesn't necessarily create a trigger but regardless
        // we say that the tool is complete anyway.
        if (frame.MaybeCreateTrigger(pitchCoords,
                                     Settings.Default.PitchLength,
                                     mSelectedPlayer))
        {
          ModelChanged = true;
        }

        IsComplete = true;
      }
    }

    public override void Cancel(VisualOverlay overlay, PlayFrame frame)
    {
      
    }
  }
}
