using System.ComponentModel.DataAnnotations;
namespace WebApiUnique.Models
{
	public class Users
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Dept { get; set; }
	}
}

