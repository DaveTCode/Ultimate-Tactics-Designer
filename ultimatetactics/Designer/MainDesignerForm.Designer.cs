using UltimateTacticsDesigner.DataModel;
using System.Drawing;
namespace UltimateTacticsDesigner.Designer
{
  partial class MainDesignerForm
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
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDesignerForm));
        this.menuStrip = new System.Windows.Forms.MenuStrip();
        this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.saveToGifToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.playAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.redTeamTooltip = new System.Windows.Forms.ToolTip(this.components);
        this.redTeamTool = new System.Windows.Forms.PictureBox();
        this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
        this.viewPanel = new UltimateTacticsDesigner.Designer.ViewPanel(this.components);
        this.toolBoxPanel = new System.Windows.Forms.Panel();
        this.discToolPictureBox = new System.Windows.Forms.PictureBox();
        this.blueTeamTool = new System.Windows.Forms.PictureBox();
        this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        this.playButton = new System.Windows.Forms.Button();
        this.playPauseButtonImageList = new System.Windows.Forms.ImageList(this.components);
        this.viewTrackBar = new UltimateTacticsDesigner.Designer.MediaSlider();
        this.playSpeedCombo = new System.Windows.Forms.ComboBox();
        this.stopButton = new System.Windows.Forms.Button();
        this.stopButtonImageList = new System.Windows.Forms.ImageList(this.components);
        this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
        this.frameCollection = new UltimateTacticsDesigner.Designer.FrameCollection();
        this.leftScrollButton = new System.Windows.Forms.Button();
        this.rightScrollButton = new System.Windows.Forms.Button();
        this.discToolTip = new System.Windows.Forms.ToolTip(this.components);
        this.blueTeamToolTip = new System.Windows.Forms.ToolTip(this.components);
        this.menuStrip.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.redTeamTool)).BeginInit();
        this.tableLayoutPanel.SuspendLayout();
        this.toolBoxPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.discToolPictureBox)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.blueTeamTool)).BeginInit();
        this.tableLayoutPanel1.SuspendLayout();
        this.tableLayoutPanel2.SuspendLayout();
        this.SuspendLayout();
        // 
        // menuStrip
        // 
        this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.playToolStripMenuItem});
        this.menuStrip.Location = new System.Drawing.Point(0, 0);
        this.menuStrip.Name = "menuStrip";
        this.menuStrip.Size = new System.Drawing.Size(668, 24);
        this.menuStrip.TabIndex = 0;
        this.menuStrip.Text = "menuStrip1";
        // 
        // fileToolStripMenuItem
        // 
        this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.saveToGifToolStripMenuItem,
            this.quitToolStripMenuItem});
        this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
        this.fileToolStripMenuItem.Text = "File";
        // 
        // saveToolStripMenuItem
        // 
        this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
        this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        this.saveToolStripMenuItem.Text = "Save";
        this.saveToolStripMenuItem.ToolTipText = "Ctrl + S";
        this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
        // 
        // loadToolStripMenuItem
        // 
        this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
        this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        this.loadToolStripMenuItem.Text = "Load";
        this.loadToolStripMenuItem.ToolTipText = "Ctrl + L";
        this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
        // 
        // saveToGifToolStripMenuItem
        // 
        this.saveToGifToolStripMenuItem.Name = "saveToGifToolStripMenuItem";
        this.saveToGifToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        this.saveToGifToolStripMenuItem.Text = "Save as video";
        this.saveToGifToolStripMenuItem.Click += new System.EventHandler(this.saveToVideoToolStripMenuItem_Click);
        // 
        // quitToolStripMenuItem
        // 
        this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
        this.quitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
        this.quitToolStripMenuItem.Text = "Quit";
        this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
        // 
        // editToolStripMenuItem
        // 
        this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
        this.editToolStripMenuItem.Name = "editToolStripMenuItem";
        this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
        this.editToolStripMenuItem.Text = "Edit";
        // 
        // undoToolStripMenuItem
        // 
        this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
        this.undoToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
        this.undoToolStripMenuItem.Text = "Undo";
        this.undoToolStripMenuItem.ToolTipText = "Ctrl + Z";
        this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
        // 
        // redoToolStripMenuItem
        // 
        this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
        this.redoToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
        this.redoToolStripMenuItem.Text = "Redo";
        this.redoToolStripMenuItem.ToolTipText = "Ctrl + Y";
        this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
        // 
        // playToolStripMenuItem
        // 
        this.playToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playAllToolStripMenuItem});
        this.playToolStripMenuItem.Name = "playToolStripMenuItem";
        this.playToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
        this.playToolStripMenuItem.Text = "Play";
        // 
        // playAllToolStripMenuItem
        // 
        this.playAllToolStripMenuItem.Name = "playAllToolStripMenuItem";
        this.playAllToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
        this.playAllToolStripMenuItem.Text = "Play All";
        this.playAllToolStripMenuItem.Click += new System.EventHandler(this.playAllToolStripMenuItem_Click);
        // 
        // redTeamTool
        // 
        this.redTeamTool.Image = global::UltimateTacticsDesigner.Properties.Resources.red_team;
        this.redTeamTool.Location = new System.Drawing.Point(3, 3);
        this.redTeamTool.Name = "redTeamTool";
        this.redTeamTool.Size = new System.Drawing.Size(20, 20);
        this.redTeamTool.TabIndex = 7;
        this.redTeamTool.TabStop = false;
        this.redTeamTooltip.SetToolTip(this.redTeamTool, "Place a red team player");
        this.redTeamTool.MouseDown += new System.Windows.Forms.MouseEventHandler(this.redTeamTool_MouseDown);
        // 
        // tableLayoutPanel
        // 
        this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.tableLayoutPanel.ColumnCount = 1;
        this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel.Controls.Add(this.viewPanel, 0, 1);
        this.tableLayoutPanel.Controls.Add(this.toolBoxPanel, 0, 0);
        this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel1, 0, 2);
        this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel2, 0, 3);
        this.tableLayoutPanel.Location = new System.Drawing.Point(12, 27);
        this.tableLayoutPanel.Name = "tableLayoutPanel";
        this.tableLayoutPanel.RowCount = 4;
        this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
        this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
        this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
        this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
        this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        this.tableLayoutPanel.Size = new System.Drawing.Size(644, 390);
        this.tableLayoutPanel.TabIndex = 3;
        // 
        // viewPanel
        // 
        this.viewPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.viewPanel.AutoSize = true;
        this.viewPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.viewPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
        this.viewPanel.Location = new System.Drawing.Point(3, 43);
        this.viewPanel.Name = "viewPanel";
        this.viewPanel.Size = new System.Drawing.Size(638, 214);
        this.viewPanel.TabIndex = 5;
        // 
        // toolBoxPanel
        // 
        this.toolBoxPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
        this.toolBoxPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.toolBoxPanel.Controls.Add(this.discToolPictureBox);
        this.toolBoxPanel.Controls.Add(this.redTeamTool);
        this.toolBoxPanel.Controls.Add(this.blueTeamTool);
        this.toolBoxPanel.Location = new System.Drawing.Point(267, 3);
        this.toolBoxPanel.Name = "toolBoxPanel";
        this.toolBoxPanel.Size = new System.Drawing.Size(110, 34);
        this.toolBoxPanel.TabIndex = 2;
        // 
        // discToolPictureBox
        // 
        this.discToolPictureBox.Image = global::UltimateTacticsDesigner.Properties.Resources.disc;
        this.discToolPictureBox.Location = new System.Drawing.Point(42, 3);
        this.discToolPictureBox.Name = "discToolPictureBox";
        this.discToolPictureBox.Size = new System.Drawing.Size(22, 20);
        this.discToolPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
        this.discToolPictureBox.TabIndex = 8;
        this.discToolPictureBox.TabStop = false;
        this.discToolTip.SetToolTip(this.discToolPictureBox, "Place disc");
        this.discToolPictureBox.Click += new System.EventHandler(this.discToolPictureBox_Click);
        // 
        // blueTeamTool
        // 
        this.blueTeamTool.Image = global::UltimateTacticsDesigner.Properties.Resources.blue_team;
        this.blueTeamTool.Location = new System.Drawing.Point(81, 3);
        this.blueTeamTool.Name = "blueTeamTool";
        this.blueTeamTool.Size = new System.Drawing.Size(20, 20);
        this.blueTeamTool.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.blueTeamTool.TabIndex = 1;
        this.blueTeamTool.TabStop = false;
        this.discToolTip.SetToolTip(this.blueTeamTool, "Place a blue team player");
        this.blueTeamTool.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueTeamTool_MouseDown);
        // 
        // tableLayoutPanel1
        // 
        this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.tableLayoutPanel1.ColumnCount = 4;
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel1.Controls.Add(this.playButton, 0, 0);
        this.tableLayoutPanel1.Controls.Add(this.viewTrackBar, 3, 0);
        this.tableLayoutPanel1.Controls.Add(this.playSpeedCombo, 2, 0);
        this.tableLayoutPanel1.Controls.Add(this.stopButton, 1, 0);
        this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 263);
        this.tableLayoutPanel1.Name = "tableLayoutPanel1";
        this.tableLayoutPanel1.RowCount = 1;
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel1.Size = new System.Drawing.Size(638, 29);
        this.tableLayoutPanel1.TabIndex = 7;
        // 
        // playButton
        // 
        this.playButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.playButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
        this.playButton.FlatAppearance.BorderSize = 0;
        this.playButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        this.playButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        this.playButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.playButton.ImageKey = "play.png";
        this.playButton.ImageList = this.playPauseButtonImageList;
        this.playButton.Location = new System.Drawing.Point(3, 3);
        this.playButton.Name = "playButton";
        this.playButton.Size = new System.Drawing.Size(24, 23);
        this.playButton.TabIndex = 0;
        this.playButton.TabStop = false;
        this.playButton.UseVisualStyleBackColor = true;
        this.playButton.Click += new System.EventHandler(this.playButton_Click);
        // 
        // playPauseButtonImageList
        // 
        this.playPauseButtonImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("playPauseButtonImageList.ImageStream")));
        this.playPauseButtonImageList.TransparentColor = System.Drawing.Color.Transparent;
        this.playPauseButtonImageList.Images.SetKeyName(0, "play.png");
        this.playPauseButtonImageList.Images.SetKeyName(1, "pause.png");
        // 
        // viewTrackBar
        // 
        this.viewTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.viewTrackBar.Animated = false;
        this.viewTrackBar.AnimationSize = 0.2F;
        this.viewTrackBar.AnimationSpeed = UltimateTacticsDesigner.Designer.MediaSlider.AnimateSpeed.Normal;
        this.viewTrackBar.AutoScrollMargin = new System.Drawing.Size(0, 0);
        this.viewTrackBar.AutoScrollMinSize = new System.Drawing.Size(0, 0);
        this.viewTrackBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
        this.viewTrackBar.BackGroundImage = null;
        this.viewTrackBar.ButtonAccentColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        this.viewTrackBar.ButtonBorderColor = System.Drawing.Color.Black;
        this.viewTrackBar.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        this.viewTrackBar.ButtonCornerRadius = ((uint)(4u));
        this.viewTrackBar.ButtonSize = new System.Drawing.Size(12, 12);
        this.viewTrackBar.ButtonStyle = UltimateTacticsDesigner.Designer.MediaSlider.ButtonType.Round;
        this.viewTrackBar.ContextMenuStrip = null;
        this.viewTrackBar.Enabled = false;
        this.viewTrackBar.LargeChange = 2;
        this.viewTrackBar.Location = new System.Drawing.Point(130, 0);
        this.viewTrackBar.Margin = new System.Windows.Forms.Padding(0);
        this.viewTrackBar.Maximum = 10;
        this.viewTrackBar.Minimum = 0;
        this.viewTrackBar.Name = "viewTrackBar";
        this.viewTrackBar.Orientation = System.Windows.Forms.Orientation.Horizontal;
        this.viewTrackBar.ShowButtonOnHover = false;
        this.viewTrackBar.Size = new System.Drawing.Size(508, 29);
        this.viewTrackBar.SliderFlyOut = UltimateTacticsDesigner.Designer.MediaSlider.FlyOutStyle.None;
        this.viewTrackBar.SmallChange = 1;
        this.viewTrackBar.SmoothScrolling = false;
        this.viewTrackBar.TabIndex = 2;
        this.viewTrackBar.TabStop = false;
        this.viewTrackBar.TickColor = System.Drawing.Color.DarkGray;
        this.viewTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
        this.viewTrackBar.TickType = UltimateTacticsDesigner.Designer.MediaSlider.TickMode.Standard;
        this.viewTrackBar.TrackBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        this.viewTrackBar.TrackDepth = 4;
        this.viewTrackBar.TrackFillColor = System.Drawing.Color.Transparent;
        this.viewTrackBar.TrackProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(101)))), ((int)(((byte)(188)))));
        this.viewTrackBar.TrackShadow = false;
        this.viewTrackBar.TrackShadowColor = System.Drawing.Color.DarkGray;
        this.viewTrackBar.TrackStyle = UltimateTacticsDesigner.Designer.MediaSlider.TrackType.Value;
        this.viewTrackBar.Value = 0;
        this.viewTrackBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.viewTrackBar_Scroll);
        // 
        // playSpeedCombo
        // 
        this.playSpeedCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.playSpeedCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.playSpeedCombo.FormattingEnabled = true;
        this.playSpeedCombo.Items.AddRange(new object[] {
            "1x",
            ".5x",
            ".25x"});
        this.playSpeedCombo.Location = new System.Drawing.Point(63, 3);
        this.playSpeedCombo.Name = "playSpeedCombo";
        this.playSpeedCombo.Size = new System.Drawing.Size(64, 21);
        this.playSpeedCombo.TabIndex = 3;
        this.playSpeedCombo.TabStop = false;
        this.playSpeedCombo.SelectedIndexChanged += new System.EventHandler(this.playSpeedCombo_SelectedIndexChanged);
        // 
        // stopButton
        // 
        this.stopButton.Enabled = false;
        this.stopButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        this.stopButton.FlatAppearance.BorderSize = 0;
        this.stopButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        this.stopButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.stopButton.ImageKey = "stop.png";
        this.stopButton.ImageList = this.stopButtonImageList;
        this.stopButton.Location = new System.Drawing.Point(33, 3);
        this.stopButton.Name = "stopButton";
        this.stopButton.Size = new System.Drawing.Size(24, 23);
        this.stopButton.TabIndex = 4;
        this.stopButton.UseVisualStyleBackColor = true;
        this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
        // 
        // stopButtonImageList
        // 
        this.stopButtonImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("stopButtonImageList.ImageStream")));
        this.stopButtonImageList.TransparentColor = System.Drawing.Color.Transparent;
        this.stopButtonImageList.Images.SetKeyName(0, "stop.png");
        // 
        // tableLayoutPanel2
        // 
        this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.tableLayoutPanel2.ColumnCount = 3;
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        this.tableLayoutPanel2.Controls.Add(this.frameCollection, 1, 0);
        this.tableLayoutPanel2.Controls.Add(this.leftScrollButton, 0, 0);
        this.tableLayoutPanel2.Controls.Add(this.rightScrollButton, 2, 0);
        this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 298);
        this.tableLayoutPanel2.Name = "tableLayoutPanel2";
        this.tableLayoutPanel2.RowCount = 2;
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        this.tableLayoutPanel2.Size = new System.Drawing.Size(638, 89);
        this.tableLayoutPanel2.TabIndex = 8;
        // 
        // frameCollection
        // 
        this.frameCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.frameCollection.AutoScroll = true;
        this.frameCollection.AutoSize = true;
        this.frameCollection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.frameCollection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        this.frameCollection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.frameCollection.Location = new System.Drawing.Point(33, 3);
        this.frameCollection.Name = "frameCollection";
        this.frameCollection.Size = new System.Drawing.Size(572, 63);
        this.frameCollection.TabIndex = 7;
        this.frameCollection.TabStop = false;
        // 
        // leftScrollButton
        // 
        this.leftScrollButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.leftScrollButton.BackgroundImage = global::UltimateTacticsDesigner.Properties.Resources.left_scroll;
        this.leftScrollButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
        this.leftScrollButton.Cursor = System.Windows.Forms.Cursors.PanWest;
        this.leftScrollButton.FlatAppearance.BorderSize = 0;
        this.leftScrollButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.leftScrollButton.Location = new System.Drawing.Point(3, 3);
        this.leftScrollButton.Name = "leftScrollButton";
        this.leftScrollButton.Size = new System.Drawing.Size(24, 63);
        this.leftScrollButton.TabIndex = 8;
        this.leftScrollButton.TabStop = false;
        this.leftScrollButton.UseVisualStyleBackColor = true;
        this.leftScrollButton.Click += new System.EventHandler(this.leftScrollButton_Click);
        // 
        // rightScrollButton
        // 
        this.rightScrollButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.rightScrollButton.BackgroundImage = global::UltimateTacticsDesigner.Properties.Resources.right_scroll;
        this.rightScrollButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
        this.rightScrollButton.Cursor = System.Windows.Forms.Cursors.PanEast;
        this.rightScrollButton.FlatAppearance.BorderSize = 0;
        this.rightScrollButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.rightScrollButton.Location = new System.Drawing.Point(611, 3);
        this.rightScrollButton.Name = "rightScrollButton";
        this.rightScrollButton.Size = new System.Drawing.Size(24, 63);
        this.rightScrollButton.TabIndex = 9;
        this.rightScrollButton.TabStop = false;
        this.rightScrollButton.UseVisualStyleBackColor = true;
        this.rightScrollButton.Click += new System.EventHandler(this.rightScrollButton_Click);
        // 
        // MainDesignerForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
        this.ClientSize = new System.Drawing.Size(668, 429);
        this.Controls.Add(this.tableLayoutPanel);
        this.Controls.Add(this.menuStrip);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.MainMenuStrip = this.menuStrip;
        this.Name = "MainDesignerForm";
        this.Text = "Ultimate Tactics Designer";
        this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainDesignerForm_FormClosed);
        this.Resize += new System.EventHandler(this.MainDesignerForm_Resize);
        this.menuStrip.ResumeLayout(false);
        this.menuStrip.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.redTeamTool)).EndInit();
        this.tableLayoutPanel.ResumeLayout(false);
        this.tableLayoutPanel.PerformLayout();
        this.toolBoxPanel.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.discToolPictureBox)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.blueTeamTool)).EndInit();
        this.tableLayoutPanel1.ResumeLayout(false);
        this.tableLayoutPanel2.ResumeLayout(false);
        this.tableLayoutPanel2.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
    private System.Windows.Forms.ToolTip redTeamTooltip;
    private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem playAllToolStripMenuItem;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    private ViewPanel viewPanel;
    private System.Windows.Forms.Panel toolBoxPanel;
    private System.Windows.Forms.PictureBox redTeamTool;
    private System.Windows.Forms.PictureBox blueTeamTool;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Button playButton;
    private MediaSlider viewTrackBar;
    private System.Windows.Forms.ComboBox playSpeedCombo;
    private System.Windows.Forms.PictureBox discToolPictureBox;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private FrameCollection frameCollection;
    private System.Windows.Forms.Button leftScrollButton;
    private System.Windows.Forms.Button rightScrollButton;
    private System.Windows.Forms.ImageList playPauseButtonImageList;
    private System.Windows.Forms.ImageList stopButtonImageList;
    private System.Windows.Forms.Button stopButton;
    private System.Windows.Forms.ToolStripMenuItem saveToGifToolStripMenuItem;
    private System.Windows.Forms.ToolTip discToolTip;
    private System.Windows.Forms.ToolTip blueTeamToolTip;
  }
}

