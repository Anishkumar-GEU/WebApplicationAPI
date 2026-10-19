using System.ComponentModel.DataAnnotations;
namespace WebApiUnique.Models
{
	public class Books
	{
		[Key]
		public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public double price { get; set; }
    }
}

