using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetLib.Models;
using NetLib.ViewModels;
using static System.Reflection.Metadata.BlobBuilder;

namespace NetLib.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public BookController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index(string? search)
        {
            
            try
            {
                BookViewModel model;

                if (string.IsNullOrEmpty(search))
                {
                    var books = await _dbContext.Books
                        .Include(b => b.BorrowRecords)
                        .AsNoTracking()
                        .ToListAsync<Book>();
                    model = new BookViewModel
                    {
                        Books = books
                    };
                    return View(model);
                }

                var searchBooks = await _dbContext.Books
                    .Include(b => b.BorrowRecords)
                    .AsNoTracking()
                    .Where<Book>(b => b.Title.Contains(search))
                    .ToListAsync();
                model = new BookViewModel
                {
                    Books = searchBooks,
                };
                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "An error occured while loading the books";
                return View("Error");
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null || id == 0)
            {
                TempData["ErrorMessage"] = "Book ID was not provided.";
                return View("NotFound");
            }
            
            try
            {
                var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Book_Id == id);
                if (book == null)
                {
                    TempData["ErrorMessage"] = $"No book found with ID {id}.";
                    return View("NotFound");
                }
                return View(book);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "An error occured while loading the book";
                return View("Error");
            }
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Books.Add(book);
                await _dbContext.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Successfully added the book: {book.Title}.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while adding the book.";
                return View(book);
            }
        }
        
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                TempData["ErrorMessage"] = "Book ID was not provided for editing.";
                return View("NotFound");
            }
           
            try
            {
                var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Book_Id == id);
                if (book == null)
                {
                    TempData["ErrorMessage"] = $"No book found with ID {id} for editing.";
                    return View("NotFound");
                }
                return View(book);
            }
            catch(Exception e)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the book for editing.";
                return View("Error");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Book book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingBook = await _dbContext.Books.FindAsync(id);
                    if (existingBook == null)
                    {
                        TempData["ErrorMessage"] = $"No book found with ID {id} for updating.";
                        return View("NotFound");
                    }

                    existingBook.Title = book.Title;
                    existingBook.Author = book.Author;
                    existingBook.ISBN = book.ISBN;
                    existingBook.PublishedDate = book.PublishedDate;

                    //_dbContext.Books.Update(book);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while editing the book.";
                    return View(book);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_dbContext.Books.Any(b => b.Book_Id == id))
                {
                    TempData["ErrorMessage"] = $"No book found with ID {book.Book_Id} during concurrency check.";
                    return View("NotFound");
                }
                else
                {
                    TempData["ErrorMessage"] = "A concurrency error occurred during the update.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the book.";
                return View("Error");
            }
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null || id == 0)
            {
                TempData["ErrorMessage"] = "Book ID was not provided for deletion.";
                return View("NotFound");
            }

            try
            {
                var book = await _dbContext.Books
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Book_Id == id);

                if (book == null)
                {
                    TempData["ErrorMessage"] = $"No book found with ID {id} for deletion.";
                    return View("NotFound");
                }

                return View(book);
            }
            catch(Exception e)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the book for deletion.";
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if(id == null || id == 0)
            {
                TempData["ErrorMessage"] = "Book ID was not provided for deletion.";
                return View("NotFound");
            }

            try
            {
                var book = await _dbContext.Books.FindAsync(id);
                if (book == null)
                {
                    TempData["ErrorMessage"] = $"No book found with ID {id} for deletion.";
                    return View("NotFound");
                }
                _dbContext.Books.Remove(book);
                await _dbContext.SaveChangesAsync();
                return View(book);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the book for deletion.";
                return View("Error");
            }
        }
    }
}
