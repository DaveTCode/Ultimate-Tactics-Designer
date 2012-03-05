using UltimateTacticsDesigner.DataModel;
using UltimateTacticsDesigner.Renderer;

namespace UltimateTacticsDesigner.Designer
{
  partial class ViewPanel
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private ViewPanelContextMenu contextMenu = null;

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

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.SuspendLayout();

      contextMenu = new ViewPanelContextMenu(this);
      this.ContextMenu = contextMenu;
      // 
      // ViewPanel
      // 
      this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
      this.ResumeLayout(false);

    }
    #endregion
  }
}
