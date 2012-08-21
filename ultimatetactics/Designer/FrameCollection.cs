using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Playbook.DataModel;
using Playbook.Renderer;
using Playbook.Properties;

namespace Playbook.Designer
{
  public partial class FrameCollection : UserControl
  {
    private int mSelectedFrameWidth;
    private int mSelectedFrameHeight;
    private int mUnselectedFrameWidth;
    private int mUnselectedFrameHeight;

    const int FRAME_PADDING_X = 15;
    const int SELECTED_FRAME_Y_PADDING = 5;
    const int UNSELECTED_FRAME_Y_PADDING = 20;
    const int LINK_WIDTH = 30;
    const int LINK_HEIGHT = 25;

    private MainDesignerForm mMainForm;
    private PlayModel mPlayModel;
    private PlayFrame mCurrentFrame;
    private Bitmap mImage;

    private int mFirstVisibleFrame = 0;

    public FrameCollection()
    {
      InitializeComponent();
    }

    internal void SetParentForm(MainDesignerForm parent)
    {
      mMainForm = parent;
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      if (!DesignMode)
      {
        e.Graphics.Clear(mMainForm.BackColor);
        Point backgroundLocation =
          new Point(-1 * mFirstVisibleFrame *
                    (mUnselectedFrameWidth + FRAME_PADDING_X), 0);
        e.Graphics.DrawImage(mImage, backgroundLocation);
      }
    }

    protected override void OnResize(EventArgs e)
    {
      if (mPlayModel != null) CreateBackgroundImage();

      base.OnResize(e);
    }

    /// <summary>
    /// Called from the designer form whenever the model changes.
    /// 
    /// Don't seem to need to automatically refresh. Presumably because
    /// setting the background image causes a refresh itself.
    /// </summary>
    internal void ModelChanged(PlayModel model)
    {
      mPlayModel = model;
      CreateBackgroundImage();
    }

    /// <summary>
    /// Needed for the context menu to have access to the play model of the
    /// calling FrameCollection.
    /// </summary>
    /// <returns>The current play model.</returns>
    internal PlayModel GetPlayModel()
    {
      return mPlayModel;
    }

    /// <summary>
    /// Called whenever the size of the component is changed. Recalculates the 
    /// sizes that the mini frames should be displayed as.
    /// </summary>
    private void RecalculateSizes()
    {
      mSelectedFrameHeight = Math.Max(1, Size.Height - SELECTED_FRAME_Y_PADDING * 2);
      mSelectedFrameWidth = Math.Max(1, mSelectedFrameHeight * 3);

      mUnselectedFrameHeight = Math.Max(1, Size.Height - UNSELECTED_FRAME_Y_PADDING * 2);
      mUnselectedFrameWidth = Math.Max(1, mUnselectedFrameHeight * 3);
    }

    /// <summary>
    /// Called from the designer form whenever the currently selected frame 
    /// changes.
    /// </summary>
    /// <param name="frame"></param>
    internal void CurrentFrameChanged(PlayFrame frame)
    {
      mCurrentFrame = frame;

      // Check to make sure that the current frame will fit onto the screen.
      // If not then adjust the scroll value until it does.
      int frameIndex = mPlayModel.GetAllFrames().IndexOf(frame);
      int numInView = NumberOfFramesInView();
      if (frameIndex < mFirstVisibleFrame)
      {
        mFirstVisibleFrame = frameIndex;
      }
      else if (frameIndex > mFirstVisibleFrame + numInView)
      {
        mFirstVisibleFrame = frameIndex - numInView;
      }

      CreateBackgroundImage();

      Refresh();
    }

