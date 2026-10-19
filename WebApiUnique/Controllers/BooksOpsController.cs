using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiUnique.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiUnique.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksOpsController : ControllerBase
    {
        public Appdb _db;
        public BooksOpsController()
        {
            _db = new Appdb();
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var res = _db.Books.ToList();
            return Ok(res); // this will return 200 status code.
        }

        [HttpPost]
        public IActionResult InsertBooks(Books book)
        {
            bool isbookexists = _db.Books.Any(x => x.Name == book.Name && x.Author == book.Author);
            if (isbookexists)
            {
                return Conflict("Book already present");
            }
            _db.Books.Add(book);
            _db.SaveChanges();
            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteBook(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest(new { msg = "Invalid Book id" });
            }
            var res = _db.Books.FirstOrDefault(book => book.Id == Id);
            if (res != null)
            {
                _db.Books.Remove(res);
                _db.SaveChanges();
            }
            return NoContent();
        }
    }
}

