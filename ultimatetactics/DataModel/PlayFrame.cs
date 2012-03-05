using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using UltimateTacticsDesigner.Renderer;

namespace UltimateTacticsDesigner.DataModel
{
  /// <summary>
  /// A single frame during the play.
  /// </summary>
  [Serializable()]
  class PlayFrame
  {
    private static long NextUniqueId = 0;

    internal long UniqueId;
    internal Dictionary<Player, List<LinearMovement>> PlayerMovement { get; set; }
    internal List<Trigger> Triggers { get; set; }
    internal String Notes { get; set; }
    internal String Name { get; set; }
    internal DiscMovement DiscFrameMovement { get; set; }
    internal PlayFrame LeftLinkedFrame { get; set; }
    internal PlayFrame RightLinkedFrame { get; set; }

    private PlayModel mPlayModel;

    /// <summary>
    /// Creates a blank frame with only the name set.
    /// </summary>
    /// <param name="name">The display name for the frame.</param>
    private PlayFrame(PlayModel model, String name)
    {
      mPlayModel = model;
      Name = name;
      Notes = "";
      PlayerMovement = new Dictionary<Player, List<LinearMovement>>();
      Triggers = new List<Trigger>();
      DiscFrameMovement = new DiscMovement(this);
      UniqueId = NextUniqueId++;
    }

    /// <summary>
    /// This constructor takes the previous frame and uses that to determine 
    /// the starting positions for the players.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="previousFrame"></param>
    /// <param name="name"></param>
    /// <param name="nextFrame"></param>
    public PlayFrame(PlayModel model, 
                     PlayFrame previousFrame, 
                     PlayFrame nextFrame, 
                     String name)
      : this(model, name)
    {
      LeftLinkedFrame = previousFrame;
      RightLinkedFrame = nextFrame;

      UpdateFromLeftLink();
      UpdateFromRightLink();
    }

    /// <summary>
    /// If the frame has a left link then this removes it (and also
    /// removes the right hand link from the previously linked frame).
    /// 
    /// If the frame doesn't have a link then it creates one to the previous
    /// frame in the sequence and then updates that frame based on the initial
    /// coordinates of each player.
    /// </summary>
    internal void ToggleLeftLink(Boolean isMaster)
    {
      if (LeftLinkedFrame != null)
      {
        LeftLinkedFrame.RightLinkedFrame = null;
        LeftLinkedFrame = null;
      }
      else
      {
        LeftLinkedFrame = mPlayModel.GetPreviousFrame(this);
        LeftLinkedFrame.RightLinkedFrame = this;

        if (isMaster)
        {
          UpdateLinkedFrames();
        }
        else
        {
          UpdateFromLeftLink();
        }
      }
    }

    /// <summary>
    /// If the frame has a right link then this removes it (and also
    /// removes the left hand link from the previously linked frame).
    /// 
    /// If the frame doesn't have a link then it creates one to the next
    /// frame in the sequence and then updates that frame based on the final
    /// coordinates of each player.
    /// </summary>
    internal void ToggleRightLink(Boolean isMaster)
    {
      if (RightLinkedFrame != null)
      {
        RightLinkedFrame.LeftLinkedFrame = null;
        RightLinkedFrame = null;
      }
      else
      {
        RightLinkedFrame = mPlayModel.GetNextFrame(this);
        RightLinkedFrame.LeftLinkedFrame = this;

        if (isMaster)
        {
          UpdateLinkedFrames();
        }
        else
        {
          UpdateFromRightLink();
        }
      }
    }

