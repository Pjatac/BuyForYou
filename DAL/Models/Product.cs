using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DAL.Models
{
	public class Product
	{
		public Product()
		{
			_defaultPicPath = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "default.png");
		}

		private string _defaultPicPath;

		[System.ComponentModel.DataAnnotations.ScaffoldColumn(false)]
		public long Id { get; set; }
		[System.ComponentModel.DataAnnotations.ScaffoldColumn(false)]
		public long OwnerId { get; set; } = 1;
		[System.ComponentModel.DataAnnotations.ScaffoldColumn(false)]
		public Nullable<long> UserId { get; set; }
		[DisplayName("Title")]
		[StringLength(50)]
		[Required(ErrorMessage = "Input title!!!")]
		public string Title { get; set; }
		[DisplayName("Short decription")]
		[StringLength(500)]
		[Required(ErrorMessage = "Input short description!!!")]
		public string ShortDescription { get; set; }
		[DisplayName("Long decription")]
		[StringLength(4000)]
		[Required(ErrorMessage = "Input longt description!!!")]
		public string LongDescription { get; set; }
		[System.ComponentModel.DataAnnotations.ScaffoldColumn(false)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd-hh}")]
		public System.DateTime DateCard { get; set; } = DateTime.Now;
		[System.ComponentModel.DataAnnotations.ScaffoldColumn(false)]
		public Nullable<System.DateTime> DateBay { get; set; }
		[DisplayName("Price")]
		[Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
		[Required(ErrorMessage = "Input price!!!")]
		public decimal Price { get; set; }

		private byte[] _picture1;
		private byte[] _picture2;
		private byte[] _picture3;

		[DisplayName("Picture 1")]
		public byte[] Picture1
		{
			get
			{
				if (_picture1 == null)
				{
					string path = _defaultPicPath;
					_picture1 = File.ReadAllBytes(path);
				}
				return _picture1;
			}
			set
			{
				_picture1 = value;
			}
		}
		[DisplayName("Picture 2")]
		public byte[] Picture2
		{
			get
			{
				if (_picture2 == null)
				{
					string path = _defaultPicPath;
					_picture2 = File.ReadAllBytes(path);
				}
				return _picture2;
			}
			set
			{
				_picture2 = value;
			}
		}
		[DisplayName("Picture 3")]
		public byte[] Picture3
		{
			get
			{
				if (_picture3 == null)
				{
					string path = _defaultPicPath;
					_picture3 = File.ReadAllBytes(path);
				}
				return _picture3;
			}
			set
			{
				_picture3 = value;
			}
		}
		[System.ComponentModel.DataAnnotations.ScaffoldColumn(false)]
		public int Sold { get; set; } = 0;

		public virtual Client Clients { get; set; }
		public virtual Client Clients1 { get; set; }

		public void SaleProduct()
		{
			Sold = 2;
			DateBay = DateTime.Now;
		}
		public void ProductReturn()
		{
			Sold = 0;
			UserId = null;
			DateBay = null;
		}
	}
}

