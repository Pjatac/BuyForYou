﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class LoginModel
	{
		[Required]
		public string Name { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}