    /// <summary>
    /// Updates the initial positions on the current frame from the previous
    /// one in the series.
    /// </summary>
    /// <param name="leftLinkedFrame">The frame from which we take this
    /// frames initial positions.</param>
    protected void UpdateFromLeftLink()
    {
      if (LeftLinkedFrame != null)
      {
        // Remove any players who were removed from the left hand frame.
        // Note this is done using back iteration because we are modifying the
        // collection as we iterate over it.
        List<Player> players = PlayerMovement.Keys.ToList();
        for (int ii = players.Count - 1; ii >= 0; ii--)
        {
          Player player = players[ii];
          if (!LeftLinkedFrame.PlayerMovement.ContainsKey(player))
          {
            RemovePlayer(player);
          }
        }

        foreach (Player player in LeftLinkedFrame.PlayerMovement.Keys)
        {
          PointF startPosition = LeftLinkedFrame.PlayerMovement[player].Last().FinalPosition;

          if (PlayerMovement.ContainsKey(player))
          {
            PlayerMovement[player].First().FinalPosition = startPosition;
          }
          else
          {
            List<LinearMovement> startList = new List<LinearMovement>();
            startList.Add(new LinearMovement(startPosition,
                                             0,
                                             player));
            PlayerMovement.Add(player, startList);
          }
        }

        // If the receiver has changed on the left hand frame then force the
        // current frame to update the thrower.
        //
        // Note that if the thrower doesn't exist in the current frame then
        // we just ignore the disc.
        if (LeftLinkedFrame.DiscFrameMovement.Thrower != null)
        {
          LinearMovement receivingCut = LeftLinkedFrame.DiscFrameMovement.ReceivingCut;
          Player thrower = LeftLinkedFrame.DiscFrameMovement.Thrower;

          if (receivingCut != null)
          {
            if (PlayerMovement.ContainsKey(receivingCut.Player))
            {
              DiscFrameMovement.Thrower = receivingCut.Player;
            }
          }
          else
          {
            DiscFrameMovement.Thrower = thrower;
          }
        }
        else
        {
          DiscFrameMovement.Clear();
        }

        // Call across to the next frame in the links. This is because 
        // updating the end positions of players can also update 
        // their start positions.
        if (RightLinkedFrame != null)
        {
          RightLinkedFrame.UpdateFromLeftLink();
        }
      }
    }

    /// <summary>
    /// Updates the final positions of the players in this frame from the 
    /// frame to it's right.
    /// </summary>
    protected void UpdateFromRightLink()
    {
      if (RightLinkedFrame != null)
      {
        // Remove any players who were removed from the right hand frame.
        // Note this is done using back iteration because we are modifying the
        // collection as we iterate over it.
        List<Player> players = PlayerMovement.Keys.ToList();
        for (int ii = players.Count - 1; ii >= 0; ii--)
        {
          Player player = players[ii];
          if (!RightLinkedFrame.PlayerMovement.ContainsKey(player))
          {
            RemovePlayer(player);
          }
        }

        foreach (Player player in RightLinkedFrame.PlayerMovement.Keys)
        {
          PointF endPosition = RightLinkedFrame.PlayerMovement[player].First().FinalPosition;

          if (PlayerMovement.ContainsKey(player))
          {
            PlayerMovement[player].Last().FinalPosition = endPosition;
          }
          else
          {
            List<LinearMovement> startList = new List<LinearMovement>();
            startList.Add(new LinearMovement(
                endPosition,
                0,
                player));
            PlayerMovement.Add(player, startList);
          }
        }

        // The receiver will be changed so that they are the same as the
        // thrower in the next frame.
        //
        // If there is currently no receiver then the thrower will be changed
        // instead (as they act as receiver).
        Player nextThrower = RightLinkedFrame.DiscFrameMovement.Thrower;
        if (nextThrower != null)
        {
          if (PlayerMovement.ContainsKey(nextThrower))
          {
            if (DiscFrameMovement.HasMoved)
            {
              DiscFrameMovement.ReceivingCut = PlayerMovement[nextThrower].Last();
            }
            else
            {
              DiscFrameMovement.Thrower = nextThrower;
            }
          }
          else
          {
            DiscFrameMovement.ReceivingCut = null;
            DiscFrameMovement.AbsoluteFlightPath =
                             RightLinkedFrame.DiscFrameMovement.EndPosition();
          }
        }
        else
        {
          DiscFrameMovement.Clear();
        }

        // Call across to the next frame in the links. This is because 
        // updating the end positions of players can also update 
        // their start positions.
        if (LeftLinkedFrame != null)
        {
          LeftLinkedFrame.UpdateFromRightLink();
        }
      }
    }

