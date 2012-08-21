using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Playbook.Renderer;
using System.Drawing;
using Playbook.Properties;
using System.Globalization;

namespace Playbook.DataModel
{
  class DesignToViewConverter
  {
    private DiscMovement mDiscFrameMovement;
    private Dictionary<Player, List<LinearMovement>> mPlayerMovement;
    private List<Trigger> mTriggers;
    private PlayFrame mFrame;
    private FramePlayData mFramePlayData;

    /// <summary>
    /// We track the player delays which can't be immediately processed in this
    /// dictionary. 
    /// 
    /// Each time that a player movement needs to be calculated we can look at 
    /// this and make sure that they are correctly padded at the start.
    /// </summary>
    private Dictionary<Player, int> mPlayerDelays;

    /// <summary>
    /// Used to keep track of which players have been fully processed.
    /// 
    /// This is not used to track whether there are cycles but just so that
    /// we don't process the same players data twice.
    /// </summary>
    private List<Player> mProcessedPlayers;

    public DesignToViewConverter(PlayFrame frame,
                                 DiscMovement discMovement, 
                                 Dictionary<Player, List<LinearMovement>> playerMovement,
                                 List<Trigger> triggers)
    {
      mDiscFrameMovement = discMovement;
      mPlayerMovement = playerMovement;
      mTriggers = triggers;
      mFrame = frame;
      mFramePlayData = new FramePlayData();
      mFramePlayData.PlayData = new List<List<ItemPlayData>>();
      mFramePlayData.PauseTexts = new Dictionary<int, String>();
      mProcessedPlayers = new List<Player>();
      mPlayerDelays = new Dictionary<Player,int>();
    }

    /// <summary>
    /// Externally accessible function to retrieve the full frame play data.
    /// 
    /// This can be called at any point after the instance has been created.
    /// </summary>
    /// <returns></returns>
    public FramePlayData GenerateData()
    {
      if (mFrame.DiscFrameMovement.Thrower == null)
      {
        GenerateDataWithNoDisc(new List<Player>());
      }
      else
      {
        GenerateDataWithDisc();
      }

      // Regardless of how we created the data up to this point we still want
      // to pad the end of the data to make it easier to play.
      AddEndPadding();

      mFramePlayData.FlipData();

      return mFramePlayData;
    }

    /// <summary>
    /// Given the start time for the disc we can calculate the entire of the 
    /// disc flight path. Note that if a player is triggered by the disc then 
    /// calculating this will tell us exactly when they are triggered:
    /// This is passed back as the return value.
    /// </summary>
    /// <param name="startCycles">The number of cycles before the
    /// disc is thrown.</param>
    private List<ItemPlayData> GenerateDiscFlight(int startCycles)
    {
      List<ItemPlayData> discMovement = new List<ItemPlayData>();

      if (!(mDiscFrameMovement.Thrower == null))
      {
        Bitmap disc = Playbook.Properties.Resources.disc;
        float halfPlayerDiameter = Settings.Default.PlayerDiameter / 2.0f;

        PointF curr = mDiscFrameMovement.StartPosition();
        PointF end = mDiscFrameMovement.EndPosition();

        // Generate the list of points through which the disc goes during 
        // the frame.
        DateTime n = DateTime.Now;
        List<PointF> discPoints = 
          mDiscFrameMovement.FlightPath((float) (Renderer.PlayThread.MAX_FRAME_TIME / 4.0));
        double a = (n - DateTime.Now).TotalMilliseconds;

        // The disc is stationary for a number of cycles at the start of the 
        // frame.
        for (int ii = 0; ii < startCycles; ii++)
        {
          discMovement.Add(new SpritePlayData(discPoints[0], 
                                              disc,
                                              halfPlayerDiameter,
                                              halfPlayerDiameter,
                                              PlayViewType.Disc));
        }

        // Add each of the points which the disc passes through to the final 
        // play data information.
        foreach (PointF point in discPoints)
        {
          discMovement.Add(new SpritePlayData(point,
                                              disc,
                                              halfPlayerDiameter,
                                              halfPlayerDiameter,
                                              PlayViewType.Disc));
        }
      }

      return discMovement;
    }

