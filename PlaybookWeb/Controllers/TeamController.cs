using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PlaybookWeb.Models;
using System.IO;

namespace PlaybookWeb.Controllers
{
  public class TeamController : Controller
  {
    //
    // GET: /Team/

    public ActionResult Index()
    {
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public ActionResult ViewPlays()
    {
      String team = User.Identity.Name;
      List<PlayGroup> pgList = DataManager.GetPlaysForTeam(team);

      return View(new Team(1, team, pgList));
    }

    [Authorize]
    public ActionResult Play()
    {
      try
      {
        int playId = int.Parse(Request.Params.Get("PlayId"));

        String playData = DataManager.GetPlayData(playId, User.Identity.Name);

        return Content(playData, "text/xml");
      }
      catch (Exception e)
      {
        return Content("", "text/xml");
      }
    }

    [Authorize]
    public ActionResult CreatePlay(int playGroupId, 
                                   String playName, 
                                   HttpPostedFileBase playData,
                                   String playText)
    {
      try
      {
        StreamReader reader = new StreamReader(playData.InputStream);
        String content = reader.ReadToEnd();

        DataManager.CreatePlay(playName,
                               content,
                               playGroupId,
                               playText,
                               User.Identity.Name);
      }
      catch (Exception e)
      {
        // Noop
      }

      return RedirectToAction("ViewPlays");
    }

    [Authorize]
    public ActionResult CreatePlayGroup(String playGroupName)
    {
      // Also make sure that the group name is not equal to the watermark.
      if (playGroupName != "" && playGroupName != "New group name")
      {
        try
        {
          DataManager.CreatePlayGroup(playGroupName, User.Identity.Name);
        }
        catch (Exception e)
        {
          //noop
        }
      }

      return RedirectToAction("ViewPlays");
    }

    [Authorize]
    public ActionResult DeletePlay()
    {
      try
      {
        int playId = int.Parse(Request.QueryString.Get("PlayId"));

        DataManager.RemovePlay(playId, User.Identity.Name);
      }
      catch (Exception e)
      {
        //noop
      }

      return RedirectToAction("ViewPlays");
    }
    
    [Authorize]
    public ActionResult DeletePlayGroup()
    {
      try
      {
        int playGroupId = int.Parse(Request.QueryString.Get("PlayGroupId"));

        DataManager.RemoveGroup(playGroupId, User.Identity.Name);
      }
      catch (Exception e)
      {
        //noop
      }

      return RedirectToAction("ViewPlays");
    }

    [Authorize]
    public ActionResult EditPlayGroup(int id, String name)
    {
      try
      {
        DataManager.EditPlayGroup(id, name, User.Identity.Name);
      }
      catch (Exception e)
      {
        //noop
      }

      return RedirectToAction("ViewPlays");
    }

    [Authorize]
    public ActionResult EditPlay(int playId,
                                 int playGroupId, 
                                 String playName, 
                                 HttpPostedFileBase playData,
                                 String playText)
    {
      try
      {
        String content = null;

        if (playData != null)
        {
          StreamReader reader = new StreamReader(playData.InputStream);
          content = reader.ReadToEnd();
        }

        DataManager.EditPlay(playId,
                             playName,
                             content,
                             playGroupId,
                             playText,
                             User.Identity.Name);
      }
      catch (Exception e)
      {
        // Noop
      }

      return RedirectToAction("ViewPlays");
    }
  }
}