    /// <summary>
    /// Updates the frames that are linked to this one.
    /// 
    /// This function is called when changes are made to this frame.
    /// </summary>
    internal void UpdateLinkedFrames()
    {
      if (LeftLinkedFrame != null)
      {
        LeftLinkedFrame.UpdateFromRightLink();
      }
      if (RightLinkedFrame != null)
      {
        RightLinkedFrame.UpdateFromLeftLink();
      }
    }

    /// <summary>
    /// Adds a single player to the frame and sets the first 
    /// </summary>
    /// <param name="player"></param>
    /// <param name="startingPosition"></param>
    public void AddPlayer(Player player, PointF startingPosition)
    {
      PlayerMovement.Add(player, new List<LinearMovement>());
      PlayerMovement[player].Add(new LinearMovement(startingPosition, 
                                                    0, 
                                                    player));
    }

    /// <summary>
    /// So that we can add players with unique ids this function can return
    /// the next free id for a given team.
    /// </summary>
    /// <param name="team"></param>
    /// <returns>0 if there are no players on that team otherwise the first 
    /// free id > 0</returns>
    public int GetNextFreePlayerId(Team team)
    {
      List<int> ids = new List<int>();

      foreach (Player player in PlayerMovement.Keys.Where(player => player.PlayerTeam == team))
      {
        ids.Add(player.VisibleID);
      }

      ids.Sort();

      int nextFreeId = 1;
      int prevId = 0;
      foreach (int id in ids)
      {
        if (id - prevId > 1)
        {
          nextFreeId = prevId + 1;
          break;
        }
        else
        {
          nextFreeId = id + 1;
          prevId = id;
        }
      }

      return nextFreeId;
    }

    /// <summary>
    /// Retrieves the closest player to a given point on the pitch. The player
    /// must be within a certain radius of the point.
    /// </summary>
    /// <param name="point">Pitch coordinates.</param>
    /// <param name="boundary">Radius in which to search.</param>
    /// <param name="minBoundary">Radius in which to exclude. 
    /// Defaults to 0.</param>
    /// <returns>null if no player found. Otherwise the closest 
    /// player.</returns>
    public Player GetClosestPlayer(PointF point, 
                                   float boundary, 
                                   float minBoundary = -1.0f)
    {
      Player closestPlayer = null;
      float minDistance = boundary;

      foreach (Player player in PlayerMovement.Keys)
      {
        PointF startLocation = PlayerMovement[player][0].FinalPosition;

        float distance = GeometryUtils.DistBetweenPoints(startLocation, point);
        if (distance < minDistance && distance > minBoundary)
        {
          minDistance = distance;
          closestPlayer = player;
        }
      }

      return closestPlayer;
    }

    /// <summary>
    /// This function creates the location data that can be used to play a 
    /// frame. This is not done in realtime during frame processing more for
    /// ease of debugging than due to any desire to optimise the viewing.
    /// 
    /// It does have the nice side effect of generating data that is 
    /// independent of the design which could be potentially used to create a
    /// viewer in a different environment (e.g. flash viewer for the web).
    /// </summary>
    /// <param name="renderer"></param>
    public Renderer.FramePlayData GenerateViewingData()
    {
      DesignToViewConverter converter = new DesignToViewConverter(this,
                                                                  DiscFrameMovement,
                                                                  PlayerMovement,
                                                                  Triggers);

      return converter.GenerateData();
    }

    /// <summary>
    /// Given a position on the pitch and a player who is going to be moved we
    /// are able to check whether the trigger lies on a cut from any other 
    /// player and if so to create a new trigger at that position so that the
    /// player moves when the trigger is hit.
    /// </summary>
    /// <param name="pitchCoords"></param>
    /// <param name="maxDistanceFromLine"></param>
    /// <param name="affectedPlayer"></param>
    /// <returns>True if a trigger was created and false otherwise.</returns>
    public Boolean MaybeCreateTrigger(PointF pitchCoords, 
                                      float maxDistanceFromLine, 
                                      Player affectedPlayer)
    {
      Boolean foundCut = false;
      CutRatio cutRatio = GetClosestCutPoint(pitchCoords,
                                             maxDistanceFromLine,
                                             affectedPlayer);

      if (cutRatio != null)
      {
        foundCut = true;
        Trigger trigger = new Trigger(cutRatio, affectedPlayer);
        Triggers.Add(trigger);
      }

      return foundCut;
    }

