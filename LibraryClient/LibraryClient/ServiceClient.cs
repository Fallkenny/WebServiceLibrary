using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Base;

namespace LibraryClient
{
    public class ServiceClient : ILibraryServiceClient
    {
        localhost.LibraryService _service;
        LibrarySerializer serializer;

        public ServiceClient()
        {
            _service = new localhost.LibraryService();
            serializer = new LibrarySerializer();

        }

        public void AddBookYearAndEdition(string code, string year, string edition)
        {
            _service.AddBookYearAndEdition(code, year, edition);
        }

        public bool AuthorExistsByCode(string str)
        {
            return _service.AuthorExistsByCode(str);
        }

        public int CountEditionsByBook(string code)
        {
            return _service.CountEditionsByBook(code);
        }

        public void CreateAuthor(string name)
        {
            _service.CreateAuthor(name);
        }

        public void CreateBook(string title, string[] authors, string year, string edition)
        {
            _service.CreateBook(title, authors, year, edition);
        }

        public void DeleteAuthorByCode(string code)
        {
            _service.DeleteAuthorByCode(code);
        }

        public void DeleteBook(Book book)
        {
            _service.DeleteBook(serializer.Serialize(book));
        }

        public void DeleteBookEdition(string code, string number)
        {
            _service.DeleteBookEdition(code, number);
        }

        public bool QueryAuthorByCodeInBooks(string code)
        {
            return _service.QueryAuthorByCodeInBooks(code);
        }

        public Dictionary<string, string> QueryAuthorByName(string str)
        {
            return serializer.Desserialize<Dictionary<string, string>>(_service.QueryAuthorByName(str));
        }

        public List<Book> QueryBookByAuthor(string str)
        {
            return serializer.Desserialize<List<Book>>(_service.QueryBookByAuthor(str));
        }

        public List<Book> QueryBookByCode(string code)
        {
            return serializer.Desserialize<List<Book>>(_service.QueryBookByCode(code));
        }

        public List<Book> QueryBookByCompleteName(string name)
        {
            return serializer.Desserialize<List<Book>>(_service.QueryBookByCompleteName(name));
        }

        public List<Book> QueryBookByName(string str)
        {
            return serializer.Desserialize<List<Book>>(_service.QueryBookByName(str));
        }

        public List<Book> QueryBookByYearAndEdition(string year, string edition)
        {
            return serializer.Desserialize<List<Book>>(_service.QueryBookByYearAndEdition(year, edition));
        }

        public bool QueryEditionByNumber(string code, string number)
        {
            return _service.QueryEditionByNumber(code, number);
        }

        public void UpdateAuthorName(string code, string newName)
        {
            _service.UpdateAuthorName(code, newName);
        }

        public void UpdateBookAuthor(string code, int[] str)
        {
            _service.UpdateBookAuthor(code, str);
        }

        public void UpdateBookName(string code, string str)
        {
            _service.UpdateBookName(code, str);
        }

        public void UpdateBookYearAndEdition(string code, string year, string edition)
        {
            _service.UpdateBookYearAndEdition(code, year, edition);
        }
    }
}
