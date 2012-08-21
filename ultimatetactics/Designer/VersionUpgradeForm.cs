using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UltimateTacticsDesigner.Properties;

namespace UltimateTacticsDesigner.Designer
{
  public partial class VersionUpgradeForm : Form
  {
    public VersionUpgradeForm()
    {
      InitializeComponent();

      newVersionLink.Links.Add(new LinkLabel.Link(0, 20, Settings.Default.DownloadServer);
    }

    private void OkButton_Click(object sender, EventArgs e)
    {
      Boolean neverShowAgain = doNotShowCheckbox.Checked;

      if (neverShowAgain)
      {
        Settings.Default.ShowUpgradeDialog = false;
      }
    }
  }
}