    /// <summary>
    /// Retrieves the closest point on any cut to the given coordinates. This 
    /// is used for placing triggers and indicating where the disc should go
    /// </summary>
    /// <param name="pitchCoords"></param>
    /// <param name="selectionMaxDistance"></param>
    /// <param name="playerToIgnore">Compares the unique ids rather than
    /// references.</param>
    /// <param name="selectionMinDistance"></param>
    /// <returns></returns>
    public CutRatio GetClosestCutPoint(PointF pitchCoords, 
                                       float selectionMaxDistance,
                                       Player playerToIgnore,
                                       float selectionMinDistance = -1.0f)
    {
      CutRatio cutRatio = null;
      LinearMovement previousMovement = null;
      float minDistanceFromLine = selectionMaxDistance;
      LinearMovement closestMovement = null;
      LinearMovement closestPreviousMovement = null;
      Player closestPlayer = null;
      PointF triggerPoint = new PointF();

      foreach (Player player in PlayerMovement.Keys.Where(player => (playerToIgnore == null) || 
                                                                    (player.UniqueId != playerToIgnore.UniqueId)))
      {
        List<LinearMovement> playerCut = PlayerMovement[player];

        if (playerCut.Count > 1)
        {
          previousMovement = playerCut[0];
          PointF startCut = playerCut[0].FinalPosition;

          foreach (LinearMovement cutSection in PlayerMovement[player].GetRange(1, playerCut.Count - 1))
          {
            PointF endCut = cutSection.FinalPosition;
            PointF closestPoint = GeometryUtils.ClosestPointLineSegment(startCut, endCut, pitchCoords);
            float distanceFromCut = GeometryUtils.DistBetweenPoints(closestPoint, pitchCoords);

            if (distanceFromCut < minDistanceFromLine && 
                distanceFromCut > selectionMinDistance)
            {
              minDistanceFromLine = distanceFromCut;
              closestMovement = cutSection;
              closestPlayer = player;
              closestPreviousMovement = previousMovement;
              triggerPoint = closestPoint;
            }

            startCut = cutSection.FinalPosition;
            previousMovement = cutSection;
          }
        }
      }

      if (closestPlayer != null)
      {
        float cutLength = GeometryUtils.DistBetweenPoints(closestMovement.FinalPosition,
                                                          closestPreviousMovement.FinalPosition);
        float distanceToTrigger = GeometryUtils.DistBetweenPoints(closestPreviousMovement.FinalPosition,
                                                                  triggerPoint);

        cutRatio = new CutRatio();
        cutRatio.CausingCut = closestMovement;
        cutRatio.PreviousCut = closestPreviousMovement;
        cutRatio.RatioAlongCut = Math.Abs(distanceToTrigger / cutLength);
        cutRatio.Player = closestPlayer;
      }

      return cutRatio;
    }

    /// <summary>
    /// Find the closest cut end to the given pitch coordinates within the 
    /// specified radius.
    /// </summary>
    /// <param name="pitchCoords">The location to search from.</param>
    /// <param name="selectionMaxDistance">Radius to search in.</param>
    /// <param name="selectionMinDistance">Radius to exclude results in.</param>
    /// <returns>null if no cut found in that radius or the closest cut.</returns>
    public LinearMovement GetClosestCutEnd(PointF pitchCoords, 
                                           float selectionMaxDistance, 
                                           float selectionMinDistance = -1.0f)
    {
      LinearMovement closestCut = null;
      float closestDistance = selectionMaxDistance;

      foreach (List<LinearMovement> playerCuts in PlayerMovement.Values)
      {
        foreach (LinearMovement cut in playerCuts)
        {
          float distance = GeometryUtils.DistBetweenPoints(pitchCoords, 
                                                           cut.FinalPosition);

          if (distance < closestDistance && distance > selectionMinDistance)
          {
            closestCut = cut;
            closestDistance = distance;
          }
        }
      }

      return closestCut;
    }

