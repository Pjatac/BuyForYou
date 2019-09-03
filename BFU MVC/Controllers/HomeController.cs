using BFU.Models;
using BFU_MVC.Models;
using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BFU_MVC.Controllers
{
    public class HomeController : Controller
    {
		ProductRepo pr = new ProductRepo();
		ClientRepo cl = new ClientRepo();
		public ActionResult Index()
        {
			if (TimeManager.Instance.CheckTime())
			{
				if (Session["Cart"] != null)
				{
					Cart cart = (Cart)Session["Cart"];
					cart.Clear();
					Session["Cart"] = cart;
				}
			}
			return View();
        }
		public PartialViewResult ShowProducts(int page = 1)
		{
			int pageSize = 12;
			IEnumerable<Product> products;
			IEnumerable<Product> productsPerPages;
			PageInfo pageInfo;
			IndexViewModel ivm;
			if (Request.Cookies["Auth"] == null)
			{
				products = pr.GetAllProducts();
			}
			else
			{
				long uid = Convert.ToInt64(Request.Cookies["Auth"].Values["UserID"]);
				products = pr.GetForeignProducts(uid);
			}
			products = products.Where(p => p.Sold == 0);
			Cart cart = (Cart)Session["cart"];
			if (Session["Sort"] != null)
			{
				int sort = (int)Session["Sort"];
				switch (sort)
				{
					case 0:
						products = new List<Product>(products.OrderBy(prod => prod.DateCard));
						break;
					case 1:
						products = new List<Product>(products.OrderByDescending(prod => prod.DateCard));
						break;
					case 2:
						products = new List<Product>(products.OrderBy(prod => prod.Price));
						break;
					case 3:
						products = new List<Product>(products.OrderByDescending(prod => prod.Price));
						break;
					case 4:
						products = new List<Product>(products.OrderBy(prod => prod.Title));
						break;
					case 5:
						products = new List<Product>(products.OrderByDescending(prod => prod.Title));
						break;
				}
			}

			productsPerPages = products.Skip((page - 1) * pageSize).Take(pageSize);
			pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = products.ToList().Count };
			ivm = new IndexViewModel { PageInfo = pageInfo, Products = productsPerPages };
			return PartialView("ShowProducts", ivm);
		}
		public PartialViewResult ShowDetails(long id)
		{
			var product = pr.FindProduct(id);
			return PartialView("ShowDetails", product);
		}
		public PartialViewResult ShowShort(Product p)
		{
			return PartialView(p);
		}
		public PartialViewResult Navigate()
		{
			return PartialView();
		}
		public PartialViewResult About()
		{
			return PartialView();
		}
		public void SetSortSelect(string id)
		{
			Int32.TryParse(id, out int sort);
			Session["Sort"] = sort;
		}
		[HttpGet]
		public ActionResult AddProduct(long? id)
		{
			if (Request.Cookies["Auth"] != null)
			{
				if (id == null)
				{
					return View();
				}
				else
				{
					Product product = pr.FindProduct((long)id);
					return View(product);
				}
			}
			return JavaScript("(alert('You must be a registred user!'))");
		}
		[HttpPost]
		public PartialViewResult AddProduct(Product product, HttpPostedFileBase uploadImage1, HttpPostedFileBase uploadImage2, HttpPostedFileBase uploadImage3)
		{
			ClientRepo cl = new ClientRepo();
			if (ModelState.IsValid)
			{
				if (uploadImage1 != null)
				{
					byte[] imageData1 = null;
					using (var binaryReader = new BinaryReader(uploadImage1.InputStream))
					{
						imageData1 = binaryReader.ReadBytes(uploadImage1.ContentLength);
					}
					product.Picture1 = imageData1;
				}
				if (uploadImage2 != null)
				{
					byte[] imageData2 = null;
					using (var binaryReader = new BinaryReader(uploadImage2.InputStream))
					{
						imageData2 = binaryReader.ReadBytes(uploadImage2.ContentLength);
					}
					product.Picture2 = imageData2;
				}
				if (uploadImage3 != null)
				{
					byte[] imageData3 = null;
					using (var binaryReader = new BinaryReader(uploadImage3.InputStream))
					{
						imageData3 = binaryReader.ReadBytes(uploadImage3.ContentLength);
					}
					product.Picture3 = imageData3;
				}
				HttpCookie cookieReq = Request.Cookies["Auth"];
				long id = Convert.ToInt64(cookieReq["UserID"]);
				product.OwnerId = cl.FindClient(id).Id;
				pr.AddProduct(product);
				return PartialView("ShowOwnProducts");
			}
			else
			{
				return PartialView();
			}
		}
		[HttpGet]
		public ActionResult AddClient(long? id)
		{
			if (id == null)
			{
				return View();
			}
			else
			{
				Client client = cl.FindClient((long)id);
				return View(client);
			}
		}
		[HttpPost]
		public ActionResult AddClient(Client client)
		{
			if (ModelState.IsValid)
			{
				ClientRepo cl = new ClientRepo();
				if (cl.FindClient((long)client.Id) != null)
				{
					cl.ChangeClient(client);
					return RedirectToAction("Index", "Home");
				}
				else
				{
					if (cl.CheckUsername(client.UserName))
					{
						ViewBag.Message = "Sorry, this nickname already in use";
						return PartialView();
					}
					else
					{
						cl.AddClient(client);
						client = cl.FindClient(client.UserName);
						HttpCookie cookie = new HttpCookie("Auth");
						cookie["AuthUser"] = client.UserName;
						cookie["UserID"] = client.Id.ToString();
						cookie.Expires = DateTime.Now.AddDays(1);
						Response.Cookies.Add(cookie);
						Session.Clear();
						return RedirectToAction("Index", "Home");
					}
				}
			}
			else
			{
				return PartialView();
			}
		}
		public ActionResult ShowOwnProducts()
		{
			if (Request.Cookies["Auth"] != null)
			{
				long id = Convert.ToInt64(Request.Cookies["Auth"].Values["UserID"]);
				var products = pr.GetOwnProducts(id);
				return View("ShowOwnProducts", products);
			}
			return JavaScript("alert('You must register!')");
		}
		public ActionResult DelProduct(long id)
		{
			long uid = pr.FindProduct(id).OwnerId;
			pr.DeleteProduct(id);
			var products = pr.GetOwnProducts(uid);
			return View("ShowOwnProducts", products);
		}
	}
}