using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using UltimateTacticsDesigner.DataModel;
using UltimateTacticsDesigner.Properties;
using UltimateTacticsDesigner.Renderer;

namespace UltimateTacticsDesigner.Designer
{
  class AddBluePlayerTool : Tool
  {
    public AddBluePlayerTool(PictureBox toolPicture)
      : base(toolPicture)
    {
      //ToolCursor = new Cursor("resources/red_team.cur");
    }

    public override void handleMouseUp(Point mouseLocation,
                                       PlayFrame frame,
                                       PitchScreenCoordConverter converter,
                                       VisualOverlay overlay)
    {
      frame.AddPlayer(new Player(Team.BLUE_TEAM, 
                                 "",
                                 Settings.Default.DefaultPlayerSpeed, 
                                 frame.GetNextFreePlayerId(Team.BLUE_TEAM)),
                      converter.screenToPitchCoords(mouseLocation));

      IsComplete = true;
      ModelChanged = true;
    }

    public override void handleMouseDown(Point mouseLocation,
                                         PlayFrame frame,
                                         PitchScreenCoordConverter converter,
                                         VisualOverlay overlay)
    {

    }

    public override void Cancel(VisualOverlay overlay, PlayFrame frame)
    {
    }
  }
}
