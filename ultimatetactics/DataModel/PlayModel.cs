using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateTacticsDesigner.DataModel
{
  /// <summary>
  /// A PlayModel is used to represent an entire play simulation. This is the 
  /// high level object and contains the information required to save the 
  /// entire simulation.
  /// </summary>
  [Serializable()]
  class PlayModel
  {
    /// <summary>
    /// The play model consists of a number of frames which are in order.
    /// </summary>
    private List<PlayFrame> mFrames = new List<PlayFrame>();

    public PlayModel()
    {

    }

    /// <summary>
    /// Adds a single frame to the end of the list.
    /// </summary>
    public PlayFrame AddFrame()
    {
      PlayFrame frame = new PlayFrame(this, LastFrame(), null, "");

      if (LastFrame() != null)
      {
        LastFrame().RightLinkedFrame = frame;
      }
      mFrames.Add(frame);

      return frame;
    }

    /// <summary>
    /// Removes a single frame from the list. No checking done to verify that
    /// what is left afterwards makes sense.
    /// </summary>
    /// <param name="frameIndex"></param>
    public void DeleteFrame(PlayFrame frame)
    {
      if (frame.RightLinkedFrame != null)
      {
        frame.RightLinkedFrame.LeftLinkedFrame = null;
      }
      if (frame.LeftLinkedFrame != null)
      {
        frame.LeftLinkedFrame.RightLinkedFrame = null;
      }

      mFrames.Remove(frame);
    }

    /// <summary>
    /// Retrieves a frame by unique id or null if no such frame exists.
    /// </summary>
    /// <param name="uniqueFrameId">The UniqueId of the frame to return</param>
    /// <returns>null if no frame exists</returns>
    public PlayFrame GetFrame(long uniqueFrameId)
    {
      foreach (PlayFrame frame in mFrames)
      {
        if (frame.UniqueId == uniqueFrameId)
        {
          return frame;
        }
      }

      return null;
    }

    /// <summary>
    /// Used when we want access to all the frames in order.
    /// </summary>
    /// <returns></returns>
    internal List<PlayFrame> GetAllFrames()
    {
      return mFrames;
    }

    /// <summary>
    /// Given a frame in the play model this returns the next frame or null
    /// if the passed in frame is the last one.
    /// </summary>
    /// <param name="playFrame"></param>
    /// <returns></returns>
    internal PlayFrame GetNextFrame(PlayFrame playFrame)
    {
      if (mFrames.Contains(playFrame))
      {
        if (mFrames.Last() == playFrame)
        {
          return null;
        }
        else
        {
          return mFrames[mFrames.IndexOf(playFrame) + 1];
        }
      }
      else 
      { 
        return null; 
      }
    }

    /// <summary>
    /// Given a frame in the play model this returns the previous frame or 
    /// null if the passed in frame is the first one.
    /// </summary>
    /// <param name="playFrame"></param>
    /// <returns></returns>
    internal PlayFrame GetPreviousFrame(PlayFrame playFrame)
    {
      if (mFrames.Contains(playFrame))
      {
        if (mFrames.First() == playFrame)
        {
          return null;
        }
        else
        {
          return mFrames[mFrames.IndexOf(playFrame) - 1];
        }
      }
      else
      {
        return null;
      }
    }

    /// <summary>
    /// Retrieve the last frame in the sequence. Returns the frame itself
    /// rather than a clone.
    /// </summary>
    /// <returns></returns>
    internal PlayFrame LastFrame()
    {
      if (mFrames.Count > 0)
      {
        return mFrames.Last();
      }
      else
      {
        return null;
      }
    }

    /// <summary>
    /// The first frame in the model. Returns the object itself, not a clone.
    /// </summary>
    /// <returns></returns>
    internal PlayFrame FirstFrame()
    {
      if (mFrames.Count > 0)
      {
        return mFrames.First();
      }
      else
      {
        return null;
      }
    }

    /// <summary>
    /// The total number of frames in the current model.
    /// </summary>
    public int FrameCount 
    {
      get 
      {
        return mFrames.Count;
      }
    }

    /// <summary>
    /// EXPENSIVE!!!
    /// 
    /// Generates all of the viewing data for all frames to calcualate
    /// the number of cycles used across all frames. Do not call on the 
    /// main GUI thread unless you don't mind it blocking.
    /// </summary>
    internal int CycleCount
    {
      get
      {
        int total = 0;

        foreach (PlayFrame frame in GetAllFrames())
        {
          total += frame.GenerateViewingData().PlayData.Count;
        }

        return total;
      }
    }
  }
}
