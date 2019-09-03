using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BFU_MVC.Models
{
	public class TimeManager
	{
		ProductRepo products;
		private static DateTime Start;
		public DateTime LastCheck;

		public static TimeManager Instance { get; } = new TimeManager();

		private TimeManager()
		{
			products = new ProductRepo();
			Start = DateTime.Now;
			LastCheck = DateTime.Now;
		}

		public bool CheckTime()
		{
			DateTime CheckTime = DateTime.Now;
			if ((CheckTime - LastCheck) >= TimeSpan.FromMinutes(1))
			{
				products.ClearCart();
				products.ReturnToSale();
				return true;
			}
			else return false;
		}
	}
}