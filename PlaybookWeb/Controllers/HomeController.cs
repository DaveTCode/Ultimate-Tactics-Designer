using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlaybookWeb.Controllers
{
  [HandleError]
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult About()
    {
      return View();
    }

    public ActionResult Download()
    {
      if (Request.QueryString.Get("installer") == "0")
      {
        return File("/Content/UltimateTacticsDesigner.zip", "application/zip", "Playbook.zip");
      }
      else
      {
        return File("/Content/UltimateTacticsDesigner.zip", "application/zip", "Playbook.zip");
      }
    }

    public ActionResult Faq()
    {
      return View();
    }

    public ActionResult Donate()
    {
      return View();
    }

    public ActionResult Version()
    {
      return Content("1.0.0");
    }
  }
}
