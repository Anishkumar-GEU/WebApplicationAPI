using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiUnique.Models;
using Microsoft.EntityFrameworkCore;

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
            /*
            var res = _db.Books.ToList();
            return Ok(res); // this will return 200 status code.
            */
            string query = "CALL select_books()";
            var res = _db.Books.FromSqlRaw(query).ToList();
            return Ok(res);
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

        [HttpPut("{Id}")]
        public IActionResult UpdateBook(int Id, Books book)
        {
            if (Id <= 0)
            {
                return BadRequest(new { msg = "Invalid Book id" });

            }
            if (Id != book.Id)
            {
                return BadRequest(new { msg = "Book id mismatched" });
            }

            var res = _db.Books.FirstOrDefault(x => x.Id == Id);
            res.Name = book.Name;
            res.Author = book.Author;
            res.price = book.price;
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetBooks), new {id = book.Id}, book);
        }
    }
}

