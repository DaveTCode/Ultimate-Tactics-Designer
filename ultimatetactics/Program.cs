using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Playbook.Designer;
using Playbook.Properties;

namespace Playbook
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      if (Settings.Default.UsersVersion != Settings.Default.Version)
      {
        Settings.Default.ShowUpgradeDialog = true;
        Settings.Default.UsersVersion = Settings.Default.Version;
        Settings.Default.Save();
      }

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainDesignerForm());
    }
  }
}
