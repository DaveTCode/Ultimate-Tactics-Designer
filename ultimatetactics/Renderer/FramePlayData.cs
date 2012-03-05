using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Globalization;

namespace UltimateTacticsDesigner.Renderer
{

  [Serializable]
  /// <summary>
  /// An instance of this class corresponds to the position and visual 
  /// representation of a single item during the play mode of the designer.
  /// 
  /// All inheriting classes must implement the render method which places
  /// some graphics onto the screen.
  /// </summary>
  abstract class ItemPlayData
  {
    public PointF ItemLocation { get; set; }
    public UltimateTacticsDesigner.DataModel.PlayViewType ViewType { get; set; }
    public Dictionary<String, String> ItemProperties { get; set; }

    public ItemPlayData(PointF position, 
                        UltimateTacticsDesigner.DataModel.PlayViewType viewType)
    {
      ItemLocation = position;
      ViewType = viewType;
      ItemProperties = new Dictionary<string, string>();
    }

    public abstract void Render(Graphics display, 
                                FrameRenderer renderer,
                                PitchScreenCoordConverter converter);
  }

  [Serializable]
  /// <summary>
  /// Represents a single sprite to be displayed on the screen in a single
  /// frame of the play.
  /// </summary>
  class SpritePlayData : ItemPlayData
  {
    private Image mSprite;
    private float mWidth;
    private float mHeight;
    protected PointF DrawLocation { get; set; }

    public SpritePlayData(PointF position, 
                          Image sprite, 
                          float width, 
                          float height,
                          UltimateTacticsDesigner.DataModel.PlayViewType viewType)
      : base(position, viewType)
    {
      mSprite = sprite;
      mWidth = width;
      mHeight = height;
    }

    public override void Render(Graphics display, 
                                FrameRenderer renderer, 
                                PitchScreenCoordConverter converter)
    {
      // Adjust the item location so that the location is at the centre of
      // the image.
      DrawLocation = new PointF(ItemLocation.X - mWidth / 2.0f,
                                ItemLocation.Y - mHeight / 2.0f);

      renderer.DrawSprite(display, converter, mSprite, DrawLocation, mWidth, mHeight);
    }
  }

  [Serializable]
  /// <summary>
  /// Represents the play information for a single player and a single cycle.
  /// This contains all the information required by the play outputter to 
  /// generate readable xml with player info as well as being used internally
  /// by this application to play the frames.
  /// </summary>
  class PlayerPlayData : SpritePlayData
  {
    public PlayerPlayData(PointF position, 
                          Image sprite,
                          float width,
                          float height,
                          int id,
                          String name,
                          UltimateTacticsDesigner.DataModel.Team team)
      : base(position, 
             sprite,
             width,
             height,
             UltimateTacticsDesigner.DataModel.PlayViewType.Player)
    {
      base.ItemProperties.Add("id", id.ToString(CultureInfo.InvariantCulture));
      base.ItemProperties.Add("team", team.UniqueId.ToString(CultureInfo.InvariantCulture));
      base.ItemProperties.Add("name", name);
    }

    public override void Render(Graphics display,
                                FrameRenderer renderer,
                                PitchScreenCoordConverter converter)
    {
      base.Render(display, renderer, converter);

      using (Font font = FrameRenderer.CreatePlayerIdFont(converter))
      {
        renderer.DrawString(display,
                            converter,
                            ItemProperties["id"],
                            DrawLocation,
                            font,
                            Brushes.White);
      }
    }
  }

  class FramePlayData
  {
    public Dictionary<int, String> PauseTexts { get; set; }
    public List<List<ItemPlayData>> PlayData { get; set; }

    /// <summary>
    /// Flips the ordering of the data in the lists.
    /// 
    /// This is used so that we can store each player data as a list and then
    /// flip it so that each list corresponds to a single cycle.
    /// 
    /// One is easier to draw and the other is easier to create.
    /// </summary>
    public void FlipData()
    {
      List<List<ItemPlayData>> flippedData = new List<List<ItemPlayData>>();
      int playerIndex = 0;

      foreach (List<ItemPlayData> itemData in PlayData)
      {
        int cycleIndex = 0;

        foreach (ItemPlayData item in itemData)
        {
          if (playerIndex == 0)
          {
            flippedData.Add(new List<ItemPlayData>());
          }

          flippedData[cycleIndex].Add(item);

          cycleIndex++;
        }

        playerIndex++;
      }

      PlayData = flippedData;
    }
  }
}
