using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateTacticsDesigner.DataModel;
using UltimateTacticsDesigner.Renderer;
using UltimateTacticsDesigner.Properties;
using System.Drawing;
using Splicer;
using Splicer.Timeline;
using Splicer.Renderer;
using Splicer.WindowsMedia;
using System.IO;
using System.Globalization;
using UltimateTactics.Designer;

namespace UltimateTacticsDesigner
{
    /// <summary>
    /// The video outputter is used to convert a play model into a video file.
    /// 
    /// It uses ffmpeg (via shell commands) to do the conversion with an 
    /// intermediary step that creates a set of jpg files from the individual 
    /// frames.
    /// 
    /// This is a threaded class which does it's work when Start is called. It
    /// is the responsibility of the calling code to ensure that the play 
    /// model doesn't change during processing. If this does happen then the
    /// video file will end up containing parts of both models.
    /// </summary>
  internal class VideoOutputter
  {
    IProgressCallback mCallback;

    private PlayModel mModel;
    private String mVideoFile;

    public VideoOutputter(String videoFile, PlayModel model)
    {
      mModel = model;
      mVideoFile = videoFile;
    }

    /// <summary>
    /// Thread start. Use in a ThreadStart. No internal checking is done to 
    /// make sure that the model doesn't change during processing so this
    /// must be ensured by the calling code.
    /// </summary>
    /// <param name="status">The IProgressCallback implementation that allows
    /// for progress to be tracked in a gui.</param>
    public void Start(object status)
    {
      mCallback = status as IProgressCallback;

      try
      {
        FFMPEGToVideo();
      }
      finally
      {
        mCallback.End();
      }
    }

    /// <summary>
    /// Takes a model and outputs it as a set of images to the output directory. 
    /// Note that if the files already exist then they'll be removed
    /// </summary>
    /// <param name="outputDir">Location for temporary images.</param>
    private void ModelToImageFiles(String outputDir)
    {
      mCallback.Begin(0, mModel.CycleCount / 4);

      Color backColor = Color.FromArgb(40, 40, 40);
      FrameRenderer renderer = new FrameRenderer(backColor,
                                                 Settings.Default.PitchColor,
                                                 Settings.Default.LineColor);

      using (Bitmap bitmap = new Bitmap((int)(Settings.Default.PitchLength * 10.0f),
                                        (int)(Settings.Default.PitchWidth * 10.0f)))
      {
        using (Graphics graphics = Graphics.FromImage(bitmap))
        {
          int imageIndex = 0;
          FramePlayData fpd;
          foreach (PlayFrame frame in mModel.GetAllFrames())
          {
            fpd = frame.GenerateViewingData();

            // I made a cop out decision to allow for slow speed playback by 
            // creating 4 times as many cycles as needed. Here though we want 
            // to keep the output format as small as possible so only 1 in every
            // 4 frames is displayed.
            //
            // Frankly, this is terrible and will definitely bite me later on.
            int ii = 0;
            foreach (List<ItemPlayData> cycleData in fpd.PlayData)
            {
              ii++;
              if (ii % 4 != 0 && cycleData != fpd.PlayData.Last()) continue; // Thusly do I cry.
              PitchScreenCoordConverter converter = new PitchScreenCoordConverter(graphics);

              graphics.Clear(backColor);
              renderer.DrawPitch(graphics, converter);

              foreach (ItemPlayData itemPlayData in cycleData)
              {
                itemPlayData.Render(graphics, renderer, converter);
              }

              imageIndex++;
              BitmapExtensions.SaveJpg100(bitmap, outputDir + Path.DirectorySeparatorChar + "image" + 
                imageIndex.ToString("D6", CultureInfo.InvariantCulture) + ".jpg");

              mCallback.Increment(1);
            }
          }
        }
      }

      renderer.Dispose();
    }

    /// <summary>
    /// Convert a play model to a video file using FFMPEG to combine the 
    /// independent images. This relies on ffmpeg being the path on the
    /// computer the application is running on.
    /// </summary>
    private void FFMPEGToVideo()
    {
      int videoWidth = (int) (Settings.Default.PitchLength * 10.0f);
      int videoHeight = (int) (Settings.Default.PitchWidth * 10.0f);
      String imageDirectory = GetTempDirectory();

      mCallback.SetText("Starting conversion to images");
      ModelToImageFiles(imageDirectory);
      mCallback.SetText("Completed conversion to images");

      System.Diagnostics.Process ffmpegProcess = new System.Diagnostics.Process();
      ffmpegProcess.StartInfo.FileName = "ffmpeg";
      ffmpegProcess.StartInfo.Arguments = "-f image2 -i \"" + imageDirectory +
        Path.DirectorySeparatorChar + "image%06d.jpg\" -y -r 60 -s " + 
        videoWidth.ToString(CultureInfo.InvariantCulture) + "x" + 
        videoHeight.ToString(CultureInfo.InvariantCulture) + " \"" + mVideoFile + "\"";

      ffmpegProcess.Start();
      ffmpegProcess.WaitForExit();
    }

    /// <summary>
    /// Create a temporary directory in the current users temp folder.
    /// 
    /// We can use this to fill with images for turning into a video.
    /// </summary>
    /// <returns>The directory to use as a string. Guaranteed to have 
    /// been created</returns>
    private String GetTempDirectory()
    {
      String tempPath = Path.GetTempPath();
      String tempDir = Path.Combine(tempPath, "playbook_temp_images");

      if (Directory.Exists(tempDir))
      {
        DirectoryInfo di = new DirectoryInfo(tempDir);
        di.Delete(true);
      }
      Directory.CreateDirectory(tempDir);

      return tempDir;
    }
  }
}
