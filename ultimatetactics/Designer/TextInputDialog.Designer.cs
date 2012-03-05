namespace UltimateTacticsDesigner.Designer
{
  partial class TextInputDialog
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.CancelDialogButton = new System.Windows.Forms.Button();
      this.OkButton = new System.Windows.Forms.Button();
      this.richTextBox1 = new System.Windows.Forms.RichTextBox();
      this.SuspendLayout();
      // 
      // CancelButton
      // 
      this.CancelDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.CancelDialogButton.Location = new System.Drawing.Point(13, 227);
      this.CancelDialogButton.Name = "CancelButton";
      this.CancelDialogButton.Size = new System.Drawing.Size(75, 23);
      this.CancelDialogButton.TabIndex = 0;
      this.CancelDialogButton.Text = "Cancel";
      this.CancelDialogButton.UseVisualStyleBackColor = true;
      this.CancelDialogButton.Click += new System.EventHandler(this.CancelButton_Click);
      // 
      // OkButton
      // 
      this.OkButton.Location = new System.Drawing.Point(197, 227);
      this.OkButton.Name = "OkButton";
      this.OkButton.Size = new System.Drawing.Size(75, 23);
      this.OkButton.TabIndex = 1;
      this.OkButton.Text = "Ok";
      this.OkButton.UseVisualStyleBackColor = true;
      this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
      // 
      // richTextBox1
      // 
      this.richTextBox1.Location = new System.Drawing.Point(13, 13);
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.Size = new System.Drawing.Size(259, 208);
      this.richTextBox1.TabIndex = 2;
      this.richTextBox1.Text = "Text entered here will display whilst the playback is paused.";
      this.richTextBox1.Click += new System.EventHandler(this.richTextBox1_Click);
      // 
      // TextInputDialog
      // 
      this.AcceptButton = this.OkButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 262);
      this.Controls.Add(this.richTextBox1);
      this.Controls.Add(this.OkButton);
      this.Controls.Add(this.CancelDialogButton);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "TextInputDialog";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Text to display during pause";
      this.TopMost = true;
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button CancelDialogButton;
    private System.Windows.Forms.Button OkButton;
    private System.Windows.Forms.RichTextBox richTextBox1;
  }
}