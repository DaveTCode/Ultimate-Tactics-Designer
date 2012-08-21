namespace Playbook.Designer
{
  partial class SpeedDialog
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
      this.speedTrackBar = new System.Windows.Forms.TrackBar();
      ((System.ComponentModel.ISupportInitialize)(this.speedTrackBar)).BeginInit();
      this.SuspendLayout();
      // 
      // CancelDialogButton
      // 
      this.CancelDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.CancelDialogButton.Location = new System.Drawing.Point(12, 63);
      this.CancelDialogButton.Name = "CancelDialogButton";
      this.CancelDialogButton.Size = new System.Drawing.Size(75, 23);
      this.CancelDialogButton.TabIndex = 0;
      this.CancelDialogButton.Text = "Cancel";
      this.CancelDialogButton.UseVisualStyleBackColor = true;
      this.CancelDialogButton.Click += new System.EventHandler(this.CancelButton_Click);
      // 
      // OkButton
      // 
      this.OkButton.Location = new System.Drawing.Point(197, 63);
      this.OkButton.Name = "OkButton";
      this.OkButton.Size = new System.Drawing.Size(75, 23);
      this.OkButton.TabIndex = 1;
      this.OkButton.Text = "Ok";
      this.OkButton.UseVisualStyleBackColor = true;
      this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
      // 
      // speedTrackBar
      // 
      this.speedTrackBar.Location = new System.Drawing.Point(12, 12);
      this.speedTrackBar.Name = "speedTrackBar";
      this.speedTrackBar.Size = new System.Drawing.Size(260, 45);
      this.speedTrackBar.TabIndex = 2;
      // 
      // SpeedDialog
      // 
      this.AcceptButton = this.OkButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 96);
      this.Controls.Add(this.speedTrackBar);
      this.Controls.Add(this.OkButton);
      this.Controls.Add(this.CancelDialogButton);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "SpeedDialog";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Text to display during pause";
      this.TopMost = true;
      ((System.ComponentModel.ISupportInitialize)(this.speedTrackBar)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button CancelDialogButton;
    private System.Windows.Forms.Button OkButton;
    private System.Windows.Forms.TrackBar speedTrackBar;
  }
}