    /// <summary>
    /// Retrieve the closest cut end within a boundary. This is used for 
    /// deciding where the disc can fly to.
    /// </summary>
    /// <param name="pitchCoords"></param>
    /// <param name="selectionMaxDistance"></param>
    /// <param name="selectionMinDistance"></param>
    /// <returns></returns>
    public LinearMovement GetClosestDiscFlightPoint(PointF pitchCoords,
                                                    float selectionMaxDistance,
                                                    float selectionMinDistance = -1.0f)
    {
      LinearMovement closestCut = null;
      float closestDistance = selectionMaxDistance;

      foreach (List<LinearMovement> playerCuts in PlayerMovement.Values)
      {
        LinearMovement lastCut = playerCuts.Last();
        float distance = GeometryUtils.DistBetweenPoints(lastCut.FinalPosition,
                                                         pitchCoords);

        if (distance < closestDistance && distance > selectionMinDistance)
        {
          closestCut = lastCut;
          closestDistance = distance;
        }
      }

      return closestCut;
    }

    /// <summary>
    /// Returns the closest trigger to the given location with a given selection radius.
    /// </summary>
    /// <param name="pitchLocation"></param>
    /// <param name="maxSelectionDistance"></param>
    /// <returns>null if no trigger found in given radius.</returns>
    public Trigger GetClosestTrigger(PointF pitchLocation, float maxSelectionDistance)
    {
      float closestDistance = maxSelectionDistance;
      Trigger closestTrigger = null;

      foreach (Trigger trigger in Triggers)
      {
        float distance = GeometryUtils.DistBetweenPoints(pitchLocation,
                                                         trigger.CausingCutRatio.GetAbsolutePosition());

        if (distance < closestDistance)
        {
          closestTrigger = trigger;
          closestDistance = distance;
        }
      }

      return closestTrigger;
    }

    /// <summary>
    /// Remove a single player from the frame including anything attached to 
    /// the player
    /// </summary>
    /// <param name="player"></param>
    public void RemovePlayer(Player player)
    {
      // If the player being removed was the one with the disc then we need
      // to remove the disc movement completely.
      //
      // If they were the one receiving the disc then we need to reset the
      // disc movement.
      if (this.DiscFrameMovement.Thrower != null && 
          this.DiscFrameMovement.Thrower.Equals(player))
      {
        DiscFrameMovement = new DiscMovement(this);
      }
      else if (DiscFrameMovement.ReceivingCut != null &&
               DiscFrameMovement.ReceivingCut.Player.Equals(player))
      {
        DiscFrameMovement.ReceivingCut = null;
        DiscFrameMovement.HasMoved = false;
      }

      // Delete any triggers which reference this player at all
      Triggers.RemoveAll(trigger => trigger.AffectedPlayer == player ||
                                    trigger.CausingCutRatio.Player == player);

      PlayerMovement.Remove(player);
    }

    /// <summary>
    /// Retrieves a list of all the players on the red team.
    /// 
    /// Used to bind to the red team grid view in the UI.
    /// </summary>
    public List<Player> RedTeamPlayers
    {
      get
      {
        return PlayerMovement.Keys.Where(player => player.PlayerTeam == Team.RED_TEAM).ToList<Player>();
      }
    }

    /// <summary>
    /// Retrieves a list of all the players on the blue team.
    /// 
    /// Used to bind to the blue team grid view in the UI.
    /// </summary>
    public List<Player> BlueTeamPlayers
    {
      get
      {
        return PlayerMovement.Keys.Where(player => player.PlayerTeam == Team.BLUE_TEAM).ToList<Player>();
      }
    }

    /// <summary>
    /// Called whenever we want to remove a specified trigger from the
    /// frame.
    /// 
    /// Noop if the trigger didn't exist.
    /// </summary>
    /// <param name="trigger">Can be null.</param>
    internal void RemoveTrigger(Trigger trigger)
    {
      if (Triggers.Contains(trigger))
      {
        Triggers.Remove(trigger);
      }
    }

