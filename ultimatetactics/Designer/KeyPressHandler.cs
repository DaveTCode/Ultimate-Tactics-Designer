using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UltimateTacticsDesigner.Designer
{
  /// <summary>
  /// The KeyPressHandler does application level key handling.
  /// </summary>
  class KeyPressHandler : IMessageFilter
  {
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;

    private Dictionary<Keys, bool> mKeyTable = new Dictionary<Keys, bool>();
    public Dictionary<Keys, bool> KeyTable { get; set; }

    private bool mKeyPressed = false;
    private MainDesignerForm mMainForm;

    public KeyPressHandler(MainDesignerForm mainForm)
    {
      mMainForm = mainForm;
      KeyTable = new Dictionary<Keys, bool>();
    }

    public bool IsKeyPressed()
    {
      return mKeyPressed; 
    }

    public bool IsKeyPressed(Keys k)
    {
      bool pressed = false;

      if (KeyTable.TryGetValue(k, out pressed))
      {
          return pressed;                  
      }

      return false; 
    }

    public bool PreFilterMessage(ref Message m)
    {
      Keys key = (Keys)m.WParam;

      mMainForm.ChooseCursor();

      if (m.Msg == WM_KEYDOWN)
      {
        KeyTable[key] = true;

        mKeyPressed = true;

        if (key == Keys.Escape)
        {
          mMainForm.CancelTool();
        }
        else if (key == Keys.Left)
        {
          mMainForm.PreviousFrame();
        }
        else if (key == Keys.Right)
        {
          mMainForm.NextFrame();
        }
        else if (key == Keys.Z)
        {
          if (IsKeyPressed(Keys.ControlKey))
          {
            mMainForm.Undo(false);
          }
        }
        else if (key == Keys.Y)
        {
          if (IsKeyPressed(Keys.ControlKey))
          {
            mMainForm.Redo();
          }
        }
        else if (key == Keys.S)
        {
          if (IsKeyPressed(Keys.ControlKey))
          {
            mMainForm.SavePlay();
          }
        }
        else if (key == Keys.L)
        {
          if (IsKeyPressed(Keys.ControlKey))
          {
            mMainForm.LoadPlay();
          }
        }
        else if (key == Keys.R)
        {
          mMainForm.UseRedPlayerTool();
        }
        else if (key == Keys.B)
        {
          mMainForm.UseBluePlayerTool();
        }
        else if (key == Keys.D)
        {
          mMainForm.UsePlaceDiscTool();
        }
      }

      if (m.Msg == WM_KEYUP)
      {
        KeyTable.Remove(key);

        mKeyPressed = false;
      }

      return false;
    }
  }
}