    /// <summary>
    /// Whenever the model or the current frame changes we need to change the 
    /// background image to one which represents the new model. This is done
    /// by building up a bitmap one frame at a time.
    /// </summary>
    /// <returns>The new background image</returns>
    private void CreateBackgroundImage()
    {
      float currFrameX = FRAME_PADDING_X;
      int frameWidth;
      int frameHeight;
      int frameYPadding;
      int frameXPadding;

      // Before rendering the background this function calculates what size
      // the small frames should be displayed as.
      RecalculateSizes();

      using (FrameRenderer renderer = new FrameRenderer(BackColor,
                                                        Settings.Default.PitchColor,
                                                        Settings.Default.LineColor))
      {
        Bitmap background = new Bitmap(calculateBackgroundWidth(),
                                       mSelectedFrameHeight +
                                       UNSELECTED_FRAME_Y_PADDING * 2);
        using (Graphics canvas = Graphics.FromImage(background))
        {
          canvas.Clear(Color.FromArgb(40, 40, 40));

          foreach (PlayFrame frame in mPlayModel.GetAllFrames())
          {
            if (frame == mCurrentFrame)
            {
              frameHeight = mSelectedFrameHeight;
              frameWidth = mSelectedFrameWidth;
              frameXPadding = FRAME_PADDING_X;
              frameYPadding = SELECTED_FRAME_Y_PADDING;
            }
            else
            {
              frameHeight = mUnselectedFrameHeight;
              frameWidth = mUnselectedFrameWidth;
              frameXPadding = FRAME_PADDING_X;
              frameYPadding = UNSELECTED_FRAME_Y_PADDING;
            }

            using (Bitmap frameImage = new Bitmap(frameWidth, frameHeight))
            {
              using (Graphics frameCanvas = Graphics.FromImage(frameImage))
              {
                renderer.DrawSmallFrame(frame,
                                    frameCanvas,
                                    new PitchScreenCoordConverter(frameCanvas));

                canvas.DrawImage(frameImage, currFrameX, frameYPadding);


                // Add the links for this frame onto the image.
                DrawLinks(canvas,
                          frame,
                          currFrameX,
                          frameHeight, frameWidth,
                          frameXPadding, frameYPadding);

                currFrameX += frameWidth + frameXPadding;
              }
            }
          }

          if (mImage != null) mImage.Dispose();
          mImage = background;
        }
      }
    }

    /// <summary>
    /// Draw left and right links for the current frame. This only draws half 
    /// of the link as the other half is drawn by the other linked frame.
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="frame"></param>
    /// <param name="currX"></param>
    /// <param name="frameHeight"></param>
    /// <param name="frameWidth"></param>
    /// <param name="frameXPadding"></param>
    /// <param name="frameYPadding"></param>
    private void DrawLinks(Graphics canvas,
                           PlayFrame frame,
                           float currX,
                           int frameHeight, 
                           int frameWidth, 
                           int frameXPadding, 
                           int frameYPadding)
    {
      float y = frameYPadding + (frameHeight - LINK_HEIGHT) / 2;

      // Left hand link. Not drawn for the first frame in the model.
      if (frame.LeftLinkedFrame != null)
      {
        canvas.DrawImage(Resources.right_link,
                         currX - frameXPadding / 2, y,
                         LINK_WIDTH, LINK_HEIGHT);
      }
      else if (mPlayModel.FirstFrame() != frame)
      {
        canvas.DrawImage(Resources.right_link_broken,
                         currX - frameXPadding / 2, y,
                         LINK_WIDTH, LINK_HEIGHT);
      }

      // Right hand link. Not drawn for the last frame in the model.
      if (frame.RightLinkedFrame != null)
      {
        canvas.DrawImage(Resources.left_link,
                         currX + frameXPadding / 2 + frameWidth - LINK_WIDTH + 1, y,
                         LINK_WIDTH, LINK_HEIGHT);
      }
      else if (mPlayModel.LastFrame() != frame)
      {
        canvas.DrawImage(Resources.left_link_broken,
                         currX + frameXPadding / 2 + frameWidth - LINK_WIDTH + 1, y,
                         LINK_WIDTH, LINK_HEIGHT);
      }
    }

    /// <summary>
    /// Calculates the size that the background image for this control needs
    /// to be.
    /// </summary>
    /// <returns></returns>
    private int calculateBackgroundWidth()
    {
      int backgroundWidth = FRAME_PADDING_X;

      // First include the unselected frames and their padding.
      backgroundWidth += (mUnselectedFrameWidth + FRAME_PADDING_X) * 
                                              mPlayModel.GetAllFrames().Count;

      // Add on the width of the selected frame.
      backgroundWidth += mSelectedFrameWidth + FRAME_PADDING_X;

      return backgroundWidth;
    }

