using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationExample.Models
{
	public class ClientViewModel
	{
		[Required]
		[MinLength(10)]
		[MaxLength(100)]
		[Display(Name = "Enter Client Id:")]
		public string ClientId { get; set; }

		//See here for list of answers
		public string Name { get; set; }
	}
}
