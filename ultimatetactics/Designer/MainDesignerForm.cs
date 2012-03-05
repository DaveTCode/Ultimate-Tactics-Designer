using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UltimateTacticsDesigner.DataModel;
using UltimateTacticsDesigner.Renderer;
using UltimateTacticsDesigner;
using UltimateTacticsDesigner.Properties;
using UltimateTactics.Designer;

namespace UltimateTacticsDesigner.Designer
{
  public partial class MainDesignerForm : Form
  {
    private PlayModel mPlayModel = new PlayModel();
    private ModelHistory mModelHistory;
    private KeyPressHandler mKeyPressHandler;
    private FrameRenderer mFrameRenderer;
    private VersionChecker mVersionChecker;

    public MainDesignerForm()
    {
      InitializeComponent();

      // The key press handler is used to track key presses across the whole
      // application.
      mKeyPressHandler = new KeyPressHandler(this);
      Application.AddMessageFilter(mKeyPressHandler);

      frameCollection.SetParentForm(this);
      frameCollection.ModelChanged(mPlayModel);

      // Set up the model history for tracking changes.
      mModelHistory = new ModelHistory();

      // Initialise the frame renderer. This component is responsible for 
      // drawing the frame whilst it is being designed.
      mFrameRenderer = new FrameRenderer(BackColor,
                                         Settings.Default.PitchColor,
                                         Settings.Default.LineColor);
      viewPanel.PlayingThread = new PlayThread(mFrameRenderer);
      viewPanel.CurrentFrame = AddFrame();
      viewPanel.Renderer = mFrameRenderer;
      viewPanel.IsDesignMode = true;
      viewPanel.DesignerForm = this;
      viewPanel.VisualOverlay = new VisualOverlay();
      viewPanel.KeyPressHandler = mKeyPressHandler;

      // Can't set the selected index of the drop down for the play speeds
      // until the objects are all added to the frame.
      playSpeedCombo.SelectedIndex = 1;
      
      // Need to set the current frame on the frame collection so that the 
      // correct frame is increased in size.
      frameCollection.CurrentFrameChanged(viewPanel.CurrentFrame);

      mVersionChecker = new VersionChecker(this);
    }

    ~MainDesignerForm()
    {
      mFrameRenderer.Dispose();
    }

    /// <summary>
    /// Used when we want access to a list of all frames in play order.
    /// </summary>
    /// <returns></returns>
    internal List<PlayFrame> AllFrames()
    {
      return mPlayModel.GetAllFrames();
    }

    /// <summary>
    /// This is called everytime there is any change to the play model.
    /// 
    /// It keeps track of the changes to the model so that they can be 
    /// undone and redone.
    /// 
    /// Also resets the data sources so that any changes in the model are 
    /// mirrored in the ui.
    /// </summary>
    public void ModelChanged()
    {
      if (viewPanel.CurrentFrame != null)
      {
        viewPanel.CurrentFrame.UpdateLinkedFrames();

        // Every time the model changes we may need to redisplay or hide the
        // disc tool.
        discToolPictureBox.Visible =
                     (viewPanel.CurrentFrame.DiscFrameMovement.Thrower == null);
      }

      try
      {
        mModelHistory.ModelChange(mPlayModel);

        if (viewPanel.CurrentFrame != null)
        {
          viewPanel.CurrentFrame.GenerateViewingData();
        }

        frameCollection.ModelChanged(mPlayModel);

        Refresh();
      }
      catch (CyclicDataModelException e)
      {
        MessageBox.Show("Change has caused a cyclic data model: " + e.ToString(), "Reverting Change");

        Undo(true);
      }

      // Each time the model changes we check to see whether the undo/redo
      // buttons should be greyed out.
      undoToolStripMenuItem.Enabled = mModelHistory.CanUndo();
      redoToolStripMenuItem.Enabled = mModelHistory.CanRedo();
    }