    /// <summary>
    /// Handles the left mouse click event to switch the displayed frame.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseClick(MouseEventArgs e)
    {
      // Left click is used for selecting which frame is visible.
      if (e.Button == System.Windows.Forms.MouseButtons.Left)
      {
        Boolean onLeftLink = false;
        Boolean onRightLink = false;

        PlayFrame clickedFrame = GetClosestFrame(e.Location, 
                                                 ref onLeftLink, 
                                                 ref onRightLink);
        if (onLeftLink && clickedFrame != mPlayModel.FirstFrame())
        {
          Boolean doToggle = true;
          LinkCreateDialogResult result = LinkCreateDialogResult.Left;

          if (clickedFrame.LeftLinkedFrame == null)
          {
            using (LinkCreateDialog dialog = new LinkCreateDialog())
            {
              if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
              {
                result = dialog.Result;
                doToggle = true;
              }
              else
              {
                doToggle = false;
              }
            }
          }

          if (doToggle)
          {
            clickedFrame.ToggleLeftLink(result == LinkCreateDialogResult.Right);
            mMainForm.ModelChanged();
          }
        }
        else if (onRightLink && clickedFrame != mPlayModel.LastFrame())
        {
          Boolean doToggle = true;
          LinkCreateDialogResult result = LinkCreateDialogResult.Right;

          if (clickedFrame.RightLinkedFrame == null)
          {
            using (LinkCreateDialog dialog = new LinkCreateDialog())
            {
              if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
              {
                result = dialog.Result;
                doToggle = true;
              }
              else
              {
                doToggle = false;
              }
            }
          }

          if (doToggle)
          {
            clickedFrame.ToggleRightLink(result == LinkCreateDialogResult.Left);
            mMainForm.ModelChanged();
          }
        }
        else if (clickedFrame != null)
        {
          mMainForm.ChangeDisplayedFrame(clickedFrame);
        }
      }
    }

    /// <summary>
    /// Retrieves the closest frame in the collection to the given position.
    /// 
    /// This is used to find a frame when a user clicks on the collection and
    /// needs to be internal so that the context menu class can use it.
    /// </summary>
    /// <param name="mouseLocation">Relative to the control.</param>
    /// <returns>The frame at the given mouse location or null if one doesn't
    /// exist.</returns>
    internal PlayFrame GetClosestFrame(Point mouseLocation, 
                                       ref Boolean onLeftLink,
                                       ref Boolean onRightLink)
    {
      int tempX = mouseLocation.X - FRAME_PADDING_X +
                  mFirstVisibleFrame * (mUnselectedFrameWidth + 
                                        FRAME_PADDING_X);
      List<PlayFrame> frames = mPlayModel.GetAllFrames();
      int frameIndex = 0;

      while (tempX >= 0 && frameIndex < frames.Count)
      {
        PlayFrame frame = frames[frameIndex];

        if (mCurrentFrame == frame)
        {
          // Two parts to this if. First checks whether the mouse location is
          // within the frame coordinates. The second checks whether it is
          // instead just on the part of the link image not in the frame.
          if ((tempX <= mSelectedFrameWidth && mouseLocation.Y >= SELECTED_FRAME_Y_PADDING) ||
              (tempX <= mSelectedFrameWidth + FRAME_PADDING_X && 
               mouseLocation.Y >= (mSelectedFrameHeight - LINK_HEIGHT) / 2 &&
               mouseLocation.Y <= (mSelectedFrameHeight + LINK_HEIGHT) / 2))
          {
            Boolean isLinkYLimit = false;

            if ((mSelectedFrameHeight + LINK_HEIGHT) / 2 > mouseLocation.Y - SELECTED_FRAME_Y_PADDING &&
                (mSelectedFrameHeight - LINK_HEIGHT) / 2 < mouseLocation.Y - SELECTED_FRAME_Y_PADDING)
            {
              isLinkYLimit = true;
            }

            if (isLinkYLimit && tempX > mSelectedFrameWidth - LINK_WIDTH / 2 - 5)
            {
              onRightLink = true;
            }
            else if (isLinkYLimit && tempX < LINK_WIDTH / 2 + 5)
            {
              onLeftLink = true;
            }

            return frame;
          }
          else
          {
            tempX -= mSelectedFrameWidth + FRAME_PADDING_X;
          }
        }
        else
        {
          if ((tempX <= mUnselectedFrameWidth + FRAME_PADDING_X &&
               mouseLocation.Y > UNSELECTED_FRAME_Y_PADDING &&
               mouseLocation.Y <= UNSELECTED_FRAME_Y_PADDING + mUnselectedFrameHeight) ||
              (tempX <= mSelectedFrameWidth + FRAME_PADDING_X &&
               mouseLocation.Y >= UNSELECTED_FRAME_Y_PADDING + 
                                  (mSelectedFrameHeight - LINK_HEIGHT) / 2 &&
               mouseLocation.Y <= UNSELECTED_FRAME_Y_PADDING + 
                                  (mSelectedFrameHeight + LINK_HEIGHT) / 2))
          {
            Boolean isLinkYLimit = false;

            if ((mUnselectedFrameHeight + LINK_HEIGHT) / 2 > mouseLocation.Y - UNSELECTED_FRAME_Y_PADDING &&
                (mUnselectedFrameHeight - LINK_HEIGHT) / 2 < mouseLocation.Y - UNSELECTED_FRAME_Y_PADDING)
            {
              isLinkYLimit = true;
            }

            if (isLinkYLimit && tempX > mUnselectedFrameWidth - LINK_WIDTH / 2 - 5)
            {
              onRightLink = true;
            }
            else if (isLinkYLimit && tempX < LINK_WIDTH / 2 + 5)
            {
              onLeftLink = true;
            }

            return frame;
          }
          else
          {
            tempX -= mUnselectedFrameWidth + FRAME_PADDING_X;
          }
        }

        frameIndex++;
      }

      return null;
    }

