namespace UltimateTacticsDesigner.Designer
{
  partial class VersionUpgradeForm
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
      this.newVersionLink = new System.Windows.Forms.LinkLabel();
      this.doNotShowCheckbox = new System.Windows.Forms.CheckBox();
      this.OkButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // newVersionLink
      // 
      this.newVersionLink.AutoSize = true;
      this.newVersionLink.Cursor = System.Windows.Forms.Cursors.Hand;
      this.newVersionLink.Location = new System.Drawing.Point(13, 9);
      this.newVersionLink.Name = "newVersionLink";
      this.newVersionLink.Size = new System.Drawing.Size(115, 13);
      this.newVersionLink.TabIndex = 0;
      this.newVersionLink.TabStop = true;
      this.newVersionLink.Text = "Download new version";
      this.newVersionLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.newVersionLink_LinkClicked);
      // 
      // doNotShowCheckbox
      // 
      this.doNotShowCheckbox.AutoSize = true;
      this.doNotShowCheckbox.Location = new System.Drawing.Point(16, 42);
      this.doNotShowCheckbox.Name = "doNotShowCheckbox";
      this.doNotShowCheckbox.Size = new System.Drawing.Size(86, 17);
      this.doNotShowCheckbox.TabIndex = 1;
      this.doNotShowCheckbox.Text = "Do not show";
      this.doNotShowCheckbox.UseVisualStyleBackColor = true;
      // 
      // OkButton
      // 
      this.OkButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.OkButton.Location = new System.Drawing.Point(197, 42);
      this.OkButton.Name = "OkButton";
      this.OkButton.Size = new System.Drawing.Size(75, 23);
      this.OkButton.TabIndex = 2;
      this.OkButton.Text = "OK";
      this.OkButton.UseVisualStyleBackColor = true;
      this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
      // 
      // VersionUpgradeForm
      // 
      this.AcceptButton = this.newVersionLink;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.OkButton;
      this.ClientSize = new System.Drawing.Size(284, 72);
      this.ControlBox = false;
      this.Controls.Add(this.OkButton);
      this.Controls.Add(this.doNotShowCheckbox);
      this.Controls.Add(this.newVersionLink);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "VersionUpgradeForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "New Version Available";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.LinkLabel newVersionLink;
    private System.Windows.Forms.CheckBox doNotShowCheckbox;
    private System.Windows.Forms.Button OkButton;
  }
}