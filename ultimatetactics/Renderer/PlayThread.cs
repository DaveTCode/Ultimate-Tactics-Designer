using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using UltimateTacticsDesigner.DataModel;
using UltimateTacticsDesigner.Designer;
using System.Windows.Forms;
using UltimateTacticsDesigner.Properties;

namespace UltimateTacticsDesigner.Renderer
{
  enum PlayThreadState
  {
    PlayRequested,
    Playing,
    PauseRequested,
    Paused,
    StopRequested,
    Stopped
  }

  class PlayThread
  {
    /// <summary>
    /// This is the number of milliseconds which the thread will wait for
    /// whilst waiting for the state to change.
    /// </summary>
    private const int THREAD_SLEEP_WAITING = 30;

    /// <summary>
    /// The length of time a single cycle of a frame is on the screen is 
    /// measured in millliseconds and is based on trying to get 60fps
    /// </summary>
    public const int DESIRED_CPS = 60;
    public const double MAX_FRAME_TIME = 1000.0 / (double) DESIRED_CPS;

    private Thread mThread;
    private Object mStateLock;
    private FrameRenderer mRenderer;

    public float PlaySpeed { get; set; }

    private PlayThreadState mThreadState;
    public PlayThreadState ThreadState
    {
      get
      {
        lock (mStateLock)
        {
          return mThreadState;
        }
      }
      private set
      {
        lock (mStateLock)
        {
          mThreadState = value;
        }
      }
    }
    public ViewPanel ViewPanel { get; set; }
    public MainDesignerForm ParentForm { get; set; }
    private FramePlayData mPlayData;
    private List<ItemPlayData> mCyclePlayData;
    private List<PlayFrame> mFrameList;
    private int mCycleIndex;

    public PlayThread(FrameRenderer renderer)
    {
      mStateLock = new Object();
      ThreadState = PlayThreadState.Stopped;
      mRenderer = renderer;
      PlaySpeed = 1.0f;
    }

    public void Start(List<PlayFrame> frames)
    {
      mFrameList = frames;
      mThread = new Thread(new ThreadStart(playLoop));
      ThreadState = PlayThreadState.Playing;
      mThread.Start();
    }

    public void Pause()
    {
      ThreadState = PlayThreadState.PauseRequested;

      while (ThreadState != PlayThreadState.Paused)
      {
        Thread.Sleep(THREAD_SLEEP_WAITING);
      }
    }

    public void Continue()
    {
      ThreadState = PlayThreadState.PlayRequested;

      while (ThreadState != PlayThreadState.Playing)
      {
        Thread.Sleep(THREAD_SLEEP_WAITING);
      }
    }

    public void Stop()
    {
      ThreadState = PlayThreadState.StopRequested;

      while (ThreadState != PlayThreadState.Stopped)
      {
        Thread.Sleep(THREAD_SLEEP_WAITING);
      }
    }

