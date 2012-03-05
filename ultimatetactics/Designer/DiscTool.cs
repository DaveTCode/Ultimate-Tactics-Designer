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
  class DiscTool : Tool
  {
    public DiscTool(PictureBox toolPicture)
      :base(toolPicture)
    {
      mToolPicture.Visible = false;
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

      Player closestPlayer = frame.GetClosestPlayer(pitchCoords, 
                                                    Settings.Default.PlayerDiameter);

      if (closestPlayer != null)
      {
        frame.AddDisc(closestPlayer);

        IsComplete = true;
        ModelChanged = true;
      }
    }

    public override void Cancel(VisualOverlay overlay, PlayFrame frame)
    {
      mToolPicture.Visible = true;
    }
  }
}