    /// <summary>
    /// Called when we want to undo a change in the model.
    /// </summary>
    /// <param name="clearUndoneModel">Set to false if you want it to
    /// be possible to redo the action.</param>
    internal void Undo(Boolean clearUndoneModel)
    {
      ModelHistoryItem historyItem = mModelHistory.Undo(clearUndoneModel);
      if (historyItem != null)
      {
        mPlayModel = historyItem.model;
        viewPanel.CurrentFrame = mPlayModel.GetFrame(viewPanel.CurrentFrame.UniqueId);
        if (viewPanel.CurrentFrame == null)
        {
          viewPanel.CurrentFrame = mPlayModel.FirstFrame();
        }
        viewPanel.Refresh();

        // Need to tell the frame collection that the model has changed as well
        // since it usually finds out from the ModelChanged function of the
        // main designer form (which isn't called during an undo).
        frameCollection.ModelChanged(mPlayModel);
        frameCollection.CurrentFrameChanged(viewPanel.CurrentFrame);
      }

      // Each time the model changes we check to see whether the undo/redo
      // buttons should be greyed out.
      undoToolStripMenuItem.Enabled = mModelHistory.CanUndo();
      redoToolStripMenuItem.Enabled = mModelHistory.CanRedo();
    }

    /// <summary>
    /// Noop if there is nothing to redo.
    /// </summary>
    internal void Redo()
    {
      ModelHistoryItem historyItem = mModelHistory.Redo();
      if (historyItem != null)
      {
        mPlayModel = historyItem.model;
        viewPanel.CurrentFrame = mPlayModel.GetFrame(viewPanel.CurrentFrame.UniqueId);
        if (viewPanel.CurrentFrame == null)
        {
          viewPanel.CurrentFrame = mPlayModel.FirstFrame();
        }
        viewPanel.Refresh();

        // Need to tell the frame collection that the model has changed as well
        // since it usually finds out from the ModelChanged function of the
        // main designer form (which isn't called during an undo).
        frameCollection.ModelChanged(mPlayModel);
        frameCollection.CurrentFrameChanged(viewPanel.CurrentFrame);
      }

      // Each time the model changes we check to see whether the undo/redo
      // buttons should be greyed out.
      undoToolStripMenuItem.Enabled = mModelHistory.CanUndo();
      redoToolStripMenuItem.Enabled = mModelHistory.CanRedo();
    }

    /// <summary>
    /// Adds a new frame to the data model and places it into the table view.
    /// </summary>
    /// <param name="frame">A newly created frame. No verification done to check
    /// that it doesn't already exist.</param>
    /// <param name="showNewFrame">Set to true if you want the new frame to 
    /// become the visible frame</param>
    internal PlayFrame AddFrame()
    {
      PlayFrame frame = mPlayModel.AddFrame();
      ModelChanged();

      return frame;
    }

    /// <summary>
    /// Removes a frame from the play model.
    /// </summary>
    /// <param name="frame"></param>
    internal void RemoveFrame(PlayFrame frame)
    {
      mPlayModel.DeleteFrame(frame);

      ModelChanged();
    }

    /// <summary>
    /// Whenever we change the frame being displayed this is called to
    /// ensure that it is done in a consistent manner.
    /// </summary>
    /// <param name="frame"></param>
    /// <param name="gridIndex"></param>
    internal void ChangeDisplayedFrame(PlayFrame frame)
    {
      // Set the underlying object which represents the frame.
      viewPanel.CurrentFrame = frame;

      frameCollection.CurrentFrameChanged(frame);
      viewPanel.Refresh();

      // When changing frames we need to check whether the disc tool should be
      // displayed or not.
      discToolPictureBox.Visible = (frame.DiscFrameMovement.Thrower == null);
    }

    /// <summary>
    /// Play the list of frames passed in.
    /// </summary>
    /// <param name="frames"></param>
    internal void StartPlayingFrames(List<PlayFrame> frames)
    {
      playButton.ImageIndex = 1;
      stopButton.Enabled = true;

      viewPanel.CurrentTool = null;
      toolBoxPanel.Enabled = false;
      viewPanel.IsDesignMode = false;
      viewPanel.PlayingThread.ParentForm = this;
      viewPanel.PlayingThread.ViewPanel = viewPanel;
      viewPanel.PlayingThread.Start(frames);
    }