    private void GenerateDataWithDisc()
    {
      if (mDiscFrameMovement.ReceivingCut == null)
      {
        GenerateDataWithNoDisc(new List<Player>());

        if (mDiscFrameMovement.HasMoved)
        {
          mFramePlayData.PlayData.Add(GenerateDiscFlight(0));
        }
      }
      else
      {
        List<Player> parentsToIgnore = new List<Player>();
        int discDelay;

        // Count the number of cycles the disc moves for
        int discCycles = GenerateDiscFlight(0).Count;

        // Count the number of cycles the receiver moves for
        int receiverCycles = GeneratePlayerPoints(mDiscFrameMovement.ReceivingCut.Player, 0).Count;

        // Firstly need to find out whether the disc starts first or
        // the receiver starts first. To do this we need to calculate 
        // the delay on the receiver without actually generating any
        // data.
        Player parent = GetParent(mDiscFrameMovement.ReceivingCut.Player);
        mPlayerDelays.Add(parent, 0);

        // TEMPORARILY add the thrower to the processed list so that they
        // cannot be involved in the receivers movement.
        mProcessedPlayers.Add(mDiscFrameMovement.Thrower);

        // Process the receivers tree starting at the parent.
        ProcessPlayer(parent);
        mProcessedPlayers.Remove(mDiscFrameMovement.Thrower);

        // The difference in total cycle count between the disc (setting off at 0)
        // and the receiver (with correct setting off time).
        int diffInCycles = (receiverCycles + 
                            mPlayerDelays[mDiscFrameMovement.ReceivingCut.Player]) - 
                           discCycles;

        // If disc cycles are fewer than the number of cycles that the receiver 
        // moves for then the disc is guaranteed to set off after the player.
        //
        // Therefore we can leave the generated disc data (this is at index 0
        // in the frame data) and regenerate all player data from the receiver
        // onwards).
        if (diffInCycles > 0)
        {
          // The delay before the disc starts is now goverened by the 
          // length of the delay before the receiver moves.
          discDelay = diffInCycles;

          // The thrower is delayed for AT LEAST the same number of cycles as
          // the receiver because the disc will not move for that number of 
          // cycles.
          mPlayerDelays.Add(mDiscFrameMovement.Thrower, discDelay);

          // Generate the rest of the player information. This is done by
          // ignoring the players who were in the receivers tree and 
          // generating all of the rest of the information.
          // The delay before the thrower starts is stored in the player 
          // delays and will be accounted for whilst processing that part
          // of the tree.
          parentsToIgnore.Add(parent);
        }
        else
        {
          // Clear the data that was generated on the receiver side as it was 
          // based off the assumption that the receiver cannot be blocked by
          // the disc (and they are).
          mFramePlayData.PlayData.Clear();
          mProcessedPlayers.Clear();

          // The disc moves immediately because it has further to travel 
          // than the receiver (measured in cycles rather than distance!)
          discDelay = 0;

          // The receiver is delayed so that they match the disc path.
          mPlayerDelays[mDiscFrameMovement.ReceivingCut.Player] = diffInCycles * -1;
        }

        // We can now calculate the disc movement.
        mFramePlayData.PlayData.Add(GenerateDiscFlight(discDelay));

        // Calculate all of the remaining players movement.
        GenerateDataWithNoDisc(parentsToIgnore);
      }
    }

