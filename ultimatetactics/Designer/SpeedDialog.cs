using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UltimateTacticsDesigner.Designer
{
  /// <summary>
  /// A dialog that can be used to input the speed of something between
  /// two values.
  /// 
  /// Used for inputting player and disc speed.
  /// </summary>
  public partial class SpeedDialog : Form
  {
    public SpeedDialog()
    {
      InitializeComponent();
    }

    public SpeedDialog(float minValue, float maxValue, float initialValue)
      : this()
    {
      speedTrackBar.Minimum = (int)minValue;
      speedTrackBar.Maximum = (int)maxValue;
      speedTrackBar.Value = (int)initialValue;
    }

    private void OkButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
    }

    public float GetSpeed()
    {
      return (float)speedTrackBar.Value;
    }
  }
}
