using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetLib.Models;
using NetLib.ViewModels;

namespace NetLib.Controllers
{
    public class BorrowController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public BorrowController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Create(int? bookId)
        {
            if (bookId == null || bookId == 0)
            {
                TempData["ErrorMessage"] = "Book ID was not provided for borrowing.";
                return View("NotFound");
            }
            try
            {
                var book = await _dbContext.Books.FindAsync(bookId);
                if (book == null)
                {
                    TempData["ErrorMessage"] = $"No book found with ID {bookId} to borrow.";
                    return View("NotFound");
                }
                if (!book.IsAvailable)
                {
                    TempData["ErrorMessage"] = $"The book '{book.Title}' is currently not available for borrowing.";
                    return View("NotAvailable");
                }
                var borrowViewModel = new BorrowViewModel
                {
                    Book_Id = book.Book_Id,
                    BookTitle = book.Title
                };
                return View(borrowViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the borrow form.";
                return View("Error");
            }
        }
        // Processes the borrowing action, creates a BorrowRecord, updates the book's availability
        // POST: Borrow/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BorrowViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var book = await _dbContext.Books.FindAsync(model.Book_Id);
                if (book == null)
                {
                    TempData["ErrorMessage"] = $"No book found with ID {model.Book_Id} to borrow.";
                    return View("NotFound");
                }
                if (!book.IsAvailable)
                {
                    TempData["ErrorMessage"] = $"The book '{book.Title}' is already borrowed.";
                    return View("NotAvailable");
                }
                var borrowRecord = new BorrowRecord
                {
                    Book_Id = book.Book_Id,
                    BorrowerName = model.BorrowerName,
                    BorrowerEmail = model.BorrowerEmail,
                    Phone = model.Phone,
                    BorrowDate = DateTime.Now
                };
                // Update the book's availability
                book.IsAvailable = false;
                _dbContext.BorrowRecords.Add(borrowRecord);
                await _dbContext.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Successfully borrowed the book: {book.Title}.";
                return RedirectToAction("Index", "Book");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing the borrowing action.";
                return View("Error");
            }
        }
        // Displays the return confirmation for a specific borrow record
        // GET: Borrow/Return/5
        public async Task<IActionResult> Return(int? borrowRecordId)
        {
            if (borrowRecordId == null || borrowRecordId == 0)
            {
                TempData["ErrorMessage"] = "Borrow Record ID was not provided for returning.";
                return View("NotFound");
            }
            try
            {
                var borrowRecord = await _dbContext.BorrowRecords
                    .Include(br => br.Book)
                    .FirstOrDefaultAsync(br => br.BorrowRecord_Id == borrowRecordId);
                if (borrowRecord == null)
                {
                    TempData["ErrorMessage"] = $"No borrow record found with ID {borrowRecordId} to return.";
                    return View("NotFound");
                }
                if (borrowRecord.ReturnDate != null)
                {
                    TempData["ErrorMessage"] = $"The borrow record for '{borrowRecord.Book.Title}' has already been returned.";
                    return View("AlreadyReturned");
                }
                var returnViewModel = new ReturnViewModel
                {
                    BorrowRecord_Id = borrowRecord.BorrowRecord_Id,
                    BookTitle = borrowRecord.Book.Title,
                    BorrowerName = borrowRecord.BorrowerName,
                    BorrowDate = borrowRecord.BorrowDate
                };
                return View(returnViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the return confirmation.";
                return View("Error");
            }
        }
        // Processes the return action, updates the BorrowRecord with the return date, updates the book's availability
        // POST: Borrow/Return/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(ReturnViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var borrowRecord = await _dbContext.BorrowRecords
                    .Include(br => br.Book)
                    .FirstOrDefaultAsync(br => br.BorrowRecord_Id == model.BorrowRecord_Id);
                if (borrowRecord == null)
                {
                    TempData["ErrorMessage"] = $"No borrow record found with ID {model.BorrowRecord_Id} to return.";
                    return View("NotFound");
                }
                if (borrowRecord.ReturnDate != null)
                {
                    TempData["ErrorMessage"] = $"The borrow record for '{borrowRecord.Book.Title}' has already been returned.";
                    return View("AlreadyReturned");
                }
                // Update the borrow record
                borrowRecord.ReturnDate = DateTime.UtcNow;
                // Update the book's availability
                borrowRecord.Book.IsAvailable = true;
                await _dbContext.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Successfully returned the book: {borrowRecord.Book.Title}.";
                return RedirectToAction("Index", "Book");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing the return action.";
                return View("Error");
            }
        }
    }
}
