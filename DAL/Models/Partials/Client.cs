using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public partial class Client
	{
		[NotMapped]
		[Compare("Pass")]
		public string ConfirmedPassword { get; set; }
	}
}
