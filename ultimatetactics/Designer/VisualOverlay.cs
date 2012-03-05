using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateTacticsDesigner.DataModel;
using System.Drawing;

namespace UltimateTacticsDesigner.Designer
{
  class VisualOverlay
  {
    internal Point MouseLocation { get; set; }
    internal Player SelectedPlayer { get; set; }
    internal LinearMovement CutStart { get; set; }
    internal Trigger SelectedTrigger { get; set; }
    internal Boolean DrawingNewCut { get; set; }
    internal Boolean DrawingDiscMovement { get; set; }
    internal Boolean PlacingDisc { get; set; }
    internal Boolean PlacingTrigger { get; set; }
    internal Player TriggerPlayer { get; set; }
    internal Boolean MovingDiscControlPoint { get; set; }

    public VisualOverlay()
    {
      Clear();
    }

    /// <summary>
    /// Clears the overlay so that nothing is displayed on top of the 
    /// design.
    /// 
    /// Should be called whenever a tool completes.
    /// </summary>
    internal void Clear()
    {
      TriggerPlayer = null;
      SelectedTrigger = null;
      SelectedPlayer = null;
      CutStart = null;
      DrawingNewCut = false;
      DrawingDiscMovement = false;
      PlacingDisc = false;
      PlacingTrigger = false;
      MovingDiscControlPoint = false;
    }
  }
}
