using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public partial class Client
	{
		[System.ComponentModel.DataAnnotations.ScaffoldColumn(false)]
		public long Id { get; set; }
		[DisplayName("Name")]
		[StringLength(50)]
		[Required(ErrorMessage = "Input name!!!")]
		public string FirstName { get; set; }
		[DisplayName("Surname")]
		[StringLength(50)]
		[Required(ErrorMessage = "Input surname!!!")]
		public string LastName { get; set; }
		[DisplayName("Birthday")]
		[AgeAttribute]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		[Required(ErrorMessage = "Input date of birthday!!!")]
		public DateTime BirthDate { get; set; }
		[DisplayName("E-mail")]
		[StringLength(50)]
		[RegularExpression(".+\\@.+\\..+")]
		[Required(ErrorMessage = "Input e-mail!!!")]
		public string Email { get; set; }
		[DisplayName("Login")]
		[StringLength(50)]
		[Required(ErrorMessage = "Input login!!!")]
		public string UserName { get; set; }
		[DisplayName("Password")]
		[StringLength(50)]
		[Required(ErrorMessage = "Input password!!!")]
		public string Pass { get; set; }
		public virtual ICollection<Product> Products { get; set; }
		public virtual ICollection<Product> Products1 { get; set; }
		public class AgeAttribute : ValidationAttribute
		{
			protected override ValidationResult IsValid(object value, ValidationContext validationContext)
			{
				DateTime dateTime;
				if (value == null)
				{
					return new ValidationResult("Input date of birth!");
				}
				else if (DateTime.TryParse((value).ToString(), out dateTime))
				{
					if (DateTime.Now.AddYears(-120).CompareTo(value) <= 0 && DateTime.Now.AddYears(-15).CompareTo(value) >= 0)
					{
						return ValidationResult.Success;
					}
					else
					{
						return new ValidationResult("Wrong age!");
					}
				}
				return ValidationResult.Success;
			}
		}
	}
}