    /// <summary>
    /// Asks the playing frame to stop.
    /// </summary>
    internal void StopPlaying()
    {
      if (viewPanel.PlayingThread != null)
      {
        viewPanel.PlayingThread.Stop();
      }
    }

    /// <summary>
    /// Asks the playing frame to pause.
    /// </summary>
    internal void PausePlaying()
    {
      if (viewPanel.PlayingThread != null)
      {
        viewPanel.PlayingThread.Pause();
      }
    }

    /// <summary>
    /// Asks the playing thread to continue when paused.
    /// </summary>
    internal void ContinuePlaying()
    {
      if (viewPanel.PlayingThread != null)
      {
        viewPanel.PlayingThread.Continue();
      }
    }

    /// <summary>
    /// Called whenever the main designer form resizes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MainDesignerForm_Resize(object sender, EventArgs e)
    {
      viewPanel.Refresh();
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SavePlay();
    }

    private void loadToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LoadPlay();
    }

    /// <summary>
    /// Called by the play thread when it finishes running whichever frames
    /// it is currently running.
    /// </summary>
    public void EndPlayingThread()
    {
      toolBoxPanel.Enabled = true;
      stopButton.Enabled = false;
      playButton.ImageIndex = 0;
      viewPanel.IsDesignMode = true;
      viewTrackBar.Value = 0;
      viewTrackBar.Enabled = false;

      viewPanel.Invalidate();
    }

    /// <summary>
    /// In order to enable the toolbox from the play thread we need to use
    /// this slightly convoluted method of using anonymous delegates.
    /// </summary>
    private void EnableToolBox()
    {
      if (toolBoxPanel.InvokeRequired)
      {
        toolBoxPanel.BeginInvoke(new MethodInvoker(delegate() { EnableToolBox(); }));
      }
      else
      {
        toolBoxPanel.Enabled = true;
      }
    }

    /// <summary>
    /// Disables the stop button from the playing thread so needs to use
    /// anonymouse delegates method.
    /// </summary>
    private void DisableStopButton()
    {
      if (stopButton.InvokeRequired)
      {
        stopButton.BeginInvoke(new MethodInvoker(delegate() { DisableStopButton(); }));
      }
      else
      {
        stopButton.Enabled = false;
      }
    }