    /// <summary>
    /// Passthrough from the context menu to the main frame so that the 
    /// context menu doesn't need to know about the main frame.
    /// </summary>
    /// <param name="frame"></param>
    internal void ResetFrame(PlayFrame frame)
    {
      mMainForm.ResetFrame(frame);
    }

    /// <summary>
    /// Passthrough from the context menu to the main frame so that the 
    /// context menu doesn't need to know about the main frame.
    /// </summary>
    /// <param name="frame"></param>
    internal void DeleteFrame(PlayFrame frame)
    {
      mMainForm.RemoveFrame(frame);
    }

    /// <summary>
    /// Passthrough from the context menu to the main frame so that the 
    /// context menu doesn't need to know about the main frame.
    /// </summary>
    internal void NewFrame()
    {
      mMainForm.AddFrame();
    }

    /// <summary>
    /// Calculates the number of frames that will fit in the frame collection
    /// panel so that we can ensure the selected frame is always there.
    /// </summary>
    /// <returns>The number of frames that can be fitted across the screen.
    /// </returns>
    private int NumberOfFramesInView()
    {
      int backgroundWidth = calculateBackgroundWidth() - mSelectedFrameWidth;

      return backgroundWidth / 
             (mUnselectedFrameWidth + FRAME_PADDING_X) + 1;
    }

    /// <summary>
    /// Sets the first visible frame. Must never be greater than the 
    /// </summary>
    /// <param name="index"></param>
    internal void DigitalScroll(int numFrames)
    {
      mFirstVisibleFrame -= numFrames;

      if (mFirstVisibleFrame < 0)
      {
        mFirstVisibleFrame = 0;
      }
      else if (mFirstVisibleFrame > mPlayModel.GetAllFrames().Count)
      {
        mFirstVisibleFrame = mPlayModel.GetAllFrames().Count;
      }

      Refresh();
    }
  }

  /// <summary>
  /// The frame collection class has a context menu that allows for performing
  /// actions on single frames or on the collection as a whole.
  /// </summary>
  class FrameCollectionContextMenu : ContextMenu
  {
    private FrameCollection mFrameCollection;
    private PlayFrame mClickedFrame;

    internal FrameCollectionContextMenu(FrameCollection frameCollection)
    {
      mFrameCollection = frameCollection;
      mClickedFrame = null;
    }

    protected override void OnPopup(EventArgs e)
    {
      MenuItems.Clear();

      // The mouse coordinates are relative to the main frame so they need to
      // be adjusted for the frame collection panel.
      Point mouseLocation = mFrameCollection.PointToClient(Cursor.Position);
      Boolean onLeftLink = false;
      Boolean onRightLink = false;

      mClickedFrame = mFrameCollection.GetClosestFrame(mouseLocation,
                                                       ref onLeftLink,
                                                       ref onRightLink);

      SetUpBaseMenu();

      if (mClickedFrame != null)
      {
        SetUpFrameMenu();
      }

      base.OnPopup(e);
    }

    private void SetUpBaseMenu()
    {
      MenuItems.Add(new MenuItem("New Frame", NewFrame_Click));
    }

    private void SetUpFrameMenu()
    {
      MenuItems.Add(new MenuItem("-"));
      if (mFrameCollection.GetPlayModel().FrameCount > 1) MenuItems.Add(new MenuItem("Delete Frame", DeleteFrame_Click));
      MenuItems.Add(new MenuItem("Reset Frame", ResetFrame_Click));
    }

    #region "Menu Click Handlers"
      
    internal void NewFrame_Click(object sender, System.EventArgs e)
    {
      mFrameCollection.NewFrame();
    }

    internal void DeleteFrame_Click(object sender, System.EventArgs e)
    {
      mFrameCollection.DeleteFrame(mClickedFrame);
    }

    internal void ResetFrame_Click(object sender, System.EventArgs e)
    {
      mFrameCollection.ResetFrame(mClickedFrame);
    }

    #endregion
  }
}
