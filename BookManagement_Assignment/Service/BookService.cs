using Repository.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class BookService
    {
        private readonly BookRepository _book = new BookRepository();
        private readonly UserService _userService = new UserService();

        public List<Book> GetBooks(int role)
        {
            if (_userService.CanView(role))
            {
                return _book.GetBooks();
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have permission to view books.");
            }
        }

        public void AddBook(Book book, int role)
        {
            if (_userService.CanCreate(role))
            {
                _book.Create(book);
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have permission to add books.");
            }
        }

        public void UpdateBook(Book book, int role)
        {
            if (_userService.CanEdit(role))
            {
                _book.Update(book);
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have permission to update books.");
            }
        }

        public Book GetBookById(int id, int role)
        {
            if (_userService.CanView(role))
            {
                return _book.GetBookByID(id);
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have permission to view this book.");
            }
        }

        public void SaveBook(Book book, int role)
        {
            if (_userService.CanCreate(role) || (_userService.CanEdit(role) && book.BookId != 0))
            {
                if (book.BookId == 0)
                {
                    // New book
                    book.BookId = _book.GetBooks().Max(b => b.BookId) + 1;
                    _book.AddBook(book);
                }
                else
                {
                    // Update book
                    _book.Update(book);
                }
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have permission to save books.");
            }
        }

        public void DeleteBook(Book book, int role)
        {
            if (_userService.CanDelete(role))
            {
                _book.Delete(book);
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have permission to delete books.");
            }
        }

        public List<Book> SearchBooks(string name, string description, int role)
        {
            if (_userService.CanSearch(role))
            {
                return _book.Search(name, description);
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have permission to search books.");
            }
        }
    }
}
