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
  class DiscFlightTool : Tool
  {
    public DiscFlightTool(PictureBox pictureBox)
      : base(pictureBox)
    {

    }

    public override void handleMouseDown(Point mouseLocation,
                                         PlayFrame frame,
                                         PitchScreenCoordConverter converter,
                                         VisualOverlay overlay)
    {
      //Noop
    }

    public override void handleMouseUp(Point mouseLocation,
                                       PlayFrame frame,
                                       PitchScreenCoordConverter converter,
                                       VisualOverlay overlay)
    {
      PointF pitchCoords = converter.screenToPitchCoords(mouseLocation);

      LinearMovement cut = frame.GetClosestDiscFlightPoint(pitchCoords,
                                                           Settings.Default.PlayerDiameter);

      if (cut == null)
      {
        // The user has clicked away from any cut. They may want the disc to 
        // go to ground so check before setting it.
        DialogResult result = MessageBox.Show(
          "You have not selected a cut to throw to, would you like the " +
          " disc to go here anyway",
          "Throw disc to ground?",
          MessageBoxButtons.YesNo);

        if (result == DialogResult.Yes)
        {
          frame.DiscFrameMovement.AbsoluteFlightPath = pitchCoords;
          frame.DiscFrameMovement.HasMoved = true;

          IsComplete = true;
          ModelChanged = true;
        }
      }
      else
      {
        frame.DiscFrameMovement.ReceivingCut = cut;
        frame.DiscFrameMovement.HasMoved = true;
        IsComplete = true;
        ModelChanged = true;
      }
    }

    public override void Cancel(VisualOverlay overlay, PlayFrame frame)
    {
    }
  }
}
