using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using UltimateTacticsDesigner.Properties;

namespace UltimateTacticsDesigner.DataModel
{
  [Serializable()]
  class Team
  {
    public static Team BLUE_TEAM = new Team(Resources.blue_team, 
                                            Resources.blue_team_selected,
                                            Resources.blue_team_outline,
                                            "Blue team");
    public static Team RED_TEAM = new Team(Resources.red_team, 
                                           Resources.red_team_selected,
                                           Resources.red_team_outline,
                                           "Red team");
    private static long NextUniqueId = 0;

    public long UniqueId;
    public String Name { get; set; }
    public Image Sprite { get; set; }
    public Image SelectedSprite { get; set; }
    public Image OutlineSprite { get; set; }

    public Team(Image sprite, 
                Image selectedSprite, 
                Image outlineSprite, 
                String name)
    {
      Name = name;
      Sprite = sprite;
      SelectedSprite = selectedSprite;
      OutlineSprite = outlineSprite;
      UniqueId = NextUniqueId++;
    }
  }
}