    /// <summary>
    /// Removes all of the cuts from a given starting location and a given 
    /// player. Also removes and triggers or disc movement to those cuts.
    /// </summary>
    /// <param name="clickedPlayer"></param>
    /// <param name="clickedCut">This cut will be removed as well.</param>
    internal void ClearCuts(Player clickedPlayer, 
                            LinearMovement clickedCut = null)
    {
      int totalCuts;
      int index;

      if (clickedCut == null)
      {
        // -1 because the first in the list is the starting location.
        index = 1;
      }
      else
      {
        index = PlayerMovement[clickedPlayer].IndexOf(clickedCut);
      }

      totalCuts = PlayerMovement[clickedPlayer].Count - index;
      
      // First we remove anything attached to the cuts that we're about to 
      // delete and then we delete the cuts themselves.
      PlayerMovement[clickedPlayer].GetRange(index, totalCuts).ForEach(ClearSingleCut);
      PlayerMovement[clickedPlayer].RemoveRange(index, totalCuts);
    }

    /// <summary>
    /// Thsi function is used to clear a single cut of all attached elements.
    /// </summary>
    /// <param name="cut"></param>
    private void ClearSingleCut(LinearMovement cut)
    {
      Triggers.RemoveAll(trigger => trigger.CausingCutRatio.CausingCut == cut);

      if (DiscFrameMovement.ReceivingCut == cut)
      {
        ClearDiscFlightPath();
      }
    }

    /// <summary>
    /// Call to remove the disc flight path without removing the disc itself.
    /// </summary>
    internal void ClearDiscFlightPath()
    {
      if (DiscFrameMovement.HasMoved)
      {
        DiscFrameMovement.ClearFlightPath();
      }
    }

    /// <summary>
    /// Used to determine whether the current location can be used to start
    /// a cut. 
    /// </summary>
    /// <param name="pitchCoords"></param>
    /// <param name="selectionMaxDistance"></param>
    /// <param name="selectionMinDistance"></param>
    /// <returns></returns>
    internal Boolean CanDrawCut(PointF pitchCoords, 
                                float selectionMaxDistance, 
                                float selectionMinDistance)
    {
      // Only count players who haven't moved.
      foreach (Player player in PlayerMovement.Keys)
      {
        if (PlayerMovement[player].Count == 1)
        {
          PointF startLocation = PlayerMovement[player][0].FinalPosition;

          float distance = GeometryUtils.DistBetweenPoints(startLocation, 
                                                           pitchCoords);
          if (distance < selectionMaxDistance && distance > selectionMinDistance)
          {
            return true;
          }
        }
        else
        {
          LinearMovement lastCut = PlayerMovement[player].Last();
          float distance = GeometryUtils.DistBetweenPoints(pitchCoords,
                                                           lastCut.FinalPosition);

          if (distance < selectionMaxDistance && 
              distance > selectionMinDistance &&
              lastCut != DiscFrameMovement.ReceivingCut)
          {
            return true;
          }
        }
      }

      return false;
    }

