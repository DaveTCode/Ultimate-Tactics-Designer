using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlaybookWeb.Models
{
  public class Team
  {
    public int Id { get; set; }
    public String Name { get; set; }
    public List<PlayGroup> PlayGroups { get; set; }

    public Team(int id, String name, List<PlayGroup> playGroups)
    {
      Id = id;
      Name = name;
      PlayGroups = playGroups;
    }
  }

  public class Play
  {
    public int Id { get; set; }
    public String Name { get; set; }
    public String Xml { get; set; }
    public String FormattedText { get; set; }

    public Play(int id, String name, String xml, String text)
    {
      Id = id;
      Name = name;
      Xml = xml;
      FormattedText = text;
    }
  }

  public class PlayGroup
  {
    public int Id { get; set; }
    public String Name { get; set; }
    public List<Play> Plays { get; set; }

    public PlayGroup(int id, String name, List<Play> plays)
    {
      Id = id;
      Name = name;
      Plays = plays;
    }
  }
}