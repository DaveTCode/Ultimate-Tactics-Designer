using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateTacticsDesigner.DataModel;

namespace UltimateTacticsDesigner.Designer
{
  [Serializable]
  class ModelHistoryItem
  {
    public PlayModel model;

    public ModelHistoryItem(PlayModel model)
    {
      this.model = model;
    }
  }

  /// <summary>
  /// Encapsulates the history of the play model from the start of the 
  /// designer to the end.
  /// 
  /// Default configuration is to have maximum 100 items in the history.
  /// This can be changed by passing the optional parameter maxItems into
  /// the constructor.
  /// </summary>
  class ModelHistory
  {
    private const int DEFAULT_MAX_ITEMS = 100;

    private List<ModelHistoryItem> mHistory = new List<ModelHistoryItem>();
    private int mActionIndex;
    private int mMaxItems;

    public ModelHistory(int maxItems = DEFAULT_MAX_ITEMS)
    {
      mActionIndex = -1;
      mMaxItems = maxItems;
    }

    /// <summary>
    /// Called whenever the entire history needs to be emptied. In particular
    /// this is called whenever the model is swapped out for another.
    /// </summary>
    public void Clear()
    {
      mHistory.Clear();
      mActionIndex = -1;
    }

    /// <summary>
    /// Clones the play model post change.
    /// 
    /// This removes any element which is currrently undone from the list
    /// completely.
    /// </summary>
    /// <param name="playModel">A deep copy clone of the play model.</param>
    public void ModelChange(PlayModel playModel)
    {
      mHistory.RemoveRange(mActionIndex + 1, mHistory.Count - mActionIndex - 1);
      mHistory.Add(new ModelHistoryItem(playModel.Clone()));
      mActionIndex++;

      if (mHistory.Count > mMaxItems)
      {
        mHistory.RemoveAt(0);
      }
    }

    /// <summary>
    /// Undo the last action that was performed on the play model.
    /// </summary>
    /// <param name="clearUndoneModel">Set to false if you want this action to
    /// be able to redone.</param>
    public ModelHistoryItem Undo(Boolean clearUndoneModel)
    {
      if (CanUndo())
      {
        if (clearUndoneModel)
        {
          mHistory.RemoveAt(mActionIndex);
        }

        mActionIndex--;
        return mHistory[mActionIndex].Clone();
      }

      return null;
    }

    /// <summary>
    /// Redo the last action that was undone on the play model.
    /// </summary>
    public ModelHistoryItem Redo()
    {
      if (CanRedo())
      {
        mActionIndex++;
        return mHistory[mActionIndex].Clone();
      }

      return null;
    }

    /// <summary>
    /// True if there are changes to undo. False otherwise.
    /// </summary>
    /// <returns></returns>
    internal bool CanUndo()
    {
      return (mActionIndex > 0);
    }

    /// <summary>
    /// True if there are changes to redo and false otherwise.
    /// </summary>
    /// <returns></returns>
    internal bool CanRedo()
    {
      return (mActionIndex < mHistory.Count - 1);
    }
  }
}