    /// <summary>
    /// Retrieve the upmost parent of the player in the tree. This relies on 
    /// each player being triggered by at most one other player.
    /// 
    /// The parent of a player is deemed to be the player who triggers the 
    /// current player.
    /// 
    /// The upmost parent is what we get if we continue this processing up the
    /// tree until we find a player who has no parent.
    /// </summary>
    /// <param name="player">The player to search for.</param>
    /// <returns>The current playe if no parents.</returns>
    private Player GetParent(Player player)
    {
      Player parent = player;
      Boolean isFinished;

      do
      {
        isFinished = true;
        foreach (Trigger trigger in mTriggers)
        {
          if (trigger.AffectedPlayer == parent)
          {
            parent = trigger.CausingCutRatio.Player;
            isFinished = false;
            break;
          }
        }
      } while (player != parent && !isFinished);

      return parent;
    }

    /// <summary>
    /// If we do not need to consider the disc movement then this function
    /// generates the entire set of player data for all players.
    /// 
    /// It does not add padding at the end to square off the data so that 
    /// needs to be done by the caller before returning the frameplaydata.
    /// </summary>
    /// <param name="parentsToIgnore">A list of players who have
    /// been processed separately and therefore need to be ignored.</param>
    private void GenerateDataWithNoDisc(List<Player> parentsToIgnore)
    {
      foreach (Player player in mPlayerMovement.Keys.Where(
                                 player => !parentsToIgnore.Contains(player)))
      {
        Boolean ignore = false;

        foreach (Trigger trigger in mTriggers)
        {
          if (trigger.AffectedPlayer == player)
          {
            ignore = true;
          }
        }

        if (!ignore)
        {
          ProcessPlayer(player);
        }
      }

      if (mFramePlayData.PlayData.Count < mPlayerMovement.Keys.Count)
      {
        throw new CyclicDataModelException();
      }
    }

    private void ProcessPlayer(Player player)
    {
      // Since we process the players in a tree structure if a player is hit 
      // more than once then there is a cycle which will not be possible to
      // resolve. This should have been prevented at the design level so
      // we throw an exception.
      if (mProcessedPlayers.Contains(player))
      {
        throw new CyclicDataModelException(player);
      }

      int cycleStart = 0;
      if (mPlayerDelays.ContainsKey(player))
      {
        cycleStart = mPlayerDelays[player];
      }

      mFramePlayData.PlayData.Add(GeneratePlayerPoints(player, cycleStart));

      mProcessedPlayers.Add(player);

      foreach (Trigger trigger in mTriggers.Where(
                   trigger => trigger.CausingCutRatio.Player == player))
      {
        ProcessPlayer(trigger.AffectedPlayer);
      }
    }

    /// <summary>
    /// This is a utilty function which generates the list of points that a 
    /// player passes through on the single frame. 
    /// 
    /// It includes the padding at the start (as a parameter).
    /// </summary>
    /// <param name="player"></param>
    /// <param name="cycleStart">The number of stationary cycles for this
    /// player.</param>
    /// <returns>A list of points that the player passes through.</returns>
    private List<ItemPlayData> GeneratePlayerPoints(Player player, 
                                                    int cycleStart)
    {
      List<ItemPlayData> playerData = new List<ItemPlayData>();
      Boolean isFirstCut = true;
      PointF startPoint = new PointF();

      // If the player has a trigger value stored off from previous 
      // calculations then this is the number of stationary cycles.
      if (mPlayerDelays.ContainsKey(player))
      {
        cycleStart = mPlayerDelays[player];
      }

      foreach (LinearMovement cut in mPlayerMovement[player])
      {
        List<Trigger> applicableTriggers = new List<Trigger>();
        List<PointF> playerPoints = new List<PointF>();

        // For each cut that we process we need to check if any trigger was
        // fired during that cut.
        applicableTriggers.AddRange(mTriggers.Where(
                       trigger => trigger.CausingCutRatio.CausingCut == cut));

        // The first cut is actually just a placeholder to indicate where
        // the player starts out on the field.
        if (isFirstCut)
        {
          startPoint = cut.FinalPosition;
          playerPoints.Add(startPoint);

          // Add on the number of cycles for which the player should stay at
          // the start location. We add on 1 so that the player spends at LEAST
          // one cycle at their start location.
          for (int ii = 0; ii < cycleStart + 1; ii++)
          {
            playerData.Add(new PlayerPlayData(startPoint,
                                              player.PlayerTeam.Sprite,
                                              Settings.Default.PlayerDiameter,
                                              Settings.Default.PlayerDiameter,
                                              player.VisibleID,
                                              player.Name,
                                              player.PlayerTeam));
          }

          isFirstCut = false;
        }
        else
        {
          playerPoints = cut.GeneratePoints(startPoint,
                                            PlayThread.MAX_FRAME_TIME / 4.0,
                                            player.MaxSpeed);

          foreach (PointF point in playerPoints)
          {
            playerData.Add(new PlayerPlayData(point,
                                              player.PlayerTeam.Sprite,
                                              Settings.Default.PlayerDiameter,
                                              Settings.Default.PlayerDiameter,
                                              player.VisibleID,
                                              player.Name,
                                              player.PlayerTeam));

            // Compare the point to each of the triggers fired during this cut
            // noting that since these are PointFs then a comparison can't be
            // absolute.
            foreach (Trigger trigger in applicableTriggers)
            {
              PointF triggerPoint = trigger.CausingCutRatio.GetAbsolutePosition();
              if (GeometryUtils.DistBetweenPoints(triggerPoint, point) < 
                  player.MaxSpeed * PlayThread.MAX_FRAME_TIME / 1000.0f)
              {
                AddUpdateDelay(trigger.AffectedPlayer, playerData.Count);
              }
            }
          }

          startPoint = cut.FinalPosition;
        }
      }

      return playerData;
    }

