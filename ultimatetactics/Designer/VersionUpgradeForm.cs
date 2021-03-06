﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Playbook.Properties;

namespace Playbook.Designer
{
  public partial class VersionUpgradeForm : Form
  {
    public VersionUpgradeForm()
    {
      InitializeComponent();
    }

    private void OkButton_Click(object sender, EventArgs e)
    {
      Boolean neverShowAgain = doNotShowCheckbox.Checked;

      if (neverShowAgain)
      {
        Settings.Default.ShowUpgradeDialog = false;
        Settings.Default.Save();
      }
    }

    private void newVersionLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      System.Diagnostics.Process.Start(Settings.Default.DownloadServer);
    }
  }
}