    /// <summary>
    /// When this classes thread is started the below loop continues to run
    /// until either the user presses pause/stop or the frame is completed.
    /// 
    /// We use our own buffered graphics object to draw onto the viewpanel as
    /// we had trouble getting double buffered graphics working properly 
    /// otherwise (therefore giving flickery animation).
    /// </summary>
    private void playLoop()
    {
      try
      {
        foreach (PlayFrame frame in mFrameList)
        {
          // Generate the play data from the frame. This is done internally
          // to the frame so that if we want to change the implementation
          // and store the data in the frame we can.
          mPlayData = frame.GenerateViewingData();

          ParentForm.BeginInvoke((MethodInvoker) delegate()
            {
              ParentForm.SetupViewTrackbar(mPlayData.PlayData.Count);
            });

          DateTime currentTime;
          int cycleIndex = 0;
          mCycleIndex = 0;
          while (cycleIndex < mPlayData.PlayData.Count)
          {
            List<ItemPlayData> singleCycleData = mPlayData.PlayData[cycleIndex];

            ThreadState = PlayThreadState.Playing;

            ParentForm.BeginInvoke((MethodInvoker) delegate() 
              { 
                ParentForm.UpdateViewTrackbar(cycleIndex); 
              });
            currentTime = DateTime.Now;
            mCyclePlayData = singleCycleData;

            // Invoked on the gui thread. Required otherwise we get cross thread
            // exceptions.
            ViewPanel.BeginInvoke((MethodInvoker) delegate() 
              { 
                ViewPanel.Refresh();
              });

            // We are attempting to draw at 60 cycles per second to the graphics
            // object. If this is achieved then we need to wait a few 
            // milliseconds after each draw to keep the graphics from being 
            // jerky.
            //
            // If the drawing is too slow we just draw again immediately. 
            // Although if this is the case then the visual will be awful.
            TimeSpan timeDiff = DateTime.Now - currentTime;
            if (timeDiff.TotalMilliseconds < MAX_FRAME_TIME)
            {
              Thread.Sleep((int)(MAX_FRAME_TIME - timeDiff.TotalMilliseconds));
            }

            // Pause requests come from the main thread.
            if (ThreadState == PlayThreadState.PauseRequested)
            {
              ThreadState = PlayThreadState.Paused;

              while (ThreadState == PlayThreadState.Paused)
              {
                Thread.Sleep(100);
              }
            }

            if (ThreadState == PlayThreadState.StopRequested)
            {
              break;
            }

            cycleIndex = mCycleIndex;
            if (PlaySpeed == 1.0f)
            {
              mCycleIndex += 4;
            }
            else if (PlaySpeed == 0.5f)
            {
              mCycleIndex += 2;
            }
            else if (PlaySpeed == 0.25f)
            {
              mCycleIndex++;
            }
          }
        }
      }
      catch (ThreadAbortException e)
      {
        // If the main class is disposed then this exception will be thrown.
        Console.WriteLine(e.Message);
      }
      finally
      {
        mCycleIndex = 0;
        ThreadState = PlayThreadState.Stopped;
        ParentForm.BeginInvoke((MethodInvoker)delegate()
          {
            ParentForm.EndPlayingThread();
          });
      }
    }

    /// <summary>
    /// Renders a single cycles worth of data. This should be called once per 
    /// cycle from the view panel whilst it is in play mode.
    /// 
    /// The use of a buffered graphics context is to reduce flickering by 
    /// double buffering the graphics. For some reason, doing that at the
    /// panel level itself doesn't work so we need to override it here.
    /// 
    /// @@@DAT: Allocation of the back buffer is an expensive operation.
    /// Ideally it wouldn't be done every frame!
    /// </summary>
    /// <param name="graphics">Passed from the event parameter on the OnPaint
    /// method.</param>
    public void RenderCurrentCycle(Graphics graphics)
    {
      if (mCyclePlayData != null)
      {
        BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
        using (BufferedGraphics myBuffer = currentContext.Allocate(graphics,
                                                                   ViewPanel.DisplayRectangle))
        {
          myBuffer.Graphics.Clear(ParentForm.BackColor);

          mRenderer.DrawPitch(myBuffer.Graphics, ViewPanel.Converter);

          // Render the players first so that the disc ends up on top.
          foreach (ItemPlayData itemPlayData in mCyclePlayData.Where(item => item.ViewType == PlayViewType.Player))
          {
            itemPlayData.Render(myBuffer.Graphics, mRenderer, ViewPanel.Converter);
          }

          foreach (ItemPlayData itemPlayData in mCyclePlayData.Where(item => item.ViewType == PlayViewType.Disc))
          {
            itemPlayData.Render(myBuffer.Graphics, mRenderer, ViewPanel.Converter);
          }

          myBuffer.Render();
        }
      }
    }

    /// <summary>
    /// Allows a specific cycle to be requested. Clamped so that it never goes
    /// outside of the range of the play data array.
    /// </summary>
    /// <param name="cycle">Should be between 0 and mPlayData.PlayData.Size - 1</param>
    internal void RequestCycle(int cycle)
    {
      // Clamp the cycle value without informing the calling code.
      cycle = (cycle < 0) ? 0 : cycle;
      cycle = (cycle >= mPlayData.PlayData.Count) ? mPlayData.PlayData.Count - 1 : cycle;

      mCycleIndex = cycle;
      mCyclePlayData = mPlayData.PlayData[mCycleIndex];
      ViewPanel.Refresh();
    }
  }
}
