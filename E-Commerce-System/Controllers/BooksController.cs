using E_Commerce_System.Models;
using E_Commerce_System.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;

namespace E_Commerce_System.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly IBookNeo4jService _bookNeo4JService;

        public BooksController(BookService bookService, IBookNeo4jService bookNeo4JService)
        {
            _bookService = bookService;
            _bookNeo4JService = bookNeo4JService;
        }

        [HttpGet]
        public ActionResult<List<Book>> Get() =>
            _bookService.Get();

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<Book> Get(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {
            _bookService.Create(book);

            return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Book bookIn)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Update(id, bookIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Remove(book.Id);

            return NoContent();
        }

        [HttpGet]
        [Route("/A")]
        public Task<List<string>> Test()
        {
            try
            {
                var res = _bookNeo4JService.Get();
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }

}
