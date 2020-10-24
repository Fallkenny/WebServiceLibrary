using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base
{
    public interface ILibraryServiceClient
    {
        void AddBookYearAndEdition(string code, string year, string edition);
        bool AuthorExistsByCode(string str);
        int CountEditionsByBook(string code);
        void CreateAuthor(string name);
        void CreateBook(string title, string[] authors, string year, string edition);
        void DeleteAuthorByCode(string code);
        void DeleteBook(Book book);
        void DeleteBookEdition(string code, string number);
        bool QueryAuthorByCodeInBooks(string code);
        Dictionary<string, string> QueryAuthorByName(string str);
        List<Book> QueryBookByAuthor(string str);
        List<Book> QueryBookByCode(string code);
        List<Book> QueryBookByCompleteName(string name);
        List<Book> QueryBookByName(string str);
        List<Book> QueryBookByYearAndEdition(string year, string edition);
        bool QueryEditionByNumber(string code, string number);
        void UpdateAuthorName(string code, string newName);
        void UpdateBookAuthor(string code, int[] str);
        void UpdateBookName(string code, string str);
        void UpdateBookYearAndEdition(string code, string year, string edition);
    }
}
