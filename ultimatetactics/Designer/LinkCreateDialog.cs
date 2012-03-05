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
  public partial class LinkCreateDialog : Form
  {
    internal LinkCreateDialogResult Result { get; set; }

    public LinkCreateDialog()
    {
      InitializeComponent();
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
      Result = LinkCreateDialogResult.Cancel;
      DialogResult = System.Windows.Forms.DialogResult.Cancel;
    }

    private void LeftLinkButton_Click(object sender, EventArgs e)
    {
      Result = LinkCreateDialogResult.Left;
      DialogResult = System.Windows.Forms.DialogResult.OK;
    }

    private void RightLinkButton_Click(object sender, EventArgs e)
    {
      Result = LinkCreateDialogResult.Right;
      DialogResult = System.Windows.Forms.DialogResult.OK;
    }
  }

  public enum LinkCreateDialogResult
  {
    Left,
    Right,
    Cancel
  }
}
