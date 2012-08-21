using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Playbook.Properties;
using System.Windows.Forms;
using System.Threading;

namespace Playbook.Designer
{
  class VersionChecker
  {
    private MainDesignerForm mMainForm;
    private Thread mThread;

    public VersionChecker(MainDesignerForm mainForm)
    {
      mMainForm = mainForm;
      mThread = new Thread(new ThreadStart(CheckVersions));
      mThread.Start();
    }

    private void CheckVersions()
    {
      String remoteVersion = GetRemoteVersion().Trim().TrimEnd(Environment.NewLine.ToCharArray());

      // If the remove version is null then the method to retrieve it failed
      // so don't want the user just silently continue.
      if (remoteVersion != null && remoteVersion != Settings.Default.Version)
      {
        mMainForm.Invoke(new Action(delegate {
          (new VersionUpgradeForm()).ShowDialog();
        }));
      }
    }

    private String GetRemoteVersion()
    {
      try
      {
        StringBuilder sb = new StringBuilder();
        byte[] buf = new byte[8192];

        HttpWebRequest request = (HttpWebRequest)
          WebRequest.Create(Settings.Default.VersionServer);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        Stream resStream = response.GetResponseStream();

        string tempString = null;
        int count = 0;

        do
        {
          count = resStream.Read(buf, 0, buf.Length);

          if (count != 0)
          {
            tempString = Encoding.ASCII.GetString(buf, 0, count);

            sb.Append(tempString);
          }
        } while (count > 0);

        return sb.ToString();
      }
      catch (Exception e)
      {
        // Don't want to bother the user if we can't connect to get the remote 
        // version information.
        return null;
      }
    }
  }
}
