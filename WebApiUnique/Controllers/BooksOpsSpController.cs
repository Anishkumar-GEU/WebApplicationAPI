using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiUnique.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiUnique.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksOpsSpController : ControllerBase
    {
        Appdb _db;
        public BooksOpsSpController()
        {
            _db = new Appdb();
        }
        [HttpGet]
        public IActionResult GetBooks()
        {
            string query = "CALL select_books()";
            var res = _db.Books.FromSqlRaw(query).ToList();
            return Ok(res);
        }

        [HttpPost]
        public IActionResult InsertBooks(Books book)
        {
            string query = "CALL insert_book(@Name, @Author, @Price)";

            MySqlParameter[] sqlpara =
            {
                new MySqlParameter("@Name", book.Name),
                new MySqlParameter("@Author", book.Author),
                new MySqlParameter("@Price", book.price)
            };

            book.Id = Convert.ToInt32(
                _db.Database.SqlQueryRaw<int>(query, sqlpara)
                .AsEnumerable().First()
            );
            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
        }
        
    }
}

