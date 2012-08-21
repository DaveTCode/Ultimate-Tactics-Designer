using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UltimateTacticsDesigner.Designer;
using UltimateTacticsDesigner.Properties;

namespace UltimateTacticsDesigner
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
      }

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainDesignerForm());
    }
  }
}