    /// <summary>
    /// Called when the program closes to ensure that the playing thread is 
    /// stopped before the program exits.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MainDesignerForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (viewPanel.PlayingThread.ThreadState != PlayThreadState.Stopped)
      {
        viewPanel.PlayingThread.Stop();
      }
    }

    private void undoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Undo(false);
    }

    private void redoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Redo();
    }

    /// <summary>
    /// Called when we want to delete a specific player.
    /// </summary>
    /// <param name="player"></param>
    internal void DeletePlayer(Player player)
    {
      viewPanel.CurrentFrame.RemovePlayer(player);
      viewPanel.Refresh();
      ModelChanged();
    }

    /// <summary>
    /// Onclick handler for the play all menu item. When clicked this puts the 
    /// designer into play mode and plays all frames in order.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void playAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (viewPanel.PlayingThread.ThreadState == PlayThreadState.Stopped)
      {
        PlayFrame frame = mPlayModel.FirstFrame();
        StartPlayingFrames(mPlayModel.GetAllFrames());
      }
    }

    /// <summary>
    /// Click handler for the save play menu item. Writes the play out in 
    /// viewable xml format for a player to read.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void savePlaybackToolStripMenuItem_Click(object sender, EventArgs e)
    {
      using (SaveFileDialog saveDialog = new SaveFileDialog())
      {
        saveDialog.OverwritePrompt = true;
        saveDialog.Title = "Save Play";

        if (saveDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
        {
          PlayViewOutputter.OutputModel(mPlayModel, saveDialog.FileName);
        }
      }
    }

    /// <summary>
    /// External save routine. Needs to be internal so that it can be called 
    /// from the application wide key handler.
    /// </summary>
    internal void SavePlay()
    {
      using (SaveFileDialog saveDialog = new SaveFileDialog())
      {
        saveDialog.OverwritePrompt = true;
        saveDialog.Title = "Save Play";
        saveDialog.AddExtension = true;
        saveDialog.Filter = "Play (*.ply)|*.ply";

        if (saveDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
        {
          using (Stream fileStream = saveDialog.OpenFile())
          {
            BinaryFormatter bin = new BinaryFormatter();
            bin.Serialize(fileStream, mPlayModel);
          }
        }
      }
    }

    /// <summary>
    /// External load routine. Needs to be internal so that it can be called
    /// from the application wide key handler.
    /// </summary>
    internal void LoadPlay()
    {
      using (OpenFileDialog openDialog = new OpenFileDialog())
      {
        openDialog.CheckFileExists = true;
        openDialog.CheckPathExists = true;
        openDialog.Multiselect = false;
        openDialog.Title = "Open Play";
        openDialog.AddExtension = true;
        openDialog.Filter = "Play (*.ply)|*.ply";

        if (openDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
        {
          using (Stream fileStream = openDialog.OpenFile())
          {
            BinaryFormatter bin = new BinaryFormatter();
            mPlayModel = (PlayModel)bin.Deserialize(fileStream);
          }

          frameCollection.ModelChanged(mPlayModel);
          ChangeDisplayedFrame(mPlayModel.FirstFrame());

          // Clear the history as this is a completely new play model.
          mModelHistory.Clear();
          mModelHistory.ModelChange(mPlayModel);
        }
      }
    }

    /// <summary>
    /// Clear all cuts up the specified one for a given player.
    /// </summary>
    /// <param name="clickedPlayer"></param>
    /// <param name="clickedCut"></param>
    internal void ClearCuts(Player clickedPlayer, 
                            LinearMovement clickedCut = null)
    {
      viewPanel.CurrentFrame.ClearCuts(clickedPlayer, clickedCut);

      viewPanel.Refresh();
      ModelChanged();
    }

    /// <summary>
    /// Deletes the disc from the current frame and reenables the tool box 
    /// so that it can be added again.
    /// </summary>
    internal void DeleteDisc()
    {
      discToolPictureBox.Visible = true;

      viewPanel.CurrentFrame.RemoveDisc();

      viewPanel.Refresh();
      ModelChanged();
    }

    internal void ClearDiscFlight()
    {
      viewPanel.CurrentFrame.DiscFrameMovement.ClearFlightPath();

      viewPanel.Refresh();
      ModelChanged();
    }

    internal void ClearTrigger(Player clickedPlayer)
    {
      viewPanel.CurrentFrame.Triggers.RemoveAll(trigger => trigger.AffectedPlayer == clickedPlayer);

      viewPanel.Refresh();
      ModelChanged();
    }

    internal void DeleteTrigger(Trigger mClickedTrigger)
    {
      viewPanel.CurrentFrame.RemoveTrigger(mClickedTrigger);

      viewPanel.Refresh();
      ModelChanged();
    }

    /// <summary>
    /// Creates a dialog box that allows the user to select the speed of the
    /// disc within certain limits.
    /// </summary>
    internal void SetDiscSpeed()
    {
      using (SpeedDialog dialog = new SpeedDialog(Settings.Default.MinDiscSpeed,
                                                  Settings.Default.MaxDiscSpeed,
                                                  viewPanel.CurrentFrame.DiscFrameMovement.DiscSpeed))
      {
        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
          viewPanel.CurrentFrame.DiscFrameMovement.DiscSpeed = dialog.GetSpeed();
        }
      }
    }

    /// <summary>
    /// Creates a dialog box that allows the user to set the speed of a given
    /// player within certain limits.
    /// </summary>
    /// <param name="player"></param>
    internal void SetPlayerSpeed(Player player)
    {
      using (SpeedDialog dialog = new SpeedDialog(Settings.Default.MinPlayerSpeed,
                                                 Settings.Default.MaxPlayerSpeed,
                                                 player.MaxSpeed))
      {
        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
          player.MaxSpeed = dialog.GetSpeed();
        }
      }
    }

    /// <summary>
    /// Called to set up the view panel to start drawing the disc flight.
    /// </summary>
    internal void StartDrawingDiscFlight()
    {
      viewPanel.CurrentTool = new DiscFlightTool(null);
      viewPanel.VisualOverlay.DrawingDiscMovement = true;
    }

    /// <summary>
    /// Click handler for the red team buttons. Note that this is done on
    /// mouse down rather than on click so that drag and drop appears to work.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void redTeamTool_MouseDown(object sender, MouseEventArgs e)
    {
      UseRedPlayerTool();
    }

    /// <summary>
    /// Click handler for the blue team buttons. Note that this is done on
    /// mouse down rather than on click so that drag and drop appears to work.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void blueTeamTool_MouseDown(object sender, MouseEventArgs e)
    {
      UseBluePlayerTool();
    }

    /// <summary>
    /// Click handler that starts playing the current frame.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void playButton_Click(object sender, EventArgs e)
    {
      if (viewPanel.PlayingThread.ThreadState == PlayThreadState.Stopped)
      {
        List<PlayFrame> frameList = new List<PlayFrame>();
        frameList.Add(viewPanel.CurrentFrame);
        StartPlayingFrames(frameList);
      }
      else if (viewPanel.PlayingThread.ThreadState == PlayThreadState.Paused)
      {
        playButton.ImageIndex = 1;
        ContinuePlaying();
      }
      else if (viewPanel.PlayingThread.ThreadState == PlayThreadState.Playing)
      {
        playButton.ImageIndex = 0;
        PausePlaying();
      }
    }

    /// <summary>
    /// Stops the currently playing frame.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void stopButton_Click(object sender, EventArgs e)
    {
      StopPlaying();
    }

    /// <summary>
    /// Called whenever the speed drop down selection is changed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void playSpeedCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (playSpeedCombo.SelectedIndex)
      {
        case 0:
          viewPanel.PlayingThread.PlaySpeed = 1.0f;
          break;
        case 1:
          viewPanel.PlayingThread.PlaySpeed = 0.5f;
          break;
        case 2:
          viewPanel.PlayingThread.PlaySpeed = 0.25f;
          break;
        default:
          viewPanel.PlayingThread.PlaySpeed = 1.0f;
          break;
      }
    }

    /// <summary>
    /// Called from the playing thread when it has created the play model to
    /// setup the correct values for the min and max of the trackbar.
    /// </summary>
    /// <param name="p"></param>
    internal void SetupViewTrackbar(int cycleCount)
    {
      viewTrackBar.SmallChange = 1;
      viewTrackBar.LargeChange = 1;
      viewTrackBar.Minimum = 0;
      viewTrackBar.Maximum = cycleCount;
      viewTrackBar.Enabled = true;
    }

    /// <summary>
    /// Called each time the playing thread steps on a frame to update the 
    /// value of the trackbar so that it indicates how far along the frame we are.
    /// </summary>
    /// <param name="value"></param>
    internal void UpdateViewTrackbar(int value)
    {
      viewTrackBar.Value = value;
    }

    /// <summary>
    /// Handles the creation of the disc. Note that we disable the disc tool
    /// as soon as this is clicked so that only one disc can be created.
    /// 
    /// It is reenabled if the disc is not added.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void discToolPictureBox_Click(object sender, EventArgs e)
    {
      UsePlaceDiscTool();
    }

    /// <summary>
    /// Cancels the current tool. This is done 
    /// </summary>
    internal void CancelTool()
    {
      viewPanel.CancelTool();
    }

    /// <summary>
    /// Change the displayed frame to the previous one in the frame collection.
    /// 
    /// If there isn't a previous frame then this is noop.
    /// </summary>
    internal void PreviousFrame()
    {
      PlayFrame frame = mPlayModel.GetPreviousFrame(viewPanel.CurrentFrame);

      if (frame != null)
      {
        ChangeDisplayedFrame(frame);
      }
    }

    /// <summary>
    /// Change the displayed frame to the next one in the frame collection.
    /// 
    /// If there isn't a 'next' then this is a noop.
    /// </summary>
    internal void NextFrame()
    {
      PlayFrame frame = mPlayModel.GetNextFrame(viewPanel.CurrentFrame);

      if (frame != null)
      {
        ChangeDisplayedFrame(frame);
      }
    }

    /// <summary>
    /// Clears the given frame of all contents.
    /// </summary>
    /// <param name="frame"></param>
    internal void ResetFrame(PlayFrame frame)
    {
      frame.Reset();

      ModelChanged();
    }

    private void leftScrollButton_Click(object sender, EventArgs e)
    {
      frameCollection.DigitalScroll(1);
    }

    private void rightScrollButton_Click(object sender, EventArgs e)
    {
      frameCollection.DigitalScroll(-1);
    }

    private void quitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      // Exiting the play thread is done inside the OnClose event so don't need
      // to do it here.
      Application.Exit();
    }

    private void viewTrackBar_Scroll(object sender, ScrollEventArgs e)
    {
      if (!viewPanel.IsDesignMode)
      {
        viewPanel.PlayingThread.RequestCycle(viewTrackBar.Value);
      }
    }

    /// <summary>
    /// Event handler for clicks on the "Save to Video" menu item. 
    /// 
    /// Allows the user to output a play as a video.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void saveToVideoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      using (SaveFileDialog saveDialog = new SaveFileDialog())
      {
        saveDialog.OverwritePrompt = true;
        saveDialog.Title = "Save as Video";
        saveDialog.AddExtension = true;
        saveDialog.Filter = "Avi (*.avi)|*.avi";

        if (saveDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
        {
          using (ProgressWindow window = new ProgressWindow())
          {
            this.Enabled = false;
            VideoOutputter outputter = new VideoOutputter(saveDialog.FileName, 
                                                          mPlayModel);

            ThreadPool.QueueUserWorkItem(new WaitCallback(outputter.Start), window);
            window.ShowDialog(this);

            this.Enabled = true;
          }
        }
      }
    }

    /// <summary>
    /// Switches the current tool to add a red player
    /// </summary>
    internal void UseRedPlayerTool()
    {
      viewPanel.VisualOverlay.SelectedPlayer =
        new Player(Team.RED_TEAM,
                   "",
                   Settings.Default.DefaultPlayerSpeed,
                   viewPanel.CurrentFrame.GetNextFreePlayerId(Team.RED_TEAM));
      viewPanel.CurrentTool = new AddRedPlayerTool(redTeamTool);
    }

    /// <summary>
    /// Switches the current tool to add a blue player.
    /// </summary>
    internal void UseBluePlayerTool()
    {
      viewPanel.VisualOverlay.SelectedPlayer =
        new Player(Team.BLUE_TEAM,
                   "",
                   Settings.Default.DefaultPlayerSpeed,
                   viewPanel.CurrentFrame.GetNextFreePlayerId(Team.BLUE_TEAM));
      viewPanel.CurrentTool = new AddBluePlayerTool(blueTeamTool);
    }

    /// <summary>
    /// Switches to the place disc tool.
    /// </summary>
    internal void UsePlaceDiscTool()
    {
      viewPanel.VisualOverlay.PlacingDisc = true;
      viewPanel.CurrentTool = new DiscTool(discToolPictureBox);
    }

    /// <summary>
    /// Switches to the draw cut tool and sets it up so that the initial
    /// end of the cut is known.
    /// </summary>
    /// <param name="mouseLocation"></param>
    internal void UseDrawCutTool(Point mouseLocation)
    {
      viewPanel.VisualOverlay.DrawingNewCut = true;
      viewPanel.CurrentTool = new PlaceCutTool(null);
      viewPanel.UpdateTool(mouseLocation);
    }

    /// <summary>
    /// Called whenever we want to decide which cursor is showing. In 
    /// particular this is exposed internally so that the KeyPressHandler can
    /// use it.
    /// </summary>
    internal void ChooseCursor()
    {
      Point mouseLocation = Cursor.Position;

      viewPanel.ChooseCursor(viewPanel.PointToClient(mouseLocation));
    }
  }
}
