using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UltimateTacticsDesigner.Renderer;
using System.Globalization;

namespace UltimateTacticsDesigner.DataModel
{
  /// <summary>
  /// The different types of item which we can output to the xml doc.
  /// </summary>
  public enum PlayViewType
  {
    Player,
    Disc,
  }

  class PlayViewOutputter
  {
    private static String OUTPUT_FILE_VERSION = "0.2";

    private PlayViewOutputter()
    {

    }
    
    /// <summary>
    /// Given a play model and a filename this function attempts to write the
    /// entire play out to an xml file using a particular format that the 
    /// viewers can then playback.
    /// 
    /// It is VERY VERY important that the version number is changed each time
    /// any change to the underlying model is made.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="filename"></param>
    public static void OutputModel(PlayModel model, String filename)
    {
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.Indent = false;
      settings.NewLineHandling = NewLineHandling.None;

      using (XmlWriter writer = XmlWriter.Create(filename, settings))
      {
        
        writer.WriteStartDocument();
        writer.WriteStartElement("play");
        OutputFileHeader(writer);

        foreach (PlayFrame frame in model.GetAllFrames())
        {
          OutputFrame(frame, writer);
        }

        writer.WriteEndElement();
        writer.WriteEndDocument();
      }
    }

    /// <summary>
    /// Output any file header information. At the moment this is just the document version info.
    /// </summary>
    /// <param name="writer"></param>
    private static void OutputFileHeader(XmlWriter writer)
    {
      writer.WriteStartElement("document_version");
      writer.WriteValue(OUTPUT_FILE_VERSION);
      writer.WriteEndElement();
    }

    /// <summary>
    /// Outputs all the contents of a single frame. This is done per cycle and
    /// then per item.
    /// </summary>
    /// <param name="frame"></param>
    /// <param name="writer"></param>
    private static void OutputFrame(PlayFrame frame, XmlWriter writer)
    {
      FramePlayData playData = frame.GenerateViewingData();
      int cycleIndex = 0;

      writer.WriteStartElement("frame");
      writer.WriteAttributeString("name", frame.Name);
      foreach (List<ItemPlayData> cyclePlayData in playData.PlayData)
      {
        String pauseText = null;

        if (playData.PauseTexts.ContainsKey(cycleIndex))
        {
          pauseText = playData.PauseTexts[cycleIndex];
        }

        OutputSingleCycle(cyclePlayData, writer, pauseText);
        cycleIndex++;
      }
      writer.WriteEndElement();
    }

    /// <summary>
    /// Outputs a single cycle as a list of items.
    /// </summary>
    /// <param name="cycle"></param>
    /// <param name="writer"></param>
    private static void OutputSingleCycle(List<ItemPlayData> cycle, 
                                          XmlWriter writer, 
                                          String pauseText)
    {
      writer.WriteStartElement("cycle");

      if (pauseText != null)
      {
        writer.WriteStartElement("pause");
        writer.WriteElementString("text", pauseText);
        writer.WriteEndElement();
      }

      foreach (ItemPlayData itemPlayData in cycle)
      {
        OutputSingleItem(itemPlayData, writer);
      }

      writer.WriteEndElement();
    }

    /// <summary>
    /// Writes a single item play data. Note that we need to write what sort of
    /// data this is so that the viewing program can make the decision about
    /// exactly how to display it.
    /// </summary>
    /// <param name="itemPlayData"></param>
    /// <param name="writer"></param>
    private static void OutputSingleItem(ItemPlayData itemPlayData, XmlWriter writer)
    {
      switch (itemPlayData.ViewType)
      {
        case PlayViewType.Player:
          writer.WriteStartElement(PlayViewType.Player.ToString());
          writer.WriteAttributeString("id", itemPlayData.ItemProperties["id"]);
          writer.WriteAttributeString("name", itemPlayData.ItemProperties["name"]);
          writer.WriteAttributeString("team", itemPlayData.ItemProperties["team"]);
          break;

        case PlayViewType.Disc:
          writer.WriteStartElement(PlayViewType.Disc.ToString());
          break;
      }

      writer.WriteElementString("x", itemPlayData.ItemLocation.X.ToString(CultureInfo.InvariantCulture));
      writer.WriteElementString("y", itemPlayData.ItemLocation.Y.ToString(CultureInfo.InvariantCulture));

      writer.WriteEndElement();
    }
  }
}
