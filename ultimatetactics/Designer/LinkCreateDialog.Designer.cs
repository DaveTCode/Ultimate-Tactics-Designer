namespace Playbook.Designer
{
  partial class LinkCreateDialog
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
      this.cancelButton = new System.Windows.Forms.Button();
      this.LeftLinkButton = new System.Windows.Forms.Button();
      this.RightLinkButton = new System.Windows.Forms.Button();
      this.infoTextBox = new System.Windows.Forms.RichTextBox();
      this.SuspendLayout();
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(13, 54);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 0;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
      // 
      // LeftLinkButton
      // 
      this.LeftLinkButton.Location = new System.Drawing.Point(96, 54);
      this.LeftLinkButton.Name = "LeftLinkButton";
      this.LeftLinkButton.Size = new System.Drawing.Size(75, 23);
      this.LeftLinkButton.TabIndex = 1;
      this.LeftLinkButton.Text = "Left";
      this.LeftLinkButton.UseVisualStyleBackColor = true;
      this.LeftLinkButton.Click += new System.EventHandler(this.LeftLinkButton_Click);
      // 
      // RightLinkButton
      // 
      this.RightLinkButton.Location = new System.Drawing.Point(177, 54);
      this.RightLinkButton.Name = "RightLinkButton";
      this.RightLinkButton.Size = new System.Drawing.Size(75, 23);
      this.RightLinkButton.TabIndex = 2;
      this.RightLinkButton.Text = "Right";
      this.RightLinkButton.UseVisualStyleBackColor = true;
      this.RightLinkButton.Click += new System.EventHandler(this.RightLinkButton_Click);
      // 
      // infoTextBox
      // 
      this.infoTextBox.BackColor = System.Drawing.SystemColors.Control;
      this.infoTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.infoTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
      this.infoTextBox.Location = new System.Drawing.Point(13, 13);
      this.infoTextBox.Name = "infoTextBox";
      this.infoTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
      this.infoTextBox.Size = new System.Drawing.Size(244, 35);
      this.infoTextBox.TabIndex = 3;
      this.infoTextBox.TabStop = false;
      this.infoTextBox.Text = "Please select which frame to use as the master\ndata when creating the link.";
      // 
      // LinkCreateDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(269, 89);
      this.ControlBox = false;
      this.Controls.Add(this.infoTextBox);
      this.Controls.Add(this.RightLinkButton);
      this.Controls.Add(this.LeftLinkButton);
      this.Controls.Add(this.cancelButton);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "LinkCreateDialog";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Create Link";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button LeftLinkButton;
    private System.Windows.Forms.Button RightLinkButton;
    private System.Windows.Forms.RichTextBox infoTextBox;
  }
}