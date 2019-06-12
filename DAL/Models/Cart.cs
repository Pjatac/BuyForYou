using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class Cart
	{
		private List<CartLine> lineCollection = new List<CartLine>();

		public void AddItem(Product product)
		{
			lineCollection.Add(new CartLine
			{
				Product = product,
				Tosale = true
			});
		}

		public void RemoveLine(Product product)
		{
			lineCollection.RemoveAll(p => p.Product.Id == product.Id);
		}

		public decimal ComputeTotalValue()
		{
			return lineCollection.Where(e => e.Tosale == true).Sum(e => e.Product.Price);
		}
		public void Clear()
		{
			lineCollection.Clear();
		}
		public int GetCount()
		{
			return lineCollection.Count;
		}
		public IEnumerable<CartLine> Lines
		{
			get { return lineCollection; }
		}
		public void Bay()
		{
			foreach (var line in lineCollection)
			{
				line.Product.SaleProduct();
			}
			Clear();
		}
	}

	public class CartLine
	{
		public Product Product { get; set; }
		public bool Tosale = true;
	}
}
