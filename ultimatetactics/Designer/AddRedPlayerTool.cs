using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Playbook.DataModel;
using Playbook.Properties;
using Playbook.Renderer;

namespace Playbook.Designer
{
  class AddRedPlayerTool : Tool
  {
    public AddRedPlayerTool(PictureBox toolPicture)
      : base(toolPicture)
    {
      //ToolCursor = new Cursor("resources/red_team.cur");
    }

    public override void handleMouseUp(Point mouseLocation, 
                                       PlayFrame frame,
                                       PitchScreenCoordConverter converter,
                                       VisualOverlay overlay)
    {
      frame.AddPlayer(new Player(Team.RED_TEAM, 
                                 "",
                                 Settings.Default.DefaultPlayerSpeed,
                                 frame.GetNextFreePlayerId(Team.RED_TEAM)), 
                      converter.screenToPitchCoords(mouseLocation));

      IsComplete = true;
      ModelChanged = true;
    }

    public override void handleMouseDown(Point mouseLocation, 
                                         PlayFrame frame,
                                         PitchScreenCoordConverter converter,
                                         VisualOverlay overlay)
    {
      //Noop for now.
    }

    public override void Cancel(VisualOverlay overlay, PlayFrame frame)
    {
    }
  }
}
