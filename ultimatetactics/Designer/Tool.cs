using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using UltimateTacticsDesigner.DataModel;
using UltimateTacticsDesigner.Renderer;

namespace UltimateTacticsDesigner.Designer
{
  abstract class Tool
  {
    //public Cursor ToolCursor;
    public Boolean IsComplete;
    public Boolean ModelChanged;

    // The tool has access to the picture box so that it can change the state
    // or image based on what it has done.
    protected PictureBox mToolPicture;

    public Tool(PictureBox toolPicture)
    {
      mToolPicture = toolPicture;
      IsComplete = false;
      ModelChanged = false;
    }

    public abstract void handleMouseUp(Point mouseLocation,
                                       PlayFrame frame,
                                       PitchScreenCoordConverter converter,
                                       VisualOverlay overlay);

    public abstract void handleMouseDown(Point mouseLocation,
                                         PlayFrame frame,
                                         PitchScreenCoordConverter converter,
                                         VisualOverlay overlay);

    public abstract void Cancel(VisualOverlay overlay, PlayFrame frame);

    public virtual void handleMouseMove(Point mouseLocation,
                                        PlayFrame frame,
                                        PitchScreenCoordConverter converter,
                                        VisualOverlay overlay)
    {
      //No op for normal tools.
    }
  }
}
