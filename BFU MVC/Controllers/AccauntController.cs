using DAL;
using DAL.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace BFU_MVC.Controllers
{
	public class AccauntController : Controller
    {
		[AllowAnonymous]
		public ActionResult Login()
		{
			if (Request.Cookies["Auth"] != null)
			{
				return PartialView("Logon");
			}
			return PartialView();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginModel model)
		{
			if (ModelState.IsValid)
			{
				ClientRepo cl = new ClientRepo();
				Client client = cl.FindClient(model.Name, model.Password);
				if (client != null)
				{
					HttpCookie cookie = new HttpCookie("Auth");
					cookie["AuthUser"] = model.Name;
					cookie["UserID"] = client.Id.ToString();
					cookie.Expires = DateTime.Now.AddDays(1);
					Response.Cookies.Add(cookie);
					Session.Clear();
					return PartialView("Logon");
				}
				else
				{
					return JavaScript("(alert('Unknown user!'))");
				}
			}
			else
			{
				return PartialView();
			}
		}
		public ActionResult Logoff()
		{
			HttpCookie cookie = new HttpCookie("Auth");
			cookie.Expires = DateTime.Now.AddDays(-1);
			Response.Cookies.Add(cookie);
			Session.Clear();
			return RedirectToAction("Index", "Home");
		}
	}
}