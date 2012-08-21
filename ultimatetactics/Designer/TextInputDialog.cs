using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Playbook.Designer
{
  public partial class TextInputDialog : Form
  {
    private Boolean mTextWasClicked = false;

    public TextInputDialog()
    {
      InitializeComponent();
    }

    public TextInputDialog(String text)
      : this()
    {
      mTextWasClicked = true;
      Text = text;
    }

    private void OkButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
    }

    /// <summary>
    /// Retrieves the text from the rich text editor
    /// </summary>
    /// <returns></returns>
    public String GetText()
    {
      return mTextWasClicked ? richTextBox1.Text : String.Empty;
    }

    private void richTextBox1_Click(object sender, EventArgs e)
    {
      if (!mTextWasClicked)
      {
        richTextBox1.Text = String.Empty;
      }

      mTextWasClicked = true;
    }
  }
}
