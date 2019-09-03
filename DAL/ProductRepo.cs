using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DAL
{
	public class ProductRepo
	{
		public IEnumerable<Product> GetOwnProducts(long id)
		{
			using (var dbContext = new BFUContext())
			{
				return dbContext.Products.ToList().FindAll(pr => pr.OwnerId == id);
			}
		}
		public IEnumerable<Product> GetForeignProducts(long id)
		{
			using (var dbContext = new BFUContext())
			{
				return dbContext.Products.ToList().FindAll(pr => pr.OwnerId != id);
			}
		}
		public IEnumerable<Product> GetAllProducts()
		{
			using (var dbContext = new BFUContext())
			{
				return dbContext.Products.ToList();
			}
		}
		public void AddProduct(Product product)
		{
			using (var dbContext = new BFUContext())
			{
				dbContext.Products.Add(product);
				dbContext.SaveChanges();
			}
		}
		public void ChangeProduct(Product product)
		{
			using (var dbContext = new BFUContext())
			{
				Product pr = FindProduct(product.Id);
				pr = product;
				dbContext.Entry(pr).State = EntityState.Modified;
				dbContext.SaveChanges();
			}
		}
		public void DeleteAllProducts()
		{
			using (var dbContext = new BFUContext())
			{
				foreach (var p in dbContext.Products)
				{
					dbContext.Products.Remove(p);
				}
				dbContext.SaveChanges();
			}
		}
		public void DeleteProduct(long id)
		{
			using (var dbContext = new BFUContext())
			{
				var pr = FindProduct(id);
				dbContext.Entry(pr).State = EntityState.Modified;
				dbContext.Products.Remove(pr);
				dbContext.SaveChanges();
			}
		}
		public Product FindProduct(long id)
		{
			using (var dbContext = new BFUContext())
			{
				return dbContext.Products.FirstOrDefault(p => p.Id == id);
			}
		}
		public Product PutToCart(long prodId, long userId)
		{
			using (var dbContext = new BFUContext())
			{
				Product pr = FindProduct(prodId);
				pr.Sold = 1;
				pr.DateBay = DateTime.Now;
				if (userId != 0)
				{
					pr.UserId = userId;
				}
				dbContext.Entry(pr).State = EntityState.Modified;
				dbContext.SaveChanges();
				return pr;
			}
		}
		public void Sale(long prodId)
		{
			using (var dbContext = new BFUContext())
			{
				Product pr = FindProduct(prodId);
				pr.SaleProduct();
				dbContext.Entry(pr).State = EntityState.Modified;
				dbContext.SaveChanges();
			}
		}
		public void TakeFromCart(long id)
		{
			using (var dbContext = new BFUContext())
			{
				Product pr = FindProduct(id);
				pr.ProductReturn();
				dbContext.Entry(pr).State = EntityState.Modified;
				dbContext.SaveChanges();
			}
		}
		public List<Product> CreateCart(long? id)
		{
			var cartProducts = new List<Product>();
			using (var dbContext = new BFUContext())
			{
				foreach (var p in dbContext.Products)
				{
					if ((p.Sold == 1 && p.UserId == id) || (p.Sold == 1 && p.UserId == null && id == null))
					{
						cartProducts.Add(p);
					}
				}
			}
			return cartProducts;
		}
		public void ClearCart()
		{
			using (var dbContext = new BFUContext())
			{
				foreach (var p in dbContext.Products)
				{
					if ((DateTime.Now - p.DateBay) >= TimeSpan.FromMinutes(1) && p.Sold == 1)
					{
						p.ProductReturn();
						dbContext.Entry(p).State = EntityState.Modified;
					}
				}
				dbContext.SaveChanges();
			}
		}
		public void ReturnToSale()
		{
			using (var dbContext = new BFUContext())
			{
				foreach (var p in dbContext.Products)
				{
					if ((DateTime.Now - p.DateBay) >= TimeSpan.FromMinutes(2) && p.Sold == 2)
					{
						p.ProductReturn();
						dbContext.Entry(p).State = EntityState.Modified;
					}
				}
				dbContext.SaveChanges();
			}
		}
	}
}