    /// <summary>
    /// Called when we want to know whether the cursor is close enough to a
    /// movable item to move it. This is tested for all movable items on the
    /// screen and is called each time the mouse moves (to check for changing
    /// the cursor) so it must be relatively efficient!
    /// </summary>
    /// <param name="pitchCoords"></param>
    /// <param name="selectionMaxDistance"></param>
    /// <param name="selectionMinDistance"></param>
    /// <returns></returns>
    internal Boolean CanMoveItem(PointF pitchCoords,
                                 float selectionMaxDistance,
                                 float selectionMinDistance)
    {
      if (IsNearControlPoint(pitchCoords, selectionMaxDistance))
      {
        return true;
      }

      foreach (Player player in PlayerMovement.Keys)
      {
        foreach (LinearMovement cut in PlayerMovement[player])
        {
          float distance = GeometryUtils.DistBetweenPoints(cut.FinalPosition, 
                                                           pitchCoords);
          
          if (distance <= selectionMaxDistance && distance >= selectionMinDistance)
          {
            return true;
          }
        }
      }

      foreach (Trigger trigger in Triggers)
      {
        float distance = GeometryUtils.DistBetweenPoints(
                  trigger.CausingCutRatio.GetAbsolutePosition(), pitchCoords);

        if (distance <= selectionMaxDistance && distance >= selectionMinDistance)
        {
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Adds the disc to the frame on a given player. Sets all of the receiving
    /// elements to null so that the disc does not move even if it wasn't 
    /// deleted properly previously.
    /// </summary>
    /// <param name="player"></param>
    public void AddDisc(Player player)
    {
      DiscFrameMovement.Thrower = player;
      DiscFrameMovement.ClearFlightPath();
    }

    /// <summary>
    /// Takes a cut and replaces the element in the player movement 
    /// dictionary with the final position that the cut gets to.
    /// </summary>
    /// <param name="cut"></param>
    /// <param name="pitchCoords">New location in pitch coordinates</param>
    public void ReplaceCut(LinearMovement cut, PointF pitchCoords)
    {
      foreach (List<LinearMovement> playerCuts in PlayerMovement.Values)
      {
        if (playerCuts.Contains(cut))
        {
          playerCuts[playerCuts.IndexOf(cut)].FinalPosition = pitchCoords;
        }
      }
    }

    #region "Retrieve items by id"

    /// <summary>
    /// Find a player by their id. This is used so that we can track players
    /// across deep copies of the play model.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>null if the id could not be found.</returns>
    public Player GetPlayerById(long id)
    {
      foreach (Player player in PlayerMovement.Keys)
      {
        if (id == player.UniqueId)
        {
          return player;
        }
      }

      return null;
    }

    /// <summary>
    /// Retrieves a cut from the play model by unique id. This is used so that
    /// we can track cuts across deep copies of the play model.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public LinearMovement GetCutById(long id)
    {
      foreach (List<LinearMovement> playerCuts in PlayerMovement.Values)
      {
        foreach (LinearMovement cut in playerCuts)
        {
          if (cut.UniqueId == id)
          {
            return cut;
          }
        }
      }

      return null;
    }

    /// <summary>
    /// Retrieves a trigger from the play model by unique id. This is used so 
    /// that we can track triggers across deep copies of the play model.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Trigger GetTriggerById(long id)
    {
      foreach (Trigger trigger in Triggers)
      {
        if (id == trigger.UniqueId)
        {
          return trigger;
        }
      }

      return null;
    }

    #endregion

    /// <summary>
    /// Called whenever we need to clear the disc. 
    /// </summary>
    internal void RemoveDisc()
    {
      DiscFrameMovement.Thrower = null;
      DiscFrameMovement.ReceivingCut = null;
      DiscFrameMovement.HasMoved = false;
    }

    /// <summary>
    /// Checks whether the given position is on the disc flight path.
    /// </summary>
    /// <param name="pitchCoordinates"></param>
    /// <param name="maxSelectionDistance"></param>
    /// <returns></returns>
    internal bool IsOnFlightPath(PointF pitchCoordinates, 
                                 float maxSelectionDistance)
    {
      if (DiscFrameMovement.HasMoved)
      {
        return (DiscFrameMovement.DistFromFlightPath(pitchCoordinates) <
                maxSelectionDistance);
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// Reset the frame by clearing all values. 
    /// </summary>
    internal void Reset()
    {
      Notes = "";
      Triggers.Clear();
      DiscFrameMovement.Clear();
      PlayerMovement.Clear();
    }

    /// <summary>
    /// Used to determine if the mouse coordinates are on the disc control
    /// point.
    /// </summary>
    /// <param name="pitchCoords"></param>
    /// <param name="maxDistance"></param>
    /// <returns></returns>
    internal bool IsNearControlPoint(PointF pitchCoords, float maxDistance)
    {
      if (DiscFrameMovement.HasMoved)
      {
        return DiscFrameMovement.DistFromControlPoint(pitchCoords) < maxDistance;
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// Checks whether a given player is the thrower.
    /// </summary>
    /// <param name="player">Any player object.</param>
    /// <returns>True if the player is the thrower. False is no thrower or 
    /// player is not the thrower</returns>
    internal bool IsThrower(Player player)
    {
      return (DiscFrameMovement.Thrower == player);
    }
  }
}
