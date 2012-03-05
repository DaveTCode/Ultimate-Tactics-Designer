using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace UltimateTacticsDesigner
{
  /// <summary>
  /// Taken from stackoverflow as a method for encoding bmp as jpg with high 
  /// quality.
  /// </summary>
  public static class BitmapExtensions
  {
    public static void SaveJpg100(Image image, string filename)
    {
      EncoderParameters encoderParameters = new EncoderParameters(1);
      encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
      image.Save(filename, GetEncoder(ImageFormat.Jpeg), encoderParameters);
    }

    public static void SaveJpg100(Image image, Stream stream)
    {
      EncoderParameters encoderParameters = new EncoderParameters(1);
      encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
      image.Save(stream, GetEncoder(ImageFormat.Jpeg), encoderParameters);
    }

    public static ImageCodecInfo GetEncoder(ImageFormat format)
    {

      ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

      foreach (ImageCodecInfo codec in codecs)
      {
        if (codec.FormatID == format.Guid)
        {
          return codec;
        }
      }
      return null;
    }
  }
}
