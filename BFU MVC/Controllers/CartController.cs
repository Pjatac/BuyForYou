using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BFU_MVC.Controllers
{
	public class CartController: Controller
	{
		ProductRepo pr = new ProductRepo();
		public ActionResult AddToCart(long Id)
		{
			long id = 0;
			var auth = Request.Cookies["Auth"];
			if (auth != null)
			{
				long.TryParse(auth.Values["UserID"], out id);
				Product product = pr.FindProduct(Id);
				GetCart().AddItem(pr.PutToCart(product.Id, id));
				return RedirectToAction("Index", "Home");
			}
			else
			{
				Product product = pr.FindProduct(Id);
				GetCart().AddItem(pr.PutToCart(product.Id, id));
				return RedirectToAction("Index", "Home");
			}

		}

		public ActionResult RemoveFromCart(long Id)
		{
			pr.TakeFromCart(Id);
			GetCart().RemoveLine(pr.FindProduct(Id));
			return View("ShowCart", GetCart());
		}

		public Cart GetCart()
		{
			Cart cart = (Cart)Session["Cart"];
			if (cart == null || cart.GetCount() == 0)
			{
				cart = new Cart();
				List<Product> prList = new List<Product>();
				long id = 0;
				var auth = Request.Cookies["Auth"];
				if (auth != null)
				{
					long.TryParse(auth.Values["UserID"], out id);
					prList = pr.CreateCart(id);
				}
				else
				{
					prList = pr.CreateCart(null);
				}
				foreach (var p in prList)
				{
					cart.AddItem(p);
				}
				Session["Cart"] = cart;
			}
			return cart;
		}
		public PartialViewResult ShowCart()
		{
			return PartialView("ShowCart", GetCart());
		}
		[HttpPost]
		public void SetToSale(string id)
		{
			long looking = Convert.ToInt64(id);
			((Cart)Session["Cart"]).Lines.FirstOrDefault(l => l.Product.Id == looking).Tosale = true;
		}
		[HttpPost]
		public void SetNotSale(string id)
		{
			long looking = Convert.ToInt64(id);
			((Cart)Session["Cart"]).Lines.FirstOrDefault(l => l.Product.Id == looking).Tosale = false;
		}
		public PartialViewResult Buy()
		{
			Cart cart = (Cart)Session["Cart"];
			List<Product> sale = new List<Product>();
			foreach (var line in cart.Lines)
			{
				if (line.Tosale)
				{
					sale.Add(line.Product);
					pr.Sale(line.Product.Id);
				}
			}
			foreach (var prod in sale)
			{
				cart.RemoveLine(prod);
			}
			return PartialView("ShowCart", GetCart());
		}
	}
}