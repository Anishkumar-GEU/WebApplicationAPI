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
    public class UsersOpsController : ControllerBase
    {
        public Appdb _db;
        public UsersOpsController()
        {
            _db = new Appdb();
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var res = _db.Users.ToList();
            return Ok(res);
            //200
        }

        [HttpPost]
        public IActionResult InsertUsers( Users data)
        {
            bool isuserexist = _db.Users.Any(x => x.Email == data.Email);
            if (isuserexist)
            {
                return Conflict("User Already Exists");
                //409
            }
            _db.Users.Add(data);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetUsers), new { id = data.Id }, data);
            //201
        }
    }
}