    /// <summary>
    /// Adds a new entry in the player delay table for the given player if
    /// one does not already exist.
    /// 
    /// If one DOES exist then it only updates it if this delay is larger.
    /// </summary>
    /// <param name="player">The player who is being delayed</param>
    /// <param name="delay">The number of cycles before the player starts
    /// moving.</param>
    /// <returns>True if an addition or update made. False otherwise.</returns>
    private Boolean AddUpdateDelay(Player player, int delay)
    {
      if (mPlayerDelays.ContainsKey(player))
      {
        if (mPlayerDelays[player] < delay)
        {
          mPlayerDelays[player] = delay;
          return true;
        }
        else 
        { 
          return false; 
        }
      }
      else
      {
        mPlayerDelays.Add(player, delay);

        return true;
      }
    }
  
    /// <summary>
    /// This function is the last one called before passing back the calculated 
    /// data.
    /// 
    /// It pads all the players and disc movement so that they have the same 
    /// number of cycles each.
    /// 
    /// We do this to allow the player to just iterate over the cycles and play
    /// everything is sees on every cycle.
    /// </summary>
    private void AddEndPadding()
    {
      int longestCycleLength = 0;

      foreach (List<ItemPlayData> listOfItems in mFramePlayData.PlayData)
      {
        if (listOfItems.Count > longestCycleLength)
        {
          longestCycleLength = listOfItems.Count;
        }
      }

      foreach (List<ItemPlayData> listOfItems in mFramePlayData.PlayData)
      {
        for (int ii = listOfItems.Count; ii < longestCycleLength; ii++)
        {
          listOfItems.Add(listOfItems.Last());
        }
      }
    }
  }

  /// <summary>
  /// Represents an exception where the data model is cyclic and so would not
  /// generate valid playing data.
  /// </summary>
  [Serializable]
  public class CyclicDataModelException : Exception
  {
    private Player mPlayer = null;

    public CyclicDataModelException()
    {

    }

    internal CyclicDataModelException(Player player)
    {
      mPlayer = player;
    }

    public override string ToString()
    {
      if (mPlayer != null)
      {
        return "The data model contains a cycle on player " +
               mPlayer.VisibleID.ToString(CultureInfo.InvariantCulture) +
               " in team " + mPlayer.PlayerTeam.Name;
      }
      else
      {
        return "The data model contains one or more cycles";
      }
    }
  }
